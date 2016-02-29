$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

$active = dnvm list -PassThru | Where-Object {$_.Active -eq $true }

$version = $active | Select -ExpandProperty Version
$runtime = $active | Select -ExpandProperty Runtime
$architecture = $active | Select -ExpandProperty Architecture

$runtimeDir = Join-Path $env:USERPROFILE "\.dnx\runtimes\dnx-$runtime-win-$architecture.$version"

Write-Host "Restoring packages" -Foreground Green
dnu restore $scriptPath --quiet | out-null

Write-Host "Building projects" -Foreground Green
$solutionPath = Join-Path $scriptPath "Rx-New.sln"
. $msbuildExe $solutionPath /m /p:Configuration=Release /p:RuntimeToolingDirectory=$runtimeDir /v:q

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests.System.Reactive"
dnx -p $testDirectory test
