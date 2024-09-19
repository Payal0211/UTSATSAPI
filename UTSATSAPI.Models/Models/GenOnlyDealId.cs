using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenOnlyDealId
    {
        public long Id { get; set; }
        public long? DealId { get; set; }
        public bool? IsInserted { get; set; }
    }
}
