using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    [Keyless]
    public class Sproc_Get_AWS_SES_Tracking_Details_Result
    {
        public string? EventType { get; set; }
        public int? TotalRecords { get; set; }
    }
}
