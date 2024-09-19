CREATE FUNCTION dbo.Fun_GetInitials (@name NVARCHAR(200))
RETURNS NVARCHAR(2)
AS
BEGIN
    DECLARE @initials NVARCHAR(10) = ''

    IF @name IS NOT NULL
    BEGIN
        -- Check if the name has a space character
        IF CHARINDEX(' ', @name) > 0
        BEGIN
            SELECT @initials = @initials + UPPER(LEFT(Item, 1))
            FROM SplitString(@name, ' ')
            WHERE RTRIM(Item) <> ''
        END
        ELSE
        BEGIN
            -- If it's a single name, return the first two characters as initials
            SET @initials = UPPER(LEFT(@name, 2))
        END
    END

	SET @initials = LEFT(@initials, 2)

    RETURN @initials
END
