cd /d %0\.. 
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S _ReSharper.*') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /S *.csproj.user') DO del "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /A /S *.suo') DO del "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /S *.ReSharper.user') DO del "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /S TestResult.xml') DO del "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /S *.sln.cache') DO del "%%G"