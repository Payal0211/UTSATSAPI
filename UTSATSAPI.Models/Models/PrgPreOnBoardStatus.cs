using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgPreOnBoardStatus
    {
        public int Id { get; set; }
        public string? PreOnBoardStatus { get; set; }
        public bool? IsDisplayToClient { get; set; }
        public bool? IsDisplayToTalent { get; set; }
        public bool? IsActive { get; set; }
    }
}
