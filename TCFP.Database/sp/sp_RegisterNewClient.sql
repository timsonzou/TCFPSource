IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE ROUTINE_NAME = 'sp_RegisterNewClient' 
             AND ROUTINE_SCHEMA = 'dbo' 
             AND ROUTINE_TYPE = 'PROCEDURE')
 EXEC ('DROP PROCEDURE dbo.sp_RegisterNewClient')
 GO

 Create Procedure sp_RegisterNewClient
	@email nvarchar(200),
	@name nvarchar(200),
	@tokenID nchar(36)
 AS
 BEGIN
	declare @lifeTime int
	select @lifeTime=cast(ParameterValue as int) from SystemParameter where ParameterCode='TokenLife'

	if not exists (select 1 from ClientProfile where Email=@email)
		insert into ClientProfile (Email, Name, Password, Status, CreatedOn, UpdatedOn)
		values (@email,@name,cast('-1' as varbinary(max)),'R',GETDATE(),GETDATE())

	update ClientToken set ExpiredOn=GETDATE() where Email=@email and Type='R' and UsedOn is null

	insert into ClientToken (TokenID, Email, Type, CreatedOn, ExpiredOn, UsedOn)
	values (@tokenID,@email,'R',GETDATE(),DATEADD(MI,@lifeTime,GETDATE()),null)
 END