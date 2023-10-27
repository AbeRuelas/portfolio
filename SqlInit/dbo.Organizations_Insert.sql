ALTER PROC [dbo].[Organizations_Insert]
			@OrganizationTypeId int 
           ,@Name  nvarchar(200)
           ,@Headline nvarchar(200) = null
           ,@Description nvarchar(max) = null
           ,@Logo nvarchar(255) = null
           ,@LocationId int
           ,@Phone nvarchar(50) = null
           ,@SiteUrl nvarchar(255) = null
		       ,@CreatedBy int
		       ,@Id int OUTPUT

AS
/* ---Test Code---

    DECLARE @Id int = 0

   DECLARE	@OrganizationTypeId int = 1
           ,@Name nvarchar(200) = 'Petr'
           ,@Headline nvarchar(200) = 'Monday, Appointment'
           ,@Description nvarchar(max) = 'Labs'
           ,@Logo nvarchar(255) = 'Central'
           ,@LocationId int = 2
           ,@Phone nvarchar(50) = '555-000-1515'
           ,@SiteUrl nvarchar(255) = 'https://www.webmd.com/default.htm'
		       ,@CreatedBy int = 8
	
	EXECUTE dbo.Organizations_Insert
			      @OrganizationTypeId
           ,@Name 
           ,@Headline
           ,@Description
           ,@Logo
           ,@LocationId
           ,@Phone 
           ,@SiteUrl
		       ,@CreatedBy 
		       ,@Id OUTPUT

	Execute dbo.Organizations_Select_ById @Id

*/

BEGIN
		
	INSERT INTO [dbo].[Organizations]
			   ([OrganizationTypeId] 
			   ,[Name]
			   ,[Headline]
			   ,[Description]
			   ,[Logo]
			   ,[LocationId]
			   ,[Phone]
			   ,[SiteUrl]
			   ,[CreatedBy])
		 VALUES
			   (@OrganizationTypeId 
			   ,@Name 
			   ,@Headline
			   ,@Description
			   ,@Logo
			   ,@LocationId
			   ,@Phone
			   ,@SiteUrl
			   ,@CreatedBy)

		SET @Id = SCOPE_IDENTITY()

END
