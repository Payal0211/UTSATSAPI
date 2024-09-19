using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetTalentList_Result
    {
        public long? ID { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Usertype { get; set; }
        public string? EmailID { get; set; }
        public string? ContactNumber { get; set; }
        public string? Achievements { get; set; }
        public string? AddressDetail { get; set; }
        public string? Status { get; set; }
        public int TotalRecords { get; set; }
        public string? AssociatedWithUplers { get; set; }
        public string? SkypeID { get; set; }
        public string? TalentRole { get; set; }
        public string? PrimarySkills { get; set; }
        public string? SecondarySkills { get; set; }
        public string? CreatedByDatetime { get; set; }
        public int? TalentStatusID { get; set; }
        public string? AccountStatus { get; set; }
        public int? PointOfContact { get; set; }
        public int? TalentStatusID_AfterClientSelection { get; set; }
        public string? AfterClientSelectionStatus { get; set; }
        public string? TalentCategory { get; set; }
        public int? IsTalentNotificationSend { get; set; }
        public string? Talent_Type { get; set; }
        public decimal? FinalCost { get; set; }
        public string? ProfileURL { get; set; }
        public long? ResumeID { get; set; }
        public int? TalentAccount_Status_ID { get; set; }
        public string? ATSNonNDAURL { get; set; }
        public string? ATSTalentLiveURL { get; set; }
    }
}
