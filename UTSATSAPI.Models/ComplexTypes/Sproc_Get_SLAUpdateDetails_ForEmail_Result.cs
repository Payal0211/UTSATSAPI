using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_SLAUpdateDetails_ForEmail_Result
    {
        public long HrId { get; set; }
        public long ContactId { get; set; }
        public string? RequestForTalent { get; set; } = string.Empty;
        public string? ClientName { get; set; } = string.Empty;
        public string? OldSlaDetails { get; set; } = string.Empty;
        public string? UpdatedSlaDetails { get; set; } = string.Empty;
        public string? UpdatedSlaReason { get; set; } = string.Empty;
        public string? ToClientEmail { get; set; } = string.Empty;
        public string? ToClientEmailName { get; set; } = string.Empty;
        public string? PocName { get; set; } = string.Empty;
        public string? PocEmail { get; set; } = string.Empty;
        public string? PocPhone { get; set; } = string.Empty;
        public string? PocDesignation { get; set; } = string.Empty;
        public long? SalesUserID { get; set; }
    }
}
