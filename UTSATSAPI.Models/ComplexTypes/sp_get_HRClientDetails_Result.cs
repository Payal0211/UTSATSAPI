using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_get_HRClientDetails_Result
    {
        public string? CompanyName { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public string? AM_UserName { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? Category { get; set; }
        public string? SalesPerson { get; set; }
        public string? Cost { get; set; }
        public string? Role { get; set; }
        public double? NoOfTalents { get; set; }
        public int? TR_Accepted { get; set; }
        public string? TimeZone { get; set; }
        public string? Managed { get; set; }
        public string? JobDetail { get; set; }
        public string? JobDetailURL { get; set; }
        public string? HR_Number { get; set; }
        public string? Priority { get; set; }
        public bool AdHocHR { get; set; }
        public string? Availability { get; set; }
        public string? AvailabilityParttime { get; set; }
        public int SpecificMonth { get; set; }
        public string? FromTimeAndToTime { get; set; }
        public bool PoolHR { get; set; }
        public string? Title { get; set; }
        public long ContactId { get; set; }
        public long? CompanyId { get; set; }
        public string? CompanyURL { get; set; }
        public string? POCFullName { get; set; }
        public string? POCEmailID { get; set; }
        public string? SalesManagerName { get; set; }
        public decimal MinYearOfExp { get; set; }
        public string? HRStatus { get; set; }
        public int HRStatusCode { get; set; }
        public int StarMarkedStatusCode { get; set; }
        public string? BQLink { get; set; }
        public string? Discovery_Call { get; set; }
        public string? GEO { get; set; }
        public string? ModeOfWork { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? CountryRegion { get; set; }
        public string? PostalCode { get; set; }
        public string? JDFileOrURL { get; set; }
        public string? LeadType { get; set; }
        public string? LeadUser { get; set; }
        public double? ActiveTR { get; set; }
        public string? ViewCompanyURL { get; set; }
        public string? HRTitle { get; set; }

        /// <summary>
        /// Added by Riya for identifying whether the HR is Pay per Hire/Credit
        /// </summary>
        public int? HRTypeId { get; set; }

        /// <summary>
        /// Added by Riya for identifying the Company type 
        /// </summary>
        public int? CompanyTypeID { get; set; }

        public bool? IsHybrid { get; set; }

    }
}
