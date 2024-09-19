using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSkillAndBudgetClientPortal
    {
        public long Id { get; set; }
        public long? Gptjdid { get; set; }
        public string? Guid { get; set; }
        public long? ContactId { get; set; }
        public string? Skills { get; set; }
        public string? AllSkills { get; set; }
        public string? Currency { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }
        public string? Ipaddress { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? ProcessType { get; set; }
        public bool? IsActive { get; set; }
        public int? BudgetType { get; set; }
        public bool? IsConfidentialBudget { get; set; }
    }
}
