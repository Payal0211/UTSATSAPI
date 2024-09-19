-----------------------------------------------------------1st sep 2023 Himani.s
ALTER TABLE storeapiurl
ADD PRIMARY KEY (ID);

IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'storeapiurl'
    AND COLUMN_NAME = 'Payload'
)
BEGIN
	ALTER TABLE storeapiurl
	ADD Payload NVARCHAR(MAX);
ENDIF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'storeapiurl'
    AND COLUMN_NAME = 'FromATS'
)
BEGIN
	ALTER TABLE storeapiurl
	ADD FromATS BIT default 0;
END