CREATE PROCEDURE [dbo].[spActionListForStation]
	@DEPTID int
AS
BEGIN
	SELECT ACT.*
	FROM DBO.vwActionList ACT WHERE ACT.DeptId = @DEPTID
	
END
