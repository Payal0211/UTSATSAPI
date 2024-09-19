using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class CustomException
    {
        public long Id { get; set; }
        public string? FunctionName { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Description { get; set; }
        public string? MachineName { get; set; }
        public string? ExceptionSource { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? UserAgent { get; set; }
        public string? RequestUrl { get; set; }
    }
}
