$repositoryRoot = split-path $MyInvocation.MyCommand.Definition

Write-Host "Building Rx.NET" -ForegroundColor Green
.\Rx.NET\Source\build-new

Write-Host "Building Ix.NET" -ForegroundColor Green
.\Ix.NET\Source\build-new