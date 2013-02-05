@REM msbuild BuildAll.proj /p:BuildSetup=1 /p:BuildNumber=2.0.30116.0 /p:RxRelease=EXPERIMENTAL

@REM I've had good success with /t:Rebuild.  But will omit it for speed.
@REM msbuild BuildAll.proj /t:Rebuild /p:BuildSetup=1 /p:SignedBuild=1 /p:BuildNumber=2.1.30201.0 /p:RxRelease=RTM
msbuild BuildAll.proj /p:BuildSetup=1 /p:SignedBuild=1 /p:BuildNumber=2.1.30201.0 /p:RxRelease=RTM
