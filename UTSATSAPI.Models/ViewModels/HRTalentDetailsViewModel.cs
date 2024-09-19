using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class HRTalentDetailsViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }

        public FilterFields_HRTalentDetail? filterFields { get; set; }
    }
    public class FilterFields_HRTalentDetail
    {
        public long? HRID { get; set; }
    }
}
