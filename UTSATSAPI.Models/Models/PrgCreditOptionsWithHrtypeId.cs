using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgCreditOptionsWithHrtypeId
    {
        public int Id { get; set; }
        public int? CreditOptionId { get; set; }
        public int? HrtypeId { get; set; }
    }
}
