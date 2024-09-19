using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenHrnotesTagUserDetail
    {
        public long Id { get; set; }
        public long? NoteId { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
