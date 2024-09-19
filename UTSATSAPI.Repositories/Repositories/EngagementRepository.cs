using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class EngagementRepository : IEngagement
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public EngagementRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }

       
        #endregion

        #region Public Methods
        public async Task<List<Sproc_Get_BusinessDashboard_Result>> ListEngagement(string param)
        {
            return await db.Set<Sproc_Get_BusinessDashboard_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_BusinessDashboard, param)).ToListAsync();
        }
        public async Task<Sproc_talent_engagement_Details_For_PHP_API_Result> TalentEngagementDetails(long? OnboardID,string Action = "")
        {
            return db.Set<Sproc_talent_engagement_Details_For_PHP_API_Result>().FromSqlRaw(string.Format("{0} {1},'{2}'", Constants.ProcConstant.Sproc_talent_engagement_Details_For_PHP_API, OnboardID, Action)).AsEnumerable().FirstOrDefault();
        }
        public NRPerCentageValue Sproc_Calculate_ActualNR_From_BR_PR(string param)
        {
            return db.Set<NRPerCentageValue>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Calculate_ActualNR_From_BR_PR, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_UTS_DashBoard_OnBoardCount_Result GetDashboardCount()
        {
            return db.Set<Sproc_UTS_DashBoard_OnBoardCount_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_UTS_DashBoard_OnBoardCount)).AsEnumerable().FirstOrDefault();
        }

        public async Task<List<Sproc_Get_OnBoardClientFeedBack_Result>> GetOnBoardFeedback(string param)
        {
            return await db.Set<Sproc_Get_OnBoardClientFeedBack_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_OnBoardClientFeedBack, param)).ToListAsync();
        }
        public async Task<List<Sproc_Get_engagement_Edit_All_BR_PR_Result>> IGet_engagement_Edit_All_BR_PR(string paramString)
        {
            return await db.Set<Sproc_Get_engagement_Edit_All_BR_PR_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_engagement_Edit_All_BR_PR, paramString)).ToListAsync();
        }

        public Sproc_Get_Renewal_Details_Result Get_Renewal_Details_Result(string param)
        {
            return db.Set<Sproc_Get_Renewal_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Renewal_Details, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_Add_Contract_Renewal_Details_Result Insert_Contract_Renewal_Details(string param)
        {
            return db.Set<Sproc_Add_Contract_Renewal_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_Contract_Renewal_Details, param)).AsEnumerable().FirstOrDefault();
        }
        public async Task<List<SelectListItem>> GetTSCNameList()
        {
            var data = db.Set<sproc_Get_TSC_List_Result>().FromSqlRaw(Constants.ProcConstant.sproc_Get_TSC_List_Result).ToList();

            return data.Select(x => new SelectListItem
            {
                Value = x.FullName.ToString(),
                Text = x.FullName.ToString()
            }).ToList();
        }

        public async Task<List<Sproc_Get_Hierarchy_For_Email_Result>> GetHierarchyForEmail(string param)
        {
            return await db.Set<Sproc_Get_Hierarchy_For_Email_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_Hierarchy_For_Email, param)).ToListAsync();
        }
        #endregion

        public Sproc_GetTSCUserEditDetail_Result Sproc_GetTSCUserEditDetail(string param)
        {
            return db.Set<Sproc_GetTSCUserEditDetail_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetTSCUserEditDetail, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_GetEmailDetailForTSCAssignment_Result GetEmailDetailForTSCAssignment(string param)
        {
            return db.Set<Sproc_GetEmailDetailForTSCAssignment_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_GetEmailDetailForTSCAssignment, param)).AsEnumerable().FirstOrDefault();
        }
        public void Sproc_UpdateTSCUser(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UpdateTSCUser, param));
        }
        public void Sproc_Get_User_For_TSCAutoAssignment_BasedOnRoundRobin(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_User_For_TSCAutoAssignment_BasedOnRoundRobin, param));
        }

        public async Task<List<UsrUser>> GetUserListForAMChange(long OldUserId)
        {
            return await db.UsrUsers.Where(x => x.IsActive == true && (x.UserTypeId == 4 || x.UserTypeId == 9) && x.Id != OldUserId).ToListAsync();
        }

        public Sproc_UTS_Get_PayOut_Basic_Information_Result PayOut_Basic_Informtion(long PayOutID)
        {
            return db.Set<Sproc_UTS_Get_PayOut_Basic_Information_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_PayOut_Basic_Information, PayOutID)).AsEnumerable().FirstOrDefault();
        }

        public void Update_AMDetails_In_PayOut(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Save_AMDetails_In_PayOut, param));
        }
        public Sproc_TalentEngagementConverted_C2H_Result Sproc_TalentEngagementConverted_C2H(string param)
        {
            return db.Set<Sproc_TalentEngagementConverted_C2H_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_TalentEngagementConverted_C2H, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_Update_HrStatus_Result GetUpdateHrStatus(string param)
        {
            return db.Set<Sproc_Update_HrStatus_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_HrStatus, param)).AsEnumerable().FirstOrDefault();
        }

    }
}
