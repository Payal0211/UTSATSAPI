using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenContact
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmailId { get; set; }
        public string? ContactNo { get; set; }
        public string? Regions { get; set; }
        public string? Skype { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public bool? IsPrimary { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsResetPassword { get; set; }
        public string? Designation { get; set; }
        public decimal? YearsofExperience { get; set; }
        public string? LinkedIn { get; set; }
        public string? Department { get; set; }
        public bool? IsPasswordChanged { get; set; }
        public string? PhotoImage { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpCreatedDate { get; set; }
        public DateTime? OtpExpiredDate { get; set; }
        public bool? OtpIsActive { get; set; }
        public bool? IsEmailSentforFirstHr { get; set; }
        public bool IsClientNotificationSend { get; set; }
        public long? HubSpotContactId { get; set; }
        public int? PasswordAttemptCount { get; set; }
    }
}
