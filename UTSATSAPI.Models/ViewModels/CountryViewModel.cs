using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CountryViewModel
    {
        public int ID { get; set; }
        public string CountryName { get; set; }
        public string CountryRegion { get; set; }
        public bool IsActive { get; set; }
    }
}
