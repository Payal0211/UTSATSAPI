
ALTER  PROCEDURE [dbo].[sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment]	
	@OnBoardID			BIGINT = null,
	@HR_ID				BIGINT = null
AS
BEGIN
	
	SELECT 
	ISNULL(CCD.SigningAuthorityName,'') AS 	InVoiceRaiseTo,
	ISNULL(CCD.SigningAuthorityEmail,'') AS InVoiceRaiseToEmail,
	ISNULL(ContractDuration,0) AS UTSContractDuration,
	ISNULL(D.BDR_MDR_Name,'NA')  BDR_MDR_Name,
	ISNULL(COM.about_Company_desc,'') AS Company_Description,
	ISNULL(OCCD.ExpectationFromTalent_FirstWeek,'') AS Talent_FirstWeek,
	ISNULL(OCCD.ExpectationFromTalent_FirstMonth,'') AS Talent_FirstMonth,
	ISNULL(OCCD.SoftwareToolsRequired,'') AS SoftwareToolsRequired,
	ISNULL(DevicesPoliciesOption,'') AS DevicesPoliciesOption,
	ISNULL(TalentDeviceDetails,'') AS TalentDeviceDetails,
	ISNULL(AdditionalCostPerMonth_RDPSecurity,0) AS AdditionalCostPerMonth_RDPSecurity,
	ISNULL(OCCD.IsRecurring,0) AS IsRecurring,
	ISNULL(ProceedWithUplers_LeavePolicyOption,'') AS ProceedWithUplers_LeavePolicyOption,
	ISNULL(ProceedWithClient_LeavePolicyOption,'') AS ProceedWithClient_LeavePolicyOption,
	ISNULL(ProceedWithClient_LeavePolicyLink,'') AS ProceedWithClient_LeavePolicyLink,
	ISNULL(ProceedWithClient_LeavePolicyFileUpload,'') AS LeavePolicyFileName,
	ISNULL(ProceedWithUplers_ExitPolicyOption,'') AS Exit_Policy,
	ISNULL(OPDM.DeviceName,'') AS Device_Radio_Option,
	ISNULL(CDP.DeviceID,0) AS DeviceID,
	ISNULL(CDP.TotalCost,0) AS TotalCost,
	ISNULL(CDP.Client_DeviceDescription,'') AS Client_DeviceDescription
	FROM gen_OnBoardTalents OBT WITH(NOLOCK) 
	LEFT JOIN gen_OnBoardClientContractDetails OCCD WITH(NOLOCK) ON OCCD.OnBoardID = OBT.ID
	LEFT JOIN gen_SalesHiringRequest SHR WITH(NOLOCK) ON SHR.ID = OBT.HiringRequest_ID
	LEFT JOIN gen_Contact C WITH(NOLOCK) ON C.ID = OBT.ContactID
	LEFT JOIN gen_Company COM WITH(NOLOCK) ON COM.ID = C.CompanyID
	LEFT JOIN gen_CompanyContractDetails CCD WITH(NOLOCK) ON CCD.CompanyID = C.CompanyID
	LEFT JOIN gen_Deals D WITH(NOLOCK) ON D.ID = SHR.DealID
	LEFT JOIN gen_OnBoardClientDevicePolicy_Details CDP WITH(NOLOCK)ON CDP.OnBoardID = OBT.ID
	LEFT JOIN prg_OnBoardPolicy_DeviceMaster OPDM WITH(NOLOCK) ON OPDM.ID = CDP.DeviceID
	WHERE OBT.ID = @OnBoardID AND  OBT.HiringRequest_ID = @HR_ID
END





