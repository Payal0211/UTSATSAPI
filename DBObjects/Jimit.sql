--06-06-24---

--For Invite client flow change in email and after login change password provision given by Mehul
sproc_UTS_UpdateContactDetails
GO
Sproc_ContactUsers_ClientPortal
Go

--05-06-24-----


ALTER TABLE gen_Company_Details
ADD  TotalFundings nvarchar(100) null
GO
Sproc_Update_TotalFunding_Details
GO

--03-06-24 
-----for UTS - 7505  Removal Of POC
https://docs.google.com/spreadsheets/d/12p7NPpbAwbiJOvAFXuNRM44oBXKzWkjgE0i7CAspJiQ/edit#gid=0


--29-05-24
-------Company Size Added ---------------
ALTER TABLE gen_Company
ADD CompanySize_RangeorAdhoc Nvarchar(500) null
GO
sproc_Update_Company_Details_From_Scrapping
GO
Sproc_InviteClient_UTS_Admin
GO
Sproc_RegisterClient_ClientPortal
GO
--------------------------------------------
Sproc_Get_UpdateHR_Details
GO







