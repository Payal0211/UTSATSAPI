using System.Text.Json.Serialization;

namespace UTSATSAPI.Models.ViewModels
{
    public class ClientFeedbackViewModel
    {
        //public int[] SkillId { get; set; }
        //public int[] ImprovedSkillId { get; set; }
        //public List<SelectListItem> SKillData { get; set; }
        //public string OnHoldReason { get; set; }
        //public string AnotherRound { get; set; }
        //public decimal? RateCurrentTalent { get; set; }
        //public decimal? FitToCompanyRate { get; set; }
        //public string NotFitDescription { get; set; }
        //public string ImproveDescription { get; set; }
        //public int TalentRoleValue { get; set; }
        //public decimal? RateProfessionalism { get; set; }
        //public string PageName { get; set; }
        //public string ActionName { get; set; }
        
        public string Role { get; set; }
        public string HdnRadiovalue { get; set; }
        public long TalentIDValue { get; set; }
        public long ContactIDValue { get; set; }
        public long HiringRequestID { get; set; }
        public long HiringRequestDetailID { get; set; }
        public long ShortlistedInterviewID { get; set; }
        public long ContactInterviewFeedbackId { get; set; }
        public bool NohireReconsiderHiringTalentYes { get; set; }
        public bool NohireReconsiderHiringTalentNo { get; set; }
        public string TopSkill { get; set; }
        public string ImprovedSkill { get; set; }
        public bool? IsClientNotificationSent { get; set; }


        public string TechnicalSkillRating { get; set; }
        public string CommunicationSkillRating { get; set; }
        public string CognitiveSkillRating { get; set; }
        public string MessageToTalent { get; set; }
        public string ClientsDecision { get; set; }
        public string Comments { get; set; }
        public string en_Id { get; set; }

        public long FeedbackId { get; set; }

        public int? RejectReasonID { get; set; }
        public string? Remark { get; set; }
        public string? OtherReason { get; set; }
    }
}
