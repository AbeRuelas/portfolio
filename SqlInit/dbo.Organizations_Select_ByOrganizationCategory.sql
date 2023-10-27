ALTER PROC [dbo].[Organizations_Select_ByOrganizationCategory]
			@PageIndex int
			,@PageSize int
			,@Id int

AS
/*----Test Code----

	DECLARE @PageIndex int = 0
			,@PageSize int = 15
			,@Id int = 3

	EXECUTE [dbo].[Organizations_Select_ByOrganizationCategory]
			@PageIndex
			,@PageSize
			,@Id

*/
BEGIN

DECLARE @offset int = @PageIndex * @PageSize

	SELECT o.[Id]
		  ,ot.[Id] as OrganizationTypeId
		  ,ot.[Name] as Category
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
		  ,CreatedBy = dbo.fn_GetUserJSON(o.CreatedBy)
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

	  WHERE ot.Id = @Id
	  
	  ORDER BY o.Id

	  OFFSET @offset Rows
	  FETCH NEXT @PageSize Rows ONLY

END
