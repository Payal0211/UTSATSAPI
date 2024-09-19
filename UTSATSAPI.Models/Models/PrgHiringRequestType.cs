using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHiringRequestType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? CompanyTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
