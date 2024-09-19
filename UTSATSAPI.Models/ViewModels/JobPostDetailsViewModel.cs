using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class JobPostDetailsViewModel
    {
        //======== Common Fields =======
        //public long ID { get; set; }
        public long? GPTJDID { get; set; }
        public string GUID { get; set; }
        public long? ContactId { get; set; }
        public string ClientIPAddress { get; set; }
        public string IPAddress { get; set; }
        public string ProcessType { get; set; }
        public int? CurrentStepId { get; set; }
        public int? NextStepId { get; set; }

        //======== Step 1 =======
        [JsonProperty("Title")]
        public string RoleName { get; set; }
        public int? ExperienceYears { get; set; }
        public int? NoOfTalents { get; set; }
        public string IsHiringLimited { get; set; }
        public string Availability { get; set; }
        public int? ContractDuration { get; set; }

        //======== Step 2 =======
        [JsonProperty("Must_have_Skills")]
        public string Skills { get; set; }
        [JsonProperty("good_to_have_skills")]
        public string AllSkills { get; set; }
        public string Currency { get; set; }
        public string CurrencySign { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }
        public bool? IsConfidentialBudget { get; set; }

        //======== Step 3 =======
        public string EmploymentType { get; set; }
        public string HowSoon { get; set; }
        public int? WorkingModeId { get; set; }
        public string CompanyLocation { get; set; }
        public long? AchievementId { get; set; }
        public int? TimeZoneId { get; set; }
        public int? TimezonePreferenceId { get; set; }
        public string? TimeZoneFromTime { get; set; }
        public string? TimeZoneEndTime { get; set; }
        public string? Reason { get; set; }
        public string? TimeZone { get; set; }
        public string? Region { get; set; }

        //======== Step 4 =======
        public string? RolesResponsibilities { get; set; }
        public string? Requirements { get; set; }
        public string? JDFileName { get; set; }
        public int? AchievedCount { get; set; }
        public string? JobDescription { get; set; }

        //Extra info
        public string? IST_TimeZone_FromTime { get; set; }
        public string? IST_TimeZone_EndTime { get; set; }

        //Location information
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? CountryID { get; set; }

        // New budget info 
        public int? BudgetType { get; set; }

        // Transparent pricing details
        public int? HiringTypePricingId { get; set; }
        public int? PayrollTypeId { get; set; }
        public string? PayrollPartnerName { get; set; }
        public string? HiringTypePricing { get; set; }
        public string? PayrollType { get; set; }
        public string? CompanyLogo { get; set; }
        public string? Whatweoffer { get; set; }
        public string? RoleOverviewDescription { get; set; }
        public List<string>? RoleOverviewDescriptionList { get; set; }
        public List<string>? RolesResponsibilitiesList { get; set; }
        public List<string>? RequirementsList { get; set; }
        public List<string>? WhatweofferList { get; set; }
        public List<string>? JobDesriptionList { get; set; }
        public bool? IsFresherAllowed { get; set; }
        public string? BudgetFromStr { get; set; }
        public string? BudgetToStr { get; set; }
        public string? HRCost { get; set; }

        // Vital Info 

        public string? CompensationOption { get; set; }
        public string? IndustryType { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
        public string? Prerequisites { get; set; }
        public string? StringSeparator { get; set; }
        public string? ToolTipMessage { get; set; }
        public int? HRTypeId { get; set; }


        // HR POC - UTS-UTS-7946
        public List<Sproc_HR_POC_ClientPortal_Result>? HRPOCUserID { get; set; }
        public bool? ShowHRPOCDetailsToTalents { get; set; }

        public int? JobTypeID { get; set; }
        public bool? ScreeningQuestionsExternallyModified { get; set; }
        public int? TotalScreeningQuestions { get; set; }
        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitID { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public long? ATS_JobLocation { get; set; }
        public string? ATS_NearByCities { get; set; }
        public bool? IsTransparentPricing { get; set; }
    }
    public class ChatGptResponseModel
    {
        public string? Job_Title { get; set; }
        public decimal? Budget_From { get; set; }
        public decimal? Budget_To { get; set; }
        public decimal? Max_Salary { get; set; }
        public string? Salary_Currency { get; set; }
        public string? Working_Zone_With_Time_Zone { get; set; }
        public string? Type_Of_Job { get; set; }
        public string? Opportunity_Type { get; set; }
        public string? Skills { get; set; }

        //[JsonProperty("Suggested Skills")]
        public string? Suggested_Skills { get; set; }
        public int? Years_Of_Experience { get; set; }
        public List<string>? Requirements { get; set; }
        public List<string>? Roles_And_Responsibilities { get; set; }

        [JsonProperty("Roles/Responsibilities")]
        public List<string>? RolesResponsibilities { get; set; }

        public List<string>? JobDescription { get; set; }
       

    }

    public class ClaudeAIResponseModel
    {
        public string Title { get; set; }
        [JsonProperty("Years_of_Experience_Required")]
        public string YearsofExperienceRequired { get; set; }
        public string Location { get; set; }
        public string Budget { get; set; }

        [JsonProperty("Role_Overview")]
        public List<string>? RoleOverviewDescription { get; set; }

        [JsonProperty("Key_Responsibilities")]
        public List<string>? RolesResponsibilities { get; set; }

        [JsonProperty("Key_Requirements")]
        public List<string>? Requirements { get; set; }

        [JsonProperty("What_we_offer")]
        public List<string>? Whatweoffer { get; set; }
        public string? Opportunity_Type { get; set; }

        public string? Budget_From { get; set; }

        public string? Budget_To { get; set; }
        public string? Max_Salary { get; set; }

        public string? Salary_Currency { get; set; }

        public string? Working_Zone_With_Time_Zone { get; set; }

        public string? Type_Of_Job { get; set; }
        [JsonProperty("Must_have_Skills")]
        public string? Skills { get; set; }

        [JsonProperty("good_to_have_skills")]
        public string? Suggested_Skills { get; set; }
        public string? JobDescription { get; set; }
    }
}
