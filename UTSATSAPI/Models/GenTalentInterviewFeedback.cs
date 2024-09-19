using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentInterviewFeedback
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ShortlistedInterviewId { get; set; }
        public string? ExperienceRate { get; set; }
        public string? GetEnoughTime { get; set; }
        public string? NoThenHowMuchYouNeed { get; set; }
        public string? RoleMatchwithCurrentSkills { get; set; }
        public string? NecessaryInformation { get; set; }
        public string? MoreInformation { get; set; }
        public string? FinancialStability { get; set; }
        public string? NewSkills { get; set; }
        public string? FinancialGrowth { get; set; }
        public string? BetterWork { get; set; }
        public string? GreatCompany { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
