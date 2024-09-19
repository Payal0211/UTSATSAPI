using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_get_HRTalentProfileReason_Result
    {
        public string? ActualReason { get; set; }
        public string? RejectionComments { get; set; }
        public string? RejectionMessageToTalents { get; set; }
    }
}
