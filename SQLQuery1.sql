USE [AssetTrackingDB]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[spGetScansByItem]
		@Item = N'123456'

SELECT	@return_value as 'Return Value'

GO
