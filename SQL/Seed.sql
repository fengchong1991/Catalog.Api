CREATE DATABASE CatalogDb
GO

USE CatalogDb
GO 

CREATE TABLE CatalogBrands 
(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Brand nvarchar(50)
)
GO

INSERT INTO CatalogBrands (Brand) VALUES ('BMW')
GO