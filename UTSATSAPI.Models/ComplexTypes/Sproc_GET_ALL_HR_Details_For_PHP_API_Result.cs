using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GET_ALL_HR_Details_For_PHP_API_Result
    {
        public long HiringRequest_ID { get; set; }
        public string? SalesUserName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmailID { get; set; }
        public long DealID { get; set; }
        public string? RequestForTalent { get; set; }
        public string? Discription { get; set; }
        public string? JDFilename { get; set; }
        public string? JDURL { get; set; }
        public decimal MonthDuration { get; set; }
        public int Quantity { get; set; }
        public string? HR_Status { get; set; }
        public string? HR_Sub_Status { get; set; }
        public string? OurObservation { get; set; }
        public string? Remark { get; set; }
        public string? CreatedbyDatetime { get; set; }
        public string? LastModifiedDatetime { get; set; }
        public string? HR_Number { get; set; }
        public string? Availability { get; set; }
        public string? PartialEngagementType { get; set; }
        public int NoofHoursworking { get; set; }
        public string? IsCreatedByClient { get; set; }
        public string? IsActive { get; set; }
        public string? IsAdHocHR { get; set; }
        public string? AcceptedBy { get; set; }
        public string? IsAccepted { get; set; }
        public string? IsManaged { get; set; }
        public string? DeleteHRReason { get; set; }
        public string? NotAcceptedHRReason { get; set; }
        public string? AdhocPassBy { get; set; }
        public string? AdhocPassDate { get; set; }
        public string? IsHiringLimited { get; set; }
        public string? IsPoolHR { get; set; }
        public string? InterviwerName { get; set; }
        public string? HR_Role { get; set; }
        public string? HR_ShiftTime { get; set; }
        public decimal YearOfExp { get; set; }
        public string? RolesResponsibilities { get; set; }
        public string? GenericInfo { get; set; }
        public string? Requirement { get; set; }
        public string? CompanyName { get; set; }
        public string? LinkedInURL { get; set; }
        public string? WebsiteURL { get; set; }
        public int TeamSize { get; set; }
        public string? CompamyLocation { get; set; }
        public long ContactID { get; set; }
        public long CompanyID { get; set; }
        public string? Cost { get; set; }
        public string? HowSoon { get; set; }
        public string? Currency { get; set; }
        public decimal Adhoc_BudgetCost { get; set; }
        public decimal BudgetFrom { get; set; }
        public decimal BudgetTo { get; set; }
        public string? AcceptedByDateTime { get; set; }
        public string? AboutCompany { get; set; }
        public bool IsHRTypeDP { get; set; }
        public decimal DPPercentage { get; set; }
        public decimal NRPercent { get; set; }
        public string? ModeOfWork { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public bool IsPartnerHR { get; set; }
        public long ChildCompanyID { get; set; }
        public string? ChildCompanyName { get; set; }
        public bool IsPartnerUser { get; set; }
        public string? SalesUserEmployeeID { get; set; }
        public string? CompanyGEO { get; set; }
        public string? CompanyIndustry_Type { get; set; }
        public string? HRSubmittedDate { get; set; }
        public int LostTRCount { get; set; }
        public string? DiscoveryCall { get; set; }
        public string? JDPath { get; set; }
        public string? ShiftStartTime { get; set; }
        public string? ShiftEndTime { get; set; }
        public string? IST_TimeZone_FromTime { get; set; }
        public string? IST_TimeZone_EndTime { get; set; }
        public decimal? OverlappingHours { get; set; }
        public string? IsHRFocused { get; set; }
        public bool IsDirectHR { get; set; }
        public int PricingId { get; set; }
        public string? PricingName { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public int PayrollTypeId { get; set; }
        public string? PayrollType { get; set; }
        public string? PayrollPartnerName { get; set; }
        public int? HRTypeId { get; set; }
        public string? HRTypeText { get; set; }
        public bool? IsHiringLimitedBool { get; set; }
        public bool? IsConfidentialBudget { get; set; }
        public bool? IsVettedProfile { get; set; }
        public bool? IsBackendHR { get; set; }
        public string? DurationType { get; set; }
        public string? JobDescription { get; set; }
        public bool? IsFresherAllowed { get; set; }
        public string? action_by_employee_id { get; set; }
        public string? Reference_HRNumber { get; set; }
        public string? SalesUserEmail { get; set; }
        public string? CompensationOption { get; set; }
        public string? CandidateIndustry { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
        public string? Prerequisites { get; set; }
        public bool? ShowHRPOCDetailsToTalents { get; set; }
        public string? SalesUserType { get; set; }
        public string? JobLocation { get; set; }
        public int? FrequencyOfficeVisitID { get; set; }
        public bool? IsOpenToWorkNearByCities { get; set; }
        public string? NearByCities { get; set; }
        public int? JobTypeID { get; set; }
        public string? SalesLead { get; set; }
        public long? ATS_JobLocationID { get; set; }
        public string? ATS_NearByCities { get; set; }
    }
}
