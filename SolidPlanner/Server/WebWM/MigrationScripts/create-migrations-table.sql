-- Run this script when you are ready to move from seeding data (in dev) to migrating data (in production)

CREATE TABLE [__EFMigrationsHistory] ( [MigrationId] nvarchar(150) NOT NULL, [ProductVersion] nvarchar(32) NOT NULL, PRIMARY KEY ([MigrationId]) );
GO