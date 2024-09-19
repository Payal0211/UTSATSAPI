using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_HubSpot_Client_Funnel_Report_PopUP_Result
    {
        public string DealID { get; set; }
        public string SalesPerson { get; set; }
        public string StageName { get; set; }
        public string DealNumber { get; set; }
    }
}
