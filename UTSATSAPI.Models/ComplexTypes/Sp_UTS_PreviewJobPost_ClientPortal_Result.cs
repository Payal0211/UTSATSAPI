using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sp_UTS_PreviewJobPost_ClientPortal_Result
    {
        //1st Tab
        public string? RoleName { get; set; }
        public long? ContactId { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public int? ExperienceYears { get; set; }
        public bool? IsFresherAllowed { get; set; }
        public int? NoOfTalents { get; set; }
        public string? IsHiringLimited { get; set; }
        public string? Availability { get; set; }
        public int? ContractDuration { get; set; }

        //2nd Tab
        public string? Skills { get; set; }
        public string? AllSkills { get; set; }
        public string? Currency { get; set; }
        public string? CurrencySign { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }

        public bool? IsConfidentialBudget { get; set; }

        //3rd Tab
        public string? EmploymentType { get; set; }
        public string? HowSoon { get; set; }
        public int? WorkingModeID { get; set; }
        public string? ModeOfWorking { get; set; }
        public string? CompanyLocation { get; set; }
        public long? AchievementID { get; set; }
        public int? Timezone_Preference_ID { get; set; }
        public int? TimeZoneId { get; set; }
        public string? TimeZone_FromTime { get; set; }
        public string? TimeZone_EndTime { get; set; }
        public bool? IsHrTypeDP { get; set; }
        public string? Reason { get; set; }
        public string? TimeZone { get; set; }
        public string? Region { get; set; }

        //4th Tab
        public string? RolesResponsibilities { get; set; }
        public string? Requirements { get; set; }
        public string? JDFileName { get; set; }
        public string? JDLink { get; set; }
        public string? JobDescription { get; set; }

        //Step info
        public int? CurrentStepId { get; set; }
        public int? NextStepId { get; set; }
        public string? ProcessType { get; set; }

        // Date Info for all tabs
        public DateTime? FirstTabDate { get; set; }
        public DateTime? SecondTabDate { get; set; }
        public DateTime? ThirdTabDate { get; set; }
        public DateTime? FourthTabDate { get; set; }

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

        public long? GPTJDID { get; set; }
        public string? IST_TimeZone_FromTime { get; set; }
        public string? IST_TimeZone_EndTime { get; set; }

        public string? JobRoleDescription { get; set; }
        public string? Whatweoffer { get; set; }

        //Location information
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? CountryID { get; set; }

        public int? BudgetType { get; set; }

        // Transparent pricing details
        public int? HiringTypePricingId { get; set; }
        public int? PayrollTypeId { get; set; }
        public string? PayrollPartnerName { get; set; }
        public string? HiringTypePricing { get; set; }

        public string? PayrollType { get; set; }
        public int? HRTypeId { get; set; }
        public bool? ShowHRPOCDetailsToTalents { get; set; }
        public int? JobTypeID { get; set; }


        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitID { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public long? ATS_JobLocationID { get; set; }
        public string? ATS_NearByCities { get; set; }
        public bool? ScreeningQuestionsExternallyModified { get; set; }
        public int? TotalScreeningQuestions { get; set; }

        public bool? IsTransparentPricing { get; set; }

        //Extra info
        //public string? IST_TimeZone_FromTime { get; set; }
        //public string? IST_TimeZone_EndTime { get; set; }

        ////Location information
        //public string? City { get; set; }
        //public string? State { get; set; }
        //public string? PostalCode { get; set; }
        //public string? Country { get; set; }
        //public string? CountryID { get; set; }

        //// New budget info 
        //public int? BudgetType { get; set; }

        //// Transparent pricing details
        //public int? HiringTypePricingId { get; set; }
        //public int? PayrollTypeId { get; set; }
        //public string? PayrollPartnerName { get; set; }
        //public string? HiringTypePricing { get; set; }
        //public string? PayrollType { get; set; }

    }
}
