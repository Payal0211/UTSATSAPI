using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetStandOutDetails_ClientPortal_Result
    {
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
        public string? ProcessType { get; set; }
        public string? ContactsLinkedInProfile { get; set; }
        public string? CompanysLinkedInProfile { get; set; }
        public long? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Website { get; set; }
    }
}
