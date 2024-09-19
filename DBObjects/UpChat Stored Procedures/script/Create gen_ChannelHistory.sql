
	--Create table if not exist prg_History_ChannelActions
	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'gen_ChannelHistory')
	BEGIN
		CREATE TABLE [dbo].[gen_ChannelHistory](
			[ID] [bigint] IDENTITY(1,1) NOT NULL,
			[ChannelID] [nvarchar](max) NULL,
			[ChannelActionID] [bigint] NOT NULL,
			[CreatedByID] [bigint] NOT NULL,
			[CreatedDateTime] [datetime] NOT NULL,
			[UserEmpID] [varchar](100) NULL,
			[IsATSUser] [bit] NULL,
			[HiringRequestID] [bigint] NULL,
		 CONSTRAINT [PK_gen_ChannelHistory] PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END