USE InsurancePartner
GO

CREATE TABLE PartnerTypes (
    PartnerTypeId INT PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL
);

CREATE TABLE Partners (
    PartnerId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(255) NOT NULL CHECK (LEN(FirstName) >= 2),
    LastName NVARCHAR(255) NOT NULL CHECK (LEN(LastName) >= 2),
    Address NVARCHAR(500) NULL,
    PartnerNumber CHAR(20) NOT NULL CHECK (LEN(PartnerNumber) = 20 AND ISNUMERIC(PartnerNumber) = 1),
    CroatianPIN NVARCHAR(11) NULL,
    PartnerTypeId INT NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreateByUser NVARCHAR(255) NOT NULL CHECK (CreateByUser LIKE '%_@_%.__%'),
    IsForeign BIT NOT NULL,
    ExternalCode NVARCHAR(20) NOT NULL CHECK (LEN(ExternalCode) >= 10 AND LEN(ExternalCode) <= 20),
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F', 'N'))
);

CREATE TABLE Policies (
    PolicyId INT IDENTITY PRIMARY KEY,
    PartnerId INT NOT NULL,
    PolicyNumber NVARCHAR(15) NOT NULL CHECK (LEN(PolicyNumber) >= 10),
    Amount DECIMAL(14,2) NOT NULL,
    CreatedAtUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
