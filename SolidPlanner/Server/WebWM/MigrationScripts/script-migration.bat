echo Don't forget to uncomment return on line 39 in WebWM Program.cs

dotnet tool update --global dotnet-ef

dotnet ef migrations script %FROM_MIGRATION_NAME_HERE% %TO_MIGRATION_NAME_HERE% --project ../../Domain --startup-project ../../WebWM --output script.sql

pause