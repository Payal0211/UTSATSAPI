using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenClientenquiryDatum
    {
        public long Id { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientName { get; set; }
        public string? CompanyName { get; set; }
        public string? Password { get; set; }
        public string? ClientIpaddress { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? CompanyId { get; set; }
    }
}
