using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public int pagenumber { get; set; }
        public int totalrecord { get; set; }

        public FilterFieldsUserRole FilterFieldsUserRole { get; set; }
    }
    public class FilterFieldsUserRole
    {
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
    }
}
