using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.ComplexTypes;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels.Request_ResponseModels;

namespace UTSATSAPI.Models.ViewModels
{
    public class HiringRequest
    {
        public HiringRequest()
        {
            addHiringRequest = new GenSalesHiringRequest();
            SalesHiringRequest_Details = new GenSalesHiringRequestDetail();
            directPlacement = new();
            Skillmulticheckbox = new();
            AllSkillmulticheckbox = new();
        }

        public string en_Id { get; set; }
        public GenDirectPlacement directPlacement { get; set; }
        public string ModeOfWorkingId { get; set; }
        public string hdnModeOfWork { get; set; }
        public int? RoleID { get; set; }
        public long OnboardId { get; set; }
        public string jdfile { get; set; }
        public string jdfileExtension { get; set; }
        public GenSalesHiringRequest addHiringRequest { get; set; }
        public string company { get; set; }
        public string contact { get; set; }
        public GenSalesHiringRequestDetail SalesHiringRequest_Details { get; set; }
        public Interviewer interviewerDetails { get; set; }
        public List<SkillOptionVM> Skillmulticheckbox { get; set; }
        public List<SkillOptionVM> AllSkillmulticheckbox { get; set; }
        public string NameOfHiringRequest { get; set; }
        public bool IsAdHocExistsHR { get; set; }
        public bool? IsClientNotificationSent { get; set; }
        public bool IsDebriefSkillsAvailable { get; set; }
        //public string GDrivePickerClientID { get; } = Config.GDrivePickerCLIENT_ID;
        //public string GDrivePickerAPI_Key { get; } = Config.GDrivePickerAPI_KEY;
        //public string GDrivePickerAPP_ID { get; } = Config.GDrivePickerAPP_ID;
        public string HRGoogleDriveJD { get; set; }

        #region Create HR
        public string clientName { get; set; }
        public string fullClientName { get; set; }
        public decimal minimumBudget { get; set; }
        public decimal maximumBudget { get; set; }
        public string? DurationType { get; set; }
        public int months { get; set; } //UTS- 3459: TODO: Remove this when functionality is created.
        public string? Currency { get; set; }
        public string? contractDuration { get; set; }
        public string? BudgetType { get; set; }
        #endregion

        #region Create HR Brief
        public string? roleAndResponsibilites { get; set; }
        public string? requirements { get; set; }
        public Skill[] skills { get; set; }
        #endregion

        //UTS-3256: Created required fields in the model for close HR.
        #region CloseHR
        public List<Sproc_Check_Validation_Message_For_Close_HR_Result> Close_HR_Result { get; set; }
        public List<string> CloseEmailName { get; set; }

        #endregion

        public string CompanyCategory { get; set; }
        public string ModeofWorkAddress { get; set; }

        /// <summary>
        /// UTS-3998: Allow edit to specific set of users.
        /// </summary>
        public bool AllowSpecialEdit { get; set; } = false;

        public string? ChatGptSkills { get; set; }
        public string? ChatGptAllSkills { get; set; }
        public List<CompanyType> CompanyTypes { get; set; }
        public LoginUserHRInfo_Edit DirectHiringInfo_edit { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public string? CompensationOption { get; set; }
        public string? HRIndustryType { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
        public string? Prerequisites { get; set; }
        public string? StringSeparator { get; set; }
        public List<Sproc_HR_POC_ClientPortal_Result>? HRPOCUserID { get; set; }
        public bool? ShowHRPOCDetailsToTalents { get; set; }
    }

    public class Skill
    {
        public int skillsID { get; set; }
        public string skillsName { get; set; }
    }

    public class TalentRoles
    {
        public int ID { get; set; }
        public string TalentRole { get; set; }
        public int noOfYears { get; set; }
        public int noOfTalent { get; set; }
        public string DurationType { get; set; }
        public string Cost { get; set; }
        public int RoleStatus { get; set; }
        public string HiringRequest_RoleStatus { get; set; }
        public Int64 HiringRequestDetailId { get; set; }

        public TalentRoles()
        {
            noOfYears = 0;
            noOfTalent = 0;
        }
    }

    public class SkillOptionVM
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public string Proficiency { get; set; }
        public string ID { get; set; }
        public string TempSkill_ID { get; set; }
    }

    public class CompanyType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
