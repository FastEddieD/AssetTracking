CREATE VIEW [dbo].[vwActionList]
	AS SELECT 
	[act].[Id], [act].[Code] as ACTION_CODE, [act].[SortCode], [act].[Description], [act].[DeptId],
	[DEPT].[Code] AS DEPT_CODE,
	[DEPT].[PlantId],
	[DEPT].[Description] AS DEPT_DESCRIPTION, 
	[PLANT].[Code] AS PLANT_CODE 
	FROM 
	[dbo].[actionitems] act
	LEFT JOIN Dbo.Dept DEPT ON act.DeptId = DEPT.Id
	LEFT JOIN DBO.Plant PLANT ON DEPt.PlantId = PLANT.ID
