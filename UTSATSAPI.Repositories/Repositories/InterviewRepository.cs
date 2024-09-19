namespace UTSATSAPI.Repositories.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Repositories.Interfaces;

    public class InterviewRepository : IInterview
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructor
        public InterviewRepository(TalentConnectAdminDBContext _db)
        {
            this.db = _db;
        }
        #endregion

        #region Public Methods
        public List<sproc_GetHiringInterview_Result> sproc_UTS_GetHiringInterview(string param)
        {
            return db.Set<sproc_GetHiringInterview_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetHiringInterview, param)).ToList();
        }
        public Sproc_InsertBookSlot_Result Sproc_InsertBookSlot(string param)
        {
            return db.Set<Sproc_InsertBookSlot_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_InsertBookSlot, param)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_InsertBookSlot_CHECK_Result Sproc_InsertBookSlot_CHECK(string param)
        {
            return db.Set<Sproc_InsertBookSlot_CHECK_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_InsertBookSlot_CHECK, param)).AsEnumerable().FirstOrDefault();
        }
        public sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact(string param)
        {
            return db.Set<sproc_Get_ContactPointofContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_ContactPointofContact, param)).AsEnumerable().FirstOrDefault();
        }

        public Sproc_InterviewRoundDetails_Result Sproc_Add_TalentSelected_ClientFeedback(string param)
        {
            return db.Set<Sproc_InterviewRoundDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_TalentSelected_ClientFeedback, param)).AsEnumerable().FirstOrDefault();
        }
        public List<sproc_UTS_InterviewerDetails_Result> sproc_GetCurrentInterviewerDetails(string param)
        {
            return db.Set<sproc_UTS_InterviewerDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetCurrentInterviewerDetails, param)).ToList();
        }
        public long sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption(string param)
        {
            long anotherRoundId = 0;
            sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption_Result obj = db.Set<sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Insert_NextInterviewRoundDetails_WithFeedbackOption, param)).AsEnumerable().FirstOrDefault();
            if(obj != null)
            {
                anotherRoundId = obj.AnotherRound_ID;
            }
            return anotherRoundId;
        }
        #endregion

    }
}
