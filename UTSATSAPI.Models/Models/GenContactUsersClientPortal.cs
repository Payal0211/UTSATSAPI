using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactUsersClientPortal
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? CompanyId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsActive { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public int? RoleId { get; set; }
    }
}
