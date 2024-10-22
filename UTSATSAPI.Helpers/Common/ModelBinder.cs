
namespace UTSATSAPI.Helpers.Common
{
    using System;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using static UTSATSAPI.Helpers.Enum;

    public static class ModelBinder
    {
        //#region FromOutside-Inside
        ////public static GenCompany BindCompanyModel(GenCompany base_genCompany, createClientRequest vMClient, int ClientStatusId, UTSATSAPIDBConnection _UTSATSAPIDBConnection, long LoggedInUserId = 0, bool isUpdate = false)
        ////{
        ////    if (base_genCompany == null)
        ////        base_genCompany = new GenCompany();

        ////    base_genCompany.Id = vMClient.company.id;
        ////    base_genCompany.Company = vMClient.company.company;
        ////    base_genCompany.LinkedInProfile = vMClient.company.linkedinProfile;
        ////    base_genCompany.Phone = vMClient.company.phone;
        ////    base_genCompany.CompanySize = vMClient.company.companySize;
        ////    base_genCompany.Address = vMClient.company.address;
        ////    base_genCompany.Website = vMClient.company.website;
        ////    base_genCompany.ClientStatusId = ClientStatusId;
        ////    //base_genCompany.TeamManagement = vMClient.company.teamManagement;
        ////    base_genCompany.LeadType = vMClient.company.LeadType;
        ////    base_genCompany.GeoId = (!string.IsNullOrEmpty(vMClient.company.location) ? _UTSATSAPIDBConnection.PrgGeos.Where(x => x.Geo.Equals(vMClient.company.location)).FirstOrDefault().Id : 0);
        ////    base_genCompany.AboutCompanyDesc = vMClient.company.AboutCompanyDesc;

        ////    base_genCompany.AnotherCompanyTypeId = vMClient.company.AnotherCompanyTypeID;
        ////    base_genCompany.IsPostaJob = vMClient.company.IsPostaJob;
        ////    base_genCompany.IsProfileView = vMClient.company.IsProfileView;
        ////    base_genCompany.CompanyTypeId = vMClient.company.CompanyTypeID;
        ////    base_genCompany.IsVettedProfile = vMClient.company.IsVettedProfile;


        ////    if (isUpdate)
        ////    {
        ////        base_genCompany.ModifiedByDatetime = DateTime.Now;
        ////        base_genCompany.ModifiedById = Convert.ToInt32(LoggedInUserId);
        ////        base_genCompany.IsTransparentPricing = vMClient.company.IsTransparentPricing;
        ////    }
        ////    else
        ////    {
        ////        base_genCompany.CreatedByDatetime = DateTime.Now;
        ////        base_genCompany.CreatedById = Convert.ToInt32(LoggedInUserId);
        ////    }
           
        ////    return base_genCompany;
        ////}

        //public static GenCompanyContractDetail BindCompanyContractDetailForClient(GenCompanyContractDetail genCompanyContractDetail, GenCompanyContractDetail result_genCompanyContractDetail, createClientRequest vMClient, long LoggedInUserId = 0, long InsertedCompanyId = 0, bool isUpdate = false)
        //{
        //    if (result_genCompanyContractDetail != null)
        //    {
        //        result_genCompanyContractDetail.Id = Convert.ToInt64(CommonLogic.Decrypt(vMClient.legalInfo.en_Id));
        //        result_genCompanyContractDetail.CompanyId = result_genCompanyContractDetail.CompanyId;
        //    }
        //    else
        //    {
        //        result_genCompanyContractDetail = new GenCompanyContractDetail();
        //        result_genCompanyContractDetail.CompanyId = InsertedCompanyId;
        //    }

        //    result_genCompanyContractDetail.SigningAuthorityFirstName = (!string.IsNullOrEmpty(vMClient.legalInfo.name) ? (vMClient.legalInfo.name.Contains(" ") ? vMClient.legalInfo.name.Split(" ")[0] : vMClient.legalInfo.name) : string.Empty); //
        //    result_genCompanyContractDetail.SigningAuthorityLastName = (!string.IsNullOrEmpty(vMClient.legalInfo.name) ? (vMClient.legalInfo.name.Contains(" ") ? vMClient.legalInfo.name.Split(" ")[1] : vMClient.legalInfo.name) : string.Empty);
        //    result_genCompanyContractDetail.SigningAuthorityName = vMClient.legalInfo.name;
        //    result_genCompanyContractDetail.SigningAuthorityEmail = vMClient.legalInfo.email;
        //    result_genCompanyContractDetail.SigningAuthorityDesignation = vMClient.legalInfo.designation;
        //    result_genCompanyContractDetail.LegalCompanyName = vMClient.legalInfo.legalCompanyName;
        //    result_genCompanyContractDetail.LegalCompanyAddress = vMClient.legalInfo.legalCompanyAddress;
        //    if (isUpdate)
        //    {
        //        result_genCompanyContractDetail.LastModifiedDatetime = DateTime.Now;
        //        result_genCompanyContractDetail.LastModifiedById = Convert.ToInt32(LoggedInUserId);
        //    }
        //    else
        //    {
        //        result_genCompanyContractDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        //        result_genCompanyContractDetail.CreatedByDatetime = DateTime.Now;

        //    }

        //    return result_genCompanyContractDetail;
        //}
        ////public static GenCompanyContractDetail BindCompanyContractDetailForClientWithID(GenCompanyContractDetail genCompanyContractDetail, GenCompanyContractDetail result_genCompanyContractDetail, createClientRequest vMClient, long LoggedInUserId = 0, long InsertedCompanyId = 0)
        ////{
        ////    if (result_genCompanyContractDetail != null)
        ////    {
        ////        genCompanyContractDetail.Id = vMClient.legalInfo.id;
        ////        genCompanyContractDetail.CompanyId = result_genCompanyContractDetail.CompanyId;
        ////    }
        ////    else
        ////    {
        ////        result_genCompanyContractDetail = new GenCompanyContractDetail();
        ////        result_genCompanyContractDetail.CompanyId = InsertedCompanyId;
        ////    }

        ////    result_genCompanyContractDetail.SigningAuthorityFirstName = (!string.IsNullOrEmpty(vMClient.legalInfo.name) ? (vMClient.legalInfo.name.Contains(" ") ? vMClient.legalInfo.name.Split(" ")[0] : vMClient.legalInfo.name) : string.Empty); //
        ////    result_genCompanyContractDetail.SigningAuthorityLastName = (!string.IsNullOrEmpty(vMClient.legalInfo.name) ? (vMClient.legalInfo.name.Contains(" ") ? vMClient.legalInfo.name.Split(" ")[0] : vMClient.legalInfo.name) : string.Empty);
        ////    result_genCompanyContractDetail.SigningAuthorityName = vMClient.legalInfo.name;
        ////    result_genCompanyContractDetail.SigningAuthorityEmail = vMClient.legalInfo.email;
        ////    result_genCompanyContractDetail.SigningAuthorityDesignation = vMClient.legalInfo.designation;
        ////    result_genCompanyContractDetail.LegalCompanyName = vMClient.legalInfo.legalCompanyName;
        ////    result_genCompanyContractDetail.LegalCompanyAddress = vMClient.legalInfo.legalCompanyAddress;
        ////    result_genCompanyContractDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        ////    return result_genCompanyContractDetail;
        ////}
        ////public static GenContact BindContact(GenContact genContact, createClientRequest vMClient, Secondaryclient secondaryClient = null, long LoggedInUserId = 0, long InsertedCompanyId = 0, bool isPrimary = false, bool IsActive = false, bool isUpdate = false)
        ////{
        ////    if (genContact == null)
        ////    {
        ////        genContact = new GenContact();
        ////    }
        ////    if (isPrimary)
        ////    {
        ////        string firstName = (!string.IsNullOrEmpty(vMClient.primaryClient.fullName) ? (vMClient.primaryClient.fullName.Contains(" ") ? vMClient.primaryClient.fullName.Split(" ")[0] : vMClient.primaryClient.fullName) : string.Empty);
        ////        string lastName = (!string.IsNullOrEmpty(vMClient.primaryClient.fullName) ? (vMClient.primaryClient.fullName.Contains(" ") ? vMClient.primaryClient.fullName.Split(" ")[1] : vMClient.primaryClient.fullName) : string.Empty);

        ////        genContact.EmailId = vMClient.primaryClient.emailId;
        ////        genContact.FirstName = firstName;
        ////        genContact.LastName = lastName;
        ////        genContact.Username = vMClient.primaryClient.emailId;
        ////        genContact.LinkedIn = vMClient.primaryClient.linkedin;
        ////        genContact.Designation = vMClient.primaryClient.designation;
        ////        genContact.CompanyId = InsertedCompanyId;
        ////        genContact.IsPrimary = isPrimary;
        ////        //genContact.PhotoImage = vMClient.primaryClient.PhotoImage ?? "clientDefaultImage.png";
        ////        //genContact.IsActive = IsActive;
        ////        genContact.FullName = vMClient.primaryClient.fullName;
        ////        genContact.ContactNo = vMClient.primaryClient.contactNo;
        ////    }
        ////    else
        ////    {
        ////        string firstName = (!string.IsNullOrEmpty(secondaryClient.fullName) ? (secondaryClient.fullName.Contains(" ") ? secondaryClient.fullName.Split(" ")[0] : secondaryClient.fullName) : string.Empty);
        ////        string lastName = (!string.IsNullOrEmpty(secondaryClient.fullName) ? (secondaryClient.fullName.Contains(" ") ? secondaryClient.fullName.Split(" ")[1] : secondaryClient.fullName) : string.Empty);
        ////        genContact.EmailId = secondaryClient.emailID;
        ////        genContact.FirstName = firstName;
        ////        genContact.LastName = lastName;
        ////        genContact.Username = secondaryClient.emailID;
        ////        genContact.LinkedIn = secondaryClient.linkedinProfile;
        ////        genContact.Designation = secondaryClient.designation;
        ////        genContact.CompanyId = InsertedCompanyId;
        ////        genContact.IsPrimary = isPrimary;
        ////        //genContact.PhotoImage = "clientDefaultImage.png";
        ////        //genContact.IsActive = IsActive;
        ////        genContact.FullName = secondaryClient.fullName;
        ////        genContact.ContactNo = secondaryClient.phoneNumber;
        ////    }
        ////    if (isUpdate)
        ////    {
        ////        genContact.LastModifiedDatetime = DateTime.Now;
        ////        genContact.LastModifiedById = Convert.ToInt32(LoggedInUserId);
        ////    }
        ////    else
        ////    {
        ////        genContact.CreatedByDatetime = DateTime.Now;
        ////        genContact.CreatedById = Convert.ToInt32(LoggedInUserId);
        ////    }
        ////    //if (vMClient.isSaveasDraft)
        ////    //    genContact.IsActive = false;
        ////    //else
        ////    //    genContact.IsActive = true;
        ////    return genContact;
        ////}

        //public static GenSalesHiringRequest BindgenSalesHiringRequest(GenSalesHiringRequest genSalesHiringRequest, HiringReqeustModel hiringRequest, long LoggedInUserId, bool isCreate = false)
        //{

        //    bool isHRActive = false;

        //    if (genSalesHiringRequest == null)
        //        genSalesHiringRequest = new GenSalesHiringRequest();
        //    else
        //        isHRActive = genSalesHiringRequest.IsActive ?? false;

        //    genSalesHiringRequest.Id = hiringRequest.Id;
        //    genSalesHiringRequest.ContactId = hiringRequest.contactId;

        //    genSalesHiringRequest.DealId = hiringRequest.dealID != null ? Convert.ToInt64(hiringRequest.dealID) : 0;
        //    genSalesHiringRequest.NoofTalents = hiringRequest.talentsNumber;
        //    genSalesHiringRequest.Availability = hiringRequest.availability;
        //    if (isCreate)
        //    {
        //        genSalesHiringRequest.StatusId = (short)prg_HiringRequestStatus.Open;
        //        genSalesHiringRequest.HrNumber = string.Format("HR{0}", DateTime.Now.ToString("ddMMyyHHmmss"));
        //        genSalesHiringRequest.JddumpId = hiringRequest.JDDump_ID; //Store the DumpID only once.
        //    }

        //    genSalesHiringRequest.Jdfilename = hiringRequest.jDFilename;
        //    genSalesHiringRequest.Jdurl = hiringRequest.jdURL;

        //    genSalesHiringRequest.SalesUserId = hiringRequest.salesPerson;
        //    if (isCreate)
        //    {
        //        genSalesHiringRequest.CreatedById = Convert.ToInt32(LoggedInUserId);
        //        genSalesHiringRequest.CreatedByDatetime = DateTime.Now;
        //    }
        //    else
        //    {
        //        genSalesHiringRequest.LastModifiedById = Convert.ToInt32(LoggedInUserId);
        //        genSalesHiringRequest.LastModifiedDatetime = DateTime.Now;
        //    }

        //    if (genSalesHiringRequest.IsAdHocHr)
        //    {
        //        genSalesHiringRequest.AdhocPassBy = Convert.ToInt32(LoggedInUserId);
        //        genSalesHiringRequest.AdhocPassDate = DateTime.Now;
        //    }
        //    else
        //    {
        //        genSalesHiringRequest.AdhocPassBy = 0;
        //        genSalesHiringRequest.AdhocPassDate = null;
        //        genSalesHiringRequest.IsPoolHr = true;
        //    }

        //    if (isCreate || hiringRequest.issaveasdraft)
        //    {
        //        genSalesHiringRequest.IsActive = false;
        //        genSalesHiringRequest.JobStatusId = (short)prg_JobStatus_ClientPortal.Draft;
        //    }
        //    else
        //    {
        //        if (isHRActive)
        //            genSalesHiringRequest.IsActive = true;
        //    }

        //    genSalesHiringRequest.PartialEngagementTypeId = hiringRequest.PartialEngagementTypeID ?? 0;
        //    genSalesHiringRequest.NoofHoursworking = hiringRequest.NoofHoursworking;

        //    #region update hr typeID & status

        //    if (hiringRequest.PayPerType == 1)//Pay per hire
        //    {
        //        genSalesHiringRequest.IsDirectHr = hiringRequest.isDirectHR ?? false;
        //        genSalesHiringRequest.HrtypeId = (short)PayPerHire.SalesHR;
        //    }
        //    if (hiringRequest.PayPerType == 2)//Pay per credit
        //    {
        //        genSalesHiringRequest.HrtypeId = (short)PayPerCredit.PostaJobCreditBased;

        //        if ((bool)hiringRequest.IsPostaJob && (bool)hiringRequest.IsProfileView)
        //        {
        //            genSalesHiringRequest.HrtypeId = (short)PayPerCredit.PostaJobViewBasedCredit;
        //        }
        //        if ((bool)hiringRequest.IsPostaJob && !(bool)hiringRequest.IsProfileView)
        //        {
        //            genSalesHiringRequest.HrtypeId = (short)PayPerCredit.PostaJobCreditBased;
        //        }
        //        if (!(bool)hiringRequest.IsPostaJob && (bool)hiringRequest.IsProfileView)
        //        {
        //            genSalesHiringRequest.HrtypeId = (short)PayPerCredit.PostajobWithViewCreditsButnoJobPostCredits;
        //        }
        //    }

        //    #endregion

        //    #region update Pay per hire/Pay per credit Fields

        //    //Pay per hire Fields
        //    if (hiringRequest.PayPerType == 1)
        //    {
        //        genSalesHiringRequest.IsHrtypeDp = hiringRequest.isHRTypeDP;
        //        if (hiringRequest.isHRTypeDP)
        //            genSalesHiringRequest.Dppercentage = hiringRequest.directPlacement.DpPercentage;
        //        else
        //            genSalesHiringRequest.TalentCostCalcPercentage = Convert.ToDecimal(hiringRequest.NRMargin);

        //        genSalesHiringRequest.Bqlink = hiringRequest.bqFormLink;
        //        genSalesHiringRequest.DiscoveryCall = hiringRequest.discoveryCallLink;

        //        //All for IsTransparent feature
        //        genSalesHiringRequest.IsTransparentPricing = hiringRequest.IsTransparentPricing;
        //        genSalesHiringRequest.HiringTypePricingId = hiringRequest.HrTypePricingId;
        //        genSalesHiringRequest.HrTypeId1 = hiringRequest.HrTypeId;

        //        if (hiringRequest.PayrollTypeId == null || hiringRequest.PayrollTypeId == 0)
        //        {
        //            genSalesHiringRequest.PayrollTypeId = 1; // default Uplers payroll
        //        }
        //        else
        //        {
        //            genSalesHiringRequest.PayrollTypeId = hiringRequest.PayrollTypeId;
        //        }

        //        if (hiringRequest.PayrollTypeId == 3)
        //        {
        //            genSalesHiringRequest.PayrollPartnerName = hiringRequest.PayrollPartnerName;
        //        }
        //    }
        //    //Pay per credit Fields
        //    if (hiringRequest.PayPerType == 2)
        //    {
        //        genSalesHiringRequest.IsHrtypeDp = true;
        //        genSalesHiringRequest.Dppercentage = 0;
        //        genSalesHiringRequest.TalentCostCalcPercentage = 0;
        //        genSalesHiringRequest.IsHiringLimited = hiringRequest.IsHiringLimited;
        //        genSalesHiringRequest.IsVettedProfile = hiringRequest.IsVettedProfile;
        //    }

        //    #endregion

        //    return genSalesHiringRequest;
        //}

        //public static GenSalesHiringRequestDetail BindgenSalesHiringRequestDetail(GenSalesHiringRequestDetail genSalesHiringRequestDetail, HiringReqeustModel hiringRequest, long hiringrequestId, long LoggedInUserId, UTSATSAPIDBConnection _UTSATSAPIDBConnection)
        //{
        //    if (genSalesHiringRequestDetail == null)
        //        genSalesHiringRequestDetail = new GenSalesHiringRequestDetail();

        //    genSalesHiringRequestDetail.Id = genSalesHiringRequestDetail.Id;
        //    genSalesHiringRequestDetail.HiringRequestId = hiringrequestId;
        //    genSalesHiringRequestDetail.DurationType = string.IsNullOrEmpty(hiringRequest.DurationType) ? "Long Term" : hiringRequest.DurationType; //Represents LongTerm/Short Term

        //    if (hiringRequest.contractDuration == "-1")
        //    {
        //        genSalesHiringRequestDetail.SpecificMonth = -1;
        //    }
        //    else
        //    {
        //        genSalesHiringRequestDetail.SpecificMonth = string.IsNullOrEmpty(hiringRequest.contractDuration) ? 0 : Convert.ToInt32(hiringRequest.contractDuration); // Represents contract duration months.
        //    }

        //    genSalesHiringRequestDetail.YearOfExp = hiringRequest.years;
        //    genSalesHiringRequestDetail.TimezoneId = hiringRequest.timeZone;
        //    genSalesHiringRequestDetail.TimezonePreferenceId = null; //UTS-4997: Save the preference id as null.
        //    genSalesHiringRequestDetail.TimezonePreference = hiringRequest.timezone_Preference;
        //    genSalesHiringRequestDetail.HowSoon = hiringRequest.howsoon;
        //    genSalesHiringRequestDetail.NoofEmployee = hiringRequest.talentsNumber;

        //    //genSalesHiringRequestDetail.RoleStatusId = 2;

        //    genSalesHiringRequestDetail.Currency = hiringRequest.Currency;
        //    genSalesHiringRequestDetail.IsConfidentialBudget = hiringRequest.IsConfidentialBudget ?? false; //UTS-6304 Confidential budget

        //    if (hiringRequest.BudgetType == "1")
        //    {
        //        genSalesHiringRequestDetail.AdhocBudgetCost = hiringRequest.AdhocBudgetCost;
        //        genSalesHiringRequestDetail.BudgetFrom = 0;
        //        genSalesHiringRequestDetail.BudgetTo = 0;
        //    }
        //    else if (hiringRequest.BudgetType == "2")
        //    {
        //        genSalesHiringRequestDetail.AdhocBudgetCost = 0;
        //        genSalesHiringRequestDetail.BudgetFrom = (hiringRequest.minimumBudget == null) ? 0 : hiringRequest.minimumBudget;
        //        genSalesHiringRequestDetail.BudgetTo = (hiringRequest.maximumBudget == null) ? 0 : hiringRequest.maximumBudget;
        //    }
        //    else if (hiringRequest.BudgetType == "3")
        //    {
        //        genSalesHiringRequestDetail.AdhocBudgetCost = 0;
        //        genSalesHiringRequestDetail.BudgetFrom = 0;
        //        genSalesHiringRequestDetail.BudgetTo = 0;
        //    }

        //    var CurrencySign = _UTSATSAPIDBConnection.PrgCurrencyExchangeRates.Where(x => x.CurrencyCode == hiringRequest.Currency).Select(x => x.CurrencySign).FirstOrDefault();
        //    if (hiringRequest.minimumBudget != null && hiringRequest.minimumBudget != 0 && hiringRequest.maximumBudget != null && hiringRequest.maximumBudget != 0)
        //    {
        //        genSalesHiringRequestDetail.Cost = (!string.IsNullOrEmpty(CurrencySign) ? CurrencySign : "") + hiringRequest.minimumBudget + " to " + (!string.IsNullOrEmpty(CurrencySign) ? CurrencySign : "") + hiringRequest.maximumBudget + " " + hiringRequest.Currency + " / Month";

        //    }
        //    else if (hiringRequest.minimumBudget == 0 && hiringRequest.maximumBudget == 0 && hiringRequest.AdhocBudgetCost > 0)
        //    {
        //        genSalesHiringRequestDetail.Cost = (!string.IsNullOrEmpty(CurrencySign) ? CurrencySign : "") + " " + hiringRequest.AdhocBudgetCost + " " + hiringRequest.Currency + " / Month";
        //    }
        //    else
        //        genSalesHiringRequestDetail.Cost = ""; //Open for any budget


        //    genSalesHiringRequestDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genSalesHiringRequestDetail.CreatedByDatetime = DateTime.Now;

        //    genSalesHiringRequestDetail.OverlapingHours = hiringRequest.OverlapingHours;
        //    genSalesHiringRequestDetail.TimeZoneFromTime = hiringRequest.TimeZoneFromTime;
        //    genSalesHiringRequestDetail.TimeZoneEndTime = hiringRequest.TimeZoneEndTime;

        //    #region Commented
        //    //genSalesHiringRequestDetail.AddOtherRole = (hiringRequest.otherRole == null) ? string.Empty : hiringRequest.otherRole;
        //    //if (hiringRequest.role == -1)
        //    //{
        //    //    var roleid = _UTSATSAPIDBConnection.PrgTalentRoles.FirstOrDefault(x => x.TalentRole == hiringRequest.otherRole);
        //    //    if (roleid != null) { genSalesHiringRequestDetail.RoleId = roleid.Id; } else { genSalesHiringRequestDetail.RoleId = 0; }
        //    //}
        //    //else
        //    //{
        //    //    genSalesHiringRequestDetail.RoleId = hiringRequest.role;
        //    //}
        //    //genSalesHiringRequestDetail.NoofEmployee = hiringRequest.addHiringRequest.NoofTalents;

        //    //genSalesHiringRequestDetail.TimezonePreferenceId = hiringRequest.SalesHiringRequest_Details.TimezonePreferenceId;
        //    //genSalesHiringRequestDetail.InterviewerId = hiringRequest.SalesHiringRequest_Details.InterviewerId;
        //    //genSalesHiringRequestDetail.CommunicationType = hiringRequest.SalesHiringRequest_Details.CommunicationType;
        //    //genSalesHiringRequestDetail.TypeofInterviewerId = hiringRequest.SalesHiringRequest_Details.TypeofInterviewerId;
        //    //genSalesHiringRequestDetail.RoleTeamSize = hiringRequest.SalesHiringRequest_Details.RoleTeamSize;

        //    //genSalesHiringRequestDetail.TimeZoneFromTime = hiringRequest.SalesHiringRequest_Details.TimeZoneFromTime;
        //    //genSalesHiringRequestDetail.TimeZoneEndTime = hiringRequest.SalesHiringRequest_Details.TimeZoneEndTime;
        //    //genSalesHiringRequestDetail.OverlapingHours = hiringRequest.SalesHiringRequest_Details.OverlapingHours;

        //    //genSalesHiringRequestDetail.Cost = hiringRequest.SalesHiringRequest_Details.Cost;
        //    //genSalesHiringRequestDetail.AdhocBudgetCost = (hiringRequest.SalesHiringRequest_Details.AdhocBudgetCost == null) ? 0 : hiringRequest.SalesHiringRequest_Details.AdhocBudgetCost;




        //    //if (hiringRequest.SalesHiringRequest_Details.AdhocBudgetCost != null && hiringRequest.SalesHiringRequest_Details.AdhocBudgetCost != 0)
        //    //    genSalesHiringRequestDetail.Cost = (!string.IsNullOrEmpty(CurrencySign) ? CurrencySign : "") + hiringRequest.SalesHiringRequest_Details.AdhocBudgetCost + " " + hiringRequest.SalesHiringRequest_Details.Currency + " / Month";


        //    //genSalesHiringRequestDetail.Remark = hiringRequest.SalesHiringRequest_Details.Remark; 
        //    #endregion

        //    return genSalesHiringRequestDetail;
        //}

        //public static GenSalesHiringRequestDetail BindgenSalesHiringRequestDetailForDebriefing(GenSalesHiringRequestDetail genSalesHiringRequestDetail, HiringRequestDebriefModel hiringRequest, long LoggedInUserId)
        //{
        //    genSalesHiringRequestDetail.RolesResponsibilities = hiringRequest.roleAndResponsibilites ?? "";
        //    genSalesHiringRequestDetail.Requirement = hiringRequest.requirements ?? "";

        //    genSalesHiringRequestDetail.JobDescription = hiringRequest.JobDescription;
        //    genSalesHiringRequestDetail.HiringRequestId = hiringRequest.hrID;
        //    genSalesHiringRequestDetail.LastModifiedById = Convert.ToInt32(LoggedInUserId);

        //    if (hiringRequest.interviewerDetails != null && hiringRequest.interviewerDetails.primaryInterviewer != null)
        //    {
        //        genSalesHiringRequestDetail.InterviewerDesignation = hiringRequest.interviewerDetails.primaryInterviewer.designation;
        //        genSalesHiringRequestDetail.InterviewerEmailId = hiringRequest.interviewerDetails.primaryInterviewer.emailID;
        //        genSalesHiringRequestDetail.InterviewerName = hiringRequest.interviewerDetails.primaryInterviewer.fullName;
        //        genSalesHiringRequestDetail.InterviewLinkedin = hiringRequest.interviewerDetails.primaryInterviewer.linkedin;
        //    }

        //    genSalesHiringRequestDetail.InterviewerYearofExperience = hiringRequest.InterviewerYearOfExp;
        //    genSalesHiringRequestDetail.InterviewerExpInMonth = hiringRequest.InterviewerExpInMonth;
        //    genSalesHiringRequestDetail.TypeofInterviewerId = hiringRequest.TypeOfInterviewerId;
        //    genSalesHiringRequestDetail.IsHrfocused = hiringRequest.IsHrfocused ?? false;
        //    genSalesHiringRequestDetail.RoleId = hiringRequest.role;

        //    hiringRequest.issaveasdraft = hiringRequest.issaveasdraft ?? false;

        //    //New Status changes
        //    if (hiringRequest.PayPerType == 1 && !(bool)hiringRequest.issaveasdraft)//Pay per hire
        //    {
        //        genSalesHiringRequestDetail.RoleStatusId = (short)prg_HiringRequest_RoleStatus.Open;
        //    }
        //    if (hiringRequest.PayPerType == 2 && !(bool)hiringRequest.issaveasdraft)//Pay per credit
        //    {
        //        genSalesHiringRequestDetail.RoleStatusId = (short)prg_HiringRequest_RoleStatus.StartedSearching;
        //    }

        //    #region Commented

        //    //if (string.Equals(hiringRequest.hrTitle, "Others", StringComparison.OrdinalIgnoreCase))
        //    //{
        //    //    genSalesHiringRequestDetail.RequestForTalent = hiringRequest.otherRole;
        //    //}
        //    //else
        //    //{
        //    //    genSalesHiringRequestDetail.RequestForTalent = hiringRequest.hrTitle;
        //    //}

        //    // Below code is commneted because now other role is removed from new design
        //    //if (genSalesHiringRequestDetail.role == -1)
        //    //{
        //    //    var roleid = _UTSATSAPIDBConnection.PrgTalentRoles.FirstOrDefault(x => x.TalentRole == hiringRequest.otherRole);
        //    //    if (roleid != null) { genSalesHiringRequestDetail.RoleId = roleid.Id; } else { genSalesHiringRequestDetail.RoleId = 0; }
        //    //}
        //    //else
        //    //{
        //    //    genSalesHiringRequestDetail.RoleId = hiringRequest.role;
        //    //}
        //    //genSalesHiringRequestDetail.Id = genSalesHiringRequestDetail.Id;
        //    //genSalesHiringRequestDetail.NoofEmployee = hiringRequest.addHiringRequest.NoofTalents;
        //    //genSalesHiringRequestDetail.Interviewer_ID = hiringRequest.addHiringRequest.ContactID;
        //    //genSalesHiringRequestDetail.InterviewerName = hiringRequest.interviewerFullName;
        //    //genSalesHiringRequestDetail.CommunicationType = hiringRequest.SalesHiringRequest_Details.CommunicationType;
        //    //genSalesHiringRequestDetail.TypeofInterviewerId = hiringRequest.FetchHiringInterviewerDetail.TypeofInterviewer_ID;
        //    //genSalesHiringRequestDetail.InterviewLinkedin = hiringRequest.interviewerLinkedin;
        //    //genSalesHiringRequestDetail.InterviewerEmailId = (hiringRequest.interviewerEmail == null) ? string.Empty : hiringRequest.interviewerEmail;
        //    //genSalesHiringRequestDetail.InterviewerYearofExperience = hiringRequest.FetchHiringInterviewerDetail.yearsOfexp;
        //    //genSalesHiringRequestDetail.InterviewerDesignation = hiringRequest.interviewerDesignation;
        //    //genSalesHiringRequestDetail.RoleTeamSize = hiringRequest.SalesHiringRequest_Details.RoleTeamSize;
        //    //genSalesHiringRequestDetail.GenericInfo = hiringRequest.hdnGenericInfoDBrief;
        //    //genSalesHiringRequestDetail.InterviewerId = hiringRequest.SalesHiringRequest_Details.InterviewerId;
        //    //genSalesHiringRequestDetail.Remark = hiringRequest.SalesHiringRequest_Details.Remark;

        //    //if (genSalesHiringRequestDetail != null)
        //    //{
        //    //    genSalesHiringRequestDetail.TimezoneId = genSalesHiringRequestDetail.TimezoneId;
        //    //    genSalesHiringRequestDetail.TimezonePreferenceId = genSalesHiringRequestDetail.TimezonePreferenceId;
        //    //    genSalesHiringRequestDetail.HowSoon = genSalesHiringRequestDetail.HowSoon;
        //    //    genSalesHiringRequestDetail.RoleId = genSalesHiringRequestDetail.RoleId;
        //    //    genSalesHiringRequestDetail.DurationType = genSalesHiringRequestDetail.DurationType;
        //    //    genSalesHiringRequestDetail.SpecificMonth = genSalesHiringRequestDetail.SpecificMonth;
        //    //    genSalesHiringRequestDetail.YearOfExp = genSalesHiringRequestDetail.YearOfExp;
        //    //    genSalesHiringRequestDetail.BudgetFrom = genSalesHiringRequestDetail.BudgetFrom;
        //    //    genSalesHiringRequestDetail.BudgetTo = genSalesHiringRequestDetail.BudgetTo;
        //    //    genSalesHiringRequestDetail.AdhocBudgetCost = genSalesHiringRequestDetail.AdhocBudgetCost;
        //    //    genSalesHiringRequestDetail.AddOtherRole = genSalesHiringRequestDetail.AddOtherRole;
        //    //}
        //    #endregion

        //    return genSalesHiringRequestDetail;
        //}

        //public static GenContactTalentPriority BindContactTalentPriority(GenSalesHiringRequest genSalesHiringRequest, GenSalesHiringRequestDetail genSalesHiringRequestDetail, ListOfTalents listOfTalents, int LoggedInUserId)
        //{
        //    GenContactTalentPriority genContactTalentPriority = new();
        //    genContactTalentPriority.ContactId = genSalesHiringRequest.ContactId;
        //    genContactTalentPriority.HiringRequestId = genSalesHiringRequest.Id;
        //    genContactTalentPriority.HiringRequestDetailId = genSalesHiringRequestDetail.Id;
        //    genContactTalentPriority.RoleId = genSalesHiringRequestDetail.RoleId;
        //    genContactTalentPriority.TalentId = listOfTalents.talentId;
        //    genContactTalentPriority.TalentPriority = "Priority 1";
        //    genContactTalentPriority.AllowedtoSelectforHr = true;
        //    genContactTalentPriority.CreatedByDatetime = DateTime.Now;
        //    genContactTalentPriority.TalentStatusIdBasedOnHr = 1;
        //    genContactTalentPriority.HrCost = String.Format("${0} USD / Month", Convert.ToString(listOfTalents.amount));
        //    genContactTalentPriority.CreatedById = LoggedInUserId;

        //    //UTS-3904: Bind the missing fields of each talent.
        //    genContactTalentPriority.CurrentCtc = listOfTalents.CurrentCTC;
        //    genContactTalentPriority.DpPercentage = listOfTalents.DpPercentage ?? genSalesHiringRequest.Dppercentage;
        //    genContactTalentPriority.TalentCost = listOfTalents.amount;
        //    genContactTalentPriority.IsReplacement = listOfTalents.IsReplacement == 1 ? true : false;
        //    genContactTalentPriority.Nrpercentage = genSalesHiringRequest.TalentCostCalcPercentage == null ? 0 : genSalesHiringRequest.TalentCostCalcPercentage;

        //    return genContactTalentPriority;
        //}

        //public static GenShortlistedTalent BindGenShortlistedTalent(GenSalesHiringRequest genSalesHiringRequest, GenSalesHiringRequestDetail genSalesHiringRequestDetail, SelectTalentModel selectTalentModel, ListOfTalents listOfTalents, int LoggedInUserId, GenContactTalentPriority genTalentBinded)
        //{
        //    GenShortlistedTalent genShortlistedTalent = new();
        //    genShortlistedTalent.HiringRequestId = selectTalentModel.HRId;
        //    genShortlistedTalent.ContactId = genSalesHiringRequest.ContactId;
        //    genShortlistedTalent.TalentId = listOfTalents.talentId;
        //    genShortlistedTalent.HiringRequestDetailId = genSalesHiringRequestDetail.Id;
        //    genShortlistedTalent.CreatedById = LoggedInUserId;
        //    genShortlistedTalent.CreatedByDatetime = DateTime.Now;

        //    return genShortlistedTalent;
        //}

        //public static GenInterviewSlotsMaster BindInterViewSlotMaster(ScheduleInterviewModel scheduleInterviewModel, int LoggedInUserId, int Round)
        //{
        //    GenInterviewSlotsMaster genInterviewSlotsMaster = new GenInterviewSlotsMaster();

        //    genInterviewSlotsMaster.ContactId = scheduleInterviewModel.ContactID;
        //    genInterviewSlotsMaster.Guid = Convert.ToString(Guid.NewGuid());
        //    genInterviewSlotsMaster.HiringRequestDetailId = scheduleInterviewModel.HiringRequest_Detail_ID;
        //    genInterviewSlotsMaster.HiringRequestId = scheduleInterviewModel.HiringRequest_ID;
        //    genInterviewSlotsMaster.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genInterviewSlotsMaster.CreatedByDatetime = System.DateTime.Now;
        //    if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
        //    {
        //        genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Interview_Scheduled;
        //    }
        //    else
        //    {
        //        genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Slot_Given;
        //    }
        //    genInterviewSlotsMaster.RescheduledBy = null;
        //    genInterviewSlotsMaster.RescheduledInterviewId = null;
        //    genInterviewSlotsMaster.RescheduledMessageShown = "";
        //    genInterviewSlotsMaster.RescheduledReasons = "";
        //    genInterviewSlotsMaster.RescheduledTypeId = null;
        //    genInterviewSlotsMaster.TalentId = scheduleInterviewModel.Talent_ID;
        //    genInterviewSlotsMaster.InterviewRoundCount = Round != 0 ? Convert.ToInt32(Round) : 1;
        //    genInterviewSlotsMaster.InterviewRoundStr = Round != 0 ? "Round " + Round.ToString() : "Round 1";

        //    return genInterviewSlotsMaster;
        //}

        //public static GenShortlistedTalentInterviewDetail BindGenShortlistedTalentInterviewDetail(ScheduleInterviewModel scheduleInterviewModel, RescheduleSlot rescheduleSlot, int LoggedInUserId, long InterviewMaster_ID)
        //{
        //    GenShortlistedTalentInterviewDetail genShortlistedTalentInterviewDetail = new GenShortlistedTalentInterviewDetail();

        //    genShortlistedTalentInterviewDetail.ContactId = scheduleInterviewModel.ContactID;
        //    genShortlistedTalentInterviewDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genShortlistedTalentInterviewDetail.CreatedByDatetime = System.DateTime.Now;
        //    genShortlistedTalentInterviewDetail.DurationInHours = 0.00m;
        //    genShortlistedTalentInterviewDetail.HiringRequestDetailId = scheduleInterviewModel.HiringRequest_Detail_ID;
        //    genShortlistedTalentInterviewDetail.HiringRequestId = scheduleInterviewModel.HiringRequest_ID;
        //    genShortlistedTalentInterviewDetail.InterviewMasterId = InterviewMaster_ID;
        //    genShortlistedTalentInterviewDetail.InterviewSlot = "Slot" + rescheduleSlot.SlotID;
        //    genShortlistedTalentInterviewDetail.IsTalentConfirmed = scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4 ? true : false;

        //    genShortlistedTalentInterviewDetail.ScheduledOn = CommonLogic.ConvertString2DateTime(rescheduleSlot.STRSlotDate);
        //    genShortlistedTalentInterviewDetail.InterviewStartTime = CommonLogic.ConvertString2DateTime(rescheduleSlot.STRStartTime).TimeOfDay;
        //    genShortlistedTalentInterviewDetail.InterviewEndTime = CommonLogic.ConvertString2DateTime(rescheduleSlot.STREndTime).TimeOfDay;

        //    TimeSpan difference = genShortlistedTalentInterviewDetail.InterviewEndTime.Value - genShortlistedTalentInterviewDetail.InterviewStartTime.Value;
        //    genShortlistedTalentInterviewDetail.DurationInHours = difference.Hours;
        //    if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
        //    {
        //        genShortlistedTalentInterviewDetail.StatusId = (short)prg_InterviewStatus.Interview_Scheduled;
        //    }
        //    else
        //    {
        //        genShortlistedTalentInterviewDetail.StatusId = (short)prg_InterviewStatus.Slot_Given;
        //    }
        //    genShortlistedTalentInterviewDetail.TalentId = scheduleInterviewModel.Talent_ID;
        //    genShortlistedTalentInterviewDetail.TimeZoneId = scheduleInterviewModel.WorkingTimeZoneID;

        //    return genShortlistedTalentInterviewDetail;
        //}

        //public static GenTalentSelectedInterviewDetail BindGenTalentSelectedInterviewDetail(ScheduleInterviewModel scheduleInterviewModel, long LoggedInUserId, long ShortlistedInterview_ID, long InterviewMaster_ID, int? Round, Meeting meeting)
        //{
        //    GenTalentSelectedInterviewDetail genTalentSelectedInterviewDetail = new GenTalentSelectedInterviewDetail();

        //    genTalentSelectedInterviewDetail.HiringRequestId = scheduleInterviewModel.HiringRequest_ID;
        //    genTalentSelectedInterviewDetail.HiringRequestDetailId = scheduleInterviewModel.HiringRequest_Detail_ID;
        //    genTalentSelectedInterviewDetail.ContactId = scheduleInterviewModel.ContactID;
        //    genTalentSelectedInterviewDetail.TalentId = scheduleInterviewModel.Talent_ID;
        //    genTalentSelectedInterviewDetail.ShortlistedInterviewId = ShortlistedInterview_ID;
        //    genTalentSelectedInterviewDetail.IsConfirmed = true;
        //    genTalentSelectedInterviewDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genTalentSelectedInterviewDetail.CreatedByDatetime = DateTime.Now;
        //    if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
        //    {
        //        genTalentSelectedInterviewDetail.StatusId = (short)prg_InterviewStatus.Interview_Scheduled;
        //    }
        //    else
        //    {
        //        genTalentSelectedInterviewDetail.StatusId = (short)prg_InterviewStatus.Slot_Given;
        //    }
        //    genTalentSelectedInterviewDetail.ContactTimeZoneId = scheduleInterviewModel.WorkingTimeZoneID;
        //    genTalentSelectedInterviewDetail.InterviewRound = Round != 0 ? Convert.ToInt32(Round.Value) : 1;
        //    genTalentSelectedInterviewDetail.InterviewRoundStr = Round != 0 ? "Round " + Round.Value.ToString() : "Round 1";
        //    genTalentSelectedInterviewDetail.InterviewMasterId = InterviewMaster_ID;

        //    genTalentSelectedInterviewDetail.AdditionalNotes = scheduleInterviewModel.Additional_Notes;
        //    if (scheduleInterviewModel.SlotType == 4)
        //    {
        //        genTalentSelectedInterviewDetail.ZoomInterviewLink = scheduleInterviewModel.InterviewCallLink;
        //    }
        //    if (scheduleInterviewModel.SlotType == 2)
        //    {
        //        if (meeting != null)
        //        {
        //            genTalentSelectedInterviewDetail.ZoomInterviewLink = meeting.join_url;
        //            genTalentSelectedInterviewDetail.ZoomMeetingId = Convert.ToString(meeting.id);
        //            genTalentSelectedInterviewDetail.ZoomMeetingscheduledOn = DateTime.Now;
        //            genTalentSelectedInterviewDetail.ZoomInterviewKitUsername = meeting.host_email;
        //            genTalentSelectedInterviewDetail.ZoomInterviewKitPassword = meeting.password;
        //        }
        //    }
        //    return genTalentSelectedInterviewDetail;
        //}

        //public static GenInterviewSlotsMaster BindInterViewSlotMasterForRescheduling(RescheduleInterviewSlotModel scheduleInterviewModel, int LoggedInUserId, int Round)
        //{
        //    GenInterviewSlotsMaster genInterviewSlotsMaster = new GenInterviewSlotsMaster();

        //    genInterviewSlotsMaster.ContactId = scheduleInterviewModel.ContactID;
        //    genInterviewSlotsMaster.Guid = Convert.ToString(Guid.NewGuid());
        //    genInterviewSlotsMaster.HiringRequestDetailId = scheduleInterviewModel.HiringRequest_Detail_ID;
        //    genInterviewSlotsMaster.HiringRequestId = scheduleInterviewModel.HiringRequest_ID;
        //    genInterviewSlotsMaster.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genInterviewSlotsMaster.CreatedByDatetime = System.DateTime.Now;
        //    if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
        //    {
        //        genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Interview_Scheduled;
        //    }
        //    else
        //    {
        //        genInterviewSlotsMaster.InterviewStatusId = (short)prg_InterviewStatus.Slot_Given;
        //    }
        //    genInterviewSlotsMaster.RescheduledBy = scheduleInterviewModel.RescheduleRequestBy ?? "";
        //    genInterviewSlotsMaster.RescheduledInterviewId = scheduleInterviewModel.InterviewMasterID;
        //    genInterviewSlotsMaster.RescheduledMessageShown = "";
        //    genInterviewSlotsMaster.RescheduledReasons = scheduleInterviewModel.ReasonforReschedule ?? "";
        //    if (!string.IsNullOrEmpty(scheduleInterviewModel.RescheduleRequestBy))
        //    {
        //        genInterviewSlotsMaster.RescheduledTypeId = scheduleInterviewModel.RescheduleRequestBy == "Client" ? 1 : 2;
        //    }
        //    genInterviewSlotsMaster.TalentId = scheduleInterviewModel.Talent_ID;
        //    genInterviewSlotsMaster.InterviewRoundCount = Round != 0 ? Convert.ToInt32(Round) : 1;
        //    genInterviewSlotsMaster.InterviewRoundStr = Round != 0 ? "Round " + Round.ToString() : "Round 1";

        //    return genInterviewSlotsMaster;
        //}

        //public static GenShortlistedTalentInterviewDetail BindGenShortlistedTalentInterviewDetailForRescheduling(RescheduleInterviewSlotModel scheduleInterviewModel, RescheduleSlot rescheduleSlot, int LoggedInUserId, long InterviewMaster_ID)
        //{
        //    GenShortlistedTalentInterviewDetail genShortlistedTalentInterviewDetail = new GenShortlistedTalentInterviewDetail();

        //    genShortlistedTalentInterviewDetail.ContactId = scheduleInterviewModel.ContactID;
        //    genShortlistedTalentInterviewDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genShortlistedTalentInterviewDetail.CreatedByDatetime = System.DateTime.Now;
        //    genShortlistedTalentInterviewDetail.DurationInHours = 0.00m;
        //    genShortlistedTalentInterviewDetail.HiringRequestDetailId = scheduleInterviewModel.HiringRequest_Detail_ID;
        //    genShortlistedTalentInterviewDetail.HiringRequestId = scheduleInterviewModel.HiringRequest_ID;
        //    genShortlistedTalentInterviewDetail.InterviewMasterId = InterviewMaster_ID;
        //    genShortlistedTalentInterviewDetail.InterviewSlot = "Slot" + rescheduleSlot.SlotID;
        //    genShortlistedTalentInterviewDetail.IsTalentConfirmed = scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4 ? true : false;

        //    genShortlistedTalentInterviewDetail.ScheduledOn = CommonLogic.ConvertString2DateTime(rescheduleSlot.SlotDate);
        //    genShortlistedTalentInterviewDetail.InterviewStartTime = CommonLogic.ConvertString2DateTime(rescheduleSlot.STRStartTime).TimeOfDay;
        //    genShortlistedTalentInterviewDetail.InterviewEndTime = CommonLogic.ConvertString2DateTime(rescheduleSlot.STREndTime).TimeOfDay;

        //    TimeSpan difference = genShortlistedTalentInterviewDetail.InterviewEndTime.Value - genShortlistedTalentInterviewDetail.InterviewStartTime.Value;
        //    genShortlistedTalentInterviewDetail.DurationInHours = difference.Hours;
        //    if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
        //    {
        //        genShortlistedTalentInterviewDetail.StatusId = (short)prg_InterviewStatus.Interview_Scheduled;
        //    }
        //    else
        //    {
        //        genShortlistedTalentInterviewDetail.StatusId = (short)prg_InterviewStatus.Slot_Given;
        //    }
        //    genShortlistedTalentInterviewDetail.TalentId = scheduleInterviewModel.Talent_ID;
        //    genShortlistedTalentInterviewDetail.TimeZoneId = scheduleInterviewModel.WorkingTimeZoneID;

        //    return genShortlistedTalentInterviewDetail;
        //}

        //public static GenTalentSelectedInterviewDetail BindGenTalentSelectedInterviewDetailForRescheduling(RescheduleInterviewSlotModel scheduleInterviewModel, long LoggedInUserId, long ShortlistedInterview_ID, long InterviewMaster_ID, int? Round, Meeting meeting)
        //{
        //    GenTalentSelectedInterviewDetail genTalentSelectedInterviewDetail = new GenTalentSelectedInterviewDetail();

        //    genTalentSelectedInterviewDetail.HiringRequestId = scheduleInterviewModel.HiringRequest_ID;
        //    genTalentSelectedInterviewDetail.HiringRequestDetailId = scheduleInterviewModel.HiringRequest_Detail_ID;
        //    genTalentSelectedInterviewDetail.ContactId = scheduleInterviewModel.ContactID;
        //    genTalentSelectedInterviewDetail.TalentId = scheduleInterviewModel.Talent_ID;

        //    if ((scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4) && scheduleInterviewModel.InterviewStatus == 4)
        //    {
        //        genTalentSelectedInterviewDetail.ShortlistedInterviewId = ShortlistedInterview_ID;
        //        genTalentSelectedInterviewDetail.IsConfirmed = true;
        //    }
        //    else
        //    {
        //        genTalentSelectedInterviewDetail.ShortlistedInterviewId = null;
        //        genTalentSelectedInterviewDetail.IsConfirmed = false;
        //    }

        //    genTalentSelectedInterviewDetail.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    genTalentSelectedInterviewDetail.CreatedByDatetime = DateTime.Now;
        //    if (scheduleInterviewModel.SlotType == 2 || scheduleInterviewModel.SlotType == 4)
        //    {
        //        genTalentSelectedInterviewDetail.StatusId = (short)prg_InterviewStatus.Interview_Scheduled;
        //    }
        //    else
        //    {
        //        genTalentSelectedInterviewDetail.StatusId = (short)prg_InterviewStatus.Slot_Given;
        //    }
        //    genTalentSelectedInterviewDetail.ContactTimeZoneId = scheduleInterviewModel.WorkingTimeZoneID;
        //    genTalentSelectedInterviewDetail.InterviewRound = Round != 0 ? Convert.ToInt32(Round.Value) : 1;
        //    genTalentSelectedInterviewDetail.InterviewRoundStr = Round != 0 ? "Round " + Round.Value.ToString() : "Round 1";
        //    genTalentSelectedInterviewDetail.InterviewMasterId = InterviewMaster_ID;

        //    genTalentSelectedInterviewDetail.AdditionalNotes = scheduleInterviewModel.Additional_Notes;
        //    if (scheduleInterviewModel.SlotType == 4)
        //    {
        //        genTalentSelectedInterviewDetail.ZoomInterviewLink = scheduleInterviewModel.InterviewCallLink;
        //    }
        //    if (scheduleInterviewModel.SlotType == 2)
        //    {
        //        if (meeting != null)
        //        {
        //            genTalentSelectedInterviewDetail.ZoomInterviewLink = meeting.join_url;
        //            genTalentSelectedInterviewDetail.ZoomMeetingId = Convert.ToString(meeting.id);
        //            genTalentSelectedInterviewDetail.ZoomMeetingscheduledOn = DateTime.Now;
        //            genTalentSelectedInterviewDetail.ZoomInterviewKitUsername = meeting.host_email;
        //            genTalentSelectedInterviewDetail.ZoomInterviewKitPassword = meeting.password;
        //        }
        //    }
        //    return genTalentSelectedInterviewDetail;
        //}

        //public static GenDirectPlacement BindgenDirectPlacement(GenDirectPlacement gendirectPlacement, HiringReqeustModel hiringRequest, long hiringrequestId, long LoggedInUserId)
        //{
        //    if (gendirectPlacement == null)
        //        gendirectPlacement = new GenDirectPlacement();

        //    gendirectPlacement.HiringRequestId = hiringrequestId;
        //    gendirectPlacement.ModeOfWork = hiringRequest.directPlacement.ModeOfWork;
        //    gendirectPlacement.Address = hiringRequest.directPlacement.Address ?? "";
        //    gendirectPlacement.State = hiringRequest.directPlacement.State ?? "";
        //    gendirectPlacement.City = hiringRequest.directPlacement.City ?? "";
        //    gendirectPlacement.Country = hiringRequest.directPlacement.Country ?? "";
        //    gendirectPlacement.PostalCode = hiringRequest.directPlacement.PostalCode ?? "";
        //    gendirectPlacement.CreatedById = Convert.ToInt32(LoggedInUserId);
        //    gendirectPlacement.CreatedByDateTime = DateTime.Now;

        //    return gendirectPlacement;
        //}

        //public static GenOnBoardClientContractDetail BindGenOnBoardClientContractDetail(OnBoardViewModel onBoardViewModel)
        //{
        //    GenOnBoardClientContractDetail onBoardClientContractDetail = new GenOnBoardClientContractDetail();

        //    onBoardClientContractDetail.ContractType = onBoardViewModel.ContractType;
        //    onBoardClientContractDetail.OnBoardId = onBoardViewModel.OnboardID;

        //    onBoardClientContractDetail.ContractStartDate = CommonLogic.ConvertString2DateTimeFormtatAdmin(onBoardViewModel.ContractStartDate);
        //    onBoardClientContractDetail.ContractEndDate = CommonLogic.ConvertString2DateTimeFormtatAdmin(onBoardViewModel.ContractEndDate);
        //    onBoardClientContractDetail.TotalDurationInMonths = onBoardViewModel.TotalDurationInMonths;
        //    onBoardClientContractDetail.ContractDuration = onBoardViewModel.ContractDuration;
        //    onBoardClientContractDetail.TalentOnBoardDate = CommonLogic.ConvertString2DateTimeFormtatAdmin(onBoardViewModel.TalentOnBoardDate);
        //    onBoardClientContractDetail.TalentOnBoardTime = CommonLogic.ConvertString2TimeFormtatAdmin(onBoardViewModel.TalentOnBoardTime);

        //    onBoardClientContractDetail.PunchStartTime = onBoardViewModel.PunchStartTime;
        //    onBoardClientContractDetail.PunchEndTime = onBoardViewModel.PunchEndTime;
        //    onBoardClientContractDetail.WorkingDays = String.Format("{0} To {1}", onBoardViewModel.StartDay, onBoardViewModel.EndDay);
        //    onBoardClientContractDetail.TalentWorkingTimeZone = onBoardViewModel.TalentWorkingTimeZone;
        //    onBoardClientContractDetail.TimezonePreferenceId = Convert.ToInt32(onBoardViewModel.Timezone_Preference_ID);
        //    onBoardClientContractDetail.FirstClientBillingDate = CommonLogic.ConvertString2DateTimeFormtatAdmin(onBoardViewModel.FirstClientBillingDate);
        //    onBoardClientContractDetail.NetPaymentDays = onBoardViewModel.NetPaymentDays;
        //    onBoardClientContractDetail.ContractRenewalSlot = onBoardViewModel.ContractRenewalSlot;

        //    onBoardClientContractDetail.ExpectationFromTalentFirstWeek = onBoardViewModel.ExpectationFromTalent_FirstWeek;
        //    onBoardClientContractDetail.ExpectationFromTalentFirstMonth = onBoardViewModel.ExpectationFromTalent_FirstMonth;
        //    onBoardClientContractDetail.ClientRemark = onBoardViewModel.Client_Remark;




        //    return onBoardClientContractDetail;
        //}

        ///// <summary>
        ///// Bindgens the sales hiring request interviewer detail.
        ///// </summary>
        ///// <param name="hiringRequestInterviewerDetail">The hiring request interviewer detail.</param>
        ///// <returns></returns>
        //public static GenSalesHiringRequestInterviewerDetail BindgenSalesHiringRequestInterviewerDetail(GenSalesHiringRequestInterviewerDetail hiringRequestInterviewerDetail)
        //{
        //    GenSalesHiringRequestInterviewerDetail genSalesHiringRequestInterviewerDetail = new GenSalesHiringRequestInterviewerDetail();
        //    genSalesHiringRequestInterviewerDetail.HiringRequestId = hiringRequestInterviewerDetail.HiringRequestId;
        //    genSalesHiringRequestInterviewerDetail.HiringRequestDetailId = hiringRequestInterviewerDetail.HiringRequestDetailId;
        //    genSalesHiringRequestInterviewerDetail.InterviewerName = hiringRequestInterviewerDetail.InterviewerName;
        //    genSalesHiringRequestInterviewerDetail.InterviewLinkedin = hiringRequestInterviewerDetail.InterviewLinkedin;
        //    genSalesHiringRequestInterviewerDetail.InterviewerYearofExperience = hiringRequestInterviewerDetail.InterviewerYearofExperience;
        //    genSalesHiringRequestInterviewerDetail.InterviewerExpInMonth = hiringRequestInterviewerDetail.InterviewerExpInMonth;
        //    genSalesHiringRequestInterviewerDetail.InterviewerEmailId = hiringRequestInterviewerDetail.InterviewerEmailId;
        //    genSalesHiringRequestInterviewerDetail.IsUsedAddMore = hiringRequestInterviewerDetail.IsUsedAddMore;
        //    genSalesHiringRequestInterviewerDetail.InterviewerDesignation = hiringRequestInterviewerDetail.InterviewerDesignation;
        //    genSalesHiringRequestInterviewerDetail.TypeofInterviewerId = hiringRequestInterviewerDetail.TypeofInterviewerId;

        //    return genSalesHiringRequestInterviewerDetail;
        //}


        //#endregion

        //#region FromInside-Outside
        //public static Company BindResponseModelCompany(Company company, GenCompany genCompany)
        //{
        //    if (company == null)
        //    {
        //        company = new Company();
        //    }
        //    company.company = genCompany.Company;
        //    company.phone = genCompany.Phone;
        //    company.address = genCompany.Address;
        //    company.en_Id = CommonLogic.Encrypt(genCompany.Id.ToString());
        //    company.companySize = genCompany.CompanySize;
        //    company.teamManagement = 0; // genCompany.TeamManagement;
        //    company.linkedinProfile = genCompany.LinkedInProfile;
        //    company.location = genCompany.Location;
        //    return company;
        //}

        //public static Primaryclient BindResponseModelPrimaryClient(Primaryclient primaryclient, GenContact genContact)
        //{
        //    if (primaryclient == null)
        //    {
        //        primaryclient = new Primaryclient();
        //    }
        //    primaryclient.fullName = genContact.FullName;
        //    primaryclient.emailId = genContact.EmailId;
        //    primaryclient.linkedin = genContact.LinkedIn;
        //    primaryclient.en_Id = CommonLogic.Encrypt(genContact.Id.ToString());
        //    primaryclient.designation = genContact.Designation;
        //    primaryclient.contactNo = genContact.ContactNo;
        //    return primaryclient;
        //}
        //#endregion
    }
}
