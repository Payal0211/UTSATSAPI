using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenSalesHrTracceptedDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public int? TrAccepted { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
