using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenMenuRight
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int UserTypeId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }

        public virtual GenMenuMaster Menu { get; set; } = null!;
        public virtual UsrUserType UserType { get; set; } = null!;
    }
}
