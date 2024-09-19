using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenOnBoardClientDevicePolicyDetail
    {
        public long Id { get; set; }
        public long? OnBoardId { get; set; }
        public int? DeviceId { get; set; }
        public int? Qty { get; set; }
        public decimal? TotalCost { get; set; }
        public bool? IsRecurring { get; set; }
        public string? ClientDeviceDescription { get; set; }
    }
}
