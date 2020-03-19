CREATE VIEW [dbo].[vwStationList]
	AS SELECT 
	[sta].[Id], 
	[sta].[Code] AS STATION, 
	[sta].[DeptId], 
	[sta].[Description] AS STATION_DESCRIPTION, 
	[sta].[XPos],
	[sta].[YPos],
	[sta].[Height],
	[sta].[Width],
	[sta].[Shape],
	[sta].[LastScanId],
	[DEPT].[Code], 
	[DEPT].[PlantId], 
	[DEPT].[Description],
	[SCANS].[ScanTime], 
	[SCANS].[StationId], 
	[SCANS].[Item], 
	[SCANS].[OperatorId], 
	[SCANS].[ActionItemId], 
	[SCANS].[Processed]
	FROM 
	[dbo].[Station] STA
	LEFT JOIN Dbo.Dept DEPT ON sta.DeptId = DEPT.Id
	LEFT JOIN DBO.Scans SCANS ON STA.LastScanId = SCANS.Id