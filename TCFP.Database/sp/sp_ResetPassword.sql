IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE ROUTINE_NAME = 'sp_ResetPassword' 
             AND ROUTINE_SCHEMA = 'dbo' 
             AND ROUTINE_TYPE = 'PROCEDURE')
 EXEC ('DROP PROCEDURE dbo.sp_ResetPassword')
 GO

 Create Procedure sp_ResetPassword
	@tokenID nchar(36),
	@password nvarchar(255)
 AS
 BEGIN
	declare @email nvarchar(200)
	select @email=Email from ClientToken where @tokenID=TokenID and UsedOn is null

	if @email is not null
	BEGIN
		update ClientToken set UsedOn=GETDATE() where TokenID = @tokenID

		update ClientProfile set Password=PWDENCRYPT(@password) where Email=@email
	END
 END