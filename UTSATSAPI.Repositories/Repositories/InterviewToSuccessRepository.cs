using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class InterviewToSuccessRepository : IInterviewToSuccess
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public InterviewToSuccessRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public API's
        public IEnumerable<Sproc_Get_InterviewToSuccess_PopUp_Result> GetInterviewToSuccessPopUpReport(string paramasString)
        {
            return db.Set<Sproc_Get_InterviewToSuccess_PopUp_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_Get_InterviewToSuccess_PopUp, paramasString)).AsEnumerable();
        }

        public IEnumerable<Sproc_GetInterviewToSuccessReport_Result> GetInterviewtoSuccessReport(string paramasString)
        {
            return db.Set<Sproc_GetInterviewToSuccessReport_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetInterviewToSuccessReport, paramasString)).AsEnumerable();
        }

        #endregion
    }
}
