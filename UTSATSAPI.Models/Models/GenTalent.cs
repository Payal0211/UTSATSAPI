using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalent
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }
        public string? EmailId { get; set; }
        public int? RoleId { get; set; }
        public string? ContactNumber { get; set; }
        public string? Achievements { get; set; }
        public string? AddressDetail { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? SkypeId { get; set; }
        public int? CurrentEmploymentStatusId { get; set; }
        public int? AssociatedwithUplersId { get; set; }
        public int? JoiningId { get; set; }
        public int? ComfortableAreaId { get; set; }
        public int? CommunicationSkillsId { get; set; }
        public bool? DirectlyWorkedInternationalClients { get; set; }
        public decimal? TotalExpYears { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public int? Status { get; set; }
        public string? SignatureFile { get; set; }
        public string? Cvfilename { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsResetPassword { get; set; }
        public string? HearAboutUplers { get; set; }
        public string? Description { get; set; }
        public string? Fees { get; set; }
        public int? TalentAccountStatusId { get; set; }
        public string? AadharcardPhotoCopy { get; set; }
        public string? PancardPhotoCopy { get; set; }
        public string? CodeRepositoryUrl { get; set; }
        public string? PhotoImage { get; set; }
        public string? PortfolioPluginUrl { get; set; }
        public string? TalentCategory { get; set; }
        public int? TalentTimeZoneId { get; set; }
        public decimal? PackageBasedonShift { get; set; }
        public string? TypeofDeveloper { get; set; }
        public bool? IsEmailSent { get; set; }
        public bool? IsAccountCreated { get; set; }
        public bool? IsLegalSigned { get; set; }
        public bool? IsProfileSubmitted { get; set; }
        public bool? IsApproved { get; set; }
        public int? PartialEngagementTypeId { get; set; }
        public int? NoofHoursworking { get; set; }
        public int? TalentStatusIdAfterClientSelection { get; set; }
        public string? MinimumFees { get; set; }
        public bool? IsPasswordChanged { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? Country { get; set; }
        public string? FaceBookId { get; set; }
        public string? InstagramId { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpCreatedDate { get; set; }
        public DateTime? OtpExpiredDate { get; set; }
        public bool? OtpIsActive { get; set; }
        public int? PreviousTalentAccountStatusId { get; set; }
        public string? AddOtherRole { get; set; }
        public int? TalentTypeId { get; set; }
        public long? ResumeId { get; set; }
        public bool? IsTalentNotificationSend { get; set; }
        public decimal? FinalCost { get; set; }
        public string? ProfileUrl { get; set; }
        public bool? PmsStatus { get; set; }
        public string? PmsUuid { get; set; }
        public int? PmsSocialProviderId { get; set; }
        public long? AtsTalentId { get; set; }
        public string? AtsTalentLiveUrl { get; set; }
        public decimal? AtsTalentInramount { get; set; }
        public bool? IsOdrActive { get; set; }
        public decimal? CurrentCtc { get; set; }
        public string? AtsNonNdaurl { get; set; }
        public string? Designation { get; set; }
    }
}
