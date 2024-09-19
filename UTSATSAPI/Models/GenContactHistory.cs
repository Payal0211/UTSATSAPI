using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
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
    }
}
