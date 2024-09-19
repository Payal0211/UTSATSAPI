using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class DontHaveJdViewModel
    {
        public long? ContactID { get; set; }
        public long? HRID { get; set; }
        public List<string>? MustHaveSkill { get; set; }
        public List<string>? GoodToHaveSkill { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
    }

    
}
