USE [AssetTrackingDB]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[spGetScansByDate]
		@ScanDate = N'5/6/19'

SELECT	@return_value as 'Return Value'

GO
