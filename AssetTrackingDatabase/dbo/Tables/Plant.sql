CREATE TABLE [dbo].[Plant]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] NVARCHAR(50) NULL, 
    [CompanyName] NVARCHAR(50) NULL, 
    [Address] NVARCHAR(50) NULL, 
    [City] NVARCHAR(50) NULL, 
    [State] NVARCHAR(50) NULL, 
    [ZipCode] NVARCHAR(50) NULL, 
    [Telephone] NVARCHAR(50) NULL
)
