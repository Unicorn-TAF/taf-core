@echo off
set PKG=Unicorn.Taf.Core
set PROJ_PATH=%cd%\..\src\%PKG%\%PKG%
set VERSION=%1
set OUT_DIR=%2

dotnet pack %PROJ_PATH%.csproj -o %OUT_DIR% -c Release -p:NuspecFile=%PROJ_PATH%.nuspec -p:Version=%VERSION%