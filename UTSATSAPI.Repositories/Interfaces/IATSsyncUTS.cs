using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IATSsyncUTS
    {
        #region Add/Edit HR
        void SaveHRPOCDetails(string param);
        Sproc_UTS_AddEdit_ATSHR_Result Sproc_UTS_AddEdit_ATSHR(string param);
        void SaveStepInfoWithUnicode(string guid, string jobDescription);
        void SaveperquisitesWithUnicode(string guid, string perquisites);
        #endregion

        #region company profile Save/update/delete calls
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
        void SaveCompanyDescUnicode(long CompanyID, string Aboutus, long loggedinUserId);
        void SaveCultureDetailUnicode(long CompanyID, string CultureDetail, long loggedinUserId);
        void SaveAdditionalInfoUnicode(long CompanyID, string AdditionalInformation, long loggedinUserId);

        #endregion
        Task<Sproc_Add_Company_Transactions_With_ATS_Result> Sproc_Add_Company_Transactions_With_ATS(string param);
        Task<GenSalesHiringRequest> GetHiringRequestbyNumber(string hiringRequestNumber);

        Task<List<Sproc_CurrencyExchangeRate_Result>> GetCurrencyExchangeRate_Results(string paramString);

        long Sproc_Insert_CompanyActionHistory(string paramstring);
        void InsertCompanyHistory(string paramstring);

        #region Maintain UtsAts logs
        Task<long> InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records);
        Task UpdateUtsAtsApiDetails(long APIRecordInsertedID, string Message);
        #endregion
    }
}
