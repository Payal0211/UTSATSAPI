CREATE PROCEDURE [Sproc_Delete_Company_CultureandPerksDetails]
	@CompanyId		  bigint = 0,
	@CultureID		  bigint = 0,
	@CreatedByID	  bigint = 0,
	@AppActionDoneBy  int = 0
AS
BEGIN

	IF EXISTS (SELECT TOP 1 ID FROM gen_Company_CultureandPerksDetails WITH(NOLOCK) WHERE ID = @CultureID AND CompanyID = @CompanyId)
	BEGIN
		DELETE FROM gen_Company_CultureandPerksDetails WHERE ID = @CultureID AND CompanyID = @CompanyId
	END
	
END
GO
