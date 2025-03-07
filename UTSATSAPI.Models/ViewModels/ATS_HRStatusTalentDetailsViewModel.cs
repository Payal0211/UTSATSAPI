using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ATS_HRStatusTalentDetailsViewModel
    {
        public Nullable<long> HRID { get; set; }
        public string ATS_HR_Status { get; set; }
        public Nullable<System.DateTime> Published_Datetime { get; set; }
    }
}
