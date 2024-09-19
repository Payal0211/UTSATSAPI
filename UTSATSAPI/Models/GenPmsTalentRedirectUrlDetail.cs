using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenPmsTalentRedirectUrlDetail
    {
        public long Id { get; set; }
        public long TalentId { get; set; }
        public string? RedirectUrl { get; set; }
        public string? PmsUuid { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public DateTime? ModifyByDatetime { get; set; }
    }
}
