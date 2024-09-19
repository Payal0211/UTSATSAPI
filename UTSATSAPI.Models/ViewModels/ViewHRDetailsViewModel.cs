using System.Diagnostics.Metrics;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

namespace UTSATSAPI.Models.ViewModels
{
    public class ViewHRDetailsViewModel
    {
        public long hrid { get; set; }
        public string hrNumber { get; set; }
        public string clientName { get; set; }
        public string company { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public string salesPerson { get; set; }
        public string hiringRequestRole { get; set; }
        public string hiringRequestTitle { get; set; }
        public string jobDescription { get; set; }
        public string JDURL { get; set; }
        public string currency { get; set; }
        public string hiringCost { get; set; }
        public decimal? budgetFrom { get; set; }
        public decimal? budgetTo { get; set; }
        public bool? IsConfidentialBudget { get; set; } = false; //UTS-6304 Confidential budget
        public string contractType { get; set; }
        public decimal? NRPercetange { get; set; }
        public int? contractDuration { get; set; }
        public decimal? requiredExperienceYear { get; set; }
        public string term { get; set; }
        public decimal? noOfTalents { get; set; }
        public string availability { get; set; }
        public string region { get; set; }
        public string? timeZone { get; set; }
        public string howSoon { get; set; }
        public long? dealID { get; set; }
        public string bqLink { get; set; }
        public string discoveryCall { get; set; }
        public string? modeOfWork { get; set; }
        public decimal? dpPercentage { get; set; }
        public string? rolesResponsibilites { get; set; }
        public string aboutCompany { get; set; }
        public string requirments { get; set; }
        public string Job_Description { get; set; }
        public List<SkillListVM> requiredSkillList { get; set; }
        public List<SkillListVM> GoodToHaveSkillList { get; set; }
        public List<InterviewerlList> interviewerlList { get; set; }
        public string? hrStatus { get; set; }
        public bool? IsHrfocused { get; set; }
        public bool? IsFresherAllowed { get; set; }

        /// <summary>
        /// Represents the GUID to identify if the HR is created from front-end.
        /// </summary>
        public string? Guid { get; set; }

        public bool? isDirectHR { get; set; }
        public string? DirectHRModeOfWork { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public sp_UTS_GetStandOutDetails_Result? AdditionalDetails { get; set; }
        public PayPerHireModel transparentModel { get; set; }
        public string? EngagementType { get; set; }
        public string? BudgetTitle { get; set; }
        public string? BudgetText { get; set; }
        public PayPerCreditModel PayPerCreditModel { get; set; }
        public bool? IsPayPerHire { get; set; }
        public bool? IsPayPerCredit { get; set; }
        public CompanyInfo? CompanyInfo { get; set; }
    }
    public class SkillListVM
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public string Proficiency { get; set; }
        public string ID { get; set; }
        public string TempSkill_ID { get; set; }
    }
    public class InterviewerlList
    {
        public string interviewerFullName { get; set; }
        public string interviewerEmail { get; set; }
        public string interviewerLinkedin { get; set; }
        public string interviewerDesignation { get; set; }
    }
    public class PayPerHireModel
    {
        //All for IsTransparent feature
        public bool? IsTransparentPricing { get; set; }
        public int? HrTypePricingId { get; set; }
        public string? HrTypePricing { get; set; }
        public decimal? PricingPercent { get; set; }
        public string? CalculatedUplersfees { get; set; }
        public int? HrTypeId { get; set; }
        public int? PayrollTypeId { get; set; }
        public string? PayrollType { get; set; }
        public string? PayrollPartnerName { get; set; }
        public string? ClientNeedsToPay { get; set; }
        public string? TalentsPay { get; set; }
        public List<string> JobType { get; set; }
        public string? EngagementType { get; set; }
        public string? EngagementText { get; set; }
        public string? BudgetTitle { get; set; }
        public string? BudgetText { get; set; }
        public List<string>? PayPerHire_I_Info { get; set; }
    }
    public class PayPerCreditModel
    {
        public List<string> JobType { get; set; }
        public string? EngagementType { get; set; }
        public string? BudgetTitle { get; set; }
        public string? BudgetText { get; set; }
        public bool? IsVettedProfile { get; set; }
    }
}
