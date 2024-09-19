using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class Sproc_Get_UpdateHR_Details_Result
    {
        public string HRNumber { get; set; }
        public string SalesUser { get; set; }
        public string RequestForTalent { get; set; }
        public string Discription { get; set; }
        public string JDFilename { get; set; }
        public string JDURL { get; set; }
        public string MonthDuration { get; set; }
        public string NoofTalents { get; set; }
        public string JobStatus { get; set; }
        public string OurObservation { get; set; }
        public string Remark { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedDatetime { get; set; }
        public string Availability { get; set; }
        public string NoofHoursworking { get; set; }
        public string TalentCostCalcPercentage { get; set; }
        public string DeleteHRReason { get; set; }
        public string NotAcceptedHRReason { get; set; }
        public string DeleteType { get; set; }
        public string TR_Accepted { get; set; }
        public string OnHoldRemark { get; set; }
        public string LossRemark { get; set; }
        public string AM_SalesPersonID { get; set; }
        public string NBD_SalesPersonID { get; set; }
        public string BQLink { get; set; }
        public string Discovery_Call { get; set; }
        public string DPPercentage { get; set; }
        public string about_Company_desc { get; set; }
        public string LastActivityDate { get; set; }
        public string OnHoldReminderFlag { get; set; }
        public string GUID { get; set; }
        public string PayrollPartnerName { get; set; }
        public string JobExpiredORClosedDate { get; set; }
        public string PauseHRReason { get; set; }
        public string PauseHRReasonOther { get; set; }
        public string NoofEmployee { get; set; }
        public string Duration { get; set; }
        public string DurationType { get; set; }
        public string Cost { get; set; }
        public string RoleStatus { get; set; }
        public string SpecificMonth { get; set; }
        public string YearOfExp { get; set; }
        public string HowSoon { get; set; }
        public string Timezone { get; set; }
        public string RoleTeamSize { get; set; }
        public string CommunicationType { get; set; }
        public string AssociatedwithUplers_ID { get; set; }
        public string PartialEngagementType_ID { get; set; }
        public string LastModifiedByID { get; set; }
        public string InterviewerName { get; set; }
        public string InterviewLinkedin { get; set; }
        public string InterviewerYearofExperience { get; set; }
        public string HR_Cost { get; set; }
        public string RolesResponsibilities { get; set; }
        public string GenericInfo { get; set; }
        public string Requirement { get; set; }
        public string Timezone_Preference { get; set; }
        public string Timezone_Preference_ID { get; set; }
        public string TimeZone_FromTime { get; set; }
        public string TimeZone_EndTime { get; set; }
        public string OverlapingHours { get; set; }
        public string InterviewerDesignation { get; set; }
        public string InterviewerEmailID { get; set; }
        public string BudgetFrom { get; set; }
        public string BudgetTo { get; set; }
        public string Currency { get; set; }
        public string AddOtherRole { get; set; }
        public string Adhoc_BudgetCost { get; set; }
        public string HistoryDate { get; set; }
        public string GlassdoorRating { get; set; }
        public string AmbitionBoxRating { get; set; }
        public string CalculatedUplersfees { get; set; }
        public string JobDescription { get; set; }
        public string MustHaveSkills { get; set; }
        public string GoodToHaveSkills { get; set; }
        public string EmploymentType { get; set; }
        public string PayrollType { get; set; }
        public string ModeOfWork { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        public string JobLocation { get; set; }
        public string FrequencyOfficeVisit { get; set; }
        public string IsOpenToWorkNearByCities { get; set; }
        public string NearByCities { get; set; }
    }
}
