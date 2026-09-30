echo Don't forget to uncomment return on line 39 in WebWM Program.cs

dotnet tool update --global dotnet-ef

dotnet ef migrations add %MIGRATION_NAME_HERE% --project ../../Domain --startup-project ../../WebWM

pause