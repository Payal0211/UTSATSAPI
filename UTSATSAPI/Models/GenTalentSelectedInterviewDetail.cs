using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentSelectedInterviewDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public long? ShortlistedInterviewId { get; set; }
        public string? ClientReadytoworkShift { get; set; }
        public string? TalentReadytoworkShift { get; set; }
        public string? ClientReadytoworkhrs { get; set; }
        public string? TalentReadytoworkhrs { get; set; }
        public string? ClientJoinWithin { get; set; }
        public string? TalentJoinWithin { get; set; }
        public bool? IsConfirmed { get; set; }
        public string? ZoomInterviewLink { get; set; }
        public string? ZoomMeetingId { get; set; }
        public string? ZoomInterviewKitUsername { get; set; }
        public string? ZoomInterviewKitPassword { get; set; }
        public DateTime? ZoomMeetingscheduledOn { get; set; }
        public bool? IsFeedbackSubmitted { get; set; }
        public DateTime? TalentFeedbackGivendatetime { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public int? StatusId { get; set; }
        public bool? IsClientFeedbackSubmitted { get; set; }
        public DateTime? ClientFeedbackGivendatetime { get; set; }
        public int? ContactTimeZoneId { get; set; }
        public bool? IsReschedule { get; set; }
        public int? RescheduleBy { get; set; }
        public DateTime? RescheduleDatetime { get; set; }
        public bool? IsNextRound { get; set; }
        public int? InterviewRound { get; set; }
        public string? InterviewRoundStr { get; set; }
        public long? InterviewMasterId { get; set; }
        public string? InterviewFeedbackStatus { get; set; }
        public string? InterviewClientDynamicStatus { get; set; }
        public string? InterviewTalentDynamicStatus { get; set; }
        public bool? IsInterviewCompletedEmailSent { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
