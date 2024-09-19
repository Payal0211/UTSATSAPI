using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgSlatime
    {
        public long Id { get; set; }
        public long ActionId { get; set; }
        public decimal Slatime { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public long? ModofiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public bool IsAdHoc { get; set; }
    }
}
