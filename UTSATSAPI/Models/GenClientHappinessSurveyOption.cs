using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenClientHappinessSurveyOption
    {
        public long Id { get; set; }
        public long? HappynessSurvayFeedbackId { get; set; }
        public int? HappynessSurvayOptionId { get; set; }
    }
}
