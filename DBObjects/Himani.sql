---Onboard enhancements  

--Scripts, currently only executed on local
--------------------------------------------------------------------
ALTER TABLE gen_OnBoardTalents
ADD ModeOFWorkingID INT

ALTER TABLE gen_OnBoardTalents
ADD City NVARCHAR(200)

ALTER TABLE gen_OnBoardTalents
ADD StateID INT
--------------------------------------------------------------------



---SPs , currently only executed on local
--------------------------------------------------------------------
Sproc_Get_AM_User
sproc_Get_PreOnboarding_Details_For_AMAssignment
sproc_Update_PreOnBoardingDetails_for_AMAssignment

--------------------------------------------------------------------







