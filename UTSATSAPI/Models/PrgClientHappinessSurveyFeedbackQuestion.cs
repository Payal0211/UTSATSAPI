using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgClientHappinessSurveyFeedbackQuestion
    {
        public int Id { get; set; }
        public string? HappinessSurveyQuestion { get; set; }
        public bool? IsActive { get; set; }
        public int? RatingFrom { get; set; }
        public int? RatingTo { get; set; }
    }
}
