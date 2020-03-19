USE [AssetTrackingDB]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[spGetScansByDate]
		@ScanDate = N'1/1/2019'

SELECT	@return_value as 'Return Value'

GO
