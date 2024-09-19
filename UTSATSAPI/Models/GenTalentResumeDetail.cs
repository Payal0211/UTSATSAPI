using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentResumeDetail
    {
        public long Id { get; set; }
        public string? TalentFirstName { get; set; }
        public string? TalentLastName { get; set; }
        public string? TalentFullName { get; set; }
        public string? TalentContactNo { get; set; }
        public string? TalentEmailId { get; set; }
        public string? TalentDesignation { get; set; }
        public string? TalentLanguages { get; set; }
        public string? TalentCertification { get; set; }
        public string? TalentResumeFileName { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public string? TalentAddress { get; set; }
        public string? TalentPersonalInfo { get; set; }
        public string? TalentResumeSkills { get; set; }
        public string? TalentExperienceSummary { get; set; }
        public string? TalentEducation { get; set; }
    }
}
