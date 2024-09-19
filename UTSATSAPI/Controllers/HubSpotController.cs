using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using UTSATSAPI.ATSCalls;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.HubSpot;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    /// <summary>
    /// HubSpotController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("[controller]/")]
    [Authorize]
    [ApiController]
    public class HubSpotController : ControllerBase
    {

        #region Private variables

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The hub spot authentication token
        /// </summary>
        private readonly string _hubSpotAuthToken;

        /// <summary>
        /// The i company
        /// </summary>
        private readonly ICompany _iCompany;
        /// <summary>
        /// The i contact repository
        /// </summary>
        private readonly IContactRepository _iContactRepository;
        /// <summary>
        /// The i hub spot repository
        /// </summary>
        private readonly IHubSpotRepository _iHubSpotRepository;
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;

        private readonly IMapper _iMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HubSpotController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="iCompany">The i company.</param>
        /// <param name="iContactRepository">The i contact repository.</param>
        /// <param name="iHubSpotRepository">The i hub spot repository.</param>
        public HubSpotController(IConfiguration configuration, ICompany iCompany, IContactRepository iContactRepository, IHubSpotRepository iHubSpotRepository, TalentConnectAdminDBContext talentConnectAdminDBContext, IMapper iMapper)
        {
            _configuration = configuration;
            _iCompany = iCompany;
            _iContactRepository = iContactRepository;
            _iHubSpotRepository = iHubSpotRepository;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _hubSpotAuthToken = _configuration.GetValue("HubSpotAuthToken", string.Empty);
            _iMapper = iMapper;
        }

        #endregion


        #region Hubspot Contact related APIs

        [HttpGet]
        [Route("GetAutoCompleteHubSpotCompany")]
        public async Task<IActionResult> GetAutoCompleteHubSpotCompany([FromQuery] string Search)
        {
            try
            {
                #region Pre-Validation 
                if (string.IsNullOrEmpty(Search))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide text for search" });
                }
                #endregion

                List<sproc_UTS_GetAutoCompleteHubSpotCompanies_Result> searchData = await _iHubSpotRepository.GetAutoCompleteHubSpotAllCompanies(Search);
                if (searchData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = searchData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Company Available" });

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("GetCompanyDetails")]
        public async Task<IActionResult> GetCompanyDetails(long? CompanyId)
        {
            try
            {

                if (CompanyId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyId is not valid" });

                ClientViewModel clientViewModel = new();

                var cmpDetails = _iCompany.GetCompanyDetails(CompanyId);
                clientViewModel.CompanyDetails = _iMapper.Map<CompanyDetails>(cmpDetails);

                if (clientViewModel.CompanyDetails is not null)
                    clientViewModel.CompanyDetails.en_Id = CommonLogic.Encrypt(clientViewModel.CompanyDetails.ID.ToString());

                var contactDetails = await _iContactRepository.GetContactDetails(CompanyId).ConfigureAwait(false);
                clientViewModel.ContactDetails = _iMapper.Map<List<ContactDetails>>(contactDetails);

                if (clientViewModel.ContactDetails is not null)
                    clientViewModel.ContactDetails.ForEach(x => x.en_Id = CommonLogic.Encrypt(x.ID.ToString()));

                var Legaldetails = await _iContactRepository.GetLegalContact(CompanyId).ConfigureAwait(false);
                clientViewModel.CompanyContract = _iMapper.Map<CompanyContract>(Legaldetails);

                if (clientViewModel.CompanyContract is not null)
                    clientViewModel.CompanyContract.en_Id = CommonLogic.Encrypt(clientViewModel.CompanyContract.Id.ToString());

                clientViewModel.contactPoc = await _iContactRepository.GetPointofContact(CompanyId).ConfigureAwait(false);

                if (clientViewModel != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = clientViewModel });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyId is not valid" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetCompanyDetailsForEditClient")]
        public async Task<IActionResult> GetCompanyDetailsForEditClient(long? CompanyId)
        {
            try
            {

                if (CompanyId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyId is not valid" });

                ClientViewModel clientViewModel = new();
                sproc_UTS_GetCompanyDetailsForEdit_Result getCompanyDetail = _iCompany.GetCompanyDetailsForEdit(CompanyId);
                //   clientViewModel.CompanyDetails = _iMapper.Map<CompanyDetails>(cmpDetails);

                if (getCompanyDetail != null)
                    clientViewModel.CompanyDetails = new()
                    {
                        ID = getCompanyDetail.ID,
                        en_Id = CommonLogic.Encrypt(getCompanyDetail.ID.ToString()),
                        CompanyName = getCompanyDetail.CompanyName,
                        Website = getCompanyDetail.Website,
                        LinkedInProfile = getCompanyDetail.LinkedInProfile,
                        CompanySize = getCompanyDetail.CompanySize,
                        Location = getCompanyDetail.Location,
                        Phone = getCompanyDetail.Phone,
                        City = getCompanyDetail.City,
                        State = getCompanyDetail.State,
                        Country = getCompanyDetail.Country,
                        Zip = getCompanyDetail.Zip,
                        Address = getCompanyDetail.Address,
                        GEO_ID = getCompanyDetail.GEO_ID,
                        TeamManagement = getCompanyDetail.TeamManagement,
                        CompanyLogo = getCompanyDetail.CompanyLogo,
                        LeadType = getCompanyDetail.LeadType,
                        LeadUserID = getCompanyDetail.LeadUserID,
                        AboutCompanyDesc = getCompanyDetail.AboutCompanyDesc,
                        IsTransparentPricing = getCompanyDetail.IsTransparentPricing,
                        JPCreditBalance = getCompanyDetail.JPCreditBalance,
                        AnotherCompanyTypeID = getCompanyDetail.AnotherCompanyTypeID,
                        IsPostaJob = getCompanyDetail.IsPostaJob,
                        IsProfileView = getCompanyDetail.IsProfileView,
                        CompanyTypeID = getCompanyDetail.CompanyTypeID,
                        IsVettedProfile = getCompanyDetail.IsVettedProfile,
                        CreditAmount = getCompanyDetail.CreditAmount,
                        CreditCurrency = getCompanyDetail.CreditCurrency,
                        JobPostCredit = getCompanyDetail.JobPostCredit,
                        VettedProfileViewCredit = getCompanyDetail.VettedProfileViewCredit,
                        NonVettedProfileViewCredit = getCompanyDetail.NonVettedProfileViewCredit
                    };
                var contactDetails = await _iContactRepository.GetContactDetails(CompanyId).ConfigureAwait(false);
                clientViewModel.ContactDetails = _iMapper.Map<List<ContactDetails>>(contactDetails);

                if (clientViewModel.ContactDetails is not null)
                    clientViewModel.ContactDetails.ForEach(x => x.en_Id = CommonLogic.Encrypt(x.ID.ToString()));

                var Legaldetails = await _iContactRepository.GetLegalContact(CompanyId).ConfigureAwait(false);
                clientViewModel.CompanyContract = _iMapper.Map<CompanyContract>(Legaldetails);

                if (clientViewModel.CompanyContract is not null)
                    clientViewModel.CompanyContract.en_Id = CommonLogic.Encrypt(clientViewModel.CompanyContract.Id.ToString());

                clientViewModel.contactPoc = await _iContactRepository.GetPointOfContactForCompany(CompanyId).ConfigureAwait(false);

                if (clientViewModel != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = clientViewModel });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyId is not valid" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetContact")]
        public async Task<IActionResult> GetContact([FromQuery] long? ContactID)
        {
            try
            {
                if (ContactID <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyId is not valid" });
                var ContactObj = await _iContactRepository.GetContactDetailsByID(ContactID);
                if (ContactObj != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ContactObj });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "ContactID is not valid" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the hub spot contacts by email.
        /// </summary>
        /// <param name="Email">The email.</param>
        /// <returns></returns>
        [HttpGet("GetHubSpotContactsByEmail")]
        public async Task<IActionResult> GetHubSpotContactsByEmail(string Email)
        {
            try
            {
                #region 
                if (!string.IsNullOrEmpty(Email))
                {
                    string myResponse = "";

                    try
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        string strHubSpotURL = $"https://api.hubapi.com/contacts/v1/contact/email/{Email}/profile";

                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(strHubSpotURL));

                        if (webRequest != null)
                        {
                            webRequest.Method = "GET";
                            webRequest.ServicePoint.Expect100Continue = false;
                            webRequest.Timeout = 50000;

                            webRequest.Headers.Add("Authorization", $"Bearer {_hubSpotAuthToken}");
                            webRequest.ContentType = "application/json";
                            //webRequest.Headers.Add("Content-Type", "application/json");
                            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0";
                        }

                        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                        Stream resStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(resStream);
                        myResponse = reader.ReadToEnd();
                    }
                    catch (WebException exception)
                    {
                        if (exception.Status == WebExceptionStatus.ProtocolError)
                        {
                            HttpWebResponse response = (HttpWebResponse)exception.Response;
                            Stream resStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(resStream);
                            myResponse = reader.ReadToEnd();
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, exception.Message));
                        }
                    }

                    if (!string.IsNullOrEmpty(myResponse))
                    {
                        dynamic JSonResponseData = JsonConvert.DeserializeObject(myResponse);
                        if (JSonResponseData != null)
                        {
                            if (JSonResponseData["properties"] != null)
                            {
                                var AllProperties = JSonResponseData["properties"];

                                long ContactId = 0;
                                long CompanyId = 0;

                                if (!string.IsNullOrEmpty(Convert.ToString(JSonResponseData.vid)))
                                {
                                    long.TryParse(Convert.ToString(JSonResponseData.vid), out ContactId);

                                    await GetContactByContactId(ContactId.ToString());
                                }

                                HubspotProperty Properties = JsonConvert.DeserializeObject<HubspotProperty>(Convert.ToString(AllProperties));

                                if (Properties != null && Properties.associatedcompanyid != null && !string.IsNullOrEmpty(Properties.associatedcompanyid.value))
                                {
                                    long.TryParse(Convert.ToString(Properties.associatedcompanyid.value), out CompanyId);

                                    var compResponse = await SaveCompanyDetailsFromCompanyID(CompanyId.ToString());

                                    if (!compResponse)
                                    {
                                        return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, "Company Details not found for Client Please Add Company For Client and Try to Fetch Data Again "));
                                    }

                                    return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "success"));
                                }
                                else
                                {
                                    string ErrorMessage = "Email found but associated company not found. Please try another email address";

                                    if (JSonResponseData["errors"] != null)
                                    {
                                        ErrorMessage = "";
                                        foreach (var error in JSonResponseData["errors"])
                                        {
                                            ErrorMessage += error.message + "\n\r";
                                        }
                                    }

                                    //return ErrorMessage;
                                    return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ErrorMessage));
                                }


                            }
                            else
                            {
                                string ErrorMessage = "Please Enter Client in HubSpot and again Fetch Details";

                                if (JSonResponseData["errors"] != null)
                                {
                                    ErrorMessage = "";
                                    foreach (var error in JSonResponseData["errors"])
                                    {
                                        ErrorMessage += error.message + "\n\r";
                                    }
                                }
                                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ErrorMessage));
                            }
                        }

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, "Please Enter Client in HubSpot and again Fetch Details"));
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, "Invalid Email. Please provide valid email address"));
                }
                #endregion

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ex.Message));
            }

            return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "Details fetched Successfully."));
        }

        #endregion



        #region private methods 

        /// <summary>
        /// Gets the contact by contact identifier.
        /// </summary>
        /// <param name="contactid">The contactid.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetContactByContactId")]
        private async Task<object> GetContactByContactId(string contactid)
        {
            string myResponse = "";
            if (!string.IsNullOrEmpty(contactid))
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    // Create a new HttpWebRequest object.
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://api.hubapi.com/contacts/v1/contact/vid/" + contactid + "/profile"));
                    if (webRequest != null)
                    {
                        webRequest.Headers.Add("Authorization", $"Bearer {_hubSpotAuthToken}");
                        webRequest.Method = "GET";
                        webRequest.ServicePoint.Expect100Continue = false;
                        webRequest.Timeout = 50000;

                        webRequest.ContentType = "application/json";
                        //webRequest.Headers.Add("Content-Type", "application/json");
                        webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0";
                    }

                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                    Stream resStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(resStream);
                    myResponse = reader.ReadToEnd();
                }
                catch (WebException exception)
                {
                    if (exception.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse response = (HttpWebResponse)exception.Response;
                        Stream resStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(resStream);
                        myResponse = reader.ReadToEnd();
                    }
                    else
                    {
                        throw;
                    }
                }

                HubspotProperty objHubSpotContactProperties = null;
                List<IdentityProfile> identityProfiles = null;
                if (!string.IsNullOrEmpty(myResponse))
                {
                    dynamic JSonResponseData = JsonConvert.DeserializeObject(myResponse);
                    if (JSonResponseData != null)
                    {
                        string emailid = "", firstname = "", lastname = "",
                             region = "", phone = "", website = "", city = "", skype = "";

                        long contactIdSel = 0;
                        bool isNewContact = false, is_primary = false;
                        long.TryParse(contactid, out contactIdSel);
                        if (contactIdSel > 0)
                        {
                            var contactDetails = await _iHubSpotRepository.GetHubSpotContactByContactID(contactIdSel);
                            if (contactDetails == null)
                            {
                                contactDetails = new GenHubSpotContact();
                                isNewContact = true;
                                contactDetails.ContactId = contactIdSel;
                            }
                            string portalid = JSonResponseData["portal-id"].Value.ToString();
                            string canonicalvid = Convert.ToString(JSonResponseData["canonical-vid"].Value);
                            string isContact = JSonResponseData["is-contact"].Value.ToString();
                            double CreatedTimeStamp = 0, ModifiedTimeStamp = 0;

                            if (!string.IsNullOrEmpty(canonicalvid))
                                contactDetails.CanonicalVid = Convert.ToInt64(canonicalvid);

                            if (!string.IsNullOrEmpty(portalid))
                                contactDetails.PortalId = Convert.ToInt32(portalid);

                            if (JSonResponseData["identity-profiles"] != null)
                            {
                                var identityprofiles = JSonResponseData["identity-profiles"];
                                identityProfiles = JsonConvert.DeserializeObject<List<IdentityProfile>>(Convert.ToString(identityprofiles));
                                foreach (var item in identityProfiles)
                                {
                                    foreach (var identityitem in item.identities)
                                    {
                                        if (identityitem.isprimary)
                                        {
                                            is_primary = true;
                                        }
                                    }
                                }
                            }

                            if (JSonResponseData["properties"] != null)
                            {
                                var AllProperties = JSonResponseData["properties"];
                                objHubSpotContactProperties = JsonConvert.DeserializeObject<HubspotProperty>(Convert.ToString(AllProperties));

                                if (objHubSpotContactProperties.email != null && !string.IsNullOrEmpty(objHubSpotContactProperties.email.value))
                                {
                                    contactDetails.EmailId = objHubSpotContactProperties.email.value;
                                    emailid = objHubSpotContactProperties.email.value;
                                }

                                if (objHubSpotContactProperties.firstname != null && !string.IsNullOrEmpty(objHubSpotContactProperties.firstname.value))
                                {
                                    contactDetails.FirstName = objHubSpotContactProperties.firstname.value;
                                    firstname = objHubSpotContactProperties.firstname.value;
                                }

                                if (objHubSpotContactProperties.lastname != null && !string.IsNullOrEmpty(objHubSpotContactProperties.lastname.value))
                                {
                                    contactDetails.LastName = objHubSpotContactProperties.lastname.value;
                                    lastname = objHubSpotContactProperties.lastname.value;
                                }

                                if (objHubSpotContactProperties.phone != null && !string.IsNullOrEmpty(objHubSpotContactProperties.phone.value))
                                {
                                    contactDetails.ContactNo = objHubSpotContactProperties.phone.value;
                                    phone = objHubSpotContactProperties.phone.value;
                                }

                                if (objHubSpotContactProperties.website != null && !string.IsNullOrEmpty(objHubSpotContactProperties.website.value))
                                {
                                    contactDetails.Website = objHubSpotContactProperties.website.value;
                                    website = objHubSpotContactProperties.website.value;
                                }

                                if (objHubSpotContactProperties.venture != null && !string.IsNullOrEmpty(objHubSpotContactProperties.venture.value))
                                {
                                    contactDetails.Venture = objHubSpotContactProperties.venture.value;
                                }

                                if (objHubSpotContactProperties.regions != null && !string.IsNullOrEmpty(objHubSpotContactProperties.regions.value))
                                {
                                    contactDetails.Regions = objHubSpotContactProperties.regions.value;
                                    region = objHubSpotContactProperties.regions.value;
                                }

                                if (objHubSpotContactProperties.city != null && !string.IsNullOrEmpty(objHubSpotContactProperties.city.value))
                                {
                                    contactDetails.City = objHubSpotContactProperties.city.value;
                                    city = objHubSpotContactProperties.city.value;
                                }

                                //if (objHubSpotContactProperties.skype != null && !string.IsNullOrEmpty(objHubSpotContactProperties.skype.value))
                                //{
                                //    contactDetails.Skype = objHubSpotContactProperties.skype.value;
                                //    skype = objHubSpotContactProperties.skype.value;
                                //}

                                if (objHubSpotContactProperties.jobtitle != null && !string.IsNullOrEmpty(objHubSpotContactProperties.jobtitle.value))
                                {
                                    contactDetails.Jobtitle = objHubSpotContactProperties.jobtitle.value;
                                }

                                if (objHubSpotContactProperties.associatedcompanyid != null && !string.IsNullOrEmpty(objHubSpotContactProperties.associatedcompanyid.value))
                                {
                                    contactDetails.CompanyId = Convert.ToInt64(objHubSpotContactProperties.associatedcompanyid.value);
                                    contactDetails.Associatedcompanyid = Convert.ToInt64(objHubSpotContactProperties.associatedcompanyid.value);
                                }

                                if (objHubSpotContactProperties.company != null && !string.IsNullOrEmpty(objHubSpotContactProperties.company.value))
                                {
                                    contactDetails.Company = objHubSpotContactProperties.company.value;
                                }

                                if (objHubSpotContactProperties.hubspot_owner_id != null && !string.IsNullOrEmpty(objHubSpotContactProperties.hubspot_owner_id.value))
                                {
                                    contactDetails.HubspotOwnerId = Convert.ToInt64(objHubSpotContactProperties.hubspot_owner_id.value);
                                }

                                if (objHubSpotContactProperties.createdate != null && !string.IsNullOrEmpty(objHubSpotContactProperties.createdate.value))
                                {
                                    CreatedTimeStamp = Convert.ToDouble(objHubSpotContactProperties.createdate.value);
                                }

                                if (objHubSpotContactProperties.lastmodifieddate != null && !string.IsNullOrEmpty(objHubSpotContactProperties.lastmodifieddate.value))
                                {
                                    ModifiedTimeStamp = Convert.ToDouble(objHubSpotContactProperties.lastmodifieddate.value);
                                }
                            }

                            contactDetails.MergedVids = "";

                            bool isActive = true;
                            if (!string.IsNullOrEmpty(isContact))
                            {
                                if (isContact == "true")
                                    isActive = false;
                            }
                            contactDetails.IsPrimary = isActive;
                            contactDetails.CreatedById = 1;

                            contactDetails.CreatedByDatetime = GetDatetimeFromTimeStamp(CreatedTimeStamp);
                            contactDetails.LastModifiedDatetime = GetDatetimeFromTimeStamp(ModifiedTimeStamp);


                            await _iHubSpotRepository.SaveHubSpotContact(contactDetails);

                            long contact_id = contactIdSel;
                            long CompanyId = 0;

                            var objCompany = await _iCompany.GetCompanyByHubSpotCompanyId(contactDetails.CompanyId ?? 0);

                            if (objCompany != null)
                            {
                                CompanyId = objCompany.Id;
                            }

                            var obj_genContact = await _iContactRepository.GetContactByHubSpotIdAndEmil(contact_id, emailid.ToLower().Trim());

                            if (obj_genContact == null)
                            {
                                obj_genContact = new GenContact();

                                obj_genContact.CreatedByDatetime = GetDatetimeFromTimeStamp(CreatedTimeStamp);
                                obj_genContact.CreatedById = 1;
                            }
                            else
                            {
                                obj_genContact.LastModifiedDatetime = GetDatetimeFromTimeStamp(ModifiedTimeStamp);
                                obj_genContact.LastModifiedById = 1;
                            }

                            obj_genContact.CompanyId = CompanyId;
                            obj_genContact.EmailId = emailid;
                            obj_genContact.Username = emailid;
                            obj_genContact.FirstName = firstname;
                            obj_genContact.LastName = lastname;
                            obj_genContact.FullName = firstname + " " + lastname;
                            obj_genContact.ContactNo = phone;
                            obj_genContact.Regions = region;
                            obj_genContact.City = city;
                            //obj_genContact.Skype = "";// skype;
                            obj_genContact.HubSpotContactId = contactIdSel;
                            obj_genContact.IsPrimary = is_primary;
                            obj_genContact.IsActive = isActive;

                            await _iContactRepository.SaveContact(obj_genContact);

                        }
                    }
                }
            }
            return "Success";
        }


        /// <summary>
        /// Saves the company details from company identifier.
        /// </summary>
        /// <param name="CompanyID">The company identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SaveCompanyDetailsFromCompanyID")]
        private async Task<bool> SaveCompanyDetailsFromCompanyID(string CompanyID)
        {
            bool blResponse = false;
            string companyid = "0";
            try
            {
                if (!string.IsNullOrEmpty(CompanyID))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var http = (HttpWebRequest)WebRequest.Create(new Uri("https://api.hubapi.com/companies/v2/companies/" + CompanyID));

                    http.Headers.Add("Authorization", $"Bearer {_hubSpotAuthToken}");
                    http.ContentType = "application/json";
                    http.Method = "GET";
                    http.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0";

                    try
                    {
                        HttpWebResponse response = http.GetResponse() as HttpWebResponse;  // from here i getting exception
                        Stream resStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(resStream);
                        var myResponse = reader.ReadToEnd();

                        if (string.IsNullOrEmpty(myResponse))
                        {
                            blResponse = false;
                            return blResponse;
                        }
                        dynamic JSonResponseData = JsonConvert.DeserializeObject(myResponse);


                        string category = "", hubspotownerid = "", hsobjectid = "", website = "", companyname = "", state = "", city = "",
                            country = "", zip = "", address = "", domain = "", industry = "", annualrevenue = "0", phone = "",
                            source = "", sourceid = "", linkedin_company_page = "", numberofemployees = "0"
                            , linkedin_url = "", region_name = "", lifecyclestage = "", lead_source_channel = "", Company_LogoName="";
                        double CreatedTimeStamp = 0, ModifiedTimeStamp = 0;
                        int company_size = 0;
                        dynamic objCompany = JSonResponseData.properties;

                        companyid = JSonResponseData.companyId.ToString();
                        string portalid = JSonResponseData.portalId.ToString();
                        string isDeleted = JSonResponseData.isDeleted.ToString();

                        if (objCompany.website != null)
                            website = objCompany.website.value;
                        if (objCompany.name != null)
                            companyname = objCompany.name.value;

                        if (objCompany.country != null)
                            country = objCompany.country.value;

                        if (objCompany.state != null)
                            state = objCompany.state.value;
                        if (objCompany.city != null)
                            city = objCompany.city.value;
                        if (objCompany.zip != null)
                            zip = objCompany.zip.value;
                        if (objCompany.address != null)
                            address = objCompany.address.value;
                        if (objCompany.domain != null)
                            domain = objCompany.domain.value;
                        if (objCompany.industry != null)
                            industry = objCompany.industry.value;

                        if (objCompany.hubspot_owner_id != null)
                        {
                            hubspotownerid = objCompany.hubspot_owner_id.value;
                            source = objCompany.hubspot_owner_id.source;
                            sourceid = objCompany.hubspot_owner_id.sourceId;
                        }
                        if (objCompany.hs_object_id != null)
                        {
                            hsobjectid = objCompany.hs_object_id.value;
                        }

                        if (objCompany.annualrevenue != null)
                            annualrevenue = objCompany.annualrevenue.value;


                        if (objCompany.phone != null)
                            phone = objCompany.phone.value;

                        if (objCompany.category != null)
                            category = objCompany.category.value;

                        //Insert Company data
                        bool isActive = true;
                        if (!string.IsNullOrEmpty(isDeleted))
                        {
                            if (isDeleted == "true")
                                isActive = false;
                        }

                        string strCreatedtimestamp = objCompany.createdate.timestamp.ToString();
                        if (!string.IsNullOrEmpty(strCreatedtimestamp))
                            CreatedTimeStamp = Convert.ToDouble(strCreatedtimestamp);

                        if (objCompany.hs_lastmodifieddate != null)
                        {
                            string strModifiedtimestamp = objCompany.hs_lastmodifieddate.timestamp.ToString();
                            if (!string.IsNullOrEmpty(strModifiedtimestamp))
                                ModifiedTimeStamp = Convert.ToDouble(strModifiedtimestamp);
                        }


                        if (objCompany.linkedin_company_page != null)
                            linkedin_company_page = objCompany.linkedin_company_page.value;

                        if (objCompany.numberofemployees != null)
                            numberofemployees = objCompany.numberofemployees.value;

                        if (objCompany.company_size != null)
                            company_size = objCompany.company_size.value;
                        if (objCompany.linkedin_url != null)
                            linkedin_url = objCompany.linkedin_url.value;
                        if (objCompany.lifecyclestage != null)
                            lifecyclestage = objCompany.lifecyclestage.value;
                        if (objCompany.region_name != null)
                            region_name = objCompany.region_name.value;
                        if (objCompany.lead_source_channel != null)
                            lead_source_channel = objCompany.lead_source_channel.value;

                        bool isExist = true;
                        long HubSpotOwnerID = 0;
                        long HubSpotcompanyid = Convert.ToInt64(companyid);
                        if(hubspotownerid !="")
                         HubSpotOwnerID = Convert.ToInt64(hubspotownerid);

                        var DbObjCompany = await _iHubSpotRepository.GetHubSpotCompanyById(HubSpotcompanyid);

                        if (DbObjCompany == null)
                        {

                            var checkObjCompany = await _iHubSpotRepository.GetHubSpotCompanyByCompanyNameAndOwnerId(companyname, HubSpotOwnerID);
                            if (checkObjCompany == null)
                            {
                                isExist = false;
                                DbObjCompany = new GenHubSpotCompany();
                                if (!string.IsNullOrEmpty(companyid))
                                    DbObjCompany.CompanyId = Convert.ToInt64(companyid);
                            }
                            else
                            {
                                DbObjCompany = checkObjCompany;
                                isExist = true;
                            }
                        }
                        else
                        {
                            isExist = true;
                        }

                        if (!string.IsNullOrEmpty(companyid))
                            DbObjCompany.CompanyId = Convert.ToInt64(companyid);


                        if (!string.IsNullOrEmpty(portalid))
                            DbObjCompany.PortalId = Convert.ToInt32(portalid);

                        DbObjCompany.Company = companyname;
                        DbObjCompany.Website = website;
                        DbObjCompany.Source = source;
                        DbObjCompany.SourceId = sourceid;

                        if (!string.IsNullOrEmpty(hubspotownerid))
                            DbObjCompany.HubspotOwnerId = Convert.ToInt64(hubspotownerid);

                        if (!string.IsNullOrEmpty(hsobjectid))
                            DbObjCompany.HsObjectId = Convert.ToInt64(hsobjectid);

                        if (!string.IsNullOrEmpty(annualrevenue))
                            DbObjCompany.Annualrevenue = Convert.ToDecimal(annualrevenue);

                        DbObjCompany.Address = address;
                        DbObjCompany.Phone = phone;
                        DbObjCompany.Domain = domain;
                        DbObjCompany.Industry = industry;
                        DbObjCompany.City = city;
                        DbObjCompany.State = state;
                        DbObjCompany.Country = country;
                        DbObjCompany.Zip = zip;
                        DbObjCompany.Category = category;

                        DbObjCompany.Numberofemployees = company_size;
                        DbObjCompany.LinkedinCompanyPage = linkedin_url;
                        DbObjCompany.RegionName = region_name;
                        DbObjCompany.Lifecyclestage = lifecyclestage;
                        DbObjCompany.LeadSourceChannel = lead_source_channel;

                        if (!string.IsNullOrEmpty(numberofemployees))
                            DbObjCompany.Numberofemployees = Convert.ToInt32(numberofemployees);
                        DbObjCompany.LinkedinCompanyPage = linkedin_company_page;

                        DbObjCompany.IsActive = isActive;
                        DbObjCompany.CreatedById = 1;
                        DbObjCompany.CreatedByDatetime = GetDatetimeFromTimeStamp(CreatedTimeStamp);
                        DbObjCompany.ModifiedByDatetime = GetDatetimeFromTimeStamp(ModifiedTimeStamp);

                        long Company_ID = Convert.ToInt64(CompanyID);
                        int Portal_Id = Convert.ToInt32(portalid);
                        long hubspot_owner_Id = 0;
                        if (hubspotownerid != "")
                            hubspot_owner_Id = Convert.ToInt64(hubspotownerid);
                        int Noofemployees = Convert.ToInt32(numberofemployees);
                        decimal Annual_Revenue = Convert.ToDecimal(annualrevenue);
                        bool IsActive = Convert.ToBoolean(isActive);
                        decimal HsObjectId = Convert.ToDecimal(hsobjectid);

                        object[] param = new object[] { Company_ID, companyname, website, Portal_Id, sourceid, source, hubspot_owner_Id, HsObjectId, Annual_Revenue, address, phone, domain, industry, city, state, country, zip, Noofemployees, linkedin_company_page, IsActive, 0, 1, System.DateTime.Now, 0, System.DateTime.Now, 0, null, 0, 0, 0, null, 0, 0, 0, 0, 0, category, region_name, lifecyclestage, lead_source_channel };
                        string paramString = CommonLogic.ConvertToParamString(param);
                        _iHubSpotRepository.Sproc_AddUpdate_Hubspot_Company(paramString);

                        try
                        {
                            if (!string.IsNullOrEmpty(website))
                            {
                                Company_LogoName = await CompanyLogo(website);
                                object[] param2 = new object[] { Company_LogoName, Company_ID };
                                string paramString2 = CommonLogic.ConvertToParamString(param2);
                                
                                _iHubSpotRepository.sproc_UpdateCompanyLogo_forHubpsoCompany(paramString2);
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        #region Send Data To ATS for Company Logo
                        if (!string.IsNullOrEmpty(Company_LogoName))
                        {
                            AddUpdateCompanyViewModel addUpdateCompanyViewModel = BindAddUpdateCompany(Company_ID, companyname, Company_LogoName);
                            try
                            {
                                long LoggedInUserId = SessionValues.LoginUserId;
                                var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                                ATSCall atsCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                atsCall.SendAddEditCompanyData(json, LoggedInUserId);
                            }
                            catch (Exception)
                            {

                            }
                        }                        
                        #endregion

                        //await Sproc_AddUpdate_Hubspot_Company(param1);

                        #region Old code
                        //await _iHubSpotRepository.SaveHubSpotCompany(DbObjCompany);

                        //long company_id = Convert.ToInt64(companyid);

                        //var obj_genCompany = await _iCompany.GetCompanyCompanyNameAndByHubSpotCompanyId(companyname, company_id);

                        //if (obj_genCompany is null)
                        //{
                        //    obj_genCompany = new GenCompany();
                        //    obj_genCompany.CreatedByDatetime = GetDatetimeFromTimeStamp(CreatedTimeStamp);
                        //    obj_genCompany.CreatedById = 1;
                        //}
                        //else
                        //{
                        //    obj_genCompany.ModifiedByDatetime = GetDatetimeFromTimeStamp(ModifiedTimeStamp);
                        //}

                        //obj_genCompany.Company = companyname;
                        //obj_genCompany.Phone = phone;
                        //obj_genCompany.Address = address;
                        //obj_genCompany.IsActive = true;
                        //obj_genCompany.HubSpotCompany = Convert.ToInt64(companyid);
                        //obj_genCompany.Category = category;
                        //obj_genCompany.Country = country;
                        //obj_genCompany.LeadType = lead_source_channel;
                        //obj_genCompany.Website = website;
                        //obj_genCompany.Zip = zip;

                        //await _iCompany.SaveCompany(obj_genCompany);
                        #endregion


                        string result = await GetAllContactsByParameterCompanyID(companyid);

                        blResponse = true;
                        return blResponse;
                    }
                    catch (Exception ex)
                    {
                        if (companyid != "0")
                        {


                            HubspotWebhookNotification DbObjhubspot;
                            DbObjhubspot = new HubspotWebhookNotification();

                            DbObjhubspot.ObjectId = Convert.ToInt64(companyid);
                            DbObjhubspot.ExceptionDetails = ex.ToString();

                            await _iHubSpotRepository.SaveHubspotWebhookNotification(DbObjhubspot);
                        }
                        blResponse = true;

                    }

                }// End of Contact Company ID null check

            }
            catch (Exception ex)
            {
                blResponse = false;
                throw ex;
            }

            return blResponse;

        }

        /// <summary>
        /// Gets all contacts by parameter company identifier.
        /// </summary>
        /// <param name="companyid">The companyid.</param>
        /// <returns></returns>
        [NonAction]
        private async Task<string> GetAllContactsByParameterCompanyID(string companyid)
        {

            int contactcount = 0;
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.DefaultConnectionLimit = 9999;

                var http = (HttpWebRequest)WebRequest.Create(new Uri("https://api.hubapi.com/companies/v2/companies/" + companyid + "/contacts"));

                http.Headers.Add("Authorization", $"Bearer {_hubSpotAuthToken}");
                http.ContentType = "application/json";
                http.Method = "GET";
                http.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0";

                HttpWebResponse response = http.GetResponse() as HttpWebResponse;  // from here i getting exception
                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                var myResponse = reader.ReadToEnd();

                dynamic JSonResponseData = JsonConvert.DeserializeObject(myResponse);
                JArray jarrContacts = (JArray)JSonResponseData["contacts"];

                if (jarrContacts.Count > 0)
                {

                    foreach (var item in jarrContacts)
                    {
                        string associatecompanyid = "", hubspotownerid = "", emailid = "", firstname = "", lastname = "", company = "",
                            state = "", venture = "", region = "", city = "", phone = "", jobtitle = "", skype = "", website = ""
                            , lead_source = "", lifecyclestage = "", designation = "", hs_persona = "";
                        double CreatedTimeStamp = 0, ModifiedTimeStamp = 0;

                        dynamic objContact = item;

                        string vid = objContact.vid.ToString();
                        string portalid = objContact.portalId.ToString();
                        string canonicalvid = objContact.canonicalVid.ToString();
                        string isContact = objContact.isContact.ToString();


                        dynamic identitiesJson = objContact.identities;
                        foreach (var inner1 in identitiesJson)
                        {
                            dynamic identiyJson = inner1.identity;
                            int cnt = 0;
                            foreach (var inner2 in identiyJson)
                            {
                                if (cnt == 0)
                                    emailid = inner2.value.Value;
                                cnt++;
                                if (inner2.timestamp != null && inner2.isPrimary == true)
                                {
                                    string strCreatedtimestamp = inner2.timestamp.ToString();
                                    if (!string.IsNullOrEmpty(strCreatedtimestamp))
                                        CreatedTimeStamp = Convert.ToDouble(strCreatedtimestamp);
                                }
                            }

                        }
                        dynamic propertiesJson = objContact.properties;
                        int propertycnt = 0;
                        foreach (var inner1 in propertiesJson)
                        {
                            if (inner1.name == "firstname")
                                firstname = inner1.value.Value.ToString();
                            else if (inner1.name == "company")
                                company = inner1.value.Value.ToString();
                            else if (inner1.name == "lastname")
                                lastname = inner1.value.Value.ToString();
                            else if (inner1.name == "lastmodifieddate")
                            {
                                if (inner1.value != null)
                                {
                                    string strModifiedtimestamp = inner1.value.Value.ToString();
                                    if (!string.IsNullOrEmpty(strModifiedtimestamp))
                                        ModifiedTimeStamp = Convert.ToDouble(strModifiedtimestamp);
                                }

                            }

                            propertycnt++;

                        }


                        //Insert Conatct data
                        bool isActive = true;
                        if (!string.IsNullOrEmpty(isContact))
                        {
                            if (isContact == "true")
                                isActive = false;
                        }

                        if (objContact.hubspot_owner_id != null)
                        {
                            hubspotownerid = objContact.hubspot_owner_id.value;
                        }

                        if (objContact.venture != null)
                        {
                            venture = objContact.venture.value;
                        }
                        if (objContact.state != null)
                        {
                            state = objContact.state.value;
                        }
                        if (objContact.regions != null)
                        {
                            region = objContact.regions.value;
                        }
                        if (objContact.city != null)
                        {
                            city = objContact.city.value;
                        }
                        if (objContact.email != null)
                        {
                            emailid = objContact.email.value;
                        }
                        if (objContact.phone != null)
                        {
                            phone = objContact.phone.value;
                        }
                        if (objContact.jobtitle != null)
                        {
                            jobtitle = objContact.jobtitle.value;
                        }
                        if (objContact.website != null)
                        {
                            website = objContact.website.value;
                        }
                        if (objContact.skype != null)
                        {
                            skype = objContact.skype.value;
                        }


                        if (objContact.lead_source != null)
                        {
                            lead_source = objContact.lead_source.value;
                        }
                        if (objContact.lifecyclestage != null)
                        {
                            lifecyclestage = objContact.lifecyclestage.value;
                        }
                        if (objContact.designation != null)
                        {
                            designation = objContact.designation.value;
                        }
                        if (objContact.hs_persona != null)
                        {
                            hs_persona = objContact.hs_persona.value;
                        }

                        long HubSpotContactId = 0;
                        if (!string.IsNullOrEmpty(vid))
                            HubSpotContactId = Convert.ToInt64(vid);

                        var objHubSpotContact = await _iHubSpotRepository.GetHubSpotContactByContactID(HubSpotContactId);

                        if (objHubSpotContact is null)
                        {
                            objHubSpotContact = new GenHubSpotContact();
                        }

                        objHubSpotContact.ContactId = HubSpotContactId;

                        if (!string.IsNullOrEmpty(canonicalvid))
                            objHubSpotContact.CanonicalVid = Convert.ToInt64(canonicalvid);

                        if (!string.IsNullOrEmpty(companyid))
                            objHubSpotContact.CompanyId = Convert.ToInt64(companyid);

                        if (!string.IsNullOrEmpty(portalid))
                            objHubSpotContact.PortalId = Convert.ToInt32(portalid);

                        if (!string.IsNullOrEmpty(hubspotownerid))
                            objHubSpotContact.HubspotOwnerId = Convert.ToInt64(hubspotownerid);

                        if (!string.IsNullOrEmpty(associatecompanyid))
                            objHubSpotContact.Associatedcompanyid = Convert.ToInt64(associatecompanyid);



                        objHubSpotContact.Company = company;
                        objHubSpotContact.EmailId = emailid;
                        objHubSpotContact.FirstName = firstname;
                        objHubSpotContact.LastName = lastname;
                        objHubSpotContact.ContactNo = phone;
                        objHubSpotContact.Website = website;
                        objHubSpotContact.Venture = venture;
                        objHubSpotContact.Regions = region;
                        objHubSpotContact.City = city;
                        //objHubSpotContact.Skype = skype;
                        objHubSpotContact.Jobtitle = jobtitle;
                        objHubSpotContact.IsPrimary = isActive;
                        objHubSpotContact.CreatedById = 1;
                        objHubSpotContact.CreatedByDatetime = GetDatetimeFromTimeStamp(CreatedTimeStamp);
                        objHubSpotContact.LastModifiedDatetime = GetDatetimeFromTimeStamp(ModifiedTimeStamp);
                        objHubSpotContact.LeadSource = lead_source;
                        objHubSpotContact.Lifecyclestage = lifecyclestage;
                        objHubSpotContact.Designation = designation;
                        objHubSpotContact.HsPersona = hs_persona;

                        object[] param = new object[] { 0, objHubSpotContact.ContactId, objHubSpotContact.CompanyId, objHubSpotContact.Company, objHubSpotContact.PortalId, objHubSpotContact.HubspotOwnerId, objHubSpotContact.CanonicalVid, objHubSpotContact.Associatedcompanyid, objHubSpotContact.FirstName, objHubSpotContact.LastName, objHubSpotContact.EmailId, website, objHubSpotContact.ContactNo, objHubSpotContact.IsPrimary, canonicalvid, null, objHubSpotContact.Venture, objHubSpotContact.Regions, city, skype, objHubSpotContact.Jobtitle, 1, System.DateTime.Now, null, null, objHubSpotContact.IsCompanyInserted, objHubSpotContact.LastInvoiceDate, objHubSpotContact.CurrencyId, objHubSpotContact.EngagementModelId, objHubSpotContact.LastInvoiceDate, objHubSpotContact.GeoId, objHubSpotContact.ZohoCustomerId, objHubSpotContact.ZohoOrganizationId, objHubSpotContact.IsActive, objHubSpotContact.IsCreatedFromUts, objHubSpotContact.LeadSource, objHubSpotContact.Designation, objHubSpotContact.Lifecyclestage, objHubSpotContact.HsPersona };
                        string paramString = CommonLogic.ConvertToParamString(param);
                        _iHubSpotRepository.Sproc_AddUpdate_HubSpot_Contact(paramString);


                        #region Old Code
                        //await _iHubSpotRepository.SaveHubSpotContact(objHubSpotContact);

                        //contactcount++;

                        //long contact_id = Convert.ToInt64(vid);
                        //long CompanyId = 0;

                        //var obj_Company = await _iCompany.GetCompanyByHubSpotCompanyId(objHubSpotContact.CompanyId ?? 0);

                        //if (obj_Company != null)
                        //{
                        //    CompanyId = obj_Company.Id;
                        //}

                        //var obj_genContact = await _iContactRepository.GetContactByHubSpotIdAndEmil(contact_id, emailid.ToLower().Trim());

                        //if (obj_genContact == null)
                        //{
                        //    obj_genContact = new GenContact();

                        //    obj_genContact.CreatedByDatetime = GetDatetimeFromTimeStamp(CreatedTimeStamp);
                        //    obj_genContact.CreatedById = 1;
                        //}
                        //else
                        //{
                        //    obj_genContact.LastModifiedDatetime = GetDatetimeFromTimeStamp(ModifiedTimeStamp);
                        //    obj_genContact.LastModifiedById = 1;
                        //}
                        //obj_genContact.CompanyId = CompanyId;
                        //obj_genContact.EmailId = emailid;
                        //obj_genContact.Username = emailid;
                        //obj_genContact.FirstName = firstname;
                        //obj_genContact.LastName = lastname;
                        //obj_genContact.FullName = firstname + " " + lastname;
                        //obj_genContact.ContactNo = phone;
                        //obj_genContact.Regions = region;
                        //obj_genContact.City = city;
                        //obj_genContact.Skype = skype;
                        //obj_genContact.HubSpotContactId = Convert.ToInt64(vid);
                        //obj_genContact.IsPrimary = isActive;
                        //obj_genContact.IsActive = isActive;

                        //await _iContactRepository.SaveContact(obj_genContact);

                        #endregion

                    }
                    return contactcount.ToString() + " records save successfully";
                }
                else
                {
                    return " Contact does not exist";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contactcount.ToString() + " records save successfully";

        }

        /// <summary>
        /// Gets the datetime from time stamp.
        /// </summary>
        /// <param name="TimeStamp">The time stamp.</param>
        /// <returns></returns>
        [NonAction]
        public DateTime GetDatetimeFromTimeStamp(double TimeStamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(TimeStamp / 1000d)).ToLocalTime();

            return dt;
        }


        public class HubspotResponse
        {
            public int status { get; set; }
            public string ErrorMessage { get; set; }
        }
        #endregion

        #region  Company Logo Scrapping
        [NonAction]
        public async Task<string> CompanyLogo(string Company)
        {
            string pythonURL = _configuration["PythonScrapeCompanyLogo"] + "?CompanyURL=" + Company;
            string content;
            string logoName = "";

            if (!string.IsNullOrWhiteSpace(pythonURL))
            {
                using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        content = await sr.ReadToEndAsync();
                        if (content.Trim() != "Logo not found")
                        {
                            logoName = content.Trim();
                        }
                    }
                };
            }

            return logoName;
        }
        #endregion

        private AddUpdateCompanyViewModel BindAddUpdateCompany(long id, string company, string logo)
        {

            AddUpdateCompanyViewModel addUpdateCompanyViewModel = new AddUpdateCompanyViewModel();

            long CompanyID = 0;
            var objCompany = _iCompany.GetCompanyByHubSpotCompanyId(id);

            if (objCompany != null)
            {
                CompanyID = objCompany.Result.Id;
            }

            addUpdateCompanyViewModel.CompanyData.ID = CompanyID;
            addUpdateCompanyViewModel.CompanyData.Company = company;
            if(_configuration["ProjectURL_API"].ToString() != "")
            addUpdateCompanyViewModel.CompanyData.CompanyLogo = _configuration["ProjectURL_API"].ToString() + "Media/CompanyLogo/" + logo;
            return addUpdateCompanyViewModel;
        }

    }
}
