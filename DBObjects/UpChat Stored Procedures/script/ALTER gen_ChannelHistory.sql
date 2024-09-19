IF EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'gen_ChannelHistory'
    AND COLUMN_NAME = 'ChannelID'
)
BEGIN
	ALTER TABLE gen_ChannelHistory
	ALTER COLUMN ChannelID NVARCHAR(MAX);
END

--ADD column UserEmpID in gen_ChannelHistory
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'gen_ChannelHistory'
    AND COLUMN_NAME = 'UserEmpID'
)
BEGIN
	ALTER TABLE gen_ChannelHistory
	ADD UserEmpID varchar(100) NULL;
END

--ADD column IsATSUser in gen_ChannelHistory
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'gen_ChannelHistory'
    AND COLUMN_NAME = 'IsATSUser'
)
BEGIN
	ALTER TABLE gen_ChannelHistory
	ADD IsATSUser BIT NULL;
END

--ADD column HiringRequestID in gen_ChannelHistory
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'gen_ChannelHistory'
    AND COLUMN_NAME = 'HiringRequestID'
)
BEGIN
	ALTER TABLE gen_ChannelHistory
	ADD HiringRequestID BIGINT NULL;
END