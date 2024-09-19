using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTicketCategory
    {
        public int Id { get; set; }
        public string? TicketCategoryName { get; set; }
        public bool? IsActive { get; set; }
    }
}
