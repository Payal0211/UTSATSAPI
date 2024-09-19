
namespace UTSATSAPI.Repositories.Interfaces
{
    using UTSATSAPI.ComplexTypes;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;

    public interface IHiringRequest
    {
        void sproc_Interviewdetailsdbrief(string param);
        void sproc_Interviewdetailsdb(string param);
        Sproc_GetCompanyDetail_Result Sproc_GetCompanyDetail(string param);
        sproc_FetchHiringInterviewerDetails_Result sproc_FetchHiringInterviewerDetails(string param);
        List<sproc_ViewTalent_Result> sproc_UTS_ViewTalent(string param);
        sproc_UTS_getCompanyNameByHiringRequestID sproc_UTS_getCompanyNameByHiringRequestID(string param);
        sproc_UTS_GetTalentProfileLog_Result sproc_UTS_GetTalentProfileLog_Result(string param);
        List<sproc_UTS_GetProfileShareDetail_Result> sproc_UTS_GetProfileShareDetail_Result(string param);
        List<sproc_UTS_GetTalentScoreCard_Result> sproc_UTS_GetTalentScoreCard_Result(string param);
        sproc_GetOnBoardData_Result sproc_GetOnBoardData_Result(string param);
        List<sproc_UTS_GetAutoCompleteContacts_Result> sproc_UTS_GetAutoCompleteContacts_Result(string param);
        List<sproc_UTS_GetAutoCompleteCompany_Result> sproc_UTS_GetAutoCompleteCompany_Result(string param);

        List<Sproc_GET_PostAcceptance_detail_Result> Sproc_GET_PostAcceptance_detail(string param);
        List<Sproc_GET_PostAcceptance_detail_Availability_Result> Sproc_GET_PostAcceptance_detail_Availability(string param);
        List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> Sproc_GET_PostAcceptance_detail_HowSoon(string param);
        Task<sproc_CloneHRFromExistHR_Result> sproc_CloneHRFromExistHR(string param);
        List<sproc_UTS_GetChildCompanyList_Result> sproc_UTS_GetChildCompanyList(string param); 
        void sproc_UTS_UpdatePartnershipDetails_ForHR(string param);

        /// <summary>
        /// Sprocs the talent hr cancelled hr list.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        List<sproc_TalentHR_CancelledHR_List_Result> sproc_TalentHR_CancelledHR_List(string param);

        /// <summary>
        /// UTS-3256: Validates and sends the proper output for closing HR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Sproc_Update_Status_For_Closed_HR_Result sproc_Update_Status_For_Clsoed_HR(string param);

        /// <summary>
        /// UTS-3256: Close/Cancel/loss the HR as per the condition.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<Sproc_Check_Validation_Message_For_Close_HR_Result> Sproc_Check_Validation_Message_For_Close_HR(string param);

        /// <summary>
        /// UTS-3641: Reopen the HR.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Sproc_Update_Status_For_Reopen_HR_Result sproc_Update_Status_For_Reopen_HR(string param);

        /// <summary>
        /// UTS-6449 Repost feature for credit base HR in UTS Admin which like Reopen HR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Sproc_RepostedJob_ClientPortal_Result Sproc_RepostedJob_ClientPortal(string param);
        Sproc_Get_HRDetails_From_DealId_Result Sproc_Get_HRDetails_From_DealId_Result(string param);

        /// <summary>
        /// UTS-3998: Update the HR details from the special users and COE team.
        /// </summary>
        /// <param name="param"></param>
        void Sproc_Update_HRDetails_From_Special_User(string param);

        /// <summary>
        /// Get the latest details of the HR SLA when data updated.
        /// </summary>
        /// <returns></returns>
        Task<List<sproc_Get_HiringRequest_SLADetails_Result>> Get_HiringRequest_SLADetails(long HRID);
        Task<object> GetAllHRDataForAdmin(long HRID, bool? isAutogenerateQuestions = null);

        /// <summary>
        /// Fetch the additional info
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        sp_UTS_GetStandOutDetails_Result sp_UTS_GetStandOutDetails(string param);

        List<Sproc_FetchSimilarRoles_UTS_Result> Sproc_FetchSimilarRoles_UTS(string param);

        /// <summary>
        /// CreditUtilization for pay per credit HR
        /// </summary>
        /// <param name="HRID"></param>
        /// <returns></returns>
        Sp_UTS_GetCreditUtilization_BasedOnHR_Result GetCreditUtilization_BasedOnHR(long HRID);
        Sproc_Update_HrStatus_Result GetUpdateHrStatus(string param);
        List<Sproc_UTS_GetCloseHRHistory_Result> Sproc_UTS_GetCloseHRHistory(string param);
        Sproc_UTS_CheckCreditAvailablilty_Result CheckCreditAvailablilty(long CompanyID);
        Sproc_UTS_AddEdit_HR_Result AddEdit_HR(string param);
        void Sproc_UTS_DeleteTestHR(string param);
        Task<sproc_CloneHRFromExistHR_Result> sproc_CloneHRFromExistHRDemoAccount(string param);
        List<Sproc_SearchLocation_Result> sproc_UTS_GetAutoComplete_CityStateWise(string param);
        List<Sproc_Get_NearByMAppingCities_Result> sproc_UTS_GetNearByCities(string param);
        List<Sproc_HR_POC_ClientPortal_Result> SaveandGetHRPOCDetails(string param);
        sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage_Result GetWarningMsg(long HRID);
        List<Sproc_Get_Frequency_Office_Visit_Result> Sproc_Get_Frequency_Office_Visit();
        Sproc_AddUpdate_TalentNotes_ClientPortal_Result Sproc_AddUpdate_TalentNotes_ClientPortal(string param);
        List<Sproc_SearchLocationOnly_Result> sproc_UTS_GetAutoComplete_CityWise(string param);
        List<Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal_Result> Get_ShortListedDetails_ForCreditFlow_ClientPortals(long hrid);
    }
}
