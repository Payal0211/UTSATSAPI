namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class HiringRequestDebriefModel
    {
        public string roleAndResponsibilites { get; set; }
        public string requirements { get; set; }
        public string JobDescription { get; set; }
        public string en_Id { get; set; }
        public long hrID { get; set; }
        //public Skill[] skills { get; set; }
        //public Skill[]? Allskills { get; set; }
        public string? skills { get; set; }
        public string? Allskills { get; set; }
        public Interviewer interviewerDetails { get; set; }
        public string? aboutCompanyDesc { get; set; }
        public bool isClient { get; set; }
        public long JDDumpID { get; set; }
        public decimal InterviewerYearOfExp { get; set; }
        public int? TypeOfInterviewerId { get; set; }
        public int? InterviewerExpInMonth { get; set; }
        public string? ActionType { get; set; } // Defines Save/Edit Action for debriefing.
        public bool? issaveasdraft { get; set; } = false; // Defines if we want to Save the debriefing as draft.
        public bool? IsHrfocused { get; set; } = false;

        /// <summary>
        /// UTS-3998: Allow edit to specific set of users.
        /// </summary>
        public bool? AllowSpecialEdit { get; set; } = false;
        /// <summary>
        /// Ashwin
        /// UTS-3989: Remove roles from HR form and Add it in Debriefing Section
        /// </summary>
        public int? role { get; set; }
        public string? otherRole { get; set; }
        public string? hrTitle { get; set; }

        public bool? isDirectHR { get; set; }

        public CompanyInfo? companyInfo { get; set; }
        public int? PayPerType { get; set; }
        public bool? IsMustHaveSkillschanged { get; set; }
        public bool? IsGoodToHaveSkillschanged { get; set; }
        public string? ParsingType { get; set; }
        public bool? IsAutogenerateQuestions { get; set; }

    }
    public class Skill
    {
        public string skillsID { get; set; }
        public string skillsName { get; set; }
    }
}
