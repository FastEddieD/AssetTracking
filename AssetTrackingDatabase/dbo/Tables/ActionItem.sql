CREATE TABLE [dbo].[ActionItems]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] NVARCHAR(50) NULL, 
    [SortCode] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(50) NULL, 
    [DeptId] INT NULL, 
    [ItemRequired] BIT NULL DEFAULT 1, 
    CONSTRAINT [FK_ActionItems_Dept] FOREIGN KEY ([DeptId]) REFERENCES [Dept]([Id])
)
