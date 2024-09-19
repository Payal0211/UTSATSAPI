	--Create table if not exist prg_History_ChannelActions
	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'prg_History_ChannelActions')
	BEGIN
		CREATE TABLE [dbo].[prg_History_ChannelActions](
		[ID] [bigint] IDENTITY(1,1) NOT NULL,
		[ActionName] [nvarchar](100) NOT NULL,
		[DisplayName] [nvarchar](100) NULL,
		[IsActive] [bit] NOT NULL CONSTRAINT [DF_prg_History_ChannelActions_IsActive]  DEFAULT ((0)),
		 CONSTRAINT [PK_prg_History_ChannelActions] PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END

  --Insert action data 
  IF NOT EXISTS(SELECT top 1 * FROM dbo.prg_History_ChannelActions where ActionName = 'AddedUser')
  BEGIN
    INSERT INTO [prg_History_ChannelActions](ActionName,DisplayName,IsActive)
	VALUES('AddedUser','Added New User',1)
  END

  IF NOT EXISTS(SELECT top 1 * FROM dbo.prg_History_ChannelActions where ActionName = 'RemovedUser')
  BEGIN
    INSERT INTO [prg_History_ChannelActions](ActionName,DisplayName,IsActive)
	VALUES('RemovedUser','Removed User',1)
  END

  IF NOT EXISTS(SELECT top 1 * FROM dbo.prg_History_ChannelActions where ActionName = 'LeftChat')
  BEGIN
    INSERT INTO [prg_History_ChannelActions](ActionName,DisplayName,IsActive)
	VALUES('LeftChat','User Left Chat',1)
  END

  IF NOT EXISTS(SELECT top 1 * FROM dbo.prg_History_ChannelActions where ActionName = 'SnoozedChannel')
  BEGIN
    INSERT INTO [prg_History_ChannelActions](ActionName,DisplayName,IsActive)
	VALUES('SnoozedChannel','Snoozed Channel',1)
  END