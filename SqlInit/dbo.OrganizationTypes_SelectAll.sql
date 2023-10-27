ALTER PROC [dbo].[OrganizationTypes_SelectAll]

AS
/*-----Test Code----
EXECUTE [dbo].[OrganizationTypes_SelectAll]
*/
  
BEGIN

	SELECT [Id]
		  ,[Name]
	FROM [dbo].[OrganizationTypes]

END
