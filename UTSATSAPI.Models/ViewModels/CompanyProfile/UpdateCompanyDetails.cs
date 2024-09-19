using Microsoft.AspNetCore.Http;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels.CompanyProfile
{
    public class GetCompanyDetails
    {
        public Sproc_Get_Basic_CompanyDetails_Result BasicDetails { get; set; }
        public List<Sproc_Get_Company_Funding_Details_Result> FundingDetails { get; set; }
        public List<Sproc_Get_Company_CultureandPerksDetails_Result> CultureDetails { get; set; }
        public List<string?> PerkDetails { get; set; }
        public List<Sproc_Get_Company_YouTubeDetails_Result> YouTubeDetails { get; set; }
        public List<sproc_UTS_GetContactDetails_Result> ContactDetails { get; set; }
        public sproc_UTS_GetCompanyEngagementDetails_Result EngagementDetails { get; set; }
        public List<int?> PocUserIds { get; set; }
        public List<sp_UTS_GetPOCUserIDByCompanyID_Result> PocUserDetails { get; set; }
        public sp_UTS_GetPOCUserIDByCompanyID_Edit_Result PocUserDetailsEdit { get; set; }
        public long? PocUserId { get; set; }
        public bool? ShowWhatsappCTA { get; set; }
        public List<Sproc_UTS_GetCompanyWhatsappDetails_Result>? WhatsappDetails { get; set; }
    }
    public class UpdateCompanyDetails
    {
        public CompanyBasicDetails BasicDetails { get; set; }
        public List<CompanyFundingDetails> FundingDetails { get; set; }
        public List<CompanyCultureDetails> CultureDetails { get; set; }
        public List<string> PerkDetails { get; set; }
        public List<CompanyYouTubeDetails> YouTubeDetails { get; set; }
        public List<ClientDetails> ClientDetails { get; set; }
        public CompanyEngagementDetails EngagementDetails { get; set; }
        public List<int> PocIds { get; set; }
        public bool? IsRedirectFromHRPage { get; set; }
        public bool? IsUpdateFromPreviewPage { get; set; } = false;
        public long? PocId { get; set; }
        public long? HRID { get; set; }
        public string? Sales_AM_NBD { get; set; }
    }

    public class SummaryDetails
    {
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public List<SummaryClientDetails> summaryClients { get; set; }
        public bool? IsRedirectFromHRPage { get; set; }
    }
    public class SummaryClientDetails
    {
        public long? ClientID { get; set; }
        public string? ClientEmail { get; set; }
        public bool? isNewlyAdded { get; set; }
    }


    public class FileUploadViewModel
    {
        public List<IFormFile> Files { get; set; }
        public bool? IsCompanyLogo { get; set; }
        public bool? IsCultureImage { get; set; }
    }

    public class ValidateClient
    {
        public long? CurrentCompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? WebsiteURL { get; set; }
        public string? WorkEmail { get; set; }
    }
}
