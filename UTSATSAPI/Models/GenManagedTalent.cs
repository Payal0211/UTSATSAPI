using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenManagedTalent
    {
        public long Id { get; set; }
        public string ManagedTalentFirstName { get; set; } = null!;
        public string ManagedTalentLastName { get; set; } = null!;
        public long ManagedHrid { get; set; }
        public decimal ManagedTalentCost { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public decimal Nrpercentage { get; set; }
        public decimal Hrcost { get; set; }
        public string? ManagedTalentEmailId { get; set; }
        public int? ManagedTalentRoleId { get; set; }
        public int? ManagedTalentTypeId { get; set; }
        public string? ManagedTalentLevel { get; set; }
        public string? ScopeOfWork { get; set; }
        public string? PocFullName { get; set; }
        public DateTime? ProposalConfirmDate { get; set; }
    }
}
