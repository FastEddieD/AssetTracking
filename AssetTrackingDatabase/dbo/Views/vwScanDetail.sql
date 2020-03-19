CREATE VIEW [dbo].[vwScanDetail]
	AS 
	SELECT 
	[scan].[Id], 
	[scan].[ScanTime], 
	[scan].[StationId],
	[scan].[Item], 
	[scan].[OperatorId],
	[scan].[ActionItemId], 
	[scan].[Processed],
	[scan].[ElapsedTime],
	[sta].[Code] as Station, 
	[sta].[DeptId], 
	[sta].[Description] as Dept,
	[sta].[LastScanId], 
	[a].[Code], 
	[a].[SortCode], 
	[a].[Description], 
	[o].[Initials] as Oper , 
	[o].[FirstName], 
	[o].[LastName], 
	[o].[PIN]
	FROM dbo.[Scans] scan
	left join dbo.station sta on scan.StationId = sta.Id
	left join dbo.ActionItems a on scan.ActionItemId = a.id
	left join dbo.Operator o on scan.OperatorId = o.id
