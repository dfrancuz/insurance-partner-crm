USE master
GO

IF not exists (SELECT * FROM sys.databases WHERE name = 'InsurancePartner')
BEGIN
    CREATE DATABASE InsurancePartner;
    PRINT 'Database [InsurancePartner] created.';
END;
ELSE
BEGIN
    PRINT 'Database [InsurancePartner] already exists.';
END;
GO
