ALTER PROC [dbo].[Organizations_Search_Paginated]
			 @PageIndex int
			,@PageSize int
			,@Query nvarchar(200)

AS
/*----Test Code----

	DECLARE @PageIndex int = 0
			,@PageSize int = 15
			,@Query nvarchar(200) = 'verdugo'
			
	EXECUTE [dbo].[Organizations_Search_Paginated]
			 @PageIndex
			,@PageSize
			,@Query
			
*/
BEGIN

DECLARE @offset int = @PageIndex * @PageSize

	SELECT o.[Id]
		  ,ot.[Id] as OrganizationTypeId
		  ,ot.[Name] as OrganizationTypeName
		  ,o.[Name]
		  ,o.[Headline]
		  ,o.[Description]
		  ,o.[Logo]
		  ,l.[Id] as LocationId
		  ,l.[LocationTypeId]
		  ,lt.[Name] as LocationName
		  ,l.[LineOne]
		  ,l.[LineTwo]
	    ,l.[City]
	    ,l.[Zip]
		  ,l.[StateId]
		  ,s.[Name] as StateName
		  ,l.[Latitude]
	    ,l.[Longitude]
		  ,l.[DateCreated]
		  ,l.[DateModified]
		  ,o.[Phone]
		  ,o.[SiteUrl]
		  ,o.[DateCreated]
		  ,o.[DateModified]
		  ,CreatedBy = dbo.fn_GetBaseUserJSON(o.CreatedBy)
		  ,[TotalCount] = COUNT(1) OVER()

	  FROM [dbo].[Organizations] as o 
	  inner join [dbo].[Users] as u on o.CreatedBy = u.Id
	  inner join dbo.OrganizationTypes as ot
	  on ot.Id = o.OrganizationTypeId
	  inner join dbo.Locations as l
	  on l.Id = o.LocationId
	  inner join dbo.LocationTypes as lt
	  on lt.Id = l.LocationTypeId
	  inner join dbo.States as s
    on s.Id = l.StateId

	  WHERE (o.[Name] LIKE '%' + @Query + '%') OR
		(o.[Description] LIKE '%' + @Query + '%') 

	  ORDER BY o.Id

	  OFFSET @offset Rows
	  FETCH NEXT @PageSize Rows ONLY

END
