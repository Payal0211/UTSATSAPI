using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_OnBoardPolicy_DeviceMaster_Update_Result
    {
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<decimal> DeviceCost { get; set; }
        public int Qty { get; set; }
        public string Client_DeviceDescription { get; set; }
        public string DeviceDescription { get; set; }

    }
}
