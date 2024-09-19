using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgBenchMark
    {
        public int Id { get; set; }
        public string? BenchMark { get; set; }
        public int? StartRange { get; set; }
        public int? EndRange { get; set; }
        public string? Color { get; set; }
        public bool? IsActive { get; set; }
    }
}
