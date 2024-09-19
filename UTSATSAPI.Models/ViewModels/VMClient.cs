namespace UTSATSAPI.Models.ViewModel
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;

    public class VMClient
    {
        #region Constructor
        public VMClient()
        {
            Company = new GenCompany();
            primaryClient = new PrimaryClient();
            HiringRequest = new GenSalesHiringRequest();
            SalesHiringRequest_Details = new GenSalesHiringRequestDetail();
            LegalInfo = new LegalInfo();
        }
        #endregion

        #region Properties
        public string ActionType { get; set; }
        public bool isSaveasDraft { get; set; }
        public List<SelectListItem> Userslist { get; set; }
        public IEnumerable<MastersResponseModel> pointOfContactName { get; set; }
        public List<TalentPointofContact> talentPointofContact { get; set; }
        public List<PrimaryClient> secondaryClients { get; set; }
        public GenCompany Company { get; set; }
        public List<long> POCList { get; set; }
        public PrimaryClient primaryClient { get; set; }
        public GenSalesHiringRequest HiringRequest { get; set; }
        public string ActionName { get; set; }
        public string primaryContactName { get; set; }
        public string secondaryContactName { get; set; }
        public IEnumerable<SelectListItem> CompanyDomain { get; set; }
        public GenSalesHiringRequestDetail SalesHiringRequest_Details { get; set; }
        public string NameOfHiringRequest { get; set; }
        public IEnumerable<SelectListItem> HiringHowSoon { get; set; }
        public IEnumerable<SelectListItem> TalentRolesDropdown { get; set; }
        public Dictionary<string, string> BindLongTermShortTerm { get; set; }
        public IEnumerable<SelectListItem> HiringLongTermShortTerm { get; set; }
        public IEnumerable<SelectListItem> HiringRequestTimeZone { get; set; }
        public IEnumerable<SelectListItem> HiringStatus { get; set; }
        public Dictionary<string, string> BindHiringAvailability { get; set; }
        public IEnumerable<SelectListItem> HiringAvailability { get; set; }
        public LegalInfo LegalInfo { get; set; }
        public IEnumerable<SelectListItem> salesPersonDropdown { get; set; }
        public IEnumerable<SelectListItem> HiringRequestTimeZonePreference { get; set; }
        public IEnumerable<SelectListItem> TimeDropDown { get; set; }
        public long ContactId { get; set; }
        public IEnumerable<SelectListItem> PartialEngagementType { get; set; }
        public bool IsAdHocExistsHR { get; set; }
        public IEnumerable<SelectListItem> CurrencyDrp { get; set; }
        public string ClientHRAdminJDSkills { get; set; }
        public string HRAdminJDDescriptionAddClient { get; set; }
        public List<SelectListItem> AddClientHRSKillData { get; set; }
        public List<string> AddClientHRSkillIds { get; set; }
        public bool IsDraftSavedAddClientHR { get; set; }
        public Dictionary<int, string> BindTeamManagement { get; set; }
        public IEnumerable<SelectListItem> TeamManagementDrp { get; set; }
        public Dictionary<string, string> BindLeadType { get; set; }
        public IEnumerable<SelectListItem> LeadTypeDrp { get; set; }
        public Dictionary<Boolean, string> BindIsHiringLimited { get; set; }
        public IEnumerable<SelectListItem> IsHiringLimitedrp { get; set; }
        public List<SelectListItem> GEODrp { get; set; }
        public List<SecondaryContacts> secondaryContacts { get; set; }

        public string CompanyId { get; set; }
        public string PrimaryClientId { get; set; }
        public List<string> SecondaryClientIds { get; set; }
        public string LegalInfoId { get; set; }
        #endregion

    }
    #region Classes
    public class TalentPointofContact
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> listUsers { get; set; }
    }
    public class SkillOptionVMAddClientHRSkills
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
        public string Proficiency { get; set; }
        public string ID { get; set; }
        public string TempSkill_ID { get; set; }
    }

    public class SecondaryContacts
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    #endregion
}
