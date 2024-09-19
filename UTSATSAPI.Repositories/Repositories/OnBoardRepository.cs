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
    public class OnBoardRepository : IOnboard
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public OnBoardRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }

        #endregion

        #region public Methods
        public void sproc_UTS_Update_SalesUserID_LegalOrKickoffDone(long OnBoardId)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_Update_SalesUserID_LegalOrKickoffDone, OnBoardId));
        }

        public void sproc_UTS_Update_LastWorkingDay_For_TalentinReplacement(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_Update_LastWorkingDay_For_TalentinReplacement, param));
        }

        public Sproc_Get_OnBoardContract_Details_Result Sproc_Get_OnBoardContract_Details(string param)
        {
           return db.Set<Sproc_Get_OnBoardContract_Details_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_OnBoardContract_Details, param)).AsEnumerable().FirstOrDefault();
        }
        public Sproc_UTS_Get_OnBoardContract_Details_Result Sproc_UTS_Get_OnBoardContract_Details(string param)
        {
            return db.Set<Sproc_UTS_Get_OnBoardContract_Details_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_UTS_OnBoardContract_Details, param)).AsEnumerable().FirstOrDefault();
        }
        public List<Sproc_UTS_Get_OnBoardClientTeamMemberDeatils_Result> GetOnBoardClientTeamMemberDeatils(string param)
        {
            return db.Set<Sproc_UTS_Get_OnBoardClientTeamMemberDeatils_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_Get_OnBoardClientTeamMemberDeatils, param)).AsEnumerable().ToList();
        }
        public Sproc_OnBoardPolicy_DeviceMaster_Update_Result Sproc_OnBoardPolicy_DeviceMaster_Update(string param)
        {
            return db.Set<Sproc_OnBoardPolicy_DeviceMaster_Update_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_OnBoardPolicy_DeviceMaster_Update, param)).AsEnumerable().FirstOrDefault();
        }

        public sproc_Get_PreOnboarding_Details_For_AMAssignment_Result sproc_Get_PreOnboarding_Details_For_AMAssignment(string param)
        {
            return db.Set<sproc_Get_PreOnboarding_Details_For_AMAssignment_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_PreOnboarding_Details_For_AMAssignment, param)).AsEnumerable().FirstOrDefault();
        }

        public void sproc_Update_PreOnBoardingDetails_for_AMAssignment(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Update_PreOnBoardingDetails_for_AMAssignment, param));
        }

        public sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment(string param)
        {
            return db.Set<sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment, param)).AsEnumerable().FirstOrDefault();
        }

        public void sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment, param));
        }


        /// <summary>
        /// Method to update the contract details of the talent.
        /// </summary>
        /// <param name="param"></param>
        public void Sproc_Add_OnBoardClientContractDetails(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Add_OnBoardClientContractDetails, param));
        }

        /// <summary>
        /// Get the list of HR's based on the contactID.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result> sproc_UTS_GetAllOpenInprocessHR_BasedOnContact(string param)
        {
            return db.Set<sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetAllOpenInprocessHR_BasedOnContact, param)).ToList();
        }

        /// <summary>
        /// Insert the onboarding summary when kickoff is done.
        /// </summary>
        /// <param name="param"></param>
        public void Sproc_Insert_Onboarding_Summary(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Insert_Onboarding_Summary, param));
        }

        public async Task<List<Sproc_Get_OnBoardTalents_Result>> GetOnBoards(string param)
        {
            return await db.Set<Sproc_Get_OnBoardTalents_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Get_OnBoardTalents, param)).ToListAsync();
        }

        public sproc_UTS_Get_Onboarding_LegalInfo_Result sproc_UTS_Get_Onboarding_LegalInfo_Result(string param)
        {
            return db.Set<sproc_UTS_Get_Onboarding_LegalInfo_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_Get_Onboarding_LegalInfo, param)).AsEnumerable().FirstOrDefault();
        }
        public void UpdateRenewalInitiated(string param)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_RenewalInitiated, param));
        }
        #endregion
    }
}
