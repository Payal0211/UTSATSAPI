using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_GetCompanyList_Result
    {
        public long? ID { get; set; }
        public string? Company { get; set; }
        public string? LinkedInProfile { get; set; }
        public int? CompanySize { get; set; }
        public string? Phone { get; set; }
        public string? CompanyDomain { get; set; }
        public int? TotalRecords { get; set; }
        public string? Location { get; set; }
        public string? Contact_Status { get; set; }
        public string? Color { get; set; }
        public decimal? Score { get; set; }
        public string? Category { get; set; }
        public long? CompanyWeightedAverageCriteriaID { get; set; }
        public int? ExistsOrNot { get; set; }
        public string? GEO { get; set; }
        public string? TeamLead { get; set; }
        public string? AM_SalesPerson { get; set; }
        public string? NBD_SalesPerson { get; set; }
        public string? Lead_Type { get; set; }
        public string? LeadUser { get; set; }
    }
}
