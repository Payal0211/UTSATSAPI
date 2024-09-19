using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenHrnotesTagUserDetail
    {
        public long Id { get; set; }
        public long? NoteId { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
