using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetContactTimeZone_Result
    {
        public int ID { get; set; }
        public string? ShortName { get; set; }
        public string? TimeZoneTitle { get; set; }
        public string? Description { get; set; }
        
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecords { get; set; }
    }
}
