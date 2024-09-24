using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.ATSCalls
{
    public static class ModelBinderATS
    {
        //public static List<gen_SalesHiringRequest_InterviewerDetails_ATS> InterviewerDetails(List<GenSalesHiringRequestInterviewerDetail> modelList)
        //{
        //    List<gen_SalesHiringRequest_InterviewerDetails_ATS> list = new();
        //    if (modelList.Any())
        //    {
        //        foreach (var model in modelList)
        //        {
        //            gen_SalesHiringRequest_InterviewerDetails_ATS obj = new();
        //            obj.ID = model.Id;
        //            obj.HiringRequest_ID = model.HiringRequestId;
        //            obj.HiringRequest_Detail_ID = model.HiringRequestDetailId;
        //            obj.InterviewerName = model.InterviewerName ?? "";
        //            obj.InterviewLinkedin = model.InterviewLinkedin ?? "";
        //            obj.InterviewerYearofExperience = model.InterviewerYearofExperience;
        //            obj.TypeofInterviewer_ID = model.TypeofInterviewerId;
        //            obj.InterviewerDesignation = model.InterviewerDesignation ?? "";
        //            obj.InterviewerEmailID = model.InterviewerEmailId ?? "";
        //            obj.IsUsedAddMore = model.IsUsedAddMore;
        //            obj.InterviewerExpInMonth = model.InterviewerExpInMonth;
        //            obj.ContactID = model.ContactId;
        //            list.Add(obj);
        //        }
        //    }
        //    return list;
        //}

        //public static List<gen_InterviewSlotsMaster_ATS> InterviewSlotsMaster(List<GenInterviewSlotsMaster> modelList)
        //{
        //    List<gen_InterviewSlotsMaster_ATS> list = new();
        //    if (modelList.Any())
        //    {
        //        foreach (var model in modelList)
        //        {
        //            gen_InterviewSlotsMaster_ATS obj = new();
        //            obj.ID = model.Id;
        //            obj.HiringRequest_ID = model.HiringRequestId;
        //            obj.HiringRequest_Detail_ID = model.HiringRequestDetailId;
        //            obj.ContactID = model.ContactId;
        //            obj.Talent_ID = model.TalentId;
        //            obj.GUID = model.Guid ?? "";
        //            obj.InterviewStatus_ID = model.InterviewStatusId;
        //            obj.CreatedByID = model.CreatedById;
        //            obj.CreatedByDatetime = model.CreatedByDatetime;
        //            obj.Rescheduled_InterviewID = model.RescheduledInterviewId;
        //            obj.Rescheduled_By = model.RescheduledBy ?? "";
        //            obj.Rescheduled_TypeID = model.RescheduledTypeId;
        //            obj.Rescheduled_Reasons = model.RescheduledReasons ?? "";
        //            obj.Rescheduled_MessageShown = model.RescheduledMessageShown ?? "";
        //            obj.InterviewRound_Count = model.InterviewRoundCount;
        //            obj.InterviewRound_Str = model.InterviewRoundStr ?? "";
        //            obj.LastModifiedByID = model.LastModifiedById;
        //            obj.LastModifiedDatetime = model.LastModifiedDatetime;
        //            list.Add(obj);
        //        }
        //    }
        //    return list;
        //}

        //public static List<gen_TalentSelected_InterviewDetails_ATS> TalentSelectedInterviewDetail(List<GenTalentSelectedInterviewDetail> modelList)
        //{
        //    List<gen_TalentSelected_InterviewDetails_ATS> list = new();
        //    if (modelList.Any())
        //    {
        //        foreach (var model in modelList)
        //        {
        //            gen_TalentSelected_InterviewDetails_ATS obj = new();
        //            obj.ID = model.Id;
        //            obj.HiringRequest_ID = model.HiringRequestId;
        //            obj.HiringRequest_Detail_ID = model.HiringRequestDetailId;
        //            obj.ContactID = model.ContactId;
        //            obj.Talent_ID = model.TalentId;
        //            obj.Shortlisted_InterviewID = model.ShortlistedInterviewId;
        //            obj.Client_ReadytoworkShift = model.ClientReadytoworkShift ?? "";
        //            obj.Talent_ReadytoworkShift = model.TalentReadytoworkShift ?? "";
        //            obj.Client_Readytoworkhrs = model.ClientReadytoworkhrs ?? "";
        //            obj.Talent_Readytoworkhrs = model.TalentReadytoworkhrs ?? "";
        //            obj.Client_JoinWithin = model.ClientJoinWithin ?? "";
        //            obj.Talent_JoinWithin = model.TalentJoinWithin ?? "";
        //            obj.IsConfirmed = model.IsConfirmed;
        //            obj.Zoom_InterviewLink = model.ZoomInterviewLink ?? "";
        //            obj.Zoom_MeetingID = model.ZoomMeetingId ?? "";
        //            obj.Zoom_InterviewKit_username = model.ZoomInterviewKitUsername ?? "";
        //            obj.Zoom_InterviewKit_password = model.ZoomInterviewKitPassword ?? "";
        //            obj.Zoom_MeetingscheduledOn = model.ZoomMeetingscheduledOn;
        //            obj.IsFeedbackSubmitted = model.IsFeedbackSubmitted;
        //            obj.TalentFeedbackGivendatetime = model.TalentFeedbackGivendatetime;
        //            obj.CreatedByID = model.CreatedById;
        //            obj.CreatedByDatetime = model.CreatedByDatetime;
        //            obj.LastModifiedByID = model.LastModifiedById;
        //            obj.LastModifiedDatetime = model.LastModifiedDatetime;
        //            obj.Status_ID = model.StatusId;
        //            obj.IsClientFeedbackSubmitted = model.IsClientFeedbackSubmitted;
        //            obj.ClientFeedbackGivendatetime = model.ClientFeedbackGivendatetime;
        //            obj.Contact_TimeZone_ID = model.ContactTimeZoneId;
        //            obj.IsReschedule = model.IsReschedule;
        //            obj.RescheduleBy = model.RescheduleBy;
        //            obj.RescheduleDatetime = model.RescheduleDatetime;
        //            obj.IsNextRound = model.IsNextRound;
        //            obj.InterviewRound = model.InterviewRound;
        //            obj.InterviewRoundStr = model.InterviewRoundStr ?? "";
        //            obj.InterviewMaster_ID = model.InterviewMasterId;
        //            obj.Interview_FeedbackStatus = model.InterviewFeedbackStatus ?? "";
        //            obj.Interview_ClientDynamic_Status = model.InterviewClientDynamicStatus ?? "";
        //            obj.Interview_TalentDynamic_Status = model.InterviewTalentDynamicStatus ?? "";
        //            obj.IsInterviewCompletedEmailSent = model.IsInterviewCompletedEmailSent;
        //            obj.AdditionalNotes = model.AdditionalNotes ?? "";
        //            list.Add(obj);
        //        }
        //    }
        //    return list;
        //}

        //public static List<gen_ShortlistedTalent_InterviewDetails_ATS> ShortlistedTalentInterviewDetail(List<GenShortlistedTalentInterviewDetail> modelList)
        //{
        //    List<gen_ShortlistedTalent_InterviewDetails_ATS> list = new();
        //    if (modelList.Any())
        //    {
        //        foreach (var model in modelList)
        //        {
        //            gen_ShortlistedTalent_InterviewDetails_ATS obj = new();
        //            obj.ID = model.Id;
        //            obj.HiringRequest_ID = model.HiringRequestId;
        //            obj.HiringRequest_Detail_ID = model.HiringRequestDetailId;
        //            obj.ContactID = model.ContactId;
        //            obj.Talent_ID = model.TalentId;
        //            obj.TimeZone_ID = model.TimeZoneId;
        //            obj.InterviewSlot = model.InterviewSlot ?? "";
        //            obj.ScheduledOn = model.ScheduledOn;
        //            obj.Interview_StartTime = model.InterviewStartTime;
        //            obj.Interview_EndTime = model.InterviewEndTime;
        //            obj.IsTalentConfirmed = model.IsTalentConfirmed;
        //            obj.Status_ID = model.StatusId;
        //            obj.CreatedByID = model.CreatedById;
        //            obj.CreatedByDatetime = model.CreatedByDatetime;
        //            obj.LastModifiedByID = model.LastModifiedById;
        //            obj.LastModifiedDatetime = model.LastModifiedDatetime;
        //            obj.IST_ScheduledOn = model.IstScheduledOn;
        //            obj.IST_Interview_StartTime = model.IstInterviewStartTime;
        //            obj.IST_Interview_EndTime = model.IstInterviewEndTime;
        //            obj.DurationInHours = model.DurationInHours;
        //            obj.NextRound_ID = model.NextRoundId;
        //            obj.InterviewMaster_ID = model.InterviewMasterId;
        //            list.Add(obj);
        //        }
        //    }
        //    return list;
        //}
    }
}
