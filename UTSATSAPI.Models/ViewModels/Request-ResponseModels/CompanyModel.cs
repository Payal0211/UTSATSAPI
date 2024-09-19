using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels.ResponseModels
{

    public class createClientRequest
    {
        public bool isSaveasDraft { get; set; }
        public Company company { get; set; }
        public Primaryclient primaryClient { get; set; }
        public Secondaryclient[] secondaryClients { get; set; }
        public Legalinfo legalInfo { get; set; }
        public poc[] pocList { get; set; }
        public string? primaryContactName { get; set; }
        public string? secondaryContactName { get; set; }
        public long contactId { get; set; }
        public string companyname { get; set; }
        public string clientemail { get; set; }
        public int? LeadUserId { get; set; }
    }

    public class Company
    {
        public string? company { get; set; }
        public string? website { get; set; }
        public string? location { get; set; }
        public string en_Id { get; set; }
        public long id { get; set; }
        public int? companySize { get; set; }
        public string? address { get; set; }
        public string? linkedinProfile { get; set; }
        public string? phone { get; set; }
        public int? teamManagement { get; set; }

        public string? LeadType { get; set; }
        public FileUploadModelBase64 fileUpload { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public decimal? JPCreditBalance { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public int? AnotherCompanyTypeID { get; set; }
        public bool? IsPostaJob { get; set; }
        public bool? IsProfileView { get; set; }
        public int? CompanyTypeID { get; set; }
        public bool? IsVettedProfile { get; set; }

        // Below properties added by Riya for UTS-6930
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public int? JobPostCredit { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }
    }

    public class poc
    {
        public long contactName { get; set; }
    }

    public class Primaryclient
    {
        public long id { get; set; }
        public string en_Id { get; set; }
        public string? fullName { get; set; }
        public string? emailId { get; set; }
        public string? contactNo { get; set; }
        public string? designation { get; set; }
        public string? linkedin { get; set; }
        public FileUploadModelBase64? fileUpload { get; set; }
        public string? PhotoImage { get; set; }
    }
    public class Legalinfo
    {
        public string en_Id { get; set; }
        public long id { get; set; }

        public string? name { get; set; }
        public string? email { get; set; }
        public string? designation { get; set; }
        public string? legalCompanyName { get; set; }
        public string? phoneNumber { get; set; }
        public string? legalCompanyAddress { get; set; }
        public bool isAcceptPolicy { get; set; }
    }
    public class Secondaryclient
    {
        public string en_Id { get; set; }
        public long id { get; set; }
        public string? fullName { get; set; }
        public string? emailID { get; set; }
        public string? countryCode { get; set; }
        public string? phoneNumber { get; set; }
        public string? designation { get; set; }
        public string? linkedinProfile { get; set; }
    }
}
