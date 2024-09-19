using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class HRDetailWithSkills
    {
        public HRDetailWithSkills()
        {
            skill = new List<SkillDetail>();
            HR_InterviewerDetails = new List<HR_InterviewerDetail>();
        }
        public Sproc_GET_ALL_HR_Details_For_PHP_API_Result HR_Details { get; set; }

        public List<SkillDetail> skill { get; set; }

        public List<HR_InterviewerDetail> HR_InterviewerDetails { get; set; }
    }

    public class HR_Data
    {
        public string Status { get; set; }
        public HRDetailWithSkills HRData { get; set; }
        public VitalInfo VitalInformation { get; set; }
        public List<HRPOC> HRPOC { get; set; }
        public bool? question_regeneration_needed { get; set; }
    }
    public class SkillDetail
    {
        public string SkillName { get; set; }
        public string Proficiency { get; set; }
        public string TempSkillName { get; set; }
    }

    public class HR_InterviewerDetail
    {
        public string InterviewerName { get; set; }
        public string InterviewerLinkedIn { get; set; }
        public decimal InterviewerYearsOfExp { get; set; }
        public int InterviewerTypeID { get; set; }
        public string InterviewerType { get; set; }
        public string InterviewerDesignation { get; set; }
        public string InterviewerEmailID { get; set; }
        public long HiringRequestID { get; set; }
        public long ID { get; set; }
    }

    public class VitalInfo
    {
        public string[] CompensationOption { get; set; }
        public string[] CandidateIndustry { get; set; }
        public string? Prerequisites { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
    }

    public class HRPOC
    {
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? ContactNo { get; set; }
        public bool? ShowEmailToTalent { get; set; }
        public bool? ShowContactNumberToTalent { get; set; }
    }
}
