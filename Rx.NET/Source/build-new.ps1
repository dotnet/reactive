$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"
# TODO: if not found, bail out

$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"
$build = "1.0.0-rc1-update1"

dnvm use $build -r clr -arch x64 -p

$runtimeDir = Join-Path $env:USERPROFILE "\.dnx\runtimes\dnx-clr-win-x64.$build"

dnu restore . --quiet

. $msbuildExe .\Rx-New.sln /m /p:Configuration=Release /p:RuntimeToolingDirectory=$runtimeDir /v:minimal
