CREATE TABLE [dbo].[Station]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] NVARCHAR(50) NULL, 
    [DeptId] INT NULL, 
    [Description] NVARCHAR(50) NULL, 
    [LastScanId] INT NULL, 
    [XPos] INT NULL DEFAULT 0, 
    [YPos] INT NULL DEFAULT 0, 
    [Width] INT NULL DEFAULT 100, 
    [Height] INT NULL DEFAULT 100, 
    [Shape] TEXT NULL  , 
    CONSTRAINT [FK_Station_ToDept] FOREIGN KEY ([DeptId]) REFERENCES [Dept]([Id]), 
    CONSTRAINT [FK_Station_ToScans] FOREIGN KEY ([LastScanId]) REFERENCES [Scans]([Id])
)
