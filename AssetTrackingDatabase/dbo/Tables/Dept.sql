CREATE TABLE [dbo].[Dept]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] NVARCHAR(50) NULL, 
    [PlantId] INT NULL, 
    [Description] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_Dept_Plant] FOREIGN KEY ([PlantId]) REFERENCES [Plant]([Id])
)
