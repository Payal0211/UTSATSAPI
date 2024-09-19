using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_OnBoardContract_Details_Result
    {
        public Nullable<long> OnBoardID { get; set; }
        public string ClientName { get; set; }
        public string Clientemail { get; set; }
        public string EngagemenID { get; set; }
        public string HiringRequestNumber { get; set; }
        public string TalentName { get; set; }
        public string TalentEmailId { get; set; }
        public string AssociatedWithUplers { get; set; }
        public string Tentative_Start_Date { get; set; }
        public int TotalDuration { get; set; }
        public string PunchTime { get; set; }
        public string TimeZone { get; set; }
        public string TalentCost { get; set; }
        public string ClientFirstDate { get; set; }
        public int NetPaymentDays { get; set; }
        public int ContractRenewalSlot { get; set; }
        public string TalentOnBoardDate { get; set; }
        public string TalentOnBoardTime { get; set; }
        public string DevicesPoliciesOption { get; set; }
        public string TalentDeviceDetails { get; set; }
        public string ExpectationFromTalent_FirstWeek { get; set; }
        public string ExpectationFromTalent_FirstMonth { get; set; }
        public string ProceedWithUplers_ExitPolicyOption { get; set; }
        public string ProceedWithUplers_LeavePolicyOption { get; set; }
        public string ProceedWithClient_LeavePolicyLink { get; set; }
        public string ProceedWithClient_LeavePolicyFileUpload { get; set; }
        public string Client_Remark { get; set; }
        public string Talent_Remark { get; set; }
        public string WorkingDay { get; set; }
        public string ProceedWithClient_LeavePolicyOption { get; set; }
        public string Tentative_End_Date { get; set; }
    }
}
