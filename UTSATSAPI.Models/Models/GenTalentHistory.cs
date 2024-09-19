using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentHistory
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? TalentActionId { get; set; }
        public string? Name { get; set; }
        public string? EmailId { get; set; }
        public int? RoleId { get; set; }
        public string? ContactNumber { get; set; }
        public int? Status { get; set; }
        public int? CurrentEmploymentStatusId { get; set; }
        public int? TalentTypeId { get; set; }
        public int? TalentAccountStatusId { get; set; }
        public string? TalentCategory { get; set; }
        public string? TalentTimeZoneId { get; set; }
        public int? TalentStatusIdAfterClientSelection { get; set; }
        public string? Fees { get; set; }
        public decimal? FinalCost { get; set; }
        public bool? IsTalent { get; set; }
        public int? AssociatedwithUplersId { get; set; }
        public int? JoiningId { get; set; }
        public decimal? TotalExpYears { get; set; }
        public string? TypeofDeveloper { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public long? HiringRequestId { get; set; }
        public string? RemarkOrreason { get; set; }
    }
}
