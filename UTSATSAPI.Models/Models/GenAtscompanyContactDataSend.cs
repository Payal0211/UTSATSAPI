using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAtscompanyContactDataSend
    {
        public long? CompanyId { get; set; }
        public long ContactId { get; set; }
        public bool? Isactive { get; set; }
        public int IsDataSend { get; set; }
    }
}
