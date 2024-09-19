namespace UTSATSAPI.Models.ComplexTypes
{
    public class sp_UTS_GetStandOutDetails_Result
    {
        public long? ID { get; set; }
        public string? GUID { get; set; }
        public long? ContactId { get; set; }
        public string? Industry_Type { get; set; }
        public int? CompanySize { get; set; }
        public string? about_Company_desc { get; set; }
        public bool? IsOffShoreLocationExp { get; set; }
        public bool? IsLineManagerReq { get; set; }
        public bool? IsTeamMemberReplacement { get; set; }
        public string? CareerProgressionPath { get; set; }
        public string? BenefitsandChallenges { get; set; }
        public string? InterviewProcess { get; set; }
        public string? ContactsLinkedInProfile { get; set; }
        public string? CompanysLinkedInProfile { get; set; }
        public long? CompanyID { get; set; }
    }
}
