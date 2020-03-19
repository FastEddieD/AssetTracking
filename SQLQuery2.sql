USE [AssetTrackingDB]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[spActionListForStation]
		@DEPTID = 1

SELECT	@return_value as 'Return Value'

GO
