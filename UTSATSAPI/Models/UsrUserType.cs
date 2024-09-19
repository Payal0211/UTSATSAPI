using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class UsrUserType
    {
        public UsrUserType()
        {
            GenMenuRights = new HashSet<GenMenuRight>();
        }

        public int Id { get; set; }
        public string? UserType { get; set; }
        public bool? IsDisplay { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsRequiredforFrontEnd { get; set; }
        public int? OrderSequence { get; set; }
        public bool? IsDisplayReporteeUser { get; set; }

        public virtual ICollection<GenMenuRight> GenMenuRights { get; set; }
    }
}
