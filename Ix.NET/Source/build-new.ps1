$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$isAppVeyor = Test-Path -Path env:\APPVEYOR
$outputLocation = Join-Path $scriptPath "testResults"
$xUnitConsolePath = ".\packages\xunit.runner.console\tools\net452\xunit.console.exe"
$rootPath = (Resolve-Path .).Path
$artifacts = Join-Path $rootPath "artifacts"


$signClientSettings = Join-Path (Join-Path (Get-Item $scriptPath).Parent.Parent.FullName "scripts") "SignClientSettings.json"
$hasSignClientSecret = !([string]::IsNullOrEmpty($env:SignClientSecret))
$signClientAppPath = ".\packages\SignClient\tools\netcoreapp2.0\SignClient.dll"

#remove any old coverage file
md -Force $outputLocation | Out-Null
$outputPath = (Resolve-Path $outputLocation).Path
$outputFileDotCover1 = Join-Path $outputPath -childpath 'coverage-ix1.dcvr'
$outputFileDotCover2 = Join-Path $outputPath -childpath 'coverage-ix2.dcvr'
$outputFileDotCover = Join-Path $outputPath -childpath 'coverage-ix.dcvr'
$outputFile = Join-Path $outputPath -childpath 'coverage-ix.xml'
Remove-Item $outputPath -Force -Recurse
md -Force $outputLocation | Out-Null

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}

# get tools
.\nuget.exe install -excludeversion SignClient -Version 0.9.0 -outputdirectory packages
.\nuget.exe install -excludeversion JetBrains.dotCover.CommandLineTools -pre -outputdirectory packages
.\nuget.exe install -excludeversion Nerdbank.GitVersioning -Version 2.0.41 -outputdirectory packages
.\nuget.exe install -excludeversion xunit.runner.console -outputdirectory packages
.\nuget.exe install -excludeversion ReportGenerator -outputdirectory packages
#.\nuget.exe install -excludeversion coveralls.io -outputdirectory packages
.\nuget.exe install -excludeversion coveralls.io.dotcover -outputdirectory packages

#update version
$versionObj = .\packages\Nerdbank.GitVersioning\tools\get-version.ps1
$packageSemVer = $versionObj.NuGetPackageVersion

Write-Host "Building $packageSemVer" -Foreground Green

New-Item -ItemType Directory -Force -Path $artifacts


Write-Host "Restoring packages for $scriptPath\Ix.NET.sln" -Foreground Green
# use nuget.exe to restore on the legacy proj type
#.\nuget.exe restore "$scriptPath\System.Interactive.Tests.Uwp.DeviceRunner\System.Interactive.Tests.Uwp.DeviceRunner.csproj"
dotnet restore "$scriptPath\Ix.NET.sln" 
# Force a restore again to get proper version numbers https://github.com/NuGet/Home/issues/4337
dotnet restore "$scriptPath\Ix.NET.sln"

Write-Host "Building $scriptPath\Ix.NET.sln" -Foreground Green

# Using MSBuild here since th UWP test project cannot be built by the dotnet CLI
#msbuild "$scriptPath\Ix.NET.sln" /m /t:build /p:Configuration=$configuration 


Write-Host "Building Packages" -Foreground Green
dotnet pack "$scriptPath\System.Interactive\System.Interactive.csproj" -c $configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
if ($LastExitCode -ne 0) { 
        Write-Host "Error with build" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
}

dotnet pack "$scriptPath\System.Interactive.Async\System.Interactive.Async.csproj" -c $configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
if ($LastExitCode -ne 0) { 
        Write-Host "Error with build" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
}

dotnet pack "$scriptPath\System.Interactive.Async.Providers\System.Interactive.Async.Providers.csproj" -c $configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
if ($LastExitCode -ne 0) { 
        Write-Host "Error with build" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
}

dotnet pack "$scriptPath\System.Interactive.Providers\System.Interactive.Providers.csproj" -c $configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
if ($LastExitCode -ne 0) { 
        Write-Host "Error with build" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
}

if($hasSignClientSecret) {
  Write-Host "Signing Packages" -Foreground Green	
  $nupgks = ls $artifacts\*Interact*.nupkg | Select -ExpandProperty FullName

  foreach ($nupkg in $nupgks) {
    Write-Host "Submitting $nupkg for signing"

    dotnet $signClientAppPath 'sign' -c $signClientSettings -i $nupkg -r $env:SignClientUser -s $env:SignClientSecret -n 'Ix.NET' -d 'Interactive Extensions for .NET' -u 'http://reactivex.io/' 

    if ($LastExitCode -ne 0) { 
        Write-Host "Error signing $nupkg" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
    }
    Write-Host "Finished signing $nupkg"
  }

} else {
  Write-Host "Client Secret not found, not signing packages"
}

Write-Host "Running tests" -Foreground Green
$testDirectory = Join-Path $scriptPath "System.Interactive.Tests"  

# OpenCover isn't working currently. So run tests on CI and coverage with JetBrains 

# Use xUnit CLI as it's significantly faster than vstest (dotnet test)
$dotnet = "$env:ProgramFiles\dotnet\dotnet.exe"
.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe cover /targetexecutable="$dotnet" /targetworkingdir="$testDirectory" /targetarguments="xunit -c $configuration" /Filters="+:module=System.Interactive;+:module=System.Interactive.Async;+:module=System.Interactive.Providers;+:module=System.Interactive.Async.Providers;-:type=Xunit*" /DisableDefaultFilters /ReturnTargetExitCode /AttributeFilters="System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" /output="$outputFileDotCover1"

if ($LastExitCode -ne 0) { 
	Write-Host "Error with tests" -Foreground Red
	if($isAppVeyor) {
	  $host.SetShouldExit($LastExitCode)
	  exit $LastExitCode
	}  
}

$testDirectory = Join-Path $scriptPath "System.Interactive.Async.Tests"  
.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe cover /targetexecutable="$dotnet" /targetworkingdir="$testDirectory" /targetarguments="xunit -c $configuration" /Filters="+:module=System.Interactive;+:module=System.Interactive.Async;+:module=System.Interactive.Providers;+:module=System.Interactive.Async.Providers;-:type=Xunit*" /DisableDefaultFilters /ReturnTargetExitCode /AttributeFilters="System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" /output="$outputFileDotCover2"

if ($LastExitCode -ne 0) { 
	Write-Host "Error with tests" -Foreground Red
	if($isAppVeyor) {
	  $host.SetShouldExit($LastExitCode)
	  exit $LastExitCode
	}  
}

.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe merge /Source="$outputFileDotCover1;$outputFileDotCover2" /Output="$outputFileDotCover"
.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe report /Source="$outputFileDotCover" /Output="$outputFile" /ReportType=DetailedXML /HideAutoProperties

# Either display or publish the results
if ($env:CI -eq 'True')
{
  .\packages\coveralls.io.dotcover\tools\coveralls.net.exe  -p DotCover "$outputFile"
}
else
{
  .\packages\ReportGenerator\tools\ReportGenerator.exe -reports:"$outputFile" -targetdir:"$outputPath"
  &"$outPutPath/index.htm"
}
