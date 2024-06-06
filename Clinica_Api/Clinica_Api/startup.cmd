@echo off
powershell -ExecutionPolicy Bypass -File "%HOME%\site\wwwroot\install-ghostscript.ps1"
dotnet %HOME%\site\wwwroot\Clinica_Api.dll
