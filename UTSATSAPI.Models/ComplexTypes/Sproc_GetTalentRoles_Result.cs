using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    public class Sproc_GetTalentRoles_Result
    {
        public int ID { get; set; }
        public string? TalentRole { get; set; }
        public string? FrontIconImage { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecords { get; set; }
        public string? TalentCategory { get; set; }
        public string? PackageCount { get; set; }
        public int existsOrNot { get; set; }
        public bool IsAdhoc { get; set; }
        public bool IsEnabled { get; set; }
        public long PitchMeRoleID { get; set; }
        public string? PitchMeRole { get; set; }
    }
}
