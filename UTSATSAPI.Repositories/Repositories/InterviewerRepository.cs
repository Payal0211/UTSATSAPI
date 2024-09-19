using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class InterviewerRepository : IInterviewer
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructor
        public InterviewerRepository(TalentConnectAdminDBContext _db)
        {
            this.db = _db;
        }
        #endregion

        #region Public Methods
        public List<sproc_UTS_FetchInterviewerDetails_Result> FetchInterviewerDetails(string param) 
        {
            return db.Set<sproc_UTS_FetchInterviewerDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_FetchInterviewerDetails, param)).ToList();
        }

        public void AddUpdateDeleteInterviewerDetails(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_AddUpdateDeleteInterviewerDetails, param));
        }
        #endregion
    }
}
