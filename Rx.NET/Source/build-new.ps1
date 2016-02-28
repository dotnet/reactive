$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"
# TODO: if not found, bail out

$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"
$build = "1.0.0-rc1-update1"

Write-Host "Setting DNVM version" -Foreground Green
dnvm use $build -r clr -arch x64 -p

$runtimeDir = Join-Path $env:USERPROFILE "\.dnx\runtimes\dnx-clr-win-x64.$build"

Write-Host "Restoring packages" -Foreground Green
dnu restore . --quiet | out-null

Write-Host "Building projects" -Foreground Green
. $msbuildExe .\Rx-New.sln /m /p:Configuration=Release /p:RuntimeToolingDirectory=$runtimeDir /v:q

Write-Host "Running tests" -Foreground Green
dnx -p "Tests.System.Reactive" test
