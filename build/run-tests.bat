:: This script finds the tests of the specified type in the repository and run them using nunit-console.
:: It is horrendously slow, and there *will* be a better way to do this.
echo Always remember to compile before running tests this way.
:: Enables use of the !var! syntax.
setlocal enabledelayedexpansion
set fastTestSuffix=Test.Fast.dll
set slowTestSuffix=Test.Slow.dll
set repositoryRoot=%CD%\..\
set nunitConsoleLocation=!repositoryroot!src\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console-x86.exe
set slowTestCategories=Slow
set fastTestCategories=Fast
set teststorun=%1
@echo off

set nunitCommand=!nunitConsoleLocation!
if !teststorun! == "f" (
set suffix=!fastTestSuffix!
set testCategoriesToExclude=!slowTestCategories!
echo Running fast tests...
) else (
set suffix=!slowTestSuffix!
set testCategoriesToExclude=!fastTestCategories!
echo Running slow tests...
)
FOR /F "DELIMS==" %%d in ('DIR "!repositoryRoot!src\" /AD /B') DO (
	set directory=!repositoryRoot!src\%%d\bin\Debug\
	
	for /F "delims==" %%f in ('DIR "!directory!" /B') do (	
    echo %%f|findstr /i !suffix! >nul:
    if not !errorlevel!==1 (
	   echo %%f|findstr /i !suffix!.config >nul:
	   if !errorlevel!==1 (set nunitCommand=!nunitCommand! !directory!%%f)
	   )
    )
)
call %nunitCommand% /nologo /exclude:!testCategoriesToExclude!,WIP
