<#
.SYNOPSIS
Compares two benchmark result sets (BenchmarkDotNet full-JSON exports) and reports
regressions and improvements.

.DESCRIPTION
Matches benchmarks across the two folders by FullName plus job (FullName includes parameter values,
e.g. 'SelectBenchmarks.Select(N: 1000)'). Each match is classified on mean time:

  - Unchanged    : |ratio - 1| <= threshold, or the two confidence intervals overlap
  - Regression   : candidate slower beyond the threshold and outside the noise
  - Improvement  : candidate faster beyond the threshold and outside the noise

Allocation changes (BytesAllocatedPerOperation) are reported separately - allocations are
near-deterministic, so a change there is signal even when timing is noisy. Rows that use the thread
pool, timers or Task.Yield vary by a few bytes from run to run, so a change is reported only when it is
larger than both -AllocToleranceBytes (default 4) and -AllocTolerancePercent (default 0.5) of the
baseline.

Each folder must be a capture that capture-baseline.ps1 completed: a folder whose environment.json
status is "running" or "failed" is refused, unless -AllowIncomplete is passed. A folder without
environment.json (not made by capture-baseline.ps1) gives a warning.

Writes a markdown report (worst regression first) and prints a summary.

.EXAMPLE
tools\compare-results.ps1 -Baseline baselines\2026-07-01-abc1234 -Candidate baselines\2026-07-02-def5678

.EXAMPLE
tools\compare-results.ps1 -Baseline old -Candidate new -ThresholdPercent 5 -OutFile compare.md
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)] [string] $Baseline,
    [Parameter(Mandatory)] [string] $Candidate,
    [double] $ThresholdPercent = 2.0,
    [string] $OutFile,
    [double] $AllocToleranceBytes = 4,
    [double] $AllocTolerancePercent = 0.5,
    [switch] $AllowIncomplete
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$inv = [System.Globalization.CultureInfo]::InvariantCulture

function Load-Results([string] $folder) {
    if (-not (Test-Path $folder)) { throw "Folder not found: $folder" }

    $environmentFile = Join-Path $folder 'environment.json'
    if (Test-Path $environmentFile) {
        $environment = Get-Content $environmentFile -Raw | ConvertFrom-Json
        # Captures made before the status field existed have no status; they are treated as complete.
        if ($environment.PSObject.Properties['status'] -and $environment.status -ne 'complete') {
            $why = if ($environment.PSObject.Properties['failure']) { ": $($environment.failure)" } else { '' }
            if (-not $AllowIncomplete) {
                throw "$folder is a capture with status '$($environment.status)'$why. Its results are partial. Capture it again, or pass -AllowIncomplete."
            }

            Write-Warning "$folder has status '$($environment.status)'$why; using its partial results (-AllowIncomplete)."
        }
    }
    else {
        Write-Warning "$folder has no environment.json, so it was not made by capture-baseline.ps1. Check that its runs are comparable."
    }

    $files = Get-ChildItem -Path $folder -Recurse -Filter '*-report-full.json'
    if (-not $files) { throw "No *-report-full.json files under $folder" }

    $map = @{}
    foreach ($file in $files) {
        $doc = Get-Content $file.FullName -Raw | ConvertFrom-Json
        foreach ($b in $doc.Benchmarks) {
            $n = $null
            if ($b.Parameters) {
                # BDN joins multiple parameters with '&' (e.g. 'Density=Boundary&N=100')
                foreach ($pair in ($b.Parameters -split '[&,]\s*')) {
                    $kv = $pair -split '=', 2
                    if ($kv.Length -eq 2 -and $kv[0].Trim() -eq 'N') {
                        $parsed = 0L
                        if ([long]::TryParse($kv[1].Trim(), [ref]$parsed)) { $n = $parsed }
                    }
                }
            }

            $stats = $b.Statistics
            if ($null -eq $stats) { continue }   # failed/skipped case

            $allocated = $null
            if ($b.PSObject.Properties['Memory'] -and $null -ne $b.Memory) {
                $allocated = $b.Memory.BytesAllocatedPerOperation
            }

            # FullName does not identify the job. A run with several jobs (for example
            # '--runtimes net8.0 net10.0') has one row per job with the same FullName, so the key
            # also carries the job, taken from DisplayInfo ('Type.Method: <job> [params]'). The random
            # 'Job-XXXXXX' id is dropped so that the same job matches across two runs. A single
            # default job keeps the bare FullName, so existing baselines still match.
            $key = $b.FullName
            if ($b.PSObject.Properties['DisplayInfo'] -and $b.DisplayInfo -match '^[^:]+:\s*(?<job>.+?)(\s*\[[^\]]*\])?$') {
                $job = ($Matches['job'] -replace '^Job-[A-Z]+', '').Trim()
                if ($job -and $job -ne 'DefaultJob') { $key = '{0} @ {1}' -f $b.FullName, $job }
            }

            if ($map.ContainsKey($key)) {
                Write-Warning "Duplicate benchmark key '$key' in $folder; keeping the last result."
            }

            $map[$key] = [pscustomobject]@{
                FullName  = $key
                Mean      = [double]$stats.Mean
                CiLower   = [double]$stats.ConfidenceInterval.Lower
                CiUpper   = [double]$stats.ConfidenceInterval.Upper
                Allocated = $allocated
                N         = $n
            }
        }
    }

    return $map
}

function Format-Num([object] $value, [string] $format = 'N1') {
    if ($null -eq $value) { return '-' }
    return ([double]$value).ToString($format, $inv)
}

$baseMap = Load-Results $Baseline
$candMap = Load-Results $Candidate
$threshold = $ThresholdPercent / 100.0

$regressions = [System.Collections.Generic.List[object]]::new()
$improvements = [System.Collections.Generic.List[object]]::new()
$unchanged = [System.Collections.Generic.List[object]]::new()
$allocChanges = [System.Collections.Generic.List[object]]::new()

foreach ($name in ($baseMap.Keys | Where-Object { $candMap.ContainsKey($_) })) {
    $b = $baseMap[$name]
    $c = $candMap[$name]
    if ($b.Mean -le 0) { continue }

    $ratio = $c.Mean / $b.Mean
    $ciOverlap = ($c.CiLower -le $b.CiUpper) -and ($b.CiLower -le $c.CiUpper)

    $row = [pscustomobject]@{
        FullName      = $name
        BaseMean      = $b.Mean
        CandMean      = $c.Mean
        Ratio         = $ratio
        WithinNoise   = $ciOverlap
        BaseAllocated = $b.Allocated
        CandAllocated = $c.Allocated
        N             = $c.N
    }

    if ([math]::Abs($ratio - 1) -le $threshold -or $ciOverlap) {
        $unchanged.Add($row)
    }
    elseif ($ratio -gt 1) {
        $regressions.Add($row)
    }
    else {
        $improvements.Add($row)
    }

    if ($null -ne $b.Allocated -and $null -ne $c.Allocated) {
        $allocDelta = [math]::Abs([double]$c.Allocated - [double]$b.Allocated)
        $allocLimit = [math]::Max($AllocToleranceBytes, [double]$b.Allocated * $AllocTolerancePercent / 100.0)
        if ($allocDelta -gt $allocLimit) {
            $allocChanges.Add($row)
        }
    }
}

$onlyBase = @($baseMap.Keys | Where-Object { -not $candMap.ContainsKey($_) } | Sort-Object)
$onlyCand = @($candMap.Keys | Where-Object { -not $baseMap.ContainsKey($_) } | Sort-Object)

function ConvertTo-MarkdownTable([object[]] $rows) {
    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.Add('| Benchmark | Base Mean (ns) | Cand Mean (ns) | Ratio | ns/N (cand) | Base B/op | Cand B/op |')
    $lines.Add('|---|---:|---:|---:|---:|---:|---:|')
    foreach ($r in $rows) {
        $perElement = if ($null -ne $r.N -and $r.N -gt 0) { Format-Num ($r.CandMean / $r.N) 'N2' } else { '-' }
        $lines.Add(('| {0} | {1} | {2} | {3} | {4} | {5} | {6} |' -f
            $r.FullName,
            (Format-Num $r.BaseMean),
            (Format-Num $r.CandMean),
            $r.Ratio.ToString('N3', $inv),
            $perElement,
            (Format-Num $r.BaseAllocated 'N0'),
            (Format-Num $r.CandAllocated 'N0')))
    }
    return $lines
}

$report = [System.Collections.Generic.List[string]]::new()
$report.Add('# Benchmark comparison')
$report.Add('')
$report.Add("- **Baseline:** $Baseline")
$report.Add("- **Candidate:** $Candidate")
$report.Add("- **Threshold:** $ThresholdPercent% (plus confidence-interval overlap check)")
$report.Add("- **Allocation tolerance:** more than $AllocToleranceBytes B and more than $AllocTolerancePercent% of the baseline")
$report.Add("- Matched: $($regressions.Count + $improvements.Count + $unchanged.Count), regressions: $($regressions.Count), improvements: $($improvements.Count), unchanged: $($unchanged.Count)")
$report.Add('')

$report.Add("## Regressions ($($regressions.Count))")
$report.Add('')
if ($regressions.Count -gt 0) {
    $report.AddRange([string[]](ConvertTo-MarkdownTable ($regressions | Sort-Object Ratio -Descending)))
} else { $report.Add('None.') }
$report.Add('')

$report.Add("## Improvements ($($improvements.Count))")
$report.Add('')
if ($improvements.Count -gt 0) {
    $report.AddRange([string[]](ConvertTo-MarkdownTable ($improvements | Sort-Object Ratio)))
} else { $report.Add('None.') }
$report.Add('')

$report.Add("## Allocation changes ($($allocChanges.Count))")
$report.Add('')
if ($allocChanges.Count -gt 0) {
    $report.AddRange([string[]](ConvertTo-MarkdownTable ($allocChanges | Sort-Object { [double]$_.CandAllocated - [double]$_.BaseAllocated } -Descending)))
} else { $report.Add('None.') }
$report.Add('')

if ($onlyBase.Count -gt 0) {
    $report.Add("## Only in baseline ($($onlyBase.Count))")
    $report.Add('')
    foreach ($name in $onlyBase) { $report.Add("- $name") }
    $report.Add('')
}
if ($onlyCand.Count -gt 0) {
    $report.Add("## Only in candidate ($($onlyCand.Count))")
    $report.Add('')
    foreach ($name in $onlyCand) { $report.Add("- $name") }
    $report.Add('')
}

if (-not $OutFile) {
    $baseName = Split-Path -Leaf ($Baseline.TrimEnd('\', '/'))
    $OutFile = Join-Path $Candidate "compare-vs-$baseName.md"
}
$report | Out-File -FilePath $OutFile -Encoding utf8

Write-Host ("Matched {0} benchmarks: {1} regressions, {2} improvements, {3} unchanged; {4} allocation changes; {5} only-in-baseline, {6} only-in-candidate." -f
    ($regressions.Count + $improvements.Count + $unchanged.Count),
    $regressions.Count, $improvements.Count, $unchanged.Count,
    $allocChanges.Count, $onlyBase.Count, $onlyCand.Count)
Write-Host "Report: $OutFile"

if ($regressions.Count -gt 0) {
    Write-Host ''
    Write-Host 'Worst regressions:'
    $regressions | Sort-Object Ratio -Descending | Select-Object -First 10 | ForEach-Object {
        Write-Host ("  {0}  x{1}" -f $_.FullName, $_.Ratio.ToString('N3', $inv))
    }
}
