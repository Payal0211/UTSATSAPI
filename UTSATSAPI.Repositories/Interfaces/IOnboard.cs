using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IOnboard
    {
        void sproc_UTS_Update_SalesUserID_LegalOrKickoffDone(long OnBoardId);

        void sproc_UTS_Update_LastWorkingDay_For_TalentinReplacement(string param);
        Sproc_Get_OnBoardContract_Details_Result Sproc_Get_OnBoardContract_Details(string param);
        Sproc_UTS_Get_OnBoardContract_Details_Result Sproc_UTS_Get_OnBoardContract_Details(string param);
        List<Sproc_UTS_Get_OnBoardClientTeamMemberDeatils_Result> GetOnBoardClientTeamMemberDeatils(string param);
        Sproc_OnBoardPolicy_DeviceMaster_Update_Result Sproc_OnBoardPolicy_DeviceMaster_Update(string param);
        sproc_Get_PreOnboarding_Details_For_AMAssignment_Result sproc_Get_PreOnboarding_Details_For_AMAssignment(string param);
        sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment_Result sproc_Get_Onboarding_Details_For_Second_Tab_AMAssignment(string param);
        void sproc_Update_PreOnBoardingDetails_for_AMAssignment(string param);
        void sproc_Update_Onboarding_Details_For_Second_Tab_AMAssignment(string param);
        /// <summary>
        /// Method to update the contract details of the talent.
        /// </summary>
        /// <param name="param"></param>
        void Sproc_Add_OnBoardClientContractDetails(string param);

        /// <summary>
        /// Get the list of HR's based on the contactID.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        List<sproc_UTS_GetAllOpenInprocessHR_BasedOnContact_Result> sproc_UTS_GetAllOpenInprocessHR_BasedOnContact(string param);

        /// <summary>
        /// Insert the onboarding summary when kick off is done.
        /// </summary>
        /// <param name="param"></param>
        void Sproc_Insert_Onboarding_Summary(string param);

        Task<List<Sproc_Get_OnBoardTalents_Result>> GetOnBoards(string param);
        sproc_UTS_Get_Onboarding_LegalInfo_Result sproc_UTS_Get_Onboarding_LegalInfo_Result(string param);
        void UpdateRenewalInitiated(string param);
        
    }
}
