CREATE TABLE [dbo].[Operator]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Initials] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [PIN] NVARCHAR(50) NULL
)
