IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmployeesDB')
BEGIN
	CREATE DATABASE EmployeesDB;
END
GO

USE EmployeesDB;
GO

-- CREATE DOCUMENT TYPE TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'DocumentType' and xtype = 'U')
BEGIN
	CREATE TABLE DocumentType (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(50) NOT NULL,
		Description NVARCHAR(100)
	);
END
GO

-- CREATE COUNTRY TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'Country' and xtype = 'U')
BEGIN
	CREATE TABLE Country (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(100) NOT NULL,
		CountryCode NVARCHAR(5) NOT NULL,
		CONSTRAINT UQ_Country_Code UNIQUE (CountryCode)
	);
END
GO


-- CREATE PROVINCE TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'Province' and xtype = 'U')
BEGIN
	CREATE TABLE Province (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(100) NOT NULL,
		CountryId INT NOT NULL,
		CONSTRAINT FK_Province_Country FOREIGN KEY (CountryId) REFERENCES Country(Id)
	);
END
GO

-- CREATE CITY TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'City' and xtype = 'U')
BEGIN
	CREATE TABLE City (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(100) NOT NULL,
		ProvinceId INT NOT NULL,
		CONSTRAINT FK_City_Province FOREIGN KEY (ProvinceId) REFERENCES Province(Id)
	);
END
GO

-- CREATE DISTRICT TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'District' and xtype = 'U')
BEGIN
	CREATE TABLE District (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Name NVARCHAR(100) NOT NULL,
		CityId INT NOT NULL,
		CONSTRAINT FK_District_City FOREIGN KEY (CityId) REFERENCES City(Id)
	);
END
GO

-- CREATE EMPLOYEE TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'Employee' and xtype = 'U')
BEGIN
	CREATE TABLE Employee ( 
		Id INT PRIMARY KEY IDENTITY(1,1),
		DocumentTypeId INT NOT NULL,
		DocumentId NVARCHAR(50) NOT NULL,
		Name NVARCHAR(100) NOT NULL,
		LastName NVARCHAR(100) NOT NULL,
		BirthDay DATE NOT NULL,
		Email NVARCHAR(100) NOT NULL,
		JoinedDate DATETIME NOT NULL DEFAULT GETDATE(),
		CostPerHour DECIMAL(10,2) NOT NULL,
		IBAN NVARCHAR(50) NULL,
		CountryId INT NOT NULL,
		ProvinceId INT NOT NULL,
		CityId INT NOT NULL,
		DistrictId INT NOT NULL,
		StreetAddress NVARCHAR(200),
		PhoneNumber NVARCHAR(20),
		PhonePrefix NVARCHAR(5),
		CONSTRAINT FK_Employee_DocumentType FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id),
		CONSTRAINT FK_Employee_Country FOREIGN KEY (CountryId) REFERENCES Country(Id),
		CONSTRAINT FK_Employee_Province FOREIGN KEY (ProvinceId) REFERENCES Province(Id),
		CONSTRAINT FK_Employee_City FOREIGN KEY (CityId) REFERENCES City(Id),
		CONSTRAINT FK_Employee_District FOREIGN KEY (DistrictId) REFERENCES District(Id),
		CONSTRAINT UQ_Employee_Email UNIQUE (Email),
		CONSTRAINT UQ_Employee_Document UNIQUE (DocumentTypeId, DocumentId),
		CONSTRAINT CK_Employee_IBAN CHECK (IBAN IS NULL OR dbo.IsValidIBAN(IBAN) = 1),
		CONSTRAINT CK_Employee_CostPerHour CHECK (CostPerHour > 0),
		CONSTRAINT CK_Employee_BirthDay CHECK (BirthDay <= GETDATE())
	);
END
GO

-- CREATE EMPLOYEE IMAGES TABLE
IF NOT EXISTS (SELECT * FROM sys.sysobjects WHERE name = 'EmployeeImage' and xtype = 'U')
BEGIN
	CREATE TABLE EmployeeImage (
		Id INT PRIMARY KEY IDENTITY(1,1),
		EmployeeId INT NOT NULL,
		ImagePath NVARCHAR(255) NOT NULL,
		ImageType NVARCHAR(50) NOT NULL,
		FileName NVARCHAR(100) NOT NULL,
		ContentType NVARCHAR(50) NOT NULL,
		FileSize INT NOT NULL,
		UploadDate DATETIME NOT NULL DEFAULT GETDATE(),
		Description NVARCHAR(200) NULL,
		IsProfilePicture BIT NOT NULL DEFAULT 0,
		CONSTRAINT FK_EmployeeImage_Employee FOREIGN KEY (EmployeeId) REFERENCES Employee(Id) ON DELETE CASCADE,
		CONSTRAINT CK_EmployeeImage_FileSize CHECK (FileSize > 0)
	);
END
GO

-- INDICES PARA MEJORAR LAS CONSULTAS
CREATE INDEX IX_Employee_Name_LastName ON Employee(Name, LastName);
CREATE INDEX IX_Employee_Email ON Employee(Email);
CREATE INDEX IX_Employee_DocumentId ON Employee(DocumentId);
CREATE INDEX IX_Employee_JoinedDate ON Employee(JoinedDate);
CREATE INDEX IX_Province_CountryId ON Province(CountryId);
CREATE INDEX IX_City_ProvinceId ON City(ProvinceId);
CREATE INDEX IX_District_CityId ON District(CityId);
CREATE INDEX IX_EmployeeImage_EmployeeId ON EmployeeImage(EmployeeId);
CREATE INDEX IX_EmployeeImage_IsProfilePicture ON EmployeeImage(EmployeeId, IsProfilePicture) WHERE IsProfilePicture = 1;
GO

-- VISTAS PARA QUE NOS MUESTRE CADA TRABAJADOR CON SU FOTO DE PERFIL
CREATE VIEW vw_EmployeeWithProfilePicture
AS
SELECT 
    e.*,
    img.ImagePath AS ProfilePhotoPath,
    img.FileName AS ProfilePhotoFileName
FROM 
    Employee e
LEFT JOIN 
    EmployeeImage img ON e.Id = img.EmployeeId AND img.IsProfilePicture = 1;
GO

-- FUNCION PARA VALIDAR EL IBAN
CREATE FUNCTION dbo.IsValidIBAN(@IBAN NVARCHAR(50))
RETURNS BIT
AS
BEGIN

    IF LEN(@IBAN) < 15 OR LEN(@IBAN) > 34
        RETURN 0;
    

    IF PATINDEX('[A-Z][A-Z][0-9]%', @IBAN) != 1
        RETURN 0;
    
    RETURN 1;
END