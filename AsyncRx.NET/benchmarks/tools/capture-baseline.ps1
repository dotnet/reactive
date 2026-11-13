<#
.SYNOPSIS
Runs the AsyncRx benchmark suite and stamps the results into baselines/<date>-<shortsha>[-label]/.

.DESCRIPTION
A thin wrapper over the Rx.NET suite's tools\capture-baseline.ps1, pointed at
Benchmarks.System.Reactive.Async with -ProjectDir. Captures land in AsyncRx.NET\benchmarks\baselines\
together with the environment they were measured on. See that script for the details.

Refuses to run with uncommitted changes unless -AllowDirty is passed.

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
    [switch] $Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$shared = Join-Path $PSScriptRoot '..\..\..\Rx.NET\Source\benchmarks\tools\capture-baseline.ps1'
$project = Join-Path (Split-Path -Parent $PSScriptRoot) 'Benchmarks.System.Reactive.Async'

& $shared -ProjectDir $project @PSBoundParameters
