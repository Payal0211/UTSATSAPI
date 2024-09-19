using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenClientHappinessSurvey
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? CompanyId { get; set; }
        public int? Rating { get; set; }
        public string? FeedbackComments { get; set; }
        public string? FeedbackUrl { get; set; }
        public bool? IsEmailSend { get; set; }
        public DateTime? FeedbackSubmittedDate { get; set; }
        public bool? IsOtherContact { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? OtherCompanyName { get; set; }
        public string? OtherClientName { get; set; }
        public string? OtherClientEmailId { get; set; }
        public string? TestimonialOptions { get; set; }
    }
}
