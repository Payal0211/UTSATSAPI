using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ICompany
    {
        Task<List<Sproc_UTS_GetCompanyList_Result>> GetCompanyList(string strparams);
        Task<List<sproc_GetCompanyLegalInfo_Result>> GetLegalInfoList(string strparams);
        Task<List<Sproc_UTS_GetHubSpotCompanyList_Result>> GetHubSpotCompanyList(string strparams);

        /// <summary>
        /// Gets the company by hub spot company identifier.
        /// </summary>
        /// <param name="HubSpotCompanyId">The hub spot company identifier.</param>
        /// <returns></returns>
        Task<GenCompany?> GetCompanyByHubSpotCompanyId(long HubSpotCompanyId);
        /// <summary>
        /// Gets the company company name and by hub spot company identifier.
        /// </summary>
        /// <param name="CompanyName">Name of the company.</param>
        /// <param name="HubSpotCompanyId">The hub spot company identifier.</param>
        /// <returns></returns>
        Task<GenCompany?> GetCompanyCompanyNameAndByHubSpotCompanyId(string CompanyName, long HubSpotCompanyId);
        /// <summary>
        /// Saves the company.
        /// </summary>
        /// <param name="genCompany">The gen company.</param>
        /// <returns></returns>
        Task<GenCompany> SaveCompany(GenCompany genCompany);

        sproc_UTS_GetCompanyDetails_Result GetCompanyDetails(long? CompanyId);
        sproc_UTS_GetCompanyDetailsForEdit_Result GetCompanyDetailsForEdit(long? CompanyId);

        #region New company profile Save/update/delete calls
        void Sproc_Add_Company_Funding_Details_Result(string paramstring);
        void Sproc_Add_Company_CultureandPerksDetails_Result(string paramstring);
        void Sproc_Add_Company_PerksDetails_Result(string paramstring);
        Sproc_Update_Basic_CompanyDetails_Result UpdateCompanyBasicDetails(string paramstring);
        sproc_UTS_UpdateContactDetails_Result UpdateClientDetails(string paramstring);
        void UpdateCompanyEngagementDetails(string paramstring);
        void Sproc_Add_YoutubeLink(string paramstring);
        void Delete_Company_CultureandPerksDetails(string paramstring);
        void Delete_Company_YoutubeDetails(string paramstring);
        void Delete_Company_PerksDetails(string paramstring);
        void Delete_Company_Funding_Details(string paramstring);
        void DeleteInsertPOCDetails(string paramstring);
        #endregion

        #region get calls for New company profile
        Sproc_CompanyDetail_TransferToATS_Result Sproc_CompanyDetail_TransferToATS_Result(long CompanyId);
        Sproc_Get_Basic_CompanyDetails_Result Sproc_Get_Basic_CompanyDetails_Result(long CompanyId);
        List<Sproc_Get_Company_Funding_Details_Result> Sproc_Get_Company_Funding_Details_Result(long CompanyId);
        List<Sproc_Get_Company_CultureandPerksDetails_Result> Sproc_Get_Company_CultureandPerksDetails_Result(long CompanyId);
        List<Sproc_Get_Company_PerksDetails_Result> Sproc_Get_Company_PerksDetails_Result(long CompanyId);
        List<Sproc_Get_Company_YouTubeDetails_Result> Sproc_Get_Company_YouTubeDetails_Result(long CompanyId);
        List<sp_UTS_GetPOCUserIDByCompanyID_Result> GetPOCUserIDByCompanyID(long CompanyId);
        sproc_UTS_GetCompanyEngagementDetails_Result GetCompanyEngagementDetails(long CompanyId);
        #endregion
        Task<GenContact> ContactDetails(string emailId, long id = 0);
        Task<GenCompany> CompanyDetails(string companyName, long id = 0);
        void sproc_Update_Company_Details_From_Scrapping_Result(string paramstring);
        Task<bool> UpdateCompanyLogo(long companyId, string logo);
        sp_UTS_GetPOCUserIDByCompanyID_Edit_Result GetPOCUserIDByCompanyIDEdit(long CompanyId);
        Task<Sp_UTS_PreviewJobPost_ClientPortal_Result> Sp_UTS_PreviewJobPost_ClientPortal_Result(string param);
        Sproc_GetStandOutDetails_ClientPortal_Result GetStandOutDetailsAsync(string param);
        void UpdatePreviewDetails(string param);
        void UpdatePreviewDetailsUTSAdmin(string param);
        object GetAllHRDataForAdmin(long HiringRequest_ID);
        Sproc_Update_HrStatus_Result GetUpdateHrStatus(string param);
        Task SaveGptJdresponse(GenGptJdresponse jdresponse);
        void SaveStepInfoWithUnicode(string guid, string jobDescription);
        void SaveperquisitesWithUnicode(string guid, string perquisites);
        void SaveCompanyDescUnicode(long CompanyID, string Aboutus, long loggedinUserId);
        void SaveCultureDetailUnicode(long CompanyID,  string CultureDetail, long loggedinUserId);
        void SaveAdditionalInfoUnicode(long CompanyID,string AdditionalInformation, long loggedinUserId);
        List<Sproc_HiringTypeDetails_ClientPortal_Result> GetHiringTypePricingDetails(string param);
        List<Sproc_UTS_GetCompanyWhatsappDetails_Result> Sproc_UTS_GetCompanyWhatsappDetails(long CompanyId);
        Sproc_UTS_SaveCompanyWhatsappDetails_Result Sproc_UTS_SaveCompanyWhatsappDetails(string param);
        void Sproc_UTS_SaveCompanyWhatsappMemberDetails(string param);
        List<Sproc_Get_Add_CompanyData_Send_Details_To_ATS_Result> Sproc_Get_Add_CompanyData_Send_Details_To_ATS(string param);


    }
}
