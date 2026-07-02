<#
.SYNOPSIS
Runs the benchmark suite and stamps the results into baselines/<date>-<shortsha>[-label]/.

.DESCRIPTION
Wraps `dotnet run` over Benchmarks.System.Reactive so every capture lands in a dated,
commit-stamped folder together with the environment it was measured on (commit, branch,
dotnet --info). The stamped folders are the inputs to compare-results.ps1.

Refuses to run with uncommitted changes (the stamped commit SHA would lie about what was
measured) unless -AllowDirty is passed.

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
    [switch] $AllowDirty
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$benchmarksRoot = Split-Path -Parent $PSScriptRoot
$project = Join-Path $benchmarksRoot 'Benchmarks.System.Reactive'
if (-not (Test-Path (Join-Path $project 'Benchmarks.System.Reactive.csproj'))) {
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

$folderName = "$date-$sha"
if ($Label) {
    # Sanitize to a safe filename segment so the label cannot contain path separators or '..'.
    $safeLabel = ($Label -replace '[^A-Za-z0-9_.-]', '-').Trim('.')
    if (-not $safeLabel) { throw "Label '$Label' contains no usable filename characters." }
    $folderName += "-$safeLabel"
}
$dest = Join-Path (Join-Path $benchmarksRoot 'baselines') $folderName

if (Test-Path (Join-Path $dest 'results')) {
    Write-Warning "Destination $dest already has results; new results will be merged into it."
}
New-Item -ItemType Directory -Force $dest | Out-Null

Write-Host "Capturing '$Filter' on $Framework at $sha ($branch) -> $dest"

# BenchmarkDotNet puts logs in the artifacts root and the exported reports in results/.
& dotnet run -c Release -f $Framework --project $project -- `
    --filter $Filter --artifacts $dest @ExtraArgs
if ($LASTEXITCODE -ne 0) {
    throw "Benchmark run failed with exit code $LASTEXITCODE"
}

dotnet --info | Out-File -FilePath (Join-Path $dest 'dotnet-info.txt') -Encoding utf8

[ordered]@{
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
} | ConvertTo-Json | Out-File -FilePath (Join-Path $dest 'environment.json') -Encoding utf8

Write-Host "Done. Results in $(Join-Path $dest 'results')"
