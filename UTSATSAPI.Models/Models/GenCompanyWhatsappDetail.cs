using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyWhatsappDetail
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string? GroupId { get; set; }
        public string? GroupName { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
