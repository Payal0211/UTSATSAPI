using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgCompanyDomain
    {
        public int Id { get; set; }
        public string? CompanyDomain { get; set; }
        public bool? IsActive { get; set; }
    }
}
