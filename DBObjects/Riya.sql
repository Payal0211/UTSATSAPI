
ALTER TABLE gen_contact
ADD EncryptedPassword NVARCHAR(500) NULL
GO

ALTER TABLE gen_TransparentPricingModel_ClientPortal 
ADD PricingPercentage Decimal(18,2) NULL
GO

ALTER TABLE gen_JobPost_VitalInfo_ClientPortal 
ADD HRID BIGINT NULL
GO

ALTER TABLE gen_Payment_History_ClientPortal
ADD RazorPayPaymentStatus NVARCHAR(500) NULL
GO

ALTER TABLE gen_Payment_History_ClientPortal
ADD CompanyID bigint NULL
GO

