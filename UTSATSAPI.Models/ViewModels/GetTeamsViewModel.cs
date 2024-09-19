using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class GetTeamsViewModel : CommonFilterModel
    {
        public string ? filters { get; set; }
        public string? Team { get; set; }
        public string? DeptName { get; set; }
        public string? UserType { get; set; }
    }
}
