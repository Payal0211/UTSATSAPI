using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenOnBoardTalent
    {
        public long Id { get; set; }
        public string? EngagemenId { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public long? ShortlistedInterviewId { get; set; }
        public DateTime? HiredDate { get; set; }
        public DateTime? Joiningdate { get; set; }
        public string? TalentFeesStr { get; set; }
        public int? ClientOnBoardingStatusId { get; set; }
        public DateTime? ClientOnBoardingDate { get; set; }
        public string? ClientConcernRemark { get; set; }
        public int? TalentOnBoardingStatusId { get; set; }
        public DateTime? TalentOnBoardingDate { get; set; }
        public string? TalentConcernRemark { get; set; }
        public int? ClientLegalStatusId { get; set; }
        public DateTime? ClientLegalDate { get; set; }
        public int? TalentLegalStatusId { get; set; }
        public DateTime? TalentLegalDate { get; set; }
        public int? KickoffStatusId { get; set; }
        public DateTime? KickoffDate { get; set; }
        public DateTime? ZoomMeetingscheduledOn { get; set; }
        public string? ZoomInterviewLink { get; set; }
        public string? ZoomMeetingId { get; set; }
        public string? ZoomInterviewKitUsername { get; set; }
        public string? ZoomInterviewKitPassword { get; set; }
        public TimeSpan? ZoomStartTime { get; set; }
        public TimeSpan? ZoomEndTime { get; set; }
        public DateTime? IstZoomScheduledOn { get; set; }
        public TimeSpan? IstZoomStartTime { get; set; }
        public TimeSpan? IstZoomEndTime { get; set; }
        public int StatusId { get; set; }
        public string? Remark { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public int? KickoffTimezonePreferenceId { get; set; }
        public bool? IsClientOnBoardEmailSend { get; set; }
        public bool? IsHrmanaged { get; set; }
        public long? ManagedTalentId { get; set; }
        public int? AmSalesPersonId { get; set; }
        public int? NbdSalesPersonId { get; set; }
        public string? DeleteHrreason { get; set; }
        public int? DeleteHrreasonId { get; set; }
        public string? LossRemark { get; set; }
        public decimal? Sowcount { get; set; }
        public string? Guid { get; set; }
        public decimal? FinalHrCost { get; set; }
        public decimal? TalentCost { get; set; }
        public decimal? Nrpercentage { get; set; }
        public DateTime? ClientClosureDate { get; set; }
        public decimal? CurrencyExchangeRate { get; set; }
        public decimal? Dpamount { get; set; }
        public string? ClientCurrencyCode { get; set; }
        public string? TalentCurrencyCode { get; set; }
        public decimal? TalentCurrencyExchangeRate { get; set; }
    }
}
