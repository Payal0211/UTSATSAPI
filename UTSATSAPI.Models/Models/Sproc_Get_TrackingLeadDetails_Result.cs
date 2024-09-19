using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.Models
{
    [Keyless]

    public class Sproc_Get_UTS_TrackingLeadDetails_Result
    {
        public int? ID { get; set; }
        public string? Actions { get; set; }
        public int? value { get; set; }

    }
}
