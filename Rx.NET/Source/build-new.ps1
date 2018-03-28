$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$isAppVeyor = Test-Path -Path env:\APPVEYOR
$outputLocation = Join-Path $scriptPath "testResults"
$openCoverPath = ".\packages\OpenCover\tools\OpenCover.Console.exe"
$xUnitConsolePath = ".\packages\xunit.runner.console\tools\net452\xunit.console.exe"
$rootPath = (Resolve-Path .).Path
$artifacts = Join-Path $rootPath "artifacts"

$signClientSettings = Join-Path (Join-Path (Get-Item $scriptPath).Parent.Parent.FullName "scripts") "SignClientSettings.json"
$hasSignClientSecret = !([string]::IsNullOrEmpty($env:SignClientSecret))
$signClientAppPath = ".\packages\SignClient\tools\netcoreapp2.0\SignClient.dll"

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
.\nuget.exe install -excludeversion SignClient -Version 0.9.0 -outputdirectory packages
.\nuget.exe install -excludeversion JetBrains.dotCover.CommandLineTools -version 2017.3.5 -outputdirectory packages
.\nuget.exe install -excludeversion Nerdbank.GitVersioning -Version 2.1.23 -outputdirectory packages
.\nuget.exe install -excludeversion xunit.runner.console -outputdirectory packages
#.\nuget.exe install -excludeversion OpenCover -Version 4.6.519 -outputdirectory packages
.\nuget.exe install -excludeversion ReportGenerator -outputdirectory packages
#.\nuget.exe install -excludeversion coveralls.io -outputdirectory packages
.\nuget.exe install -excludeversion coveralls.io.dotcover -outputdirectory packages

#update version
$versionObj = .\packages\Nerdbank.GitVersioning\tools\get-version.ps1
$packageSemVer = $versionObj.NuGetPackageVersion

Write-Host "Building $packageSemVer" -Foreground Green

New-Item -ItemType Directory -Force -Path $artifacts

Write-Host "Building $scriptPath\System.Reactive.sln" -Foreground Green
msbuild "$scriptPath\System.Reactive.sln" /restore /t:build /m /p:Configuration=$configuration /bl:.\artifacts\build.binlog /p:RestoreAdditionalProjectFallbackFolders=""
if ($LastExitCode -ne 0) { 
        Write-Host "Error with build" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
}


Write-Host "Building Packages" -Foreground Green
msbuild "$scriptPath\src\System.Reactive.Interfaces\System.Reactive.Interfaces.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\src\System.Reactive\System.Reactive.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\src\Microsoft.Reactive.Testing\Microsoft.Reactive.Testing.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\src\System.Reactive.Observable.Aliases\System.Reactive.Observable.Aliases.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true

msbuild "$scriptPath\facades\System.Reactive.Core\System.Reactive.Core.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\facades\System.Reactive.Experimental\System.Reactive.Experimental.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Linq\System.Reactive.Linq.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.PlatformServices\System.Reactive.PlatformServices.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\facades\System.Reactive.Providers\System.Reactive.Providers.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Runtime.Remoting\System.Reactive.Runtime.Remoting.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true 
msbuild "$scriptPath\facades\System.Reactive.Windows.Forms\System.Reactive.Windows.Forms.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\facades\System.Reactive.Windows.Threading\System.Reactive.Windows.Threading.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true
msbuild "$scriptPath\facades\System.Reactive.WindowsRuntime\System.Reactive.WindowsRuntime.csproj" /t:pack /p:Configuration=$configuration /p:PackageOutputPath=$artifacts /p:NoPackageAnalysis=true

.\nuget.exe pack "$scriptPath\facades\System.Reactive.Compatibility.nuspec" -Version $packageSemVer -MinClientVersion 2.12 -nopackageanalysis -outputdirectory "$artifacts" 

if($hasSignClientSecret) {
  Write-Host "Signing Packages" -Foreground Green
  $nupgks = ls $artifacts\*React*.nupkg | Select -ExpandProperty FullName

  foreach ($nupkg in $nupgks) {
    Write-Host "Submitting $nupkg for signing"

    dotnet $signClientAppPath 'sign' -c $signClientSettings -i $nupkg -r $env:SignClientUser -s $env:SignClientSecret -n 'Rx.NET' -d 'Reactive Extensions for .NET' -u 'http://reactivex.io/' 

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
$testDirectory = Join-Path $scriptPath "tests\Tests.System.Reactive"

# OpenCover isn't working currently. So run tests on CI and coverage with JetBrains 

$dotnet = "$env:ProgramFiles\dotnet\dotnet.exe"
#.\packages\JetBrains.dotCover.CommandLineTools\tools\dotCover.exe analyse /targetexecutable="$dotnet" /targetworkingdir="$testDirectory" /targetarguments="test -c $configuration --no-build --filter `"SkipCI!=true`"" /Filters="+:module=System.Reactive;+:module=Microsoft.Reactive.Testing;+:module=System.Reactive.Observable.Aliases;-:type=Xunit*" /DisableDefaultFilters /ReturnTargetExitCode /AttributeFilters="System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" /Output="$outputFile" /ReportType=DetailedXML /HideAutoProperties
dotnet test $testDirectory --no-build --no-restore -c "Release" --filter "SkipCI!=true"

if ($LastExitCode -ne 0) { 
	Write-Host "Error with tests" -Foreground Red
	if($isAppVeyor) {
	  $host.SetShouldExit($LastExitCode)
	  exit $LastExitCode
	}  
}

# Either display or publish the results
if ($env:CI -eq 'True')
{
  .\packages\coveralls.io.dotcover\tools\coveralls.net.exe -f -p DotCover "$outputFile"
}
else
{
  .\packages\ReportGenerator\tools\ReportGenerator.exe -reports:"$outputFile" -targetdir:"$outputPath"
  &"$outPutPath/index.htm"
}
