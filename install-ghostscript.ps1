# install - ghostscript.ps1
$ghostscriptUrl = "https://github.com/ArtifexSoftware/ghostpdl-downloads/releases/download/gs950/gs950w64.exe"
$installerPath = "$env:HOME\site\wwwroot\gs950w64.exe"
Invoke - WebRequest - Uri $ghostscriptUrl - OutFile $installerPath
Start - Process - FilePath $installerPath - ArgumentList "/S /D=$env:ProgramFiles\gs" - NoNewWindow - Wait

# Add Ghostscript to the PATH
$envPath = [System.Environment]:: GetEnvironmentVariable('PATH', [System.EnvironmentVariableTarget]:: Machine)
if (-not($envPath - like "*$env:ProgramFiles\gs\gs9.50\bin*")) {
    [System.Environment]:: SetEnvironmentVariable('PATH', "$envPath;$env:ProgramFiles\gs\gs9.50\bin", [System.EnvironmentVariableTarget]:: Machine)
}
