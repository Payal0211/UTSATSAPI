using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructor
        public ATSsyncUTSRepository(TalentConnectAdminDBContext _db)
        {
            this.db = _db; 
        }
        #endregion

        #region Public Methods
        public void SaveHRPOCDetails(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_HR_POC_ClientPortal, param));
        }

        public Sproc_UTS_AddEdit_ATSHR_Result Sproc_UTS_AddEdit_ATSHR(string param)
        {
            return db.Set<Sproc_UTS_AddEdit_ATSHR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_AddEdit_ATSHR, param)).AsEnumerable().FirstOrDefault();
        }

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
