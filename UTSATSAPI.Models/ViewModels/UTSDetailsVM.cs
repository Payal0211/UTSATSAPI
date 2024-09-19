using Microsoft.AspNetCore.Mvc.Rendering;

namespace UTSATSAPI.Models.ViewModels
{
    public class UTSDetailsVM
    {
        public EsalesClientAM? esalesClientAM { get; set; }
        public List<ClientDetailsVM> ClientDetails { get; set; }
    }
    public class CompanyVM
    {
        public long? CompanyID { get; set; }
        public long? ContactID { get; set; }
    }
    public class EsalesAMDataResponse
    {
        public EsalesAMDataResponse()
        {
            EsalesClientAM ResponseEsalesClientAM = new EsalesClientAM();
        }
        public int Status { get; set; }
        public string Message { get; set; }

        public ResponseEsalesClientAM ResponseEsalesClientAM { get; set; }
    }

    public class EsalesClientAM
    {
        public string Managed_SelfManagedHR { get; set; }
        public string GEO { get; set; } //UK
        public string HR_Number { get; set; }
        public string EngagemenID { get; set; }
        //public string ClientName { get; set; }
        //public string ClientEmailId { get; set; }
        public string CompanyName { get; set; }
        //public string NBD_SalesPerson { get; set; }
        public string EngagementModel { get; set; } //Dedicated FTE
        public string NBD_Lead_EmployeeID { get; set; } //UP0147 get ID
        public string SalesPerson_EmployeeID { get; set; } //UP0147 get ID
        //public string NBD_LeadPerson { get; set; }
        public string CreatedByEmployeeId { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public string CompanySize { get; set; }
        public string phone { get; set; }
        public string industry { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string Zip { get; set; }
        public string CompanySource { get; set; }
        public string LinkedInProfile { get; set; }
        public string TalentName { get; set; }
    }

    public class ClientDetailsVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } //UK
        public string IsPrimary { get; set; }
        public string EmailID { get; set; }
        public string regions { get; set; } //UK
        public string skype { get; set; }
        public string city { get; set; }
    }

    public class ResponseEsalesClientAM
    {
        public string? SalesPerson_EmployeeID { get; set; }
        public string? HR_Number { get; set; }
        public string? EngagemenID { get; set; }
        public string? GEOLocationHeadEmployeeId { get; set; }
        public string? NBD_Lead_EmployeeID { get; set; }
        public string? AMName { get; set; }
    }

    public class ResponseAMAssignment
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public long AMUserID { get; set; }
        public string BAU_UTS_Tagging { get; set; }
    }


    public class AMTeam
    {
        public long Id { get; set; }
        public long AMId { get; set; }
        public string Type { get; set; }
        public long GEOID { get; set; }
        public int SortNo { get; set; }
        public string? Flag { get; set; }
        public IEnumerable<SelectListItem> AMList { get; set; }
        public IEnumerable<SelectListItem> GEOList { get; set; }
    }

    public class AMTeamDetails
    {
        public long Id { get; set; }
        public string AmName { get; set; }

        public string Type { get; set; }
        public string GEO { get; set; }
        public int GEOId { get; set; }
    }
}
