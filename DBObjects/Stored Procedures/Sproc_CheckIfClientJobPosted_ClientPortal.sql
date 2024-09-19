USE [TalentConnect]
GO
/****** Object:  StoredProcedure [dbo].[Sproc_CheckIfClientJobPosted_ClientPortal]    Script Date: 16-04-2024 17:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Sproc_CheckIfClientJobPosted_ClientPortal]  
  @ContactID BIGINT NULL  
AS  
BEGIN  
   
 DECLARE @IsJobPosted BIT = 1 -- As per the request from Yash all the pay per hire users will redirect to Dashboard only whether job posted or not -- 16 April 
 DECLARE @CompanyID bigint = 0  
 Declare @DCSalesEmpID as Nvarchar(20) = null  
 Declare @DCSalesUserID as bigint = 0  
 Declare @DCUserDescription as Nvarchar(Max) = ''  
 DECLARE @Username NVARCHAR(250) = ''  
 DECLARE @Companyname NVARCHAR(250) = ''  
 DECLARE @ShowCompanyInput BIT = 0  
 DECLARE @CompanyTypeID INT = 1;  
 DECLARE @AnotherCompanyTypeID INT = 0  
  
 SELECT @CompanyID = CompanyID,  
   @Username = Username,  
   @Companyname = CO.Company,  
   @CompanyTypeID = ISNULL(CO.CompanyTypeID,1),  
   @AnotherCompanyTypeID = CO.AnotherCompanyTypeID  
   FROM gen_Contact C WITH(NOLOCK)   
   INNER JOIN gen_Company CO WITH(NOLOCK) ON C.CompanyID = CO.ID  
   WHERE C.Id = @ContactID  
  
 --IF((SELECT COUNT(1) FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE ContactID IN (SELECT ID FROM gen_Contact with(nolock) WHERE CompanyID = @CompanyID)) > 0)  
 --BEGIN  
 -- SET @IsJobPosted = 1  
 --END  
   
  
 IF(@CompanyTypeID = 2 OR ISNULL(@AnotherCompanyTypeID,0) = 2)  
 BEGIN  
  SET @IsJobPosted = 1   
  SET @CompanyTypeID = 2  
 END  
  
 SELECT @IsJobPosted AS IsJobPosted, @ShowCompanyInput AS ShowCompanyInput, @CompanyTypeID AS CompanyTypeID  
      
END