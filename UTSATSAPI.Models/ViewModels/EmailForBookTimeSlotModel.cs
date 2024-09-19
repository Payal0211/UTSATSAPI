namespace UTSATSAPI.Models.ViewModels
{
    public class EmailForBookTimeSlotModel
    {
        public string TalentName { get; set; }
        public string HR_Number { get; set; }
        public string TalentEmail { get; set; }
        public string TalentRole { get; set; }
        public string Designation { get; set; }
        public string? TalentType { get; set; }
        public decimal? DPPercentage { get; set; }
        public string? Discovery_Call { get; set; }
        public decimal? CurrentCTC { get; set; }
        public string TalentFirstName { get; set; }
        public string TalentLastName { get; set; }
        public string TalentSuccessEmail { get; set; }
        public string clientName { get; set; }
        public string position { get; set; }
        public string CompanyName { get; set; }
        public int? RoleID { get; set; }
        public string ClientEmail { get; set; }
        public string TypeofDeveloper { get; set; }
        public bool IsResetPassword { get; set; }
        public bool IsResetPasswordForTalent { get; set; }

        public string ManagerEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Interviewer { get; set; }
        public long HRSalesPersonID { get; set; }
        public string salesName { get; set; }
        public string salesemailid { get; set; }
        public bool IsAdHoc { get; set; }
        public bool IsManaged { get; set; }
        public bool IsClientNotificationSend { get; set; }
        public bool IsTalentNotificationSend { get; set; }
        public string talentStatusAfterClientSelection { get; set; }

        public long AM_SalesPersonID { get; set; }
        public string AM_SalesPersonEmailID { get; set; }
        public string AM_SalesPersonName { get; set; }

        public long NBD_SalesPersonID { get; set; }
        public string NBD_SalesPersonEmailID { get; set; }
        public string NBD_SalesPersonName { get; set; }

        public string AM_AssignedSalesManagerEmailID { get; set; }
        public string AM_AssignedSalesManagerName { get; set; }

        public decimal InvoiceAmount { get; set; }
        public string EngagementID { get; set; }
        public string Currancy { get; set; }
        public string Reason { get; set; }
        public string InvoiceNumber { get; set; }
        public string SalesUserName { get; set; }
        public string SalesUseEMail { get; set; }
        public decimal? yearsofExperience { get; set; }
        public string? priority { get; set; }
        public decimal? finalCost { get; set; }
        public string InvoiceCreationDate { get; set; }
        public string? HRStatus { get; set; }
    }
}
