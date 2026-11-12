<#
.SYNOPSIS
Compares two AsyncRx benchmark result sets and reports regressions and improvements.

.DESCRIPTION
A thin wrapper over the Rx.NET suite's tools\compare-results.ps1, which is not specific to either
library: it matches benchmarks by full name across two folders of BenchmarkDotNet full-JSON reports.
See that script for the classification rules.

Gated AsyncRx operators pay a thread-pool hop per element, so their timings are noisier than the Rx
suite's; consider -ThresholdPercent 5 and give more weight to the allocation columns.

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
    [string] $OutFile
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$shared = Join-Path $PSScriptRoot '..\..\..\Rx.NET\Source\benchmarks\tools\compare-results.ps1'

& $shared @PSBoundParameters
