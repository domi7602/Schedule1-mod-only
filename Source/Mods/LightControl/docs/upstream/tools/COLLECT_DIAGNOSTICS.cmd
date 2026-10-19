@echo off
pushd "%~dp0.."
if errorlevel 1 exit /b 1
powershell.exe -NoProfile -File "%~dp0collect-diagnostics.ps1" -Interactive
set "LightControlExit=%errorlevel%"
popd
echo.
pause
exit /b %LightControlExit%
