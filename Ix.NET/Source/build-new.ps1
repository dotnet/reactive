$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

Write-Host "Restoring packages" -Foreground Green
dotnet restore $scriptPath | out-null

Write-Host "Building projects" -Foreground Green
$projects = gci $scriptPath -Directory `
  | Where-Object { Test-Path (Join-Path $_.FullName "project.json")  } `
  | Select -ExpandProperty FullName

foreach ($project in $projects) {
  dotnet build $project
}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests"
cd  $testDirectory 
dotnet test

