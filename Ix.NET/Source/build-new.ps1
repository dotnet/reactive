$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}

$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

# get version
.\nuget.exe install -excludeversion -pre gitversion.commandline -outputdirectory packages
.\packages\gitversion.commandline\tools\gitversion.exe /l console /output buildserver /updateassemblyinfo

$versionObj = .\packages\gitversion.commandline\tools\gitversion.exe | ConvertFrom-Json 

$version = $versionObj.NuGetVersionV2

Write-Host "Version: $version"

Write-Host "Restoring packages" -Foreground Green
dotnet restore $scriptPath | out-null

Write-Host "Building projects" -Foreground Green
$projects = gci $scriptPath -Directory `
  | Where-Object { Test-Path (Join-Path $_.FullName "project.json")  } `
  | Select -ExpandProperty FullName

foreach ($project in $projects) {
  dotnet build -c "$configuration" $project  
}

$nuspecDir = Join-Path $scriptPath "NuSpecs"

Write-Host "Building Packages" -Foreground Green
$nuspecs = ls $nuspecDir\*.nuspec | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path .\artifacts

foreach ($nuspec in $nuspecs) {
   .\nuget pack $nuspec -Version $version -Properties "Configuration=$configuration" -MinClientVersion 3.4 -outputdirectory .\artifacts
}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests"  
dotnet test $testDirectory -c $configuration

