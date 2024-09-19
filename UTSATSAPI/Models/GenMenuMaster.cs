using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenMenuMaster
    {
        public GenMenuMaster()
        {
            GenMenuRights = new HashSet<GenMenuRight>();
        }

        public int Id { get; set; }
        public string MenuName { get; set; } = null!;
        public string MenuUrl { get; set; } = null!;
        public int MenuOrder { get; set; }
        public int? ParentMenuId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public string? LiClass { get; set; }
        public string? ControllerName { get; set; }

        public virtual ICollection<GenMenuRight> GenMenuRights { get; set; }
    }
}
