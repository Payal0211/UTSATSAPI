using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgOnBoardPolicyDeviceMaster
    {
        public int Id { get; set; }
        public string? DeviceName { get; set; }
        public decimal? DeviceCost { get; set; }
        public decimal? DeviceCostInr { get; set; }
        public string? DeviceDescription { get; set; }
    }
}
