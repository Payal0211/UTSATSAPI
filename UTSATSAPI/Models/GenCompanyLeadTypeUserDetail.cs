using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenCompanyLeadTypeUserDetail
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public int? LeadTypeUserId { get; set; }
    }
}
