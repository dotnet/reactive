$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$isAppVeyor = Test-Path -Path env:\APPVEYOR
$outputLocation = Join-Path $scriptPath "testResults"
$openCoverPath = ".\packages\OpenCover\tools\OpenCover.Console.exe"
$xUnitConsolePath = ".\packages\xunit.runner.console\tools\xunit.console.exe"
$rootPath = (Resolve-Path .).Path
$artifacts = Join-Path $rootPath "artifacts"

$signClientSettings = Join-Path (Join-Path (Get-Item $scriptPath).Parent.Parent.FullName "scripts") "SignClientSettings.json"
$hasSignClientSecret = !([string]::IsNullOrEmpty($env:SignClientSecret))
$signClientAppPath = Join-Path (Join-Path (Join-Path .\Packages "SignClient") "Tools") "SignClient.dll"

#remove any old coverage file
md -Force $outputLocation | Out-Null
$outputPath = (Resolve-Path $outputLocation).Path
$outputFileDotCover1 = Join-Path $outputPath -childpath 'coverage-rx1.dcvr'
$outputFileDotCover2 = Join-Path $outputPath -childpath 'coverage-rx2.dcvr'
$outputFileDotCover = Join-Path $outputPath -childpath 'coverage-rx.dcvr'
$outputFile = Join-Path $outputPath -childpath 'coverage-rx.xml'
Remove-Item $outputPath -Force -Recurse
md -Force $outputLocation | Out-Null

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}

# get tools
.\nuget.exe install -excludeversion SignClient -Version 0.5.0-beta4 -pre -outputdirectory packages
.\nuget.exe install -excludeversion JetBrains.dotCover.CommandLineTools -pre -outputdirectory packages
.\nuget.exe install -excludeversion gitversion.commandline -pre -outputdirectory packages
.\nuget.exe install -excludeversion xunit.runner.console -pre -outputdirectory packages
#.\nuget.exe install -excludeversion OpenCover -Version 4.6.519 -outputdirectory packages
.\nuget.exe install -excludeversion ReportGenerator -outputdirectory packages
#.\nuget.exe install -excludeversion coveralls.io -outputdirectory packages


#update version
.\packages\gitversion.commandline\tools\gitversion.exe /l console /output buildserver

New-Item -ItemType Directory -Force -Path $artifacts

Write-Host "Restoring packages for $scriptPath\System.Reactive.sln" -Foreground Green
msbuild "$scriptPath\System.Reactive.sln" /m /t:restore /p:Configuration=$configuration 

Write-Host "Building $scriptPath\System.Reactive.sln" -Foreground Green
msbuild "$scriptPath\System.Reactive.sln" /m /t:build /p:Configuration=$configuration 

Write-Host "Building Packages" -Foreground Green
msbuild "$scriptPath\src\System.Reactive\System.Reactive.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true /p:NuGetBuildTasksPackTargets="workaround"
msbuild "$scriptPath\src\Microsoft.Reactive.Testing\Microsoft.Reactive.Testing.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\src\System.Reactive.Observable.Aliases\System.Reactive.Observable.Aliases.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true

msbuild "$scriptPath\facades\System.Reactive.Core\System.Reactive.Core.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true /p:NuGetBuildTasksPackTargets="workaround"
msbuild "$scriptPath\facades\System.Reactive.Experimental\System.Reactive.Experimental.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Interfaces\System.Reactive.Interfaces.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Linq\System.Reactive.Linq.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.PlatformServices\System.Reactive.PlatformServices.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true /p:NuGetBuildTasksPackTargets="workaround"
msbuild "$scriptPath\facades\System.Reactive.Providers\System.Reactive.Providers.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Runtime.Remoting\System.Reactive.Runtime.Remoting.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Windows.Forms\System.Reactive.Windows.Forms.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\facades\System.Reactive.Windows.Threading\System.Reactive.Windows.Threading.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true /p:NuGetBuildTasksPackTargets="workaround"
msbuild "$scriptPath\facades\System.Reactive.WindowsRuntime\System.Reactive.WindowsRuntime.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true /p:NuGetBuildTasksPackTargets="workaround"


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
$testDirectory = Join-Path $scriptPath "tests\Tests.System.Reactive"

# OpenCover and Coveralls isn't working currently. So run tests on CI and coverage with JetBrains locally

if($env:CI -eq 'True')
{
	# Run .NET Core only for now until perf improves on the runner for .net desktop
	dotnet test "$testDirectory\Tests.System.Reactive.csproj" -c $configuration --no-build -f netcoreapp1.0 --filter:SkipCI!=true

	if ($LastExitCode -ne 0) { 
		Write-Host "Error with tests" -Foreground Red
		if($isAppVeyor) {
		  $host.SetShouldExit($LastExitCode)
		}  
	}

	# run .net desktop tests
	& $xUnitConsolePath $testDirectory\bin\$configuration\net46\Tests.System.Reactive.dll -notrait SkipCI=true
	if ($LastExitCode -ne 0) { 
		Write-Host "Error with tests" -Foreground Red
		if($isAppVeyor) {
		  $host.SetShouldExit($LastExitCode)
		}  
	}
}
else
{
	$dotnet = "$env:ProgramFiles\dotnet\dotnet.exe"
	.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe cover /targetexecutable="$dotnet" /targetworkingdir="$testDirectory" /targetarguments="test -c $configuration --no-build -f netcoreapp1.0 --filter:SkipCI!=true" /output="$outputFileDotCover1" /Filters="+:module=System.Reactive;+:module=Microsoft.Reactive.Testing;+:module=System.Reactive.Observable.Aliases;-:type=Xunit*" /DisableDefaultFilters /ReturnTargetExitCode

	if ($LastExitCode -ne 0) { 
		Write-Host "Error with tests" -Foreground Red
		if($isAppVeyor) {
		  $host.SetShouldExit($LastExitCode)
		}  
	}

	.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe cover /targetexecutable="$xUnitConsolePath" /targetworkingdir="$testDirectory\bin\$configuration\net46\" /targetarguments="Tests.System.Reactive.dll -notrait SkipCI=true" /output="$outputFileDotCover2" /Filters="+:module=System.Reactive;+:module=Microsoft.Reactive.Testing;+:module=System.Reactive.Observable.Aliases;-:type=Xunit*" /DisableDefaultFilters /ReturnTargetExitCode

	if ($LastExitCode -ne 0) { 
		Write-Host "Error with tests" -Foreground Red
		if($isAppVeyor) {
		  $host.SetShouldExit($LastExitCode)
		}  
	}

	#.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe analyze /targetexecutable="$dotnet" /targetworkingdir="$testDirectory" /targetarguments="test -c $configuration --no-build --filter:SkipCI!=true" /ReportType=DetailedXML /output="$outputFile" /Filters="+:module=System.Reactive;+:module=Microsoft.Reactive.Testing;+:module=System.Reactive.Observable.Aliases;-:type=Xunit*" /DisableDefaultFilters /HideAutoProperties /ReturnTargetExitCode

	.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe merge /Source="$outputFileDotCover1;$outputFileDotCover2" /Output="$outputFileDotCover"
	.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe report /Source="$outputFileDotCover" /Output="$outputFile" /ReportType=DetailedXML /HideAutoProperties

	.\packages\ReportGenerator\tools\ReportGenerator.exe -reports:"$outputFile" -targetdir:"$outputPath"
    &"$outPutPath/index.htm"
}



# We bail out here because OpenCover isn't properly reporting coverage 
exit

# Execute OpenCover with a target of "dotnet test"
# & $openCoverPath -register:user -oldStyle -mergeoutput -target:dotnet.exe -targetdir:"$testDirectory" -targetargs:"test --no-build --filter:SkipCI!=true" -output:"$outputFile" -skipautoprops -returntargetcode -excludebyattribute:"System.Diagnostics.DebuggerNonUserCodeAttribute;System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" -nodefaultfilters  -hideskipped:All -filter:"+[*]* -[*.Tests]* -[Tests.*]* -[xunit.*]* -[*]Xunit.*" 



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