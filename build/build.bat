@echo off
:: NuGet.exe must be in the path for the nuget functionality to work here.
cd /d %0\.. 

setlocal enabledelayedexpansion
set solutionAndMainProjectName=NFeature
set msBuildLocation=%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

title !solutionAndMainProjectName!

:: Accept command line parameter for non-interactive mode.
if "%1" == "" goto loop
set task= "%1"
set interactive= "false"
goto switch

:loop
set interactive= "true"
set /p task= !solutionAndMainProjectName! build script usage: (b)uild(d)ebug / (b)uild(s)taging / (b)uild(r)elease  / (c)lean / (f)ast (t)ests / (s)low (t)ests / (n)uget (pack)?:

set task= "%task%"

:switch
if %task% == "bd" goto builddebug
if %task% == "br" goto buildrelease
if %task% == "bs" goto buildstaging
if %task% == "c" goto clean
if %task% == "ft" goto fasttests
if %task% == "st" goto slowtests
if %task% == "npack" goto nugetpack
if %task% == "npush" goto nugetpush

:resume
echo.
echo Completed at %date% %time%
echo.
if %interactive% == "true" goto loop
goto done

:builddebug
!msBuildLocation! /m:8 /verbosity:q /p:Configuration=Debug "%CD%\..\src\!solutionAndMainProjectName!.sln"
goto resume

:buildrelease
!msBuildLocation! /m:8 /p:Configuration=Release "%CD%\..\src\!solutionAndMainProjectName!.sln"
goto resume

:buildstaging
!msBuildLocation! /m:8 /p:Configuration=Staging "%CD%\..\src\!solutionAndMainProjectName!.sln"
goto resume

:nugetpack
cd %CD%\..
nuget pack %CD%\src\!solutionAndMainProjectName!\!solutionAndMainProjectName!.csproj -Prop Configuration=Release -Symbols
cd /d %0\.. 
goto resume

:clean
call %CD%\..\src\clean.bat
::return working directory to the location of this script
cd /d %0\.. 
goto resume

:fasttests
call %CD%\run-tests.bat "f"
goto resume

:slowtests
call %CD%\run-tests.bat "s"
goto resume

:done