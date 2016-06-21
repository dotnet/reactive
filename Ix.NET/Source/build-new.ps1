$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$nuspecDir = Join-Path $scriptPath "NuSpecs"


if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}

$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

# get tools
.\nuget.exe install -excludeversion -pre gitversion.commandline -outputdirectory packages

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

# Get Reference Generator
.\nuget.exe install -excludeversion -pre NuSpec.ReferenceGenerator -outputdirectory packages

Write-Host "Restoring packages" -Foreground Green
dotnet restore $scriptPath | out-null

Write-Host "Building projects" -Foreground Green
$projects = gci $scriptPath -Directory `
   | Where-Object { ($_.Name -notlike "*DeviceRunner") -and (Test-Path (Join-Path $_.FullName "project.json"))  } `

foreach ($project in $projects) {
  dotnet build -c "$configuration" $project.FullName  
  
  $ns = Join-Path $nuSpecDir "$($project.Name).nuspec"  
    
  if(Test-Path $ns)
  {
    Write-Host "Invoking RefGen on $ns" 
    
    $baseDir = Join-Path $project.FullName "bin" | join-path -ChildPath "$configuration"
    $projJson = Join-Path $project.FullName "project.json"    
    
    Write-Host RefGen.exe generate-cross "-p" `"$projJson`" "-d" `"$baseDir`" "-l" `"$($project.Name).dll`" "-n" `"$ns`"
    .\packages\nuspec.referencegenerator\tools\RefGen.exe generate-cross -p "$projJson" -d "$baseDir" -l "$($project.Name).dll" -n "$ns"
  }
}

Write-Host "Building Packages" -Foreground Green
$nuspecs = ls $nuspecDir\*.nuspec | Select -ExpandProperty FullName

New-Item -ItemType Directory -Force -Path .\artifacts

foreach ($nuspec in $nuspecs) {
   .\nuget pack $nuspec -symbols -Version $version -Properties "Configuration=$configuration" -MinClientVersion 2.12 -outputdirectory .\artifacts
}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests"  
dotnet test $testDirectory -c $configuration

Write-Host "Reverting AssemblyInfo's" -Foreground Green
gci $scriptPath -re -in AssemblyInfo.cs | %{ git checkout $_ } 

