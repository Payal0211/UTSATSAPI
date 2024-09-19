using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenOnBoardTalentsClientFeedback
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? ContactId { get; set; }
        public long? OnBoardId { get; set; }
        public string? FeedbackType { get; set; }
        public string? FeedbackComment { get; set; }
        public string? FeedbackActionToTake { get; set; }
        public DateTime? FeedbackCreatedDateTime { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string? FileName { get; set; }
    }
}
