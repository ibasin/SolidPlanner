echo be sure to run this as Adminsitrator
sc.exe create "SolidPlanner Web App" binpath= "%~dp0..\bin\Release\net10.0\WebWM.exe /s"
pause