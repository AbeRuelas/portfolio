ALTER PROC [dbo].[OrganizationMembers_Delete_ById]
				@Id INT
AS

/* ----TEST CODE----
Declare @Id int = 2

EXEC [dbo].[OrganizationMembers_Delete_ById]
			@Id
*/

BEGIN

	DELETE FROM [dbo].[OrganizationMembers]
	WHERE  Id = @Id

END
