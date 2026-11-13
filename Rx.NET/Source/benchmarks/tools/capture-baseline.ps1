<#
.SYNOPSIS
Runs the benchmark suite and stamps the results into baselines/<date>-<shortsha>[-label]/.

.DESCRIPTION
Wraps `dotnet run` over Benchmarks.System.Reactive so every capture lands in a dated,
commit-stamped folder together with the environment it was measured on (commit, branch,
dotnet --info). The stamped folders are the inputs to compare-results.ps1.

-ProjectDir points the script at a different benchmark project; the AsyncRx.NET suite uses it
through its own tools\capture-baseline.ps1 wrapper. Baselines then land in a baselines/ folder
next to that project.

Refuses to run with uncommitted changes (the stamped commit SHA would lie about what was
measured) unless -AllowDirty is passed.

Refuses to write into a folder that already exists (a second capture on the same day and commit
would mix two runs under one environment.json) unless -Force is passed, which deletes the old folder
first. Use -Label to keep several captures of the same commit.

environment.json carries a status: "running" while the benchmarks run, then "complete" or
"failed". compare-results.ps1 refuses a folder whose status is not "complete".

.EXAMPLE
tools\capture-baseline.ps1 -Filter *SelectBenchmarks* -Framework net10.0

.EXAMPLE
tools\capture-baseline.ps1 -Filter * -Label pre-spike -ExtraArgs @('--runtimes','net8.0','net10.0')
#>
[CmdletBinding()]
param(
    [string] $Filter = '*',
    [string] $Framework = 'net10.0',
    [string] $Label,
    [string[]] $ExtraArgs = @(),
    [switch] $AllowDirty,
    # Replace an existing capture folder with the same name instead of refusing to run.
    [switch] $Force,
    # Benchmark project folder. Defaults to the Benchmarks.System.Reactive project beside this tools folder.
    [string] $ProjectDir
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ($ProjectDir) {
    $project = (Resolve-Path $ProjectDir).Path
}
else {
    $project = Join-Path (Split-Path -Parent $PSScriptRoot) 'Benchmarks.System.Reactive'
}
$benchmarksRoot = Split-Path -Parent $project
if (-not (Get-ChildItem -Path $project -Filter '*.csproj' -File -ErrorAction SilentlyContinue)) {
    throw "Benchmark project not found under $project"
}

$dirty = git -C $benchmarksRoot status --porcelain
if ($dirty -and -not $AllowDirty) {
    throw "Working tree is dirty - the stamped commit SHA would not describe what is measured. Commit/stash first, or pass -AllowDirty."
}

$sha = (git -C $benchmarksRoot rev-parse --short HEAD).Trim()
$fullSha = (git -C $benchmarksRoot rev-parse HEAD).Trim()
$branch = (git -C $benchmarksRoot rev-parse --abbrev-ref HEAD).Trim()
$date = Get-Date -Format 'yyyy-MM-dd'

# Active Windows power plan (a common source of run-to-run drift). Best effort: null on non-Windows or if
# powercfg is unavailable/fails — a metadata probe must never abort the capture.
$powerPlan = $null
$isWindowsOs = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform(
    [System.Runtime.InteropServices.OSPlatform]::Windows)
if ($isWindowsOs) {
    try {
        $schemeOutput = (& powercfg /getactivescheme 2>$null | Out-String).Trim()
        if ($LASTEXITCODE -eq 0 -and $schemeOutput) {
            # e.g. "Power Scheme GUID: 381b4222-f694-41f0-9685-ff5bb260df2e  (Balanced)"
            if ($schemeOutput -match '([0-9a-fA-F-]{36})\s*\((.+)\)') {
                $powerPlan = [ordered]@{ guid = $Matches[1]; name = $Matches[2] }
            }
            else {
                $powerPlan = [ordered]@{ guid = $null; name = $schemeOutput }
            }
        }
    }
    catch {
        $powerPlan = $null
    }
}

# Runtime settings that change JIT tiering, PGO, ReadyToRun use or the GC, and therefore the numbers. Both the
# DOTNET_ and the legacy COMPlus_ prefixes are read. Empty object if none are set.
$runtimeKnobs = [ordered]@{}
Get-ChildItem env: |
    Where-Object { $_.Name -match '^(DOTNET|COMPlus)_(Tiered|TC_|ReadyToRun|OSR|JitEnable|gc)' } |
    Sort-Object Name |
    ForEach-Object { $runtimeKnobs[$_.Name] = $_.Value }

$folderName = "$date-$sha"
if ($Label) {
    # Sanitize to a safe filename segment so the label cannot contain path separators or '..'.
    $safeLabel = ($Label -replace '[^A-Za-z0-9_.-]', '-').Trim('.')
    if (-not $safeLabel) { throw "Label '$Label' contains no usable filename characters." }
    $folderName += "-$safeLabel"
}
$dest = Join-Path (Join-Path $benchmarksRoot 'baselines') $folderName

if (Test-Path $dest) {
    if (-not $Force) {
        throw "Destination $dest already exists. A second capture would mix two runs under one environment.json. Pass -Label to keep both, or -Force to replace it."
    }

    Write-Warning "Replacing the existing capture in $dest (-Force)."
    Remove-Item -Recurse -Force $dest
}
New-Item -ItemType Directory -Force $dest | Out-Null

$environmentFile = Join-Path $dest 'environment.json'
$environment = [ordered]@{
    status    = 'running'
    commit    = $fullSha
    shortSha  = $sha
    branch    = $branch
    date      = (Get-Date -Format 'o')
    framework = $Framework
    filter    = $Filter
    extraArgs = $ExtraArgs
    dirty     = [bool]$dirty
    machine   = $env:COMPUTERNAME
    os        = [System.Runtime.InteropServices.RuntimeInformation]::OSDescription
    powerPlan = $powerPlan
    runtimeEnvironment = $runtimeKnobs
}

function Write-Environment([string] $status, [string] $reason) {
    $environment.status = $status
    if ($reason) { $environment.failure = $reason }
    $environment | ConvertTo-Json | Out-File -FilePath $environmentFile -Encoding utf8
}

# Stamped before the run, so that a folder left by a run that crashed or was stopped says so.
Write-Environment 'running'
dotnet --info | Out-File -FilePath (Join-Path $dest 'dotnet-info.txt') -Encoding utf8

Write-Host "Capturing '$Filter' on $Framework at $sha ($branch) -> $dest"

# BenchmarkDotNet puts logs in the artifacts root and the exported reports in results/.
& dotnet run -c Release -f $Framework --project $project -- `
    --filter $Filter --artifacts $dest @ExtraArgs
if ($LASTEXITCODE -ne 0) {
    $reason = "Benchmark run failed with exit code $LASTEXITCODE"
    Write-Environment 'failed' $reason
    throw "$reason. The partial results in $dest are marked as failed."
}

# A command line that BenchmarkDotNet rejects, or a filter that matches nothing, exits 0 with no reports.
if (-not (Get-ChildItem -Path $dest -Recurse -Filter '*-report-full.json' -ErrorAction SilentlyContinue)) {
    $reason = 'The benchmark run wrote no *-report-full.json files'
    Write-Environment 'failed' $reason
    throw "$reason under $dest. Check the filter and the extra arguments."
}

Write-Environment 'complete'
Write-Host "Done. Results in $(Join-Path $dest 'results')"
