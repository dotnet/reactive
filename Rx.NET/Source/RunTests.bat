@echo off

set _use_code_coverage=0
for %%a in (%*) do if "%%a"=="-coverage" set _use_code_coverage=1

set _release_build=0
for %%a in (%*) do if "%%a"=="-release" set _release_build=1

set _build_rx_tmp=%temp%\Rx
rd /s /q "%_build_rx_tmp%"
mkdir "%_build_rx_tmp%"

set _runtest_list=Tests.System.Reactive.dll

set _test_coverage_targets=System.Reactive.Core.dll System.Reactive.Linq.dll System.Reactive.PlatformServices.dll

if %_release_build%==1 (
  call msbuild /p:Configuration=Release45 /p:RunCodeAnalysis=false /p:OutputPath="%_build_rx_tmp%" Rx.sln
) else (
  call msbuild /p:Configuration=Debug45 /p:RunCodeAnalysis=false /p:OutputPath="%_build_rx_tmp%" Rx.sln
)

set _src_rx=%cd%

pushd "%_build_rx_tmp%"

set _original_path=%path%
path %path%;c:\Program Files (x86)\Microsoft Visual Studio 14.0\Team Tools\Performance Tools;

if %_use_code_coverage%==1 (
  for %%a in (%_test_coverage_targets%) do call vsinstr -coverage %%a
  start vsperfmon -coverage -output:%cd%\runtests.coverage
)

call mstest /testcontainer:%_runtest_list%

if %_use_code_coverage%==1 (
  vsperfcmd -shutdown
  copy %cd%\runtests.coverage "%_src_rx%\"
)

path %_original_path%

popd