using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetClientList_Result 
    {
        public long? CompanyID { get; set; }
        public long? ClientID { get; set; }
        public string? AddedDate { get; set; }
        public string? CompanyName { get; set; }
        public string? WebSite { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public string? GEO { get; set; }
        public string? POC { get; set; }
        public int? AM_SalesPersonID { get; set; }
        public string? AM_UserName { get; set; }
        public string? Status { get; set; }
        public string? StatusColor { get; set; }
        public string? InputSource { get; set; }
        public int TotalRecords { get; set; }
        public string? SourceCategory { get; set; }
        public string? CompanyModel { get; set; }
        public string? CreditUtilization { get; set; }
        public string? InviteName { get; set; }
        public string? InviteDate { get; set; }
        public string? SSO_Login { get; set; }
        public string? IsActive { get; set; }
        public bool? IsGSpaceCreated { get; set; }
        public string? AccessType { get; set; }
    }
}
