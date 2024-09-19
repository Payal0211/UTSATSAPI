CREATE TABLE [dbo].[gen_PaymentOrderDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Order_ID] [nvarchar](250) NULL,
	[Order_Response] [nvarchar](max) NULL,
	[Amount] [decimal](18, 2) NULL,
	[Currency] [nvarchar](50) NULL,
	[ReceiptID] [nvarchar](50) NULL,
	[CompanyID] [bigint] NULL,
	[CreatedByID] [bigint] NULL,
	[CreatedByDateTime] [datetime] NULL,
	[ModifiedByID] [bigint] NULL,
	[ModifiedByDateTime] [datetime] NULL,
 CONSTRAINT [PK_gen_PaymentOrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE gen_Payment_History_ClientPortal 
ADD Order_ID [nvarchar](250) NULL


