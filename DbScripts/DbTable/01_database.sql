USE master;
GO
IF DB_ID (N'NearixHR') IS NULL
 BEGIN
    CREATE DATABASE [NearixHR]
END