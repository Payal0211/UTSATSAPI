using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class HRActivityUsingPagination
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }

        public FilterFields_HRActivity? filterFields { get; set; }
    }
    public class FilterFields_HRActivity
    {
        public long? HRID { get; set; }
    }
}
