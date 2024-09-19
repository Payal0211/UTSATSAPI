using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenNurtureEmailLog
    {
        public long Id { get; set; }
        public long? ClientId { get; set; }
        public long? CompanyId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastEmailSentDateTime { get; set; }
        public int? EmailSentCount { get; set; }
        public int? SubjectId { get; set; }
    }
}
