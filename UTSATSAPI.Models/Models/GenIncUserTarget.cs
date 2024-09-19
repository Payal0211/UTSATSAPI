using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenIncUserTarget
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public decimal? UserTarget { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public decimal? SelfTarget { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
