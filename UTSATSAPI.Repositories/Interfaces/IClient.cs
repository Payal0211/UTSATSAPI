namespace UTSATSAPI.Repositories.Interfaces
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

    public interface IClient
    {
        Task<List<Sproc_GetPointOfContact_UserDetails_Result>> Sproc_GetPointOfContact_UserDetails(string param);
        Task<List<sproc_UTS_GetClientList_Result>> sproc_GetClient(string param);
        Task<IEnumerable<UsrUser>> GetUserListForAMChange(long OldUserId);
        void UpdateAMDetails(string param);
        Task<List<Sproc_Get_Hierarchy_For_Email_Result>> GetHierarchyForEmail(string param);
        Task<List<sp_UTS_GetPOCDetails_Result>> GetPOCDetails(string param);
        sp_UTS_GetCompanyClientDetails_Result GetCompanyClientDetails(string param);
        Task<List<sp_UTS_GetClientWiseHRDetails_Result>> GetClientWiseHRDetails(string param);
        Task<List<sproc_GetClientHappinessSurvey_Result>> sproc_GetClientHappinessSurvey(string param);
        Task<GenCompany> GetCompanyByName(string companyName);
        Task<GenContact> GetGenContactById(long ContactID);

        Task<GenTalent> GetGenTalentByEmail(string email);
        Task<GenContact> GetGenContactByEmail(string email);
        Task<IEnumerable<UsrUser>> GetUsrUserList();
        Task<UsrUser> GetUsrUserById(long  id);
        Task<UsrUser> GetUsrUserListByLoggedInUserId(long loggedInUserId);
        
        Task<PrgGeo> GetPrgGeosById(int prgGeosId);
        Task<PrgCompanyType> GetPrgCompanyTypeById(int? Id);
        Task<IEnumerable<GenCompany>> GetAllGenCompanysById(long id);
        Task<GenCompany> GetGenCompanysById(long id);
        Task<GenCompanyContractDetail> GetGenCompanyContractDetailsById(long id);
        Task<GenCompany> GetGenCompaniesById(long? id);
        Task<GenContactPointofContact> GetGenContactPointofContactsByContactIdUserId(long contactId, long userId);
        Task<IEnumerable<GenContactPointofContact>> GetGenContactPointofContactsListByContactId(long contactId);
        Task<IEnumerable<GenContact>> GetGenContactListByCondition(long? companyId);
        Task<GenContact> GetGenContactByCompanyId(long CompanyID);


        Task<bool> CreateCompanyContractDetail(GenCompanyContractDetail genCompanyContractDetail);
        Task<bool> UpdateCompanyContractDetail(GenCompanyContractDetail genCompanyContractDetail);

        Task<bool> CreateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts);

        Task<bool> UpdateGenContactPointofContacts(GenContactPointofContact genContactPointofContacts);

        Task<IEnumerable<SelectListItem>> GetUserslist();

        Task<bool> CreateGenCompanies(GenCompany genCompany);
        Task<bool> UpdateGenCompanies(GenCompany genCompany);
        
        Task<bool> CreateGenContact(GenContact genContact);
        Task<bool> UpdateGenContact(GenContact genContact);
        Task<bool> DeleteGenContactsByList(IEnumerable<GenContact> genContact);
        Task<bool> DeleteGenContactPointofContactByList(IEnumerable<GenContactPointofContact> genContactPointofContactList);
        Task<bool> SaveClientLeadUser(long? companyId, int? leadUserId);
        Task<List<sproc_UTS_GetAutoCompleteCompanies_Result>> sproc_GetAutoCompleteCompanies(string param);
        Task<bool> SaveHappinessSurveyFeedback(ClientHappinessSurveyViewModel ClientHappinessSurvey, long? loggedInUserId);
        Task<List<sproc_GetClientHappinessSurveyOptions_Result>> HappinessSurveysOption();
        Task<List<Sproc_Get_TrackingLead_Details_for_ClientSource_Result>> TrackingLeadDetailClientSource(long? ContactID);
        List<Sproc_GET_CreditPlanDetails_ClientPortal_Result> GetCreditTransaction(string paramasString);

        Task<bool> SendEmailForHappinessSurveyFeedback(long feedbackID, long? LoggedInUserId);
        Task<bool> updateGenCompany(long? ContactID, CompanyInfo companyInfo);

        /// <summary>
        /// Fetch all the details of the job that is in draft
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Sp_UTS_PreviewJobPost_ClientPortal_Result Sp_UTS_PreviewJobPost_ClientPortal_Result(string param);

        /// <summary>
        /// Get the access of the logged in user to Add client
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Sproc_UTS_GetAddClientAccess_Result Sproc_UTS_GetAddClientAccess(string param);
        Task<GenContact> SignUp(ClientSignUp clientSignUp, bool IsHobspot);
        Task<UsrUser> UserDetails(long id = 0);
        Sproc_ValidateAddClient_Result Sproc_Validate_AddClient(string param);
        //Sproc_ValidateAddClient_Result Sproc_Validate_AddClientTemp(string param);
        List<Sproc_GetActiveSalesUserList_Result> Sproc_GetActiveSalesUserLists();
        List<Sproc_Get_CreditWithAmount_Result> Sproc_Get_CreditWithAmountLists();

        List<Sproc_Fetch_All_SalesUsers_WithHead_For_Client_Result> Sproc_Fetch_All_SalesUsers_WithHead_For_Client(string param);
        void Sproc_Update_SpaceID_For_Client(string param);
        List<Sproc_Get_SalesUserWithHead_FOr_HiringRequest_Result> Sproc_Get_SalesUserWithHead_FOr_HiringRequest(long param);
        Task<List<Sproc_Get_Credit_Transaction_CompanyWise_Result>> Sproc_Get_Credit_Transaction_CompanyWise(string param);
        void Sproc_Reset_AllHR_TalentStatus();
        void sproc_Add_AWS_Email_Payload_Result(string Payload);
        void Sproc_Add_AWS_SES_EmailTracking(string param);
        
    }
}
