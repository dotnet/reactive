$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$nuspecDir = Join-Path $scriptPath "NuSpecs"

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}
$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

# get version
.\nuget.exe install -excludeversion gitversion.commandline -outputdirectory packages
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

#need to ensure core is built first due to bad dependency order determination by dotnet build

$coreDir = gci $scriptPath -Directory | where-object {$_.Name -eq "System.Reactive.Core"} | Select -First 1
dotnet build -c "$configuration" $coreDir.FullName

Write-Host "Building projects" -Foreground Green
$projects = gci $scriptPath -Directory `
   | Where-Object { ($_.Name -notlike "*DeviceRunner") -and (Test-Path (Join-Path $_.FullName "project.json"))  } `

foreach ($project in $projects) {
  dotnet build -c "$configuration" $project.FullName  
  if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode)  }

}

Write-Host "Building Packages" -Foreground Green
$nuspecs = ls $nuspecDir\*.nuspec | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path .\artifacts

foreach ($nuspec in $nuspecs) 
{
  $symbolSwitch = "-symbols"
  
  # nuget will error if we pass -symbols to the meta-package
  if($nuSpec -like "*\System.Reactive.nuspec")
  {  
    $symbolSwitch = ""
  }
   
   .\nuget pack $nuspec $symbolSwitch -Version $version -Properties "Configuration=$configuration" -MinClientVersion 2.12 -outputdirectory .\artifacts
   if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode)  }

}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests.System.Reactive"
dotnet test $testDirectory -c "$configuration"
if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode)  }

Write-Host "Reverting AssemblyInfo's" -Foreground Green
gci $scriptPath -re -in AssemblyInfo.cs | %{ git checkout $_ } 