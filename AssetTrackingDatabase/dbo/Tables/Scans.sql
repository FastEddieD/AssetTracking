CREATE TABLE [dbo].[Scans]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ScanTime] DATETIME NULL, 
    [StationId] INT NULL, 
    [Item] NVARCHAR(50) NULL, 
    [OperatorId] INT NULL, 
    [ActionItemId] INT NULL, 
    [Processed] BIT NULL DEFAULT 0, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL, 
	[ElapsedTime] INT DEFAULT 0,
    CONSTRAINT [FK_Scans_ToStation] FOREIGN KEY ([StationId]) REFERENCES [Station]([Id]), 
    CONSTRAINT [FK_Scans_ToOperator] FOREIGN KEY ([OperatorId]) REFERENCES [Operator]([Id]), 
    CONSTRAINT [FK_Scans_ToActionItem] FOREIGN KEY ([ActionItemId]) REFERENCES [ActionItems]([Id])
)
