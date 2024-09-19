using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class MatcherUserLists
    {
        public int? status { get; set; }
        public string? message { get; set; }
        public List<User>? users { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string email { get; set; }
    }
}
