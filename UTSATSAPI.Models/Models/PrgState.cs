using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgState
    {
        public int Id { get; set; }
        public string? State { get; set; }
        public int? CountryId { get; set; }
    }
}
