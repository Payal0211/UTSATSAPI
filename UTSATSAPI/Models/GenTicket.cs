using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTicket
    {
        public int Id { get; set; }
        public int? TicketCategoryId { get; set; }
        public int? TicketStatusId { get; set; }
        public string? TicketSubject { get; set; }
        public string? Brief { get; set; }
        public string? PageUrl { get; set; }
        public string? ScreenShot { get; set; }
        public string? VedioUrl { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? OtherCategory { get; set; }
        public string? Comments { get; set; }
        public int? TicketTechnicalJustificationId { get; set; }
    }
}
