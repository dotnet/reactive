$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$nuspecDir = Join-Path $scriptPath "NuSpecs"
$isAppVeyor = Test-Path -Path env:\APPVEYOR

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}

$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

# get tools
.\nuget.exe install -excludeversion gitversion.commandline -outputdirectory packages

#update version
.\packages\gitversion.commandline\tools\gitversion.exe /l console /output buildserver /updateassemblyinfo

$versionObj = .\packages\gitversion.commandline\tools\gitversion.exe | ConvertFrom-Json 

$version = $versionObj.MajorMinorPatch
$tag = $versionObj.PreReleaseLabel
$preRelNum = $versionObj.CommitsSinceVersionSourcePadded

if($tag -ne ""){
  $version = "$version-$tag-$preRelNum"
}

Write-Host "Version: $version"

Write-Host "Restoring packages" -Foreground Green
dotnet restore $scriptPath | out-null

Write-Host "Building projects" -Foreground Green
$projects = gci $scriptPath -Directory `
   | Where-Object { ($_.Name -notlike "*DeviceRunner") -and (Test-Path (Join-Path $_.FullName "project.json"))  } `

foreach ($project in $projects) {
  dotnet build -c "$configuration" $project.FullName
  if ($LastExitCode -ne 0) { 
    Write-Host "Error building project $project" -Foreground Red
    if($isAppVeyor) {
      $host.SetShouldExit($LastExitCode)
    }  
  } 
}

Write-Host "Building Packages" -Foreground Green
$nuspecs = ls $nuspecDir\*.nuspec | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path .\artifacts

foreach ($nuspec in $nuspecs) {
   .\nuget pack $nuspec -symbols -Version $version -Properties "Configuration=$configuration" -MinClientVersion 2.12 -outputdirectory .\artifacts
   if ($LastExitCode -ne 0) { 
    Write-Host "Error packing NuGet $nuspec" -Foreground Red
    if($isAppVeyor) {
      $host.SetShouldExit($LastExitCode)
    }  
  }

}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests"  
dotnet test $testDirectory -c $configuration
if ($LastExitCode -ne 0) { 
    Write-Host "Error with tests" -Foreground Red
    if($isAppVeyor) {
      $host.SetShouldExit($LastExitCode)
    }  
}

Write-Host "Reverting AssemblyInfo's" -Foreground Green
gci $scriptPath -re -in AssemblyInfo.cs | %{ git checkout $_ } 

