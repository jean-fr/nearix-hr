# nearix-hr
Instructions

I/ Database - run against MSSQL 
	a/ Create the DB by running the script \DbScripts\DbTable\01_database.sql
	b/ Create the table by running the script \DbScripts\DbTable\02_table.sql
	c/ Create the stored procedures by running the scripts (one after other) in DbScripts\StoredProcedures

II/ Application
	a/ Set the correct MSSQL connection string in the \src\Nearix.HR.Web\Nearix.HR.Web\appsettings.json, in the section AppSettings:ConnectionString
	b/ Set the correct Upload Directory in the \src\Nearix.HR.Web\Nearix.HR.Web\appsettings.json, in the section AppSettings:UploadDirectoy

Finally build and run the application

the Log file can be found in here: Nearix.HR.Web\bin\Debug\netcoreapp3.1\logs

The upload can handle up to 100 MB and can be increased
