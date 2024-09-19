using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CreditUtilizationListFilter
    {
        public long? CompanyId { get; set; }
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
    }
}
