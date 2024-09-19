using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactGetCallBackHistory
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public DateTime? CallBackCreatedDatetime { get; set; }
        public bool IscallBackShow { get; set; }
    }
}
