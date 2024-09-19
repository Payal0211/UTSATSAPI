CREATE PROCEDURE [dbo].[Sproc_Get_NearByMAppingCities]
	@LocationId			bigint = 0
AS
BEGIN

		select  NBL.MappingDistrictID as NearByDistrictID,SD.District as NearByDistrictName
		from	gen_NearByLocations NBL WITH(NOLOCK)
				inner join prg_State_District SD WITH(NOLOCK) ON SD.ID = NBL.MappingDistrictID
		where   NBL.DistrictID = @LocationId

END