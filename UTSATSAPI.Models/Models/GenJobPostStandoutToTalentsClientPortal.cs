using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenJobPostStandoutToTalentsClientPortal
    {
        public long Id { get; set; }
        public long? Gptjdid { get; set; }
        public string? Guid { get; set; }
        public long? ContactId { get; set; }
        public string? IndustryType { get; set; }
        public int? CompanySize { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public bool? IsOffShoreLocationExp { get; set; }
        public bool? IsLineManagerReq { get; set; }
        public bool? IsTeamMemberReplacement { get; set; }
        public string? CareerProgressionPath { get; set; }
        public string? BenefitsandChallenges { get; set; }
        public string? InterviewProcess { get; set; }
        public string? Ipaddress { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? ProcessType { get; set; }
        public bool? IsActive { get; set; }
        public string? ContactsLinkedInProfile { get; set; }
        public string? CompanysLinkedInProfile { get; set; }
        public long? CompanyId { get; set; }
        public string? Website { get; set; }
    }
}
