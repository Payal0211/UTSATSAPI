using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenSalesHiringRequestDetailsHistory
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public int? RoleId { get; set; }
        public int? NoofEmployee { get; set; }
        public decimal? Duration { get; set; }
        public string? DurationType { get; set; }
        public string? Cost { get; set; }
        public int? RoleStatusId { get; set; }
        public int? SpecificMonth { get; set; }
        public decimal? YearOfExp { get; set; }
        public string? HowSoon { get; set; }
        public int? TimezoneId { get; set; }
        public int? TypeofInterviewerId { get; set; }
        public int? RoleTeamSize { get; set; }
        public string? CommunicationType { get; set; }
        public int? AssociatedwithUplersId { get; set; }
        public int? PartialEngagementTypeId { get; set; }
        public int? NoOfHoursWorking { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public long? InterviewerId { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewLinkedin { get; set; }
        public decimal? InterviewerYearofExperience { get; set; }
        public string? HrCost { get; set; }
        public string? RolesResponsibilities { get; set; }
        public string? GenericInfo { get; set; }
        public string? Requirement { get; set; }
        public string? TimezonePreference { get; set; }
        public int? TimezonePreferenceId { get; set; }
        public string? TimeZoneFromTime { get; set; }
        public string? TimeZoneEndTime { get; set; }
        public decimal? OverlapingHours { get; set; }
        public string? InterviewerDesignation { get; set; }
        public int? NotMatchAfterAcceptanceTimezoneId { get; set; }
        public string? InterviewerEmailId { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }
        public string? Currency { get; set; }
        public string? AddOtherRole { get; set; }
        public decimal? AdhocBudgetCost { get; set; }
        public string? Remark { get; set; }
        public int? PreviousRoleStatusId { get; set; }
        public int? InterviewerExpInMonth { get; set; }
    }
}
