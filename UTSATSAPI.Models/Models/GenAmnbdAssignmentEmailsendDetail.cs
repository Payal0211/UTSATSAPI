using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAmnbdAssignmentEmailsendDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? ContactId { get; set; }
        public int? AmSalesPersonId { get; set; }
        public int? NbdSalesPersonId { get; set; }
        public int? NeedEmailSend { get; set; }
    }
}
