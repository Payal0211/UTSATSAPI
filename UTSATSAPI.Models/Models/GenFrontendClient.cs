using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenFrontendClient
    {
        public int Id { get; set; }
        public string EmailId { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public DateTime? CreatedDatetime { get; set; }
    }
}
