IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE ROUTINE_NAME = 'sp_GetUserToken' 
             AND ROUTINE_SCHEMA = 'dbo' 
             AND ROUTINE_TYPE = 'PROCEDURE')
 EXEC ('DROP PROCEDURE dbo.sp_GetUserToken')
 GO

 Create Procedure sp_GetUserToken
	@tokenID nchar(36)
 AS
 BEGIN
	select * from [ClientToken] where [TokenID]=@tokenID
 END