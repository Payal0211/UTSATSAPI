using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class AcceptHRViewModel
    {
        public long HRID { get; set; }
        public int AcceptValue { get; set; }
        public string? Reason { get; set; }
        public bool? IsInternal { get; set; }
    }
}
