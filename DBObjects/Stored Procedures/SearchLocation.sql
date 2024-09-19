ALTER PROCEDURE [dbo].[SearchLocation]
    @SearchText NVARCHAR(255)
AS
BEGIN
    -- Search in SubDistrict
    IF EXISTS (
        SELECT 1
        FROM prg_State_SubDistrict SS WITH(NOLOCK)
        INNER JOIN prg_State_District SD WITH(NOLOCK) ON SS.DistrictID = SD.ID 
        WHERE SS.SubDistrict LIKE @SearchText + '%'
    )
    BEGIN
        SELECT distinct
            SS.SubDistrict + ',' + SD.District + ',' + SD.State + ',' + SD.Country AS Location
        FROM prg_State_SubDistrict SS WITH(NOLOCK)
        INNER JOIN prg_State_District SD WITH(NOLOCK) ON SS.DistrictID = SD.ID 
        WHERE SS.SubDistrict LIKE @SearchText + '%';
        RETURN;
    END

    -- Search in Revised City
    IF EXISTS (
        SELECT 1
        FROM prg_State_SubDistrict SS WITH(NOLOCK)
        INNER JOIN prg_State_District SD WITH(NOLOCK) ON SS.DistrictID = SD.ID 
        WHERE SD.RevisedCity LIKE @SearchText + '%'
    )
    BEGIN
        SELECT  distinct
            SD.RevisedCity + ',' + SD.District + ',' + SD.State + ',' + SD.Country AS Location
        FROM prg_State_SubDistrict SS WITH(NOLOCK)
        INNER JOIN prg_State_District SD WITH(NOLOCK) ON SS.DistrictID = SD.ID 
        WHERE SD.RevisedCity LIKE @SearchText + '%';
        RETURN;
    END

    -- Search in District
    IF EXISTS (
        SELECT 1
        FROM prg_State_District SD WITH(NOLOCK)
        WHERE SD.District LIKE @SearchText + '%'
    )
    BEGIN
        SELECT  distinct
            SD.District + ',' + SD.State + ',' + SD.Country AS Location
        FROM prg_State_District SD WITH(NOLOCK)
        WHERE SD.District LIKE @SearchText + '%';
        RETURN;
    END

    -- Search in State
    IF EXISTS (
        SELECT 1
        FROM prg_State_District SD WITH(NOLOCK)
        WHERE SD.State LIKE @SearchText + '%'
    )
    BEGIN
        SELECT distinct
            SD.State + ',' + SD.Country AS Location
        FROM prg_State_District SD WITH(NOLOCK)
        WHERE SD.State LIKE @SearchText + '%';
        RETURN;
    END

    -- If nothing found, return empty result
    SELECT '' AS Location;
END;
