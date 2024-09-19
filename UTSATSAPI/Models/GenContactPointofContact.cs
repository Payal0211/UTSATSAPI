using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenContactPointofContact
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public int? UserId { get; set; }
        public string? Description { get; set; }
    }
}
