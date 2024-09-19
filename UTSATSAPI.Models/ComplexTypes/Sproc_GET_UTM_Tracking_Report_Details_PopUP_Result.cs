using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GET_UTM_Tracking_Report_Details_PopUP_Result
    {
        public int? ID { get; set; }
        public string? Client { get; set; }
        public Int64? ClientId { get; set; }
        public string? HRNumber { get; set; }
        public Int64? HRID { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Browser { get; set; }
        public string? Device { get; set; }
        public string? IP_Address { get; set; }
        public string? Actions { get; set; }

    }
}
