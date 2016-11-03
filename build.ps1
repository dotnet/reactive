$repositoryRoot = split-path $MyInvocation.MyCommand.Definition

Write-Host "Building Rx.NET" -ForegroundColor Green
.\Rx.NET\Source\build-new

Write-Host "Building Ix.NET" -ForegroundColor Green
.\Ix.NET\Source\build-new

if ($env:CI -ne 'True') {
  Write-Host "Reverting AssemblyInfo's" -Foreground Green
  gci $scriptPath -re -in AssemblyInfo.cs | %{ git checkout $_ } 
}