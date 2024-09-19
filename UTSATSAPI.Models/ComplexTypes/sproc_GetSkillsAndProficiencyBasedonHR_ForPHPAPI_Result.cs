using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI_Result
    {
        public string Skill { get; set; }
        public string Proficiency { get; set; }
        public string TempSkill { get; set; }
    }
}
