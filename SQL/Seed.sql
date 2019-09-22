/*
Create database for catalog api
*/
CREATE DATABASE CatalogDb
GO

USE CatalogDb
GO 

CREATE TABLE CatalogBrands 
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Brand nvarchar(50)
)
GO


CREATE TABLE CatalogTypes
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Type nvarchar(50)
)
GO


CREATE TABLE CatalogItems
(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(100),
	Description NVARCHAR(1000),
	Price DECIMAL(19,4),
	PictureUri NVARCHAR(100),
	CatalogTypeId INT NOT NULL FOREIGN KEY REFERENCES CatalogType(Id),
	CatalogBrandId INT NOT NULL FOREIGN KEY REFERENCES CatalogBrands(Id),
	AvailableStock INT,
	RestockThreshold INT,
	MaxStockThreshold INT,
	OnReorder BIT
)

INSERT INTO CatalogBrands (Brand) VALUES ('BMW')
GO

/*
Create database for integration event log
*/
CREATE DATABASE IntegrationEventLog
GO

USE IntegrationEventLog
GO

CREATE TABLE IntegrationEventLog (
	EventId UNIQUEIDENTIFIER PRIMARY KEY,
	EventTypeName NVARCHAR(100),
	State INT,
	TimesSent INT,
	CreationTime DATETIME,
	Content NVARCHAR(MAX),
	TranscationId NVARCHAR(200)
)
