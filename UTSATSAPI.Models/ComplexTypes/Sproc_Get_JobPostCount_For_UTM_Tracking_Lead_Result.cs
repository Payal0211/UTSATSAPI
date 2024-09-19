using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_JobPostCount_For_UTM_Tracking_Lead_Result
    {
        public int? JobCount { get; set; }
    }
}
