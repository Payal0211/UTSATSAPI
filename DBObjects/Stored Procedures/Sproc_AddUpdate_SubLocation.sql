CREATE PROCEDURE [dbo].[Sproc_AddUpdate_SubLocation]
  @SubLocationId			bigint = null,
  @LocationID				bigint = null,
  @sub_location				Nvarchar(Max) = null
AS
BEGIN

		IF EXISTS(Select 1 from prg_State_SubDistrict WITH(NOLOCK) where ID = @SubLocationId)	
			BEGIN
					Update prg_State_SubDistrict
					SET	   SubDistrict = Isnull(@sub_location,SubDistrict)
					where  ID = @SubLocationId

			END
		ELSE
			BEGIN
					Insert into prg_State_SubDistrict(ID,DistrictID,SubDistrict)
					Values (@SubLocationId,@LocationID,@sub_location)

					SET @SubLocationId = @@IDENTITY
			END
			
			Select @SubLocationId as ID

END