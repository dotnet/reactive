$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$isAppVeyor = Test-Path -Path env:\APPVEYOR
$outputLocation = Join-Path $scriptPath "testResults"
$openCoverPath = ".\packages\OpenCover\tools\OpenCover.Console.exe"
$rootPath = (Resolve-Path .).Path
$artifacts = Join-Path $rootPath "artifacts"

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

# get tools
.\nuget.exe install -excludeversion SignClient -Version 0.5.0-beta4 -pre -outputdirectory packages
.\nuget.exe install -excludeversion OpenCover -outputdirectory packages
.\nuget.exe install -excludeversion ReportGenerator -outputdirectory packages
.\nuget.exe install -excludeversion coveralls.io -outputdirectory packages

New-Item -ItemType Directory -Force -Path $artifacts

Write-Host "Restoring packages for $scriptPath\System.Reactive.sln" -Foreground Green
msbuild "$scriptPath\System.Reactive.sln" /t:restore /p:Configuration=$configuration 

Write-Host "Building $scriptPath\System.Reactive.sln" -Foreground Green
msbuild "$scriptPath\System.Reactive.sln" /t:build /p:Configuration=$configuration 

Write-Host "Building Packages" -Foreground Green
msbuild "$scriptPath\System.Reactive\System.Reactive.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts
msbuild "$scriptPath\Microsoft.Reactive.Testing\Microsoft.Reactive.Testing.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts
msbuild "$scriptPath\System.Reactive.Observable.Aliases\System.Reactive.Observable.Aliases.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts


if($hasSignClientSecret) {
  Write-Host "Signing Packages" -Foreground Green
  $nupgks = ls $artifacts\*React*.nupkg | Select -ExpandProperty FullName

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

vstest.console "$testDirectory\bin\$configuration\net46\Tests.System.Reactive.dll" /TestAdapterPath:"$testDirectory\bin\$configuration\net46" /Parallel


exit # TODO 

# Execute OpenCover with a target of "dotnet test"
#& $openCoverPath -register:user -oldStyle -mergeoutput -target:dotnet.exe -targetdir:"$testDirectory" -targetargs:"test $testDirectory -c $configuration -notrait SkipCI=true" -output:"$outputFile" -skipautoprops -returntargetcode -excludebyattribute:"System.Diagnostics.DebuggerNonUserCodeAttribute;System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" -nodefaultfilters  -hideskipped:All -filter:"+[*]* -[*.Tests]* -[Tests.*]* -[xunit.*]* -[*]Xunit.*" 

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