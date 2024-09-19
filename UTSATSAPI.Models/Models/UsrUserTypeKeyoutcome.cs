using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class UsrUserTypeKeyoutcome
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }
        public string? UserTypeKeyoutcome { get; set; }
        public bool? IsPartofFunnel { get; set; }
        public bool? IsActive { get; set; }
    }
}
