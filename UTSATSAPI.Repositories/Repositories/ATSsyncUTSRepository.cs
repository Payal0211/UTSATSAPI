using Aspose.Words.XAttr;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class ATSsyncUTSRepository : IATSsyncUTS
    {
        #region Variables
        private UTSATSAPIDBConnection db;
        #endregion

        #region Constructor
        public ATSsyncUTSRepository(UTSATSAPIDBConnection _db)
        {
            this.db = _db; 
        }
        #endregion

        #region Public Methods

        #region Add/Edit HR
        public void SaveHRPOCDetails(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_HR_POC_ClientPortal, param));
        }

        public Sproc_UTS_AddEdit_ATSHR_Result Sproc_UTS_AddEdit_ATSHR(string param)
        {
            return db.Set<Sproc_UTS_AddEdit_ATSHR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_AddEdit_ATSHR, param)).AsEnumerable().FirstOrDefault();
        }

        public void SaveStepInfoWithUnicode(string guid, string jobDescription)
        {
            var Guid = new SqlParameter("@Guid", SqlDbType.NVarChar) { Value = guid };
            var JobDescription = new SqlParameter("@JobDescription", SqlDbType.NVarChar) { Value = jobDescription };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_UnicodeValues @GUID, @JobDescription", parameters: new[] { Guid, JobDescription });

        }

        public void SaveperquisitesWithUnicode(string guid, string perquisites)
        {
            var Guid = new SqlParameter("@Guid", SqlDbType.NVarChar) { Value = guid };
            var perquisite = new SqlParameter("@Prerequisites", SqlDbType.NVarChar) { Value = perquisites };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_Prerequisites_UnicodeValues @GUID, @Prerequisites", parameters: new[] { Guid, perquisite });

        }
        #endregion

        #region Add/Edit Company profile
        public void Sproc_Add_Company_Funding_Details_Result(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_Funding_Details, paramstring));
        }

        public void Sproc_Add_Company_CultureandPerksDetails_Result(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_CultureandPerksDetails, paramstring));
        }

        public void Sproc_Add_Company_PerksDetails_Result(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_PerksDetails, paramstring));
        }

        public Sproc_Update_Basic_CompanyDetails_Result UpdateCompanyBasicDetails(string paramstring)
        {
            return db.Set<Sproc_Update_Basic_CompanyDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_Basic_CompanyDetails, paramstring)).AsEnumerable().FirstOrDefault();
        }

        public sproc_UTS_UpdateContactDetails_Result UpdateClientDetails(string paramstring)
        {
            return db.Set<sproc_UTS_UpdateContactDetails_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdateContactDetails, paramstring)).AsEnumerable().FirstOrDefault();
        }

        public void Sproc_Add_YoutubeLink(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_YoutubeLink, paramstring));
        }

        public void UpdateCompanyEngagementDetails(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_Company_EngagementDetails, paramstring));
        }


        public void Delete_Company_CultureandPerksDetails(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_CultureandPerksDetails, paramstring));
        }

        public void Delete_Company_YoutubeDetails(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_YouTubeDetails, paramstring));
        }

        public void Delete_Company_PerksDetails(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_PerksDetails, paramstring));
        }

        public void Delete_Company_Funding_Details(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Delete_Company_Funding_Details, paramstring));
        }

        public void DeleteInsertPOCDetails(string paramstring)
        {
            db.Database.ExecuteSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_UpdatePOCUserIDsByCompanyID, paramstring));
        }

        public void SaveCompanyDescUnicode(long CompanyID, string Aboutus, long loggedinUserId)
        {
            var Company = new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID };
            var AboutCompany = new SqlParameter("@Aboutus", SqlDbType.NVarChar) { Value = Aboutus };
            var UserId = new SqlParameter("@LoggedInUserId", SqlDbType.BigInt) { Value = loggedinUserId };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_Company_UnicodeValues @CompanyID, @Aboutus, @LoggedInUserId", parameters: new[] { Company, AboutCompany, UserId });

        }

        public void SaveCultureDetailUnicode(long CompanyID, string CultureDetail, long loggedinUserId)
        {
            var Company = new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID };
            var Culture = new SqlParameter("@CultureDetail", SqlDbType.NVarChar) { Value = CultureDetail };
            var UserId = new SqlParameter("@LoggedInUserId", SqlDbType.BigInt) { Value = loggedinUserId };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_CultureDetail_UnicodeValues @CompanyID, @CultureDetail ,@LoggedInUserId", parameters: new[] { Company, Culture, UserId });

        }

        public void SaveAdditionalInfoUnicode(long CompanyID, string AdditionalInformation, long loggedinUserId)
        {
            var Company = new SqlParameter("@CompanyID", SqlDbType.BigInt) { Value = CompanyID };
            var AdditionalInfo = new SqlParameter("@AdditionalInformation", SqlDbType.NVarChar) { Value = AdditionalInformation };
            var UserId = new SqlParameter("@LoggedInUserId", SqlDbType.BigInt) { Value = loggedinUserId };

            db.Database.ExecuteSqlRaw("EXEC Sproc_Update_AdditionalInfo_UnicodeValues @CompanyID, @AdditionalInformation,@LoggedInUserId", parameters: new[] { Company, AdditionalInfo, UserId });

        }
        #endregion

        public Task<Sproc_Add_Company_Transactions_With_ATS_Result> Sproc_Add_Company_Transactions_With_ATS(string param)
        {
            return db.Set<Sproc_Add_Company_Transactions_With_ATS_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Company_Transactions_With_ATS, param)).FirstOrDefaultAsync();
        }
        #endregion

        #region Maintain UtsAts logs
        public long InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records)
        {
            GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();

            utsAtsApi_Records.FromApiUrl = gen_UtsAtsApi_Records.FromApiUrl;
            utsAtsApi_Records.ToApiUrl = gen_UtsAtsApi_Records.ToApiUrl;    //Here API URL of ATS will come.
            utsAtsApi_Records.PayloadToSend = gen_UtsAtsApi_Records.PayloadToSend;
            utsAtsApi_Records.CreatedById = gen_UtsAtsApi_Records.CreatedById;
            utsAtsApi_Records.CreatedByDateTime = DateTime.Now;
            utsAtsApi_Records.HrId = gen_UtsAtsApi_Records.HrId;
            db.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
            db.SaveChanges();

            return utsAtsApi_Records.Id;
        }
        public void UpdateUtsAtsApiDetails(long APIRecordInsertedID, string Message)
        {
            #region Update record in gen_UtsAts_Records
            GenUtsAtsApiRecord utsAtsApi_Records = db.GenUtsAtsApiRecords.Where(x => x.Id == APIRecordInsertedID).FirstOrDefault();
            if (utsAtsApi_Records != null)
            {
                utsAtsApi_Records.ResponseReceived = Message;
                CommonLogic.DBOperator(db, utsAtsApi_Records, EntityState.Modified);
            }
            #endregion
        }
        #endregion
    }
}
