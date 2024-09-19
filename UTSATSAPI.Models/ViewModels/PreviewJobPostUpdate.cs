using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

namespace UTSATSAPI.Models.ViewModels
{
    public class PreviewJobPostUpdate
    {
        public string? RoleName { get; set; }
        public string? Currency { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }
        public int? WorkingModeID { get; set; }
        public int? ContractDuration { get; set; }
        public string? EmploymentType { get; set; }
        public string? Howsoon { get; set; }
        public decimal? ExperienceYears { get; set; }
        public bool? IsFresherAllowed { get; set; }
        public string? RolesResponsibilities { get; set; }
        public string? Requirements { get; set; }
        public long? HRID { get; set; }
        public long? GPTJDID { get; set; }
        public string? IsHiringLimited { get; set; }
        public string? Availability { get; set; }
        public string? CompanyLocation { get; set; }
        public long? AchievementId { get; set; }
        public string? JDFileName { get; set; }
        public string? Skills { get; set; }
        public string? AllSkills { get; set; }
        public int? NoOfTalents { get; set; }
        public int? TimezonePreferenceId { get; set; }
        public int? TimeZoneId { get; set; }
        public string? TimeZoneFromTime { get; set; }
        public string? TimeZoneEndTime { get; set; }
        public string? IndustryType { get; set; }
        public int? CompanySize { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public string? ProcessType { get; set; }
        public bool? JobPostedInSameSession { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public bool? NoBudgetBar { get; set; }
        public long? CookieId { get; set; }
        public int? HiringTypePricingId { get; set; }
        public int? PayrollTypeId { get; set; }
        public string? PayrollPartnerName { get; set; }

        // UTM Details
        public string? UTMCountry { get; set; }
        public string? UTMState { get; set; }
        public string? UTMCity { get; set; }
        public string? UTMBrowser { get; set; }
        public string? UTMDevice { get; set; }
        public string? IPAddress { get; set; }

        // Save the job description now.
        public string? JobDescription { get; set; }

        // Ensures if HR is save as draft
        public bool? IsActive { get; set; } = true;
        public int? AnotherCompanyId { get; set; }
        public bool? IsConfidentialBudget { get; set; }

        public string? CompanyLogo { get; set; }

        public FileUploadModelBase64Update? fileUpload { get; set; }
        public string? Whatweoffer { get; set; }
        public string? RoleOverviewDescription { get; set; }

        // Vital Info 

        public string? CompensationOption { get; set; }
        public string? HRIndustryType { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
        public string? Prerequisites { get; set; }
        public string? StringSeparator { get; set; }
        public bool? IsMustHaveSkillschanged { get; set; }
        public bool? IsGoodToHaveSkillschanged { get; set; }
        public string? ModeOfWork { get; set; }

        // HR POC - UTS- 7946
        //public List<string>? HRPOCUserID { get; set; }
        public List<POCInfo>? HRPOCUserDetails { get; set; }
        public bool? ShowHRPOCDetailsToTalents { get; set; }
        public int? JobTypeID { get; set; }

        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitID { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public long? ATS_JobLocation { get; set; }
        public string? ATS_NearByCities { get; set; }
    }
    public class FileUploadModelBase64Update
    {
        public string Base64ProfilePic { get; set; }
        public string Extenstion { get; set; }
    }
}
