IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE ROUTINE_NAME = 'sp_ForgetPassword' 
             AND ROUTINE_SCHEMA = 'dbo' 
             AND ROUTINE_TYPE = 'PROCEDURE')
 EXEC ('DROP PROCEDURE dbo.sp_ForgetPassword')
 GO

 Create Procedure sp_ForgetPassword
	@email nvarchar(200),
	@tokenID nchar(36)
 AS
 BEGIN
	declare @lifeTime int
	select @lifeTime=cast(ParameterValue as int) from SystemParameter where ParameterCode='TokenLife'

	if exists (select 1 from ClientProfile where Email=@email)
	begin
		insert into ClientToken (TokenID, Email, Type, CreatedOn, ExpiredOn, UsedOn)
		values (@tokenID,@email,'F',GETDATE(),DATEADD(MI,@lifeTime,GETDATE()),null)

		select 1
	end
	else
		select 0
 END