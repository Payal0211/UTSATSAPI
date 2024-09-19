namespace UTSATSAPI.Models.ViewModels
{
    public class OnBoardViewModel
    {
        public long OnboardID { get; set; }
        public string ContractType { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public int TotalDurationInMonths { get; set; }
        public int ContractDuration { get; set; }
        public string TalentOnBoardDate { get; set; }
        public string TalentOnBoardTime { get; set; }
        public string PunchStartTime { get; set; }
        public string PunchEndTime { get; set; }
        public string WokringDays { get; set; }
        public string TalentWorkingTimeZone { get; set; }
        public int Timezone_Preference_ID { get; set; }
        public string FirstClientBillingDate { get; set; }
        public int NetPaymentDays { get; set; }
        public int ContractRenewalSlot { get; set; }
        public string DevicesPoliciesOption { get; set; }
        public string TalentDeviceDetails { get; set; }
        public string ExpectationFromTalent_FirstWeek { get; set; }
        public string ExpectationFromTalent_FirstMonth { get; set; }
        public string Client_Remark { get; set; }
        public string ProceedWithUplers_LeavePolicyOption { get; set; }
        public string ProceedWithClient_LeavePolicyFileUpload { get; set; }
        public string ProceedWithClient_LeavePolicyLink { get; set; }
        public string ProceedWithClient_LeavePolicyOption { get; set; }
        public string ProceedWithUplers_ExitPolicyOption { get; set; }

        public string ClientName { get; set; }
        public string Clientemail { get; set; }
        public string EngagemenID { get; set; }
        public string HiringRequestNumber { get; set; }
        public string TalentName { get; set; }
        public string TalentEmailId { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public List<TeamMembers> TeamMemebers { get; set; }
    }

    public class TeamMembers
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ReportingTo { get; set; }
        public string Linkedin { get; set; }
        public string Email { get; set; }
        public string Buddy { get; set; }
    }
    public class ArchiveInvoice
    {
        public ArchiveInvoice()
        {
            invoiceId = new List<InvoiceIds>();
        }
        public List<InvoiceIds> invoiceId { get; set; }
        public string ArchiveReason { get; set; }
    }
    public class InvoiceIds
    {
        public long? ID { get; set; }
    }
}
