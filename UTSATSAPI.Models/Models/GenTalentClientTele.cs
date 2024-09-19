using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentClientTele
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? ClientTele { get; set; }
    }
}
