Param(
  [string]$build = "1.0.0-rc1-update1",
  [string]$runtime = "clr",
  [string]$architecture = "x64"
)

Write-Host "Setting DNVM version" -Foreground Green
dnvm use $build -r $runtime -arch $architecture -p

Write-Host "Building Rx.NET" -ForegroundColor Green
.\Rx.NET\Source\build-new

Write-Host "Building Ix.NET" -ForegroundColor Green
.\Ix.NET\Source\build-new