using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.Models
{
    [Keyless]
    public class Sproc_Get_TrackingLead_Details_for_ClientSource_Result
    {
        public string? Actions { get; set; }
        public string? Source { get; set; }
        public string? Medium { get; set; }
        public string? Campaign { get; set; }
        public string? Content { get; set; }
        public string? Term { get; set; }
        public string? Placement { get; set; }
        public string? Ref_URL { get; set; }
    }
}
