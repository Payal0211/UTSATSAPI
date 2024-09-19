using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class TalentEngagementDetailsViewModel
    {
        public long? HiringRequest_ID { get; set; }
        public long? ATSTalentId { get; set; }
        public string? engagement_id { get; set; }
        public DateTime? engagement_start_date { get; set; }
        public DateTime? engagement_end_date { get; set; }
        public string? engagement_status { get; set; }
        public string? talent_status { get; set; }
        public DateTime? joining_date { get; set; }
        public DateTime? lost_date { get; set; }
        public DateTime? last_working_date { get; set; }
        public string? talent_statustag { get; set; }
        public string? Action { get; set; }
        public DateTime? Action_date { get; set; }

    }
}
