using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentRoleTemplate
    {
        public int Id { get; set; }
        public string? TemplateName { get; set; }
        public int? RoleId { get; set; }
        public string? SkillType { get; set; }
        public string? RequiredFor { get; set; }
        public string? SkillIds { get; set; }
        public int? MaxPoints { get; set; }
        public bool? IsActive { get; set; }
    }
}
