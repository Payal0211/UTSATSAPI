using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactPointofContact
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public int? UserId { get; set; }
        public string? Description { get; set; }
        public long? CompanyId { get; set; }
    }
}
