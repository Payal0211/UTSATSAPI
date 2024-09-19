using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgIncSalgoalSlab
    {
        public int Id { get; set; }
        public string? Salgoal { get; set; }
        public int? MinLimitPercentage { get; set; }
        public int? MaxLimitPercentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
