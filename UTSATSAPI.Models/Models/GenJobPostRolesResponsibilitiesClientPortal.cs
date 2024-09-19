using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenJobPostRolesResponsibilitiesClientPortal
    {
        public long Id { get; set; }
        public long? Gptjdid { get; set; }
        public string? Guid { get; set; }
        public string? RolesResponsibilities { get; set; }
        public string? Requirements { get; set; }
        public string? JdfileName { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? ClientIpaddress { get; set; }
        public string? Ipaddress { get; set; }
        public long? ContactId { get; set; }
        public string? ProcessType { get; set; }
        public bool? IsActive { get; set; }
        public string? JobDescription { get; set; }
        public string? JobRoleDescription { get; set; }
        public string? Whatweoffer { get; set; }
    }
}
