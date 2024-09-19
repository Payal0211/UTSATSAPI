ALTER PROCEDURE [dbo].[Sproc_Update_CultureDetail_UnicodeValues]
	@CompanyID				bigint = 0,
	@CultureDetail			NVARCHAR(MAX) = null,
	@LoggedInUserId			bigint = 0
AS
BEGIN

			

		


			IF (@CompanyID > 0 and len(@CultureDetail) > 0)
				BEGIN
					IF EXISTS(select 1 from gen_Company_Details WITH(NOLOCK) where CompanyID = @CompanyID)
						BEGIN
							Update gen_Company_Details
							SET	   Culture = Isnull(@CultureDetail,Culture),
									ModifiedByID = @LoggedInUserId,
									ModifiedDateTime = getdate()
							where  CompanyId = @CompanyID
						END
					ELSE
						BEGIN
								INSERT INTO gen_Company_Details(CompanyId,CreatedByID,CreatedDateTime,Culture)
								Values(@CompanyID,@LoggedInUserId,Getdate(),@CultureDetail)

						END

			END


			
END