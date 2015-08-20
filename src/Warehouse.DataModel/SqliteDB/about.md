## EF6 with SQLite and code first
SQLite doesn't support auto create database
http://stackoverflow.com/questions/22174212/entity-framework-6-with-sqlite-3-code-first-wont-create-tables

>Unfortunately, the EF6 provider implementation in System.Data.SQLite.EF6 doesn't support creating tables. 
I downloaded the SQLite source code to have a look but couldn't find anything for creating tables and for migrations. 
The EF6 provider is basically the same as their Linq implementation so it's all aimed at querying the database rather than modifying it.
I currently do all of my work with SQL Server and generate sql scripts for SQLite using the SQL Server Compact & SQLite Toolbox. 
The scripts can then be run using an SQLiteCommand to simulate migrations.

###Update:
In EF7 support for SQL server compact has been dropped and a new provider for SQLite is being developed by the EF team. 
The provider will use Microsoft's managed SQLite wrapper project, Microsoft.Data.SQLite rather than the System.Data.SQLite project. 
This will also allow for using EF7 on iOS, Android, Windows Phone / Mobile, Linux, Mac etc. as Microsoft's wrapper is being developed as a portable library.
It's all still in beta but you can get nuget packages from the ASP.Net development feeds at MyGet (dev, master, release) if you wish to have a look. 
Look for the EntityFramework.SQLite package.