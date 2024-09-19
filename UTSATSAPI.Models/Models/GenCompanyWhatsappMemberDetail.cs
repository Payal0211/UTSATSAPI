using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyWhatsappMemberDetail
    {
        public long Id { get; set; }
        public long? WhatsappDetailId { get; set; }
        public long? UserId { get; set; }
        public bool? IsAdmin { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
