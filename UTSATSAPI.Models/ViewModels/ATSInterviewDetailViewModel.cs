using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class ATSInterviewDetailViewModel
    {
        public ATSInterviewDetailViewModel()
        {
            HR_InterviewerDetails = new List<gen_SalesHiringRequest_InterviewerDetails_ATS>();
            SelectedTalent_InterviewDetails = new List<gen_TalentSelected_InterviewDetails_ATS>();
            ShortListedTalent_InterviewDetails = new List<gen_ShortlistedTalent_InterviewDetails_ATS>();
            Interview_SlotMaster = new List<gen_InterviewSlotsMaster_ATS>();
        }
        public long UTS_HiringRequestID { get; set; }
        public long ATS_TalentID { get; set; }
        public long UTS_TalentID { get; set; }
        public List<gen_SalesHiringRequest_InterviewerDetails_ATS> HR_InterviewerDetails { get; set; }
        public List<gen_TalentSelected_InterviewDetails_ATS> SelectedTalent_InterviewDetails { get; set; }
        public List<gen_ShortlistedTalent_InterviewDetails_ATS> ShortListedTalent_InterviewDetails { get; set; }
        public List<gen_InterviewSlotsMaster_ATS> Interview_SlotMaster { get; set; }
    }
    public class gen_SalesHiringRequest_InterviewerDetails_ATS
    {
        public long? ID { get; set; }
        public long? HiringRequest_ID { get; set; }
        public long? HiringRequest_Detail_ID { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public decimal? InterviewerYearofExperience { get; set; }
        public int? TypeofInterviewer_ID { get; set; }
        public string? InterviewerDesignation { get; set; }
        public string? InterviewerEmailID { get; set; }
        public bool? IsUsedAddMore { get; set; }
        public int? InterviewerExpInMonth { get; set; }
        public long? ContactID { get; set; }
    }
    public class gen_TalentSelected_InterviewDetails_ATS
    {
        public long? ID { get; set; }
        public long? HiringRequest_ID { get; set; }
        public long? HiringRequest_Detail_ID { get; set; }
        public long? ContactID { get; set; }
        public long? Talent_ID { get; set; }
        public long? Shortlisted_InterviewID { get; set; }
        public string? Client_ReadytoworkShift { get; set; }
        public string? Talent_ReadytoworkShift { get; set; }
        public string? Client_Readytoworkhrs { get; set; }
        public string? Talent_Readytoworkhrs { get; set; }
        public string? Client_JoinWithin { get; set; }
        public string? Talent_JoinWithin { get; set; }
        public bool? IsConfirmed { get; set; }
        public string? Zoom_InterviewLink { get; set; }
        public string? Zoom_MeetingID { get; set; }
        public string? Zoom_InterviewKit_username { get; set; }
        public string? Zoom_InterviewKit_password { get; set; }
        public DateTime? Zoom_MeetingscheduledOn { get; set; }
        public bool? IsFeedbackSubmitted { get; set; }
        public DateTime? TalentFeedbackGivendatetime { get; set; }
        public int? CreatedByID { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedByID { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public int? Status_ID { get; set; }
        public bool? IsClientFeedbackSubmitted { get; set; }
        public DateTime? ClientFeedbackGivendatetime { get; set; }
        public int? Contact_TimeZone_ID { get; set; }
        public bool? IsReschedule { get; set; }
        public int? RescheduleBy { get; set; }
        public DateTime? RescheduleDatetime { get; set; }
        public bool? IsNextRound { get; set; }
        public int? InterviewRound { get; set; }
        public string? InterviewRoundStr { get; set; }
        public long? InterviewMaster_ID { get; set; }
        public string? Interview_FeedbackStatus { get; set; }
        public string? Interview_ClientDynamic_Status { get; set; }
        public string? Interview_TalentDynamic_Status { get; set; }
        public bool? IsInterviewCompletedEmailSent { get; set; }
        public string? AdditionalNotes { get; set; }
    }
    public class gen_ShortlistedTalent_InterviewDetails_ATS
    {
        public long? ID { get; set; }
        public long? HiringRequest_ID { get; set; }
        public long? HiringRequest_Detail_ID { get; set; }
        public long? ContactID { get; set; }
        public long? Talent_ID { get; set; }
        public int? TimeZone_ID { get; set; }
        public string? InterviewSlot { get; set; }
        public DateTime? ScheduledOn { get; set; }
        public TimeSpan? Interview_StartTime { get; set; }
        public TimeSpan? Interview_EndTime { get; set; }
        public bool? IsTalentConfirmed { get; set; }
        public int? Status_ID { get; set; }
        public int? CreatedByID { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedByID { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public DateTime? IST_ScheduledOn { get; set; }
        public TimeSpan? IST_Interview_StartTime { get; set; }
        public TimeSpan? IST_Interview_EndTime { get; set; }
        public decimal? DurationInHours { get; set; }
        public long? NextRound_ID { get; set; }
        public long? InterviewMaster_ID { get; set; }
    }
    public class gen_InterviewSlotsMaster_ATS
    {
        public long? ID { get; set; }
        public long? HiringRequest_ID { get; set; }
        public long? HiringRequest_Detail_ID { get; set; }
        public long? ContactID { get; set; }
        public long? Talent_ID { get; set; }
        public string? GUID { get; set; }
        public int? InterviewStatus_ID { get; set; }
        public int? CreatedByID { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public long? Rescheduled_InterviewID { get; set; }
        public string? Rescheduled_By { get; set; }
        public int? Rescheduled_TypeID { get; set; }
        public string? Rescheduled_Reasons { get; set; }
        public string? Rescheduled_MessageShown { get; set; }
        public int? InterviewRound_Count { get; set; }
        public string? InterviewRound_Str { get; set; }
        public int? LastModifiedByID { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
