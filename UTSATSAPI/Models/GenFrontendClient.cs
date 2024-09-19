using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenFrontendClient
    {
        public int Id { get; set; }
        public string EmailId { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public DateTime? CreatedDatetime { get; set; }
    }
}
