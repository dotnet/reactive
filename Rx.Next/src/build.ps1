$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

Write-Host "Building $scriptPath\Rx.Next.sln" -Foreground Green
msbuild "$scriptPath\Rx.Next.sln" /t:build /m /p:Configuration=$configuration 
if ($LastExitCode -ne 0) { 
        Write-Host "Error with build" -Foreground Red
        if($isAppVeyor) {
          $host.SetShouldExit($LastExitCode)
          exit $LastExitCode
        }  
}
