using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IEngagement
    {
        Task<List<Sproc_Get_BusinessDashboard_Result>> ListEngagement(string param);
        Task<Sproc_talent_engagement_Details_For_PHP_API_Result> TalentEngagementDetails(long? OnboardID,string Action = "");
        Sproc_UTS_DashBoard_OnBoardCount_Result GetDashboardCount();
        NRPerCentageValue Sproc_Calculate_ActualNR_From_BR_PR(string param);
        Task<List<Sproc_Get_OnBoardClientFeedBack_Result>> GetOnBoardFeedback(string param);
        Task<List<Sproc_Get_engagement_Edit_All_BR_PR_Result>> IGet_engagement_Edit_All_BR_PR(string paramString);
        Sproc_Get_Renewal_Details_Result Get_Renewal_Details_Result(string param);
        Sproc_Add_Contract_Renewal_Details_Result Insert_Contract_Renewal_Details(string param);
        Task<List<SelectListItem>> GetTSCNameList();
        Task<List<Sproc_Get_Hierarchy_For_Email_Result>> GetHierarchyForEmail(string param);
        Sproc_GetTSCUserEditDetail_Result Sproc_GetTSCUserEditDetail(string param);
        void Sproc_UpdateTSCUser(string param);
        void Sproc_Get_User_For_TSCAutoAssignment_BasedOnRoundRobin(string param);
        Sproc_GetEmailDetailForTSCAssignment_Result GetEmailDetailForTSCAssignment(string param);
        Task<List<UsrUser>> GetUserListForAMChange(long OldUserId);
        Sproc_UTS_Get_PayOut_Basic_Information_Result PayOut_Basic_Informtion(long PayOutID);
        void Update_AMDetails_In_PayOut(string param);
        Sproc_TalentEngagementConverted_C2H_Result Sproc_TalentEngagementConverted_C2H(string param);
        Sproc_Update_HrStatus_Result GetUpdateHrStatus(string param);
    }
}
