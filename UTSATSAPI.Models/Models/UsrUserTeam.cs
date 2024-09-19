using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class UsrUserTeam
    {
        public int Id { get; set; }
        public string? UserTeam { get; set; }
        public bool? IsActive { get; set; }
    }
}
