using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class UserTeamViewModel
    {
        public long ID { get; set; }
        public Nullable<long> DeptID { get; set; }
        public string? Team { get; set; }
        public Nullable<long> TeamHeadID { get; set; }
        public Nullable<long> UserTypeID { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsTeamForNewUser { get; set; }
        public Nullable<long> RoleHeadID { get; set; }
        public IEnumerable<SelectListItem>? DepartmentDrp { get; set; }
        public IEnumerable<SelectListItem>? TeamHeadDrp { get; set; }
        public Dictionary<string, string>? BindIsActive { get; set; }
        public Dictionary<string, string>? BindIsNewUser { get; set; }
        public IEnumerable<SelectListItem>? DrpIsActive { get; set; }
        public IEnumerable<SelectListItem>? DrpIsNewUser { get; set; }
        public IEnumerable<SelectListItem>? RoleHeadDrp { get; set; }
    }
}
