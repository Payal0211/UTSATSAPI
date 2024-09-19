using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactHistory
    {
        public long Id { get; set; }
        public long? ClientId { get; set; }
        public int? ClientActionId { get; set; }
        public DateTime? LoggedInTime { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public long? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public long? HiringRequestId { get; set; }
        public long? TalentId { get; set; }
        public long? AtstalentId { get; set; }
        public string? Location { get; set; }
        public string? Device { get; set; }
        public int? TalentStatusId { get; set; }
        public string? Ipaddress { get; set; }
        public string? Browser { get; set; }
        public int? AppActionDoneBy { get; set; }
        public long? UserId { get; set; }
    }
}
