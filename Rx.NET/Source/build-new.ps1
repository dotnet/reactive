$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$nuspecDir = Join-Path $scriptPath "NuSpecs"
$isAppVeyor = Test-Path -Path env:\APPVEYOR
$outputLocation = Join-Path $scriptPath "testResults"
$openCoverPath = ".\packages\OpenCover\tools\OpenCover.Console.exe"

$signClientSettings = Join-Path (Join-Path (Get-Item $scriptPath).Parent.Parent.FullName "scripts") "SignClientSettings.json"
$hasSignClientSecret = !([string]::IsNullOrEmpty($env:SignClientSecret))
$signClientAppPath = Join-Path (Join-Path (Join-Path .\Packages "SignClient") "Tools") "SignClient.dll"

#remove any old coverage file
md -Force $outputLocation | Out-Null
$outputPath = (Resolve-Path $outputLocation).Path
$outputFile = Join-Path $outputPath -childpath 'coverage-rx.xml'
Remove-Item $outputPath -Force -Recurse
md -Force $outputLocation | Out-Null

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}
$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

# get version
.\nuget.exe install -excludeversion gitversion.commandline -outputdirectory packages
.\nuget.exe install -excludeversion SignClient -Version 0.5.0-beta4 -pre -outputdirectory packages
.\nuget.exe install -excludeversion OpenCover -outputdirectory packages
.\nuget.exe install -excludeversion ReportGenerator -outputdirectory packages
.\nuget.exe install -excludeversion coveralls.io -outputdirectory packages


#update version
.\packages\gitversion.commandline\tools\gitversion.exe /l console /output buildserver /updateassemblyinfo

$versionObj = .\packages\gitversion.commandline\tools\gitversion.exe | ConvertFrom-Json 

$version = $versionObj.MajorMinorPatch
$tag = $versionObj.PreReleaseLabel
$preRelNum = $versionObj.CommitsSinceVersionSourcePadded
$preRelNum2 = $versionObj.PreReleaseNumber

if($tag -ne ""){
  if($preRelNum -ne "00000") {
    $version = "$version-$tag-$preRelNum"
  }
  elseif ($preRelNum2 -ne "0") {
    $version = "$version-$tag$preRelNum2"
  }
  else {
    $version = "$version-$tag"
  }
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

foreach ($nuspec in $nuspecs) 
{
  $symbolSwitch = "-symbols"
  
  # nuget will error if we pass -symbols to the meta-package
  if($nuSpec -like "*\System.Reactive.nuspec")
  {  
    $symbolSwitch = ""
  }
   
   .\nuget pack $nuspec $symbolSwitch -Version $version -Properties "Configuration=$configuration" -MinClientVersion 2.12 -outputdirectory .\artifacts
   if ($LastExitCode -ne 0) { 
    Write-Host "Error packing NuGet $nuspec" -Foreground Red
    if($isAppVeyor) {
      $host.SetShouldExit($LastExitCode)
    }  
  }
}

if($hasSignClientSecret) {
  Write-Host "Signing Packages" -Foreground Green
  $nupgks = ls .\artifacts\*React*.nupkg | Select -ExpandProperty FullName

  foreach ($nupkg in $nupgks) {
    Write-Host "Submitting $nupkg for signing"

    dotnet $signClientAppPath 'zip' -c $signClientSettings -i $nupkg -s $env:SignClientSecret -n 'Rx.NET' -d 'Reactive Extensions for .NET' -u 'http://reactivex.io/' 

    if ($LastExitCode -ne 0) { 
        Write-Host "Error signing $nupkg" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
        }  
    }
    Write-Host "Finished signing $nupkg"
  }

} else {
  Write-Host "Client Secret not found, not signing packages"
}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "Tests.System.Reactive"

# Execute OpenCover with a target of "dotnet test"
& $openCoverPath -register:user -oldStyle -mergeoutput -target:dotnet.exe -targetdir:"$testDirectory" -targetargs:"test $testDirectory -c $configuration -notrait SkipCI=true" -output:"$outputFile" -skipautoprops -returntargetcode -excludebyattribute:"System.Diagnostics.DebuggerNonUserCodeAttribute;System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" -nodefaultfilters  -hideskipped:All -filter:"+[*]* -[*.Tests]* -[Tests.*]* -[xunit.*]* -[*]Xunit.*" 

if ($LastExitCode -ne 0) { 
    Write-Host "Error with tests" -Foreground Red
    if($isAppVeyor) {
      $host.SetShouldExit($LastExitCode)
    }  
}

# Either display or publish the results
if ($env:CI -eq 'True')
{
  .\packages\coveralls.io\tools\coveralls.net.exe  --opencover "$outputFile" --full-sources
}
else
{
    .\packages\ReportGenerator\tools\ReportGenerator.exe -reports:"$outputFile" -targetdir:"$outputPath"
     &$outPutPath/index.htm
}

if ($env:CI -ne 'True') {
  Write-Host "Reverting AssemblyInfo's" -Foreground Green
  gci $scriptPath -re -in AssemblyInfo.cs | %{ git checkout $_ } 
}