using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class GetTalentProfileLogViewModel
    {
        public long talentid { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}
