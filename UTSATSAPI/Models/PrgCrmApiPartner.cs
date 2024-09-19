using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgCrmApiPartner
    {
        public PrgCrmApiPartner()
        {
            PrgCrmApiDetails = new HashSet<PrgCrmApiDetail>();
        }

        public int Id { get; set; }
        public string? Apipartner { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<PrgCrmApiDetail> PrgCrmApiDetails { get; set; }
    }
}
