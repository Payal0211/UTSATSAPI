using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class AchivedTargetViewModel
    {
        public int pagenumber { get; set; }

        public int totalrecord { get; set; }

        public AchivedTargetFilter achivedTargetFilter { get; set; }

    }
    public class AchivedTargetFilter
    {
        public string Client { get; set; }
        public string HRNumber { get; set; }
        public string EngagemenID { get; set; }
        public string TalentName { get; set; }
        public string User_Role { get; set; }
        public string InvoiceNumber { get; set; }
        public string CompanyCategory { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}
