using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class HiringReqeustModel
    {
        public bool issaveasdraft { get; set; }
        public bool isneedmore { get; set; }
        public string clientName { get; set; }
        public string companyName { get; set; }
        public long contactId { get; set; }
        public string? howsoon { get; set; }
        public string en_Id { get; set; }
        public long Id { get; set; }
        public string? Currency { get; set; }
        public string? BudgetType { get; set; }
        public decimal minimumBudget { get; set; }
        public decimal maximumBudget { get; set; }
        public bool? IsConfidentialBudget { get; set; } = false; //UTS-6304 Confidential budget
        public decimal? AdhocBudgetCost { get; set; }
        public decimal? NRMargin { get; set; }
        public int salesPerson { get; set; }
        public string ChildCompanyName { get; set; }
        public string? contractDuration { get; set; }
        public string? availability { get; set; }
        public int years { get; set; }
        public string? DurationType { get; set; }
        public int months { get; set; } //UTS- 3459: TODO: need to remove this property once the flow is checked.
        public int? timeZone { get; set; }
        public int? timeZonePreferenceId { get; set; }
        public string? timezone_Preference { get; set; }
        public int talentsNumber { get; set; }
        public string? dealID { get; set; }
        public string? bqFormLink { get; set; }
        public string? discoveryCallLink { get; set; }
        public string? modeOfWorkingId { get; set; }
        public bool isHRTypeDP { get; set; }
        public DirectPlacementViewModel directPlacement { get; set; }
        public JDInfo jDInfo { get; set; }
        public string jDFilename { get; set; }
        public string? jdURL { get; set; }
        public long? JDDump_ID { get; set; }
        public bool? IsHiringLimited { get; set; }
        public decimal? OverlapingHours { get; set; }
        public string? TimeZoneFromTime { get; set; }
        public string? TimeZoneEndTime { get; set; }
        public int? PartialEngagementTypeID { get; set; }
        public int? NoofHoursworking { get; set; }

        /// <summary>
        /// UTS-3998: Allow edit to specific set of users.
        /// </summary>
        public bool? AllowSpecialEdit { get; set; } = false;

        public Interviewer interviewerDetails { get; set; }

        //All for IsTransparent feature
        public bool? IsTransparentPricing { get; set; }
        public int? HrTypePricingId { get; set; }
        public int? HrTypeId { get; set; }
        public int? PayrollTypeId { get; set; }
        public string? PayrollPartnerName { get; set; }
        public string? CalculatedUplersfees { get; set; }
        public int? PayPerType { get; set; }
        public bool? isDirectHR { get; set; }
        public CompanyInfo? companyInfo { get; set; }
        public bool? IsPostaJob { get; set; } = false;
        public bool? IsProfileView { get; set; } = false;
        public bool? IsVettedProfile { get; set; }
        public bool? IsFresherAllowed { get; set; } = false;
        public string? ParsingType { get; set; }
        public string? jDDescription { get; set; }
        // Vital Info 

        public string? CompensationOption { get; set; }
        public string? HRIndustryType { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
        public string? Prerequisites { get; set; }
        public string? StringSeparator { get; set; }
        public bool? IsMustHaveSkillschanged { get; set; }
        public bool? IsGoodToHaveSkillschanged { get; set; }

        // HR POC - UTS-7946
        //public List<string>? HRPOCUserID { get; set; }
        public List<POCInfo>? HRPOCUserDetails { get; set; }
        public bool? ShowHRPOCDetailsToTalents { get; set; }

        // Job TypeId -- UTS-7976
        public int? JobTypeID { get; set; }
        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitID { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public long? ATS_JobLocationID { get; set; }
        public string? ATS_NearByCities { get; set; }
    }

    public class JDInfo
    {
        public string? jDURL { get; set; }
        public string? jDFilename { get; set; }
        public string? jDDescription { get; set; }
        public string? jDDump_ID { get; set; }
        public string? hdnSkills { get; set; }
    }

    public class CompanyInfo
    {
        public long? companyID { get; set; }
        public string? companyName { get; set; }
        public string? website { get; set; }
        public string? linkedInURL { get; set; }
        public string? industry { get; set; }
        public int? companySize { get; set; }
        public string? aboutCompanyDesc { get; set;}
        public bool? IsPostaJob { get; set; } = false;
        public bool? IsProfileView { get; set; } = false;
    }

    public class POCInfo
    {
        public long? POCUserID { get; set; }
        public string? ContactNo { get; set; }
        public bool? ShowEmailToTalent { get; set; }
        public bool? ShowContactNumberToTalent { get; set; }
    }
}