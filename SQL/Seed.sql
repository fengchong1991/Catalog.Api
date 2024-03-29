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
	PictureFileName NVARCHAR(100),
	CatalogTypeId INT NOT NULL FOREIGN KEY REFERENCES CatalogTypes(Id),
	CatalogBrandId INT NOT NULL FOREIGN KEY REFERENCES CatalogBrands(Id),
	AvailableStock INT,
	RestockThreshold INT,
	MaxStockThreshold INT,
	OnReorder BIT
)

INSERT INTO CatalogBrands (Brand) VALUES ('BMW')
GO

INSERT INTO CatalogTypes (Type) VALUES ('CAR')
GO

INSERT INTO CatalogItems (Name, Description, Price, CatalogTypeId, CatalogBrandId, AvailableStock, RestockThreshold, MaxStockThreshold, OnReorder) VALUES ('M3', 'Performance Car', '100000', '1', '1', '10', '5', '10', 1)
GO


/*
Create table for integration event log
*/
CREATE TABLE IntegrationEventLog (
	EventId UNIQUEIDENTIFIER PRIMARY KEY,
	EventTypeName NVARCHAR(100),
	State INT,
	TimesSent INT,
	CreationTime DATETIME,
	Content NVARCHAR(MAX),
	TransactionId NVARCHAR(200)
)
