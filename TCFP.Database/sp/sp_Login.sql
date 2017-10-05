IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE ROUTINE_NAME = 'sp_Login' 
             AND ROUTINE_SCHEMA = 'dbo' 
             AND ROUTINE_TYPE = 'PROCEDURE')
 EXEC ('DROP PROCEDURE dbo.sp_Login')
 GO

 Create Procedure sp_Login
	@email nvarchar(200),
	@password nvarchar(256)
 AS
 BEGIN
	select PWDCOMPARE(@password,Password) from ClientProfile where Email=@email
 END