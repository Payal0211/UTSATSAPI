using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenSystemConfiguration
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
        public string? Key { get; set; }
        public bool? IsActive { get; set; }
    }
}
