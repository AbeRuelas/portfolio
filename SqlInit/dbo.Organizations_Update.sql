ALTER proc [dbo].[Organizations_Update]
				 @OrganizationTypeId int
				,@Name nvarchar(200)
				,@Headline nvarchar(200) = null
				,@Description varchar(max) = null
				,@Logo nvarchar(255) = null
				,@LocationId int
				,@Phone nvarchar(50) = null
				,@SiteUrl nvarchar(255) = null
				,@CreatedBy int
				,@Id int

AS
/*-----Test Code----

	  DECLARE  @OrganizationTypeId int = 4
			  ,@Name nvarchar(200) = 'Kaiser'
			  ,@Headline nvarchar(200) = 'Driven by a commitment to overall health, our mission statement guides our work.'
			  ,@Description varchar(max) = 'Kaiser Permanente exists to provide high-quality, affordable health care services and to improve the health of our members and the communities we serve.'
			  ,@Logo nvarchar(255) = 'https://about.kaiserpermanente.org/content/dam/internet/kp/comms/import/uploads/2012/02/Current-2008-KPcen_307.jpg'
			  ,@LocationId int = 2
			  ,@Phone nvarchar(50) = '404-364-7000'
			  ,@SiteUrl nvarchar(255) = 'https://healthy.kaiserpermanente.org/southern-california/front-door'
			  ,@CreatedBy int = 8
			  ,@Id int = 4

	EXECUTE [dbo].[Organizations_Update]
			  @OrganizationTypeId 
			  ,@Name 
			  ,@Headline  
			  ,@Description  
			  ,@Logo
			  ,@LocationId
			  ,@Phone  
			  ,@SiteUrl  
			  ,@CreatedBy  	
			  ,@Id

EXECUTE dbo.Organizations_Select_ById @Id
*/

BEGIN

	  DECLARE @DateNow datetime2 = GETUTCDATE()
	   
		UPDATE [dbo].[Organizations]
		   SET [OrganizationTypeId] = @OrganizationTypeId
			  ,[Name] = @Name 
			  ,[Headline] = @Headline 
			  ,[Description] = @Description 
			  ,[Logo] = @Logo 
			  ,[LocationId] = @LocationId
			  ,[Phone] = @Phone
			  ,[SiteUrl] = @SiteUrl
			  ,[CreatedBy] = @CreatedBy
			  ,[DateModified] = @DateNow

        WHERE Id = @Id

 END
