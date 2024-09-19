namespace UTSATSAPI.Controllers
{
    using DocumentFormat.OpenXml.Drawing;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Dynamic;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModel;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.CompanyProfile;
    using UTSATSAPI.Repositories.Interfaces;
    using static Google.Apis.Requests.BatchRequest;
    using static UTSATSAPI.Helpers.Enum;

    [Authorize]
    [Route("Company/", Name = "Company")]

    public class CompanyController : ControllerBase
    {
        #region Variables
        private readonly ICompany _iCompany;
        private readonly IContactRepository _iContactRepository;
        private readonly IUniversalProcRunner _iUniversalProcRunner;
        private readonly IConfiguration _iConfiguration;
        private readonly TalentConnectAdminDBContext _iDBContext;
        private readonly IClient _iClient;
        private readonly IHiringRequest _iHiringRequest;
        #endregion

        #region Constructors
        public CompanyController(ICompany iCompany, IConfiguration configuration, IContactRepository contactRepository, IClient client, IUniversalProcRunner universalProcRunner, IHiringRequest iHiringRequest, TalentConnectAdminDBContext _DBContext)
        {
            _iCompany = iCompany;
            _iConfiguration = configuration;
            _iContactRepository = contactRepository;
            _iClient = client;
            _iUniversalProcRunner = universalProcRunner;
            _iDBContext = _DBContext;
            _iHiringRequest = iHiringRequest;
        }
        #endregion

        #region Public APIs

        [HttpPost("List")]
        public async Task<ObjectResult> GetCompanyList([FromBody] ViewAllCompanyViewModel viewAllCompany)
        {
            try
            {
                #region PreValidation
                if (viewAllCompany == null || (viewAllCompany.pagenumber == 0 || viewAllCompany.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                viewAllCompany.filterFields_CompanyList ??= new();

                viewAllCompany.Sortdatafield = string.IsNullOrEmpty(viewAllCompany.Sortdatafield) ? "Id" : viewAllCompany.Sortdatafield;
                viewAllCompany.Sortorder = string.IsNullOrEmpty(viewAllCompany.Sortorder) ? "DESC" : viewAllCompany.Sortorder;

                object[] param = new object[] {
                viewAllCompany.pagenumber, viewAllCompany.totalrecord, viewAllCompany.Sortdatafield, viewAllCompany.Sortorder,
                viewAllCompany.filterFields_CompanyList.Company, viewAllCompany.filterFields_CompanyList.CompanyDomain,
                viewAllCompany.filterFields_CompanyList.Location, viewAllCompany.filterFields_CompanyList.Contact_Status,
                viewAllCompany.filterFields_CompanyList.GEO, viewAllCompany.filterFields_CompanyList.AM_SalesPerson,
                viewAllCompany.filterFields_CompanyList.NBD_SalesPerson, viewAllCompany.filterFields_CompanyList.TeamLead,
                viewAllCompany.filterFields_CompanyList.Lead_Type, viewAllCompany.filterFields_CompanyList.LeadUser};

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_UTS_GetCompanyList_Result> companyListData = await _iCompany.GetCompanyList(paramasString).ConfigureAwait(false);

                if (companyListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(companyListData, companyListData[0].TotalRecords.Value, viewAllCompany.totalrecord, viewAllCompany.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Company Available", Details = CustomRendering.EmptyRows() });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetDetails")]
        public async Task<ObjectResult> GetCompanyDetail(long CompanyID, string companyurl = "")
        {
            try
            {
                if (CompanyID > 0)
                {
                    GetCompanyDetails getCompanyDetails = await FetchCompanyDetails(CompanyID);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = getCompanyDetails });
                }
                else if (CompanyID == 0)
                {
                    GetCompanyDetails getCompanyDetails = await FetchCompanyDetailsFromAI(companyurl);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = getCompanyDetails });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID/companyurl not valid", Details = null });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("UpdateDetails")]
        public async Task<ObjectResult> UpdateCompanyDetails([FromBody] UpdateCompanyDetails updateDetails)
        {
            try
            {
                #region Variable
                long LoggedInUserId = SessionValues.LoginUserId;
                long? CompanyID = 0;
                bool IsNewCompany = false;
                short? Portal = (short)AppActionDoneBy.UTS;
                #endregion

                #region Validation
                if (updateDetails != null)
                {
                    if (updateDetails.BasicDetails == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company Basic Details must not be empty.", Details = null });
                    }
                    CompanyID = updateDetails?.BasicDetails?.CompanyID ?? 0;

                    updateDetails.IsUpdateFromPreviewPage = updateDetails.IsUpdateFromPreviewPage ?? false;

                    if (CompanyID > 0)
                        IsNewCompany = false;
                    else
                        IsNewCompany = true;

                    if ((bool)updateDetails.IsUpdateFromPreviewPage)
                    {
                        if (CompanyID == 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company ID must be greater than 0.", Details = null });
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(updateDetails?.BasicDetails?.CompanyName))
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company Name Required.", Details = null });
                        }
                        if (string.IsNullOrEmpty(updateDetails?.BasicDetails?.WebsiteUrl))
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company Website is Required.", Details = null });
                        }
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Object must not be empty.", Details = null });
                }
                if (updateDetails?.ClientDetails != null)
                {
                    foreach (var item in updateDetails.ClientDetails)
                    {
                        #region need to indeitify its new client or existing client

                        long ClientID = 0;
                        bool IsNewClient = true;

                        if (!string.IsNullOrEmpty(item.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(item.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(item.en_Id)) > 0)
                        {
                            ClientID = Convert.ToInt64(CommonLogic.Decrypt(item.en_Id));
                            IsNewClient = false;
                        }

                        #endregion

                        #region Check Validation for new client only 
                        if (IsNewClient)
                        {
                            Sproc_ValidateAddClient_Result obj1 = new();
                            object[] ValidateParam = new object[]
                            {
                               item.emailId,
                               updateDetails.BasicDetails.CompanyName,
                               CompanyID,
                               string.Empty
                            };
                            string paramasString = CommonLogic.ConvertToParamString(ValidateParam);

                            obj1 = _iClient.Sproc_Validate_AddClient(paramasString);

                            if (obj1.status == 400)
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = obj1.result, Details = obj1 });
                        }
                        #endregion
                    }
                }
                #endregion

                #region Save React payload

                var routeData = HttpContext.GetRouteData();
                var routeName = routeData.Values["controller"] + "/" + routeData.Values["action"];
                var ReactPayload = JsonConvert.SerializeObject(updateDetails);

                GenUtsadminReactPayload ReactPayloadObj = new();
                ReactPayloadObj.CompanyId = updateDetails?.BasicDetails?.CompanyID;
                ReactPayloadObj.Hrid = null;
                ReactPayloadObj.CreatedById = LoggedInUserId;
                ReactPayloadObj.Apiname = routeName.ToString();
                ReactPayloadObj.Payload = ReactPayload;
                ReactPayloadObj.AppActionDoneBy = (short)AppActionDoneBy.UTS;

                _iUniversalProcRunner.InsertReactPayload(ReactPayloadObj);

                #endregion

                #region 1) ADD/Update Company & Company Basic Details -- Sproc_Update_Basic_CompanyDetails
                if (updateDetails.BasicDetails != null)
                {
                    string CompanyLogo = null;
                    if (!string.IsNullOrEmpty(updateDetails.BasicDetails.CompanyLogo))
                    {
                        CompanyLogo = System.IO.Path.GetFileName(updateDetails.BasicDetails.CompanyLogo);
                    }

                    updateDetails.BasicDetails.IsDeleteCompanyLogo = updateDetails?.BasicDetails?.IsDeleteCompanyLogo ?? false;
                    if ((bool)updateDetails.BasicDetails.IsDeleteCompanyLogo)
                    {
                        if (!string.IsNullOrEmpty(CompanyLogo))
                        {
                            var path = _iConfiguration["AdminProjectURL"].ToString();
                            string filePath = string.Format(@"{0}/{1}", path, CompanyLogo);

                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                        CompanyLogo = string.Empty;
                    }

                    object[] param = new object[]
                    {
                            CompanyID,
                            updateDetails?.BasicDetails?.CompanyName,
                            updateDetails?.BasicDetails?.FoundedYear,
                            updateDetails?.BasicDetails?.CompanySize,
                            updateDetails?.BasicDetails?.WebsiteUrl,
                            updateDetails?.BasicDetails?.CompanyType,
                            updateDetails?.BasicDetails?.Industry,
                            updateDetails?.BasicDetails?.Headquaters,
                            updateDetails?.BasicDetails?.AboutCompanyDesc != null ? null : updateDetails?.BasicDetails?.AboutCompanyDesc,
                            updateDetails?.BasicDetails?.Culture != null ? null : updateDetails?.BasicDetails?.Culture,
                            LoggedInUserId,
                            CompanyLogo,
                            updateDetails?.BasicDetails?.IsSelfFunded,
                            Portal,
                            updateDetails?.BasicDetails?.LinkedInProfile,
                            updateDetails?.BasicDetails?.TeamSize
                    };

                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    Sproc_Update_Basic_CompanyDetails_Result result =
                    _iCompany.UpdateCompanyBasicDetails(paramString);

                    if (result != null && result.CompanyID > 0)
                    {
                        CompanyID = result.CompanyID;


                        #region Update Company Details About desc With Unicode Characters
                        if (!string.IsNullOrEmpty(updateDetails?.BasicDetails?.AboutCompanyDesc))
                            _iCompany.SaveCompanyDescUnicode(CompanyID ?? 0, updateDetails?.BasicDetails?.AboutCompanyDesc, LoggedInUserId);
                        #endregion

                        #region Update  Culture With Unicode Characters
                        if (!string.IsNullOrEmpty(updateDetails?.BasicDetails?.Culture))
                            _iCompany.SaveCultureDetailUnicode(CompanyID ?? 0, updateDetails?.BasicDetails?.Culture, LoggedInUserId);
                        #endregion

                    }

                }
                #endregion

                #region 2) Update Funding Details -- Sproc_Add_Company_Funding_Details
                bool SelfFunded = updateDetails?.BasicDetails?.IsSelfFunded ?? false;
                if (!SelfFunded && updateDetails?.FundingDetails != null)
                {
                    foreach (var item in updateDetails.FundingDetails)
                    {
                        object[] param = new object[]
                        {
                                CompanyID,
                                item?.FundingAmount,
                                item?.FundingRound,
                                item?.Series,
                                item?.MONTH,
                                item?.YEAR,
                                item?.Investors,
                                LoggedInUserId,
                                Portal,
                                item?.FundingID,
                                item?.AdditionalInformation != null ? null : item?.AdditionalInformation
                        };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                        _iCompany.Sproc_Add_Company_Funding_Details_Result(paramString);

                        #region Update AddiInfo With Unicode Characters
                        if (!string.IsNullOrEmpty(item?.AdditionalInformation))
                            _iCompany.SaveAdditionalInfoUnicode(CompanyID ?? 0, item?.AdditionalInformation, LoggedInUserId);
                        #endregion
                    }
                }
                #endregion

                #region 3) Add Culture Image -- Sproc_Add_Company_CultureandPerksDetails
                if (updateDetails?.CultureDetails != null)
                {
                    foreach (var item in updateDetails.CultureDetails)
                    {
                        if (!string.IsNullOrEmpty(item.Culture_Image) &&
                            (item?.CultureID == 0 || item?.CultureID == null))
                        {
                            string Culture_Image = System.IO.Path.GetFileName(item.Culture_Image);

                            object[] param = new object[]
                            {
                                CompanyID,
                                Culture_Image,
                                LoggedInUserId,
                                Portal,
                                item?.CultureID
                            };
                            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iCompany.Sproc_Add_Company_CultureandPerksDetails_Result(paramString);
                        }
                    }
                }
                #endregion

                #region 4) Update Perk Details -- Sproc_Add_Company_PerksDetails
                if (updateDetails?.PerkDetails != null && updateDetails.PerkDetails.Any())
                {
                    string PerksString = string.Join(",", updateDetails.PerkDetails);
                    if (!string.IsNullOrEmpty(PerksString))
                    {
                        object[] param = new object[]
                        {
                                CompanyID,
                                PerksString,
                                LoggedInUserId,
                                Portal
                        };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                        _iCompany.Sproc_Add_Company_PerksDetails_Result(paramString);
                    }
                }
                #endregion

                #region 5) Add YouTube Details -- Sproc_Add_YoutubeLink
                if (updateDetails?.YouTubeDetails != null)
                {
                    foreach (var item in updateDetails.YouTubeDetails)
                    {
                        if (!string.IsNullOrEmpty(item.YoutubeLink) &&
                            (item?.YoutubeID == 0 || item?.YoutubeID == null))
                        {
                            object[] param = new object[]
                            {
                                CompanyID,
                                item?.YoutubeLink,
                                LoggedInUserId,
                                Portal,
                                item?.YoutubeID
                            };
                            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iCompany.Sproc_Add_YoutubeLink(paramString);
                        }
                    }
                }
                #endregion

                #region 6) Add/Update Contact(Client) Details  -- sproc_UTS_UpdateContactDetails
                List<SummaryClientDetails> summaryClients = new List<SummaryClientDetails>();
                if (updateDetails?.ClientDetails != null)
                {
                    foreach (var item in updateDetails.ClientDetails)
                    {
                        #region need to identify it is new client or existing client

                        long ClientID = 0;
                        bool IsNewClient = true;
                        string DecryptID = string.Empty;

                        if (!string.IsNullOrEmpty(item.en_Id))
                        {
                            DecryptID = CommonLogic.Decrypt(item.en_Id);
                        }

                        if (!string.IsNullOrEmpty(DecryptID) && Convert.ToInt64(DecryptID) > 0)
                        {
                            ClientID = Convert.ToInt64(DecryptID);
                            IsNewClient = false;
                        }

                        #endregion

                        #region SP call : sproc_UTS_UpdateContactDetails
                        object[] param = new object[]
                        {
                                 CompanyID,
                                 ClientID,
                                 item?.fullName,
                                 item?.emailId,
                                 item?.designation,
                                 item?.AccessRoleId,
                                 item?.isPrimary,
                                 item?.phoneNumber,
                                 LoggedInUserId,
                                 Portal,
                                 item?.Password,
                                 item?.EncryptedPassword
                         };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                        sproc_UTS_UpdateContactDetails_Result result = _iCompany.UpdateClientDetails(paramString);

                        #endregion

                        #region Summary details
                        SummaryClientDetails summaryClient = new SummaryClientDetails();
                        if (IsNewClient && result != null && result.ContactID > 0)
                            summaryClient.ClientID = result.ContactID;
                        else
                            summaryClient.ClientID = ClientID;
                        summaryClient.ClientEmail = item.emailId;
                        summaryClient.isNewlyAdded = IsNewClient;
                        summaryClients.Add(summaryClient);
                        #endregion

                        #region for new client only we need to send mail
                        if (IsNewClient && result != null && result.ContactID > 0)
                        {
                            GenContact genContact = _iDBContext.GenContacts.Where(x => x.Id == result.ContactID).FirstOrDefault();
                            if (genContact != null)
                            {
                                EmailBinder emailBinder = new EmailBinder(_iConfiguration, _iDBContext);

                                // Company registered from backend so send password set link
                                var loggedInUserDetail = await _iClient.UserDetails(LoggedInUserId).ConfigureAwait(false);
                                if (loggedInUserDetail != null)
                                {
                                    emailBinder.SetPasswordSendEmail(genContact, loggedInUserDetail.FullName, loggedInUserDetail.EmailId, loggedInUserDetail.Designation, updateDetails.PocId ?? 0);
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region 7) Update Company Engengement Details  -- Sproc_Update_Company_EngagementDetails
                if (updateDetails?.EngagementDetails != null)
                {
                    object[] param = new object[]
                    {
                        CompanyID,
                        updateDetails?.EngagementDetails?.CompanyTypeID,
                        updateDetails?.EngagementDetails?.AnotherCompanyTypeID,
                        updateDetails?.EngagementDetails?.IsPostaJob,
                        updateDetails?.EngagementDetails?.IsProfileView,
                        updateDetails?.EngagementDetails?.JPCreditBalance,
                        updateDetails?.EngagementDetails?.IsTransparentPricing,
                        updateDetails?.EngagementDetails?.IsVettedProfile,
                        updateDetails?.EngagementDetails?.CreditAmount,
                        updateDetails?.EngagementDetails?.CreditCurrency,
                        updateDetails?.EngagementDetails?.JobPostCredit,
                        updateDetails?.EngagementDetails?.VettedProfileViewCredit,
                        updateDetails?.EngagementDetails?.NonVettedProfileViewCredit,
                        updateDetails?.EngagementDetails?.HiringTypePricingId,
                        LoggedInUserId,
                        Portal
                    };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iCompany.UpdateCompanyEngagementDetails(paramString);
                }
                #endregion

                #region 8) Update Company POC Details  -- sproc_UTS_UpdatePOCUserIDsByCompanyID
                if (updateDetails?.PocId != null && updateDetails.PocId > 0)
                {
                    //string PocIdsString = string.Join(",", updateDetails.PocIds);
                    long PocID = updateDetails.PocId ?? 0;
                    long HRID = updateDetails.HRID ?? 0;
                    string Sales_AM_NBD = updateDetails.Sales_AM_NBD ?? "";
                    if (PocID > 0)
                    {
                        object[] param = new object[]
                            {
                                CompanyID,
                                //PocIdsString,
                                PocID,
                                LoggedInUserId,
                                HRID,
                                Sales_AM_NBD
                             };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                        _iCompany.DeleteInsertPOCDetails(paramString);
                    }
                }
                #endregion

                #region CompanyLogoScrap To InternalTeam Email while create company
                if (IsNewCompany)
                {
                    string ClientName = "";
                    if (updateDetails.ClientDetails != null && updateDetails.ClientDetails.Any())
                    {
                        ClientName = updateDetails?.ClientDetails[0]?.fullName ?? "";
                    }

                    string CompanyName = updateDetails?.BasicDetails?.CompanyName ?? "";
                    bool IsCompanyLogoFound = false;
                    string CompanyLogo = "";

                    if (!string.IsNullOrEmpty(updateDetails?.BasicDetails?.CompanyLogo))
                    {
                        IsCompanyLogoFound = true;
                        CompanyLogo = System.IO.Path.GetFileName(updateDetails?.BasicDetails?.CompanyLogo);
                    }
                    string WebsiteUrl = updateDetails?.BasicDetails?.WebsiteUrl ?? "";

                    EmailBinder email = new EmailBinder(_iConfiguration, _iDBContext);
                    email.SendEmailToInternalTeamWhenCompanyLogoScrap(ClientName, CompanyName, IsCompanyLogoFound, CompanyLogo, WebsiteUrl, updateDetails?.PocId);
                }
                #endregion

                #region ATSCalls
                if (!_iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    AddUpdateCompanyViewModel addUpdateCompanyViewModel = null;
                    if (CompanyID > 0)
                    {
                        addUpdateCompanyViewModel = await MakeCompanyModelForATS((long)CompanyID);
                    }

                    if (addUpdateCompanyViewModel != null)
                    {
                        try
                        {
                            var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                            ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                            aTSCall.SendAddEditCompanyData(json, LoggedInUserId);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Updated Company profile details", Details = null });
                        }
                    }
                }
                #endregion

                #region return summary object
                SummaryDetails summaryDetails = new SummaryDetails();
                summaryDetails.CompanyID = CompanyID;
                summaryDetails.CompanyName = updateDetails?.BasicDetails?.CompanyName;
                summaryDetails.IsRedirectFromHRPage = updateDetails?.IsRedirectFromHRPage ?? false;
                summaryDetails.summaryClients = summaryClients;
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Updated Company profile details", Details = summaryDetails });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("ValidateCompanyClient")]
        public async Task<ObjectResult> ValidateClient([FromBody] ValidateClient validate)
        {
            try
            {
                if (validate == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty", Details = validate });
                }
                if (string.IsNullOrEmpty(validate.CompanyName))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "WorkEmail / CompanyName is blank", Details = validate });
                }

                Sproc_ValidateAddClient_Result obj = new();

                object[] param = new object[]
                {
                     validate.WorkEmail ?? "",
                     validate.CompanyName,
                     validate.CurrentCompanyID ?? 0,
                     validate.WebsiteURL ?? ""
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                obj = _iClient.Sproc_Validate_AddClient(paramasString);

                if (obj.status == 200)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Validation Successfully", Details = obj });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = obj.result, Details = obj });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("UploadImage")]
        public async Task<ObjectResult> UploadImage([FromForm] FileUploadViewModel model)
        {
            try
            {
                if (model.Files == null || model.Files.Count == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No files uploaded.", Details = null });

                string UploadFolderPath = "", GetFolderPath = "";
                List<string> uploadedFiles = new List<string>();

                if (model.IsCompanyLogo.HasValue && model.IsCompanyLogo.Value)
                {
                    UploadFolderPath = _iConfiguration["AdminProjectURL"].ToString();
                    GetFolderPath = _iConfiguration["UTSAdminContactLogo"].ToString();
                }
                if (model.IsCultureImage.HasValue && model.IsCultureImage.Value)
                {
                    UploadFolderPath = _iConfiguration["CultureImagesUpload"].ToString();
                    GetFolderPath = _iConfiguration["CultureImages"].ToString();
                }

                //If file path does not exists then create the path.
                if (!Directory.Exists(UploadFolderPath))
                {
                    Directory.CreateDirectory(UploadFolderPath);
                }

                foreach (var file in model.Files)
                {
                    if (file.Length > 0)
                    {
                        string filePath = string.Format(@"{0}/{1}", UploadFolderPath, file.FileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        filePath = GetFolderPath + file.FileName;
                        uploadedFiles.Add(filePath);
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Files uploaded Successfully.", Details = uploadedFiles });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DeleteFundingDetails")]
        public async Task<ObjectResult> DeleteFundingDetails([FromBody] DeleteFundingDetails deleteFunding)
        {
            try
            {
                long LoginUserId = SessionValues.LoginUserId;
                short? Portal = (short)AppActionDoneBy.FrontEnd;

                if (deleteFunding == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Object must not be empty", Details = null });
                }

                if (deleteFunding.CompanyID > 0 && deleteFunding.FundingID > 0)
                {
                    object[] param = new object[]
                    {
                            deleteFunding.CompanyID,
                            deleteFunding.FundingID,
                            LoginUserId,
                            Portal,
                        };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                    _iCompany.Delete_Company_Funding_Details(paramString);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Deleted", Details = null });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID/FundingID not valid", Details = null });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DeleteCultureImage")]
        public async Task<ObjectResult> DeleteCultureImage([FromBody] DeleteCultureImage deleteImage)
        {
            try
            {
                long LoginUserId = SessionValues.LoginUserId;
                short? Portal = (short)AppActionDoneBy.FrontEnd;

                if (deleteImage == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Object must not be empty", Details = null });
                }

                if (deleteImage.CompanyID > 0 && deleteImage.CultureID > 0)
                {
                    object[] param = new object[]
                    {
                            deleteImage.CompanyID,
                            deleteImage.CultureID,
                            LoginUserId,
                            Portal,
                        };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iCompany.Delete_Company_CultureandPerksDetails(paramString);

                    //Delete image from path if exist
                    if (!string.IsNullOrEmpty(deleteImage.Culture_Image))
                    {
                        string Culture_Image = System.IO.Path.GetFileName(deleteImage.Culture_Image);

                        var path = _iConfiguration["CultureImagesUpload"].ToString();
                        string filePath = string.Format(@"{0}/{1}", path, deleteImage.Culture_Image);

                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Deleted", Details = null });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID/CultureID not valid", Details = null });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DeleteYouTubeDetails")]
        public async Task<ObjectResult> DeleteYouTubeDetails([FromBody] DeleteYouTubeDetails deleteVideo)
        {
            try
            {
                long LoginUserId = SessionValues.LoginUserId;
                short? Portal = (short)AppActionDoneBy.FrontEnd;

                if (deleteVideo == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Object must not be empty", Details = null });
                }

                if (deleteVideo.CompanyID > 0 && deleteVideo.YoutubeID > 0)
                {
                    object[] param = new object[]
                    {
                            deleteVideo.CompanyID,
                            deleteVideo.YoutubeID,
                            LoginUserId,
                            Portal,
                        };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iCompany.Delete_Company_YoutubeDetails(paramString);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Deleted", Details = null });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID/CultureID not valid", Details = null });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("SyncCompanyProfile")]
        public async Task<ObjectResult> SyncCompanyProfile(long CompanyID)
        {
            long LoggedInUserId = SessionValues.LoginUserId;
            try
            {
                #region ATSCalls
                if (!_iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    AddUpdateCompanyViewModel addUpdateCompanyViewModel = null;
                    if (CompanyID > 0)
                    {
                        addUpdateCompanyViewModel = await MakeCompanyModelForATS((long)CompanyID);
                    }

                    if (addUpdateCompanyViewModel != null)
                    {
                        try
                        {
                            var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                            ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                            aTSCall.SendAddEditCompanyData(json, LoggedInUserId);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = null });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = null });
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CommonMethods
        [NonAction]
        public async Task<AddUpdateCompanyViewModel> MakeCompanyModelForATS(long CompanyID)
        {
            AddUpdateCompanyViewModel addUpdateCompanyViewModel = new AddUpdateCompanyViewModel();

            //Basic Details
            addUpdateCompanyViewModel.CompanyData = _iCompany.Sproc_CompanyDetail_TransferToATS_Result(CompanyID);
            if (addUpdateCompanyViewModel.CompanyData != null &&
                !string.IsNullOrEmpty(addUpdateCompanyViewModel.CompanyData.CompanyLogo))
            {
                addUpdateCompanyViewModel.CompanyData.CompanyLogo = _iConfiguration["UTSAdminContactLogo"].ToString() + addUpdateCompanyViewModel.CompanyData.CompanyLogo;
            }

            //FundingDetails
            addUpdateCompanyViewModel.FundingDetails = _iCompany.Sproc_Get_Company_Funding_Details_Result(CompanyID);

            //CultureImages
            addUpdateCompanyViewModel.CultureDetails = _iCompany.Sproc_Get_Company_CultureandPerksDetails_Result(CompanyID);
            if (addUpdateCompanyViewModel.CultureDetails != null && addUpdateCompanyViewModel.CultureDetails.Any())
            {
                addUpdateCompanyViewModel.CultureDetails
                    .Where(item => !string.IsNullOrEmpty(item.CultureImage))
                    .ToList()
                    .ForEach(item => item.CultureImage = _iConfiguration["CultureImages"].ToString() + item.CultureImage);
            }

            //Perk
            List<Sproc_Get_Company_PerksDetails_Result> PerkDetails = _iCompany.Sproc_Get_Company_PerksDetails_Result(CompanyID);
            if (PerkDetails != null && PerkDetails.Any())
            {
                addUpdateCompanyViewModel.PerkDetails = PerkDetails.Select(x => x.Perks).ToList();
            }

            //YouTubeDetails
            addUpdateCompanyViewModel.YouTubeDetails = _iCompany.Sproc_Get_Company_YouTubeDetails_Result(CompanyID);

            //ContactDetails
            addUpdateCompanyViewModel.ContactDetails = await _iContactRepository.GetContactDetails(CompanyID);
            if (addUpdateCompanyViewModel.ContactDetails != null)
                addUpdateCompanyViewModel.ContactDetails.ForEach(x => x.en_Id = CommonLogic.Encrypt(x.ID.ToString()));

            //Pass POCName
            sp_UTS_GetPOCUserIDByCompanyID_Edit_Result obj = _iCompany.GetPOCUserIDByCompanyIDEdit(CompanyID);
            if (obj != null)
                addUpdateCompanyViewModel.PocUserName = obj.POCName;

            return addUpdateCompanyViewModel;
        }

        [NonAction]
        public async Task<GetCompanyDetails> FetchCompanyDetails(long CompanyID)
        {
            GetCompanyDetails getCompanyDetails = new GetCompanyDetails();

            //BasicDetails
            getCompanyDetails.BasicDetails = _iCompany.Sproc_Get_Basic_CompanyDetails_Result(CompanyID);
            if (getCompanyDetails.BasicDetails != null &&
                    !string.IsNullOrEmpty(getCompanyDetails.BasicDetails.CompanyLogo))
            {
                getCompanyDetails.BasicDetails.CompanyLogo = getCompanyDetails.BasicDetails.CompanyLogo;
            }

            //FundingDetails
            getCompanyDetails.FundingDetails = _iCompany.Sproc_Get_Company_Funding_Details_Result(CompanyID);

            //CultureImage
            getCompanyDetails.CultureDetails = _iCompany.Sproc_Get_Company_CultureandPerksDetails_Result(CompanyID);
            if (getCompanyDetails.CultureDetails != null && getCompanyDetails.CultureDetails.Any())
            {
                getCompanyDetails.CultureDetails
                    .Where(item => !string.IsNullOrEmpty(item.CultureImage))
                    .ToList()
                    .ForEach(item => item.CultureImage = _iConfiguration["CultureImages"].ToString() + item.CultureImage);
            }

            //Perk
            List<Sproc_Get_Company_PerksDetails_Result> PerkDetails = _iCompany.Sproc_Get_Company_PerksDetails_Result(CompanyID);
            if (PerkDetails != null && PerkDetails.Any())
                getCompanyDetails.PerkDetails = PerkDetails.Select(x => x.Perks).ToList();

            //YouTubeDetails
            getCompanyDetails.YouTubeDetails = _iCompany.Sproc_Get_Company_YouTubeDetails_Result(CompanyID);

            //ContactDetails
            getCompanyDetails.ContactDetails = await _iContactRepository.GetContactDetails(CompanyID);
            if (getCompanyDetails.ContactDetails != null)
                getCompanyDetails.ContactDetails.ForEach(x => x.en_Id = CommonLogic.Encrypt(x.ID.ToString()));

            //EngagementDetails
            getCompanyDetails.EngagementDetails = _iCompany.GetCompanyEngagementDetails(CompanyID);

            //POCUserID
            getCompanyDetails.PocUserDetails = _iCompany.GetPOCUserIDByCompanyID(CompanyID);
            if (getCompanyDetails.PocUserDetails != null && getCompanyDetails.PocUserDetails.Any())
                getCompanyDetails.PocUserIds = getCompanyDetails.PocUserDetails.Select(x => x.POCUserID).ToList();

            getCompanyDetails.PocUserDetailsEdit = _iCompany.GetPOCUserIDByCompanyIDEdit(CompanyID);
            if (getCompanyDetails.PocUserDetailsEdit != null)
                getCompanyDetails.PocUserId = getCompanyDetails.PocUserId;

            // UTS-8531: Show whatsapp CTA in company detail page.
            try
            {
                getCompanyDetails.WhatsappDetails = _iCompany.Sproc_UTS_GetCompanyWhatsappDetails(CompanyID);
                if (getCompanyDetails.WhatsappDetails != null && getCompanyDetails.WhatsappDetails.Count == 0 && getCompanyDetails.PocUserDetailsEdit != null)
                {
                    getCompanyDetails.ShowWhatsappCTA = true;
                }
                else
                {
                    getCompanyDetails.ShowWhatsappCTA = false;
                }
            }
            catch
            {
                getCompanyDetails.WhatsappDetails = null;
                getCompanyDetails.ShowWhatsappCTA = false;
            }

            return getCompanyDetails;
        }

        [NonAction]
        public async Task<GetCompanyDetails> FetchCompanyDetailsFromAI(string CompanyUrl)
        {
            GetCompanyDetails getCompanyDetails = new GetCompanyDetails();

            var basicCompanyDetails = await GetCompanyBasicDetailsFromPython(CompanyUrl);
            if (basicCompanyDetails != null)
            {
                #region Fetch Logo from Parsing and save into file directory on server 
                try
                {
                    string CompanyLogoName = await CompanyLogo(CompanyUrl);
                    if (!string.IsNullOrEmpty(CompanyLogoName))
                    {
                        basicCompanyDetails.CompanyLogo = CompanyLogoName;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                #endregion

                getCompanyDetails.BasicDetails = new Sproc_Get_Basic_CompanyDetails_Result();
                getCompanyDetails.BasicDetails.AboutCompany = basicCompanyDetails.Brief_About_Company;
                getCompanyDetails.BasicDetails.CompanyLogo = basicCompanyDetails.CompanyLogo;
                getCompanyDetails.BasicDetails.CompanyType = basicCompanyDetails.Company_Type;
                getCompanyDetails.BasicDetails.CompanyIndustry = basicCompanyDetails.Company_Industry_Type;
                getCompanyDetails.BasicDetails.Headquaters = basicCompanyDetails.Headquarters;
                getCompanyDetails.BasicDetails.FoundedYear = basicCompanyDetails.Founded_In;
                getCompanyDetails.BasicDetails.Culture = basicCompanyDetails.Brief_About_Culture;
            }

            var otherCompanyDetails = await GetCompanyOtherDetailsFromPython(CompanyUrl);
            if (otherCompanyDetails != null)
            {
                getCompanyDetails.CultureDetails = new List<Sproc_Get_Company_CultureandPerksDetails_Result>();
                if (otherCompanyDetails.CultureDetails != null)
                {
                    foreach (var item in otherCompanyDetails.CultureDetails)
                    {
                        Sproc_Get_Company_CultureandPerksDetails_Result item1 = new Sproc_Get_Company_CultureandPerksDetails_Result
                        {
                            CultureID = 0,
                            CultureImage = item.Culture_Images
                        };
                        getCompanyDetails.CultureDetails.Add(item1);
                    }
                }
                if (otherCompanyDetails.Funding_Details != null)
                {
                    getCompanyDetails.FundingDetails = new List<Sproc_Get_Company_Funding_Details_Result>();
                    foreach (var item in otherCompanyDetails.Funding_Details)
                    {
                        Sproc_Get_Company_Funding_Details_Result item1 = new Sproc_Get_Company_Funding_Details_Result
                        {
                            AllInvestors = item.Investors,
                            FundingAmount = item.Funding_Amount,
                            FundingMonth = item.Month,
                            FundingRound = item.Funding_Round,
                            FundingYear = item.Year,
                            Series = item.Type
                        };
                        getCompanyDetails.FundingDetails.Add(item1);
                    }
                }

            }


            return getCompanyDetails;
        }
        #endregion

        #region Get Company details from Company URL        
        [HttpGet("GetCompanyDetailsFromURL")]
        public async Task<ObjectResult> GetCompanyDetailsFromURL(string companyurl, long CompanyID)
        {
            try
            {
                #region PreValidation
                if (companyurl == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company URL is not blank." });
                }

                if (CompanyID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID is not null." });
                }
                #endregion
                string LogedInUserID = SessionValues.LoginUserId.ToString();
                long LogedInUserIdval = SessionValues.LoginUserId;
                AllCompanyDetails companyDetailsViewModel = new AllCompanyDetails();
                ClientSignUp clientSignUp = new ClientSignUp();
                clientSignUp.CompanyId = CompanyID;

                var genCompany = _iCompany.CompanyDetails(null, CompanyID);

                //UTS- 5977: Fetch the company details from company name from python file
                companyDetailsViewModel = await GetCompanyDetailsFromPython(companyurl);
                if (companyDetailsViewModel != null)
                {
                    //clientSignUp.CompanySize = companyDetailsViewModel.Company_Size;
                    clientSignUp.CompanySize_RangeorAdhoc = companyDetailsViewModel.Company_Size;
                    clientSignUp.CompanyIndustryType = companyDetailsViewModel.Company_Industry_Type;
                    clientSignUp.BriefAboutCompany = companyDetailsViewModel.Brief_About_Company;
                    clientSignUp.Culture = companyDetailsViewModel.Brief_About_Culture;
                    clientSignUp.FoundedYear = companyDetailsViewModel.Founded_In;
                    clientSignUp.CompanyType = companyDetailsViewModel.Company_Type;
                    clientSignUp.Headquaters = companyDetailsViewModel.Headquarters;

                    object[] param = new object[] { CompanyID, clientSignUp.CompanySize_RangeorAdhoc, clientSignUp.CompanyIndustryType, clientSignUp.BriefAboutCompany, clientSignUp.Culture, clientSignUp.FoundedYear, clientSignUp.CompanyType, clientSignUp.Headquaters, LogedInUserIdval };
                    string paramString = CommonLogic.ConvertToParamString(param);
                    _iCompany.sproc_Update_Company_Details_From_Scrapping_Result(paramString);
                }



                #region Fetch Logo
                if (genCompany != null)
                {
                    if (!string.IsNullOrEmpty(genCompany.Result.CompanyLogo))
                    {
                        try
                        {
                            string CompanyLogoName = await CompanyLogo(companyurl);
                            if (!string.IsNullOrEmpty(CompanyLogoName))
                            {
                                clientSignUp.CompanyLogo = CompanyLogoName;
                            }
                            if (CompanyLogoName != "")
                            {
                                //EmailBinder email = new EmailBinder(_iConfiguration, _iDBContext);
                                //var emailres = email.SendEmailToInternalTeamWhenCompanyLogoScrap(clientSignUp.FullName, clientSignUp.CompanyName, clientSignUp.WorkEmail, true, CompanyLogoName, companyurl);

                                _iCompany.UpdateCompanyLogo(CompanyID, CompanyLogoName);
                                AddUpdateCompanyViewModel addUpdateCompanyViewModel = BindAddUpdateCompany(CompanyID, clientSignUp.CompanyName, CompanyLogoName);
                                try
                                {
                                    var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                                    ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                                    aTSCall.SendAddEditCompanyData(json, 1);
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }

                #endregion

                short? Portal = (short)AppActionDoneBy.FrontEnd;
                #region Save Funding details Perks Details and Culture details and Other Company Details


                if (companyDetailsViewModel.Funding_Details != null)
                {
                    var genCompanyFundingDetails = _iDBContext.GenCompanyFundingDetails.Where(x => x.CompanyId == CompanyID).ToList();
                    if (genCompanyFundingDetails == null)
                    {
                        foreach (var item in companyDetailsViewModel.Funding_Details)
                        {
                            object[] param = new object[] { CompanyID, item.Funding_Amount, item.Funding_Round, item.Type, item.Month, item.Year, item.Investors, LogedInUserIdval, Portal, 0 };
                            string paramString = CommonLogic.ConvertToParamString(param);
                            _iCompany.Sproc_Add_Company_Funding_Details_Result(paramString);
                        }
                    }
                }

                if (companyDetailsViewModel.CultureDetails != null)
                {
                    var genCompanyCulutureandPerksDetails = _iDBContext.GenCompanyCultureandPerksDetails.Where(x => x.CompanyId == CompanyID).ToList();
                    if (genCompanyCulutureandPerksDetails == null)
                    {
                        foreach (var item in companyDetailsViewModel.CultureDetails)
                        {
                            object[] param = new object[] { CompanyID, item.Culture_Images, LogedInUserIdval, Portal, 0 };
                            string paramString = CommonLogic.ConvertToParamString(param);
                            _iCompany.Sproc_Add_Company_CultureandPerksDetails_Result(paramString);
                        }
                    }
                }

                if (companyDetailsViewModel.Perks != null)
                {
                    var genCompanyPerkDetails = _iDBContext.GenCompanyPerkDetails.Where(x => x.CompanyId == CompanyID).ToList();
                    if (genCompanyPerkDetails == null)
                    {
                        string PerksString = string.Join(",", companyDetailsViewModel.Perks);
                        if (!string.IsNullOrEmpty(PerksString))
                        {
                            object[] param = new object[]
                            {
                                CompanyID,
                                PerksString,
                                LogedInUserIdval,
                                Portal
                            };
                            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iCompany.Sproc_Add_Company_PerksDetails_Result(paramString);
                        }
                    }
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "", Details = companyDetailsViewModel });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region  Company Logo Scrapping
        [NonAction]
        public async Task<string> CompanyLogo(string Company)
        {
            string pythonURL = _iConfiguration["PythonScrapeCompanyLogo"] + "?CompanyURL=" + Company;
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

        #region  Fetch Company Details
        private async Task<AllCompanyDetails> GetCompanyDetailsFromPython(string companyName)
        {
            string pythonURL = _iConfiguration["PythonParsingCompanyDetailsFromName"] + "?CompanyURL=" + companyName;
            string content;
            AllCompanyDetails? companyDetailsViewModel = new AllCompanyDetails();

            try
            {
                if (!string.IsNullOrWhiteSpace(pythonURL))
                {
                    using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            content = await sr.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                string pattern = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))+(?(Open)(?!))\}";
                                string JsonString = "";
                                Regex regex = new Regex(pattern);
                                MatchCollection matches = regex.Matches(content);
                                foreach (Match match in matches)
                                {
                                    JsonString = match.Value;
                                }
                                if (!string.IsNullOrEmpty(JsonString))
                                {
                                    companyDetailsViewModel = JsonConvert.DeserializeObject<AllCompanyDetails>(JsonString);
                                }
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                companyDetailsViewModel = new AllCompanyDetails();
            }

            return companyDetailsViewModel;
        }

        private AddUpdateCompanyViewModel BindAddUpdateCompany(long id, string company, string logo)
        {

            AddUpdateCompanyViewModel addUpdateCompanyViewModel = new AddUpdateCompanyViewModel();

            addUpdateCompanyViewModel.CompanyData.ID = id;
            addUpdateCompanyViewModel.CompanyData.Company = company;
            if (!string.IsNullOrEmpty(logo))
            {
                addUpdateCompanyViewModel.CompanyData.CompanyLogo = _iConfiguration["ProjectURL_API"].ToString() + "Media/CompanyLogo/" + logo;
            }
            else
            {
                addUpdateCompanyViewModel.CompanyData.CompanyLogo = string.Empty;
            }
            return addUpdateCompanyViewModel;
        }
        #endregion

        [NonAction]
        public async Task<AllCompanyDetails> GetCompanyDetailsFromAI(string companyurl)
        {
            AllCompanyDetails companyDetailsViewModel = new AllCompanyDetails();
            #region PreValidation
            if (companyurl == null)
            {
                return companyDetailsViewModel;
            }
            #endregion

            string LogedInUserID = SessionValues.LoginUserId.ToString();
            long LogedInUserIdval = SessionValues.LoginUserId;
            ClientSignUp clientSignUp = new ClientSignUp();

            //UTS- 5977: Fetch the company details from company name from python file
            companyDetailsViewModel = await GetCompanyDetailsFromPython(companyurl);

            #region Fetch Logo
            try
            {
                string CompanyLogoName = await CompanyLogo(companyurl);
                if (!string.IsNullOrEmpty(CompanyLogoName))
                {
                    companyDetailsViewModel.CompanyLogo = CompanyLogoName;
                }
            }
            catch (Exception)
            {

                throw;
            }
            #endregion

            return companyDetailsViewModel;
        }

        #region Get Company Basic details from Company URL        
        [HttpGet("GetCompanyBasicDetailsFromURL")]
        public async Task<ObjectResult> GetCompanyBasicDetailsFromURL(string companyurl, long CompanyID)
        {
            try
            {
                #region PreValidation
                if (companyurl == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company URL is not blank." });
                }

                if (CompanyID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID is not null." });
                }
                #endregion
                string LogedInUserID = SessionValues.LoginUserId.ToString();
                long LogedInUserIdval = SessionValues.LoginUserId;
                BasicCompanyDetails companyDetailsViewModel = new BasicCompanyDetails();
                ClientSignUp clientSignUp = new ClientSignUp();
                clientSignUp.CompanyId = CompanyID;

                var genCompany = _iCompany.CompanyDetails(null, CompanyID);

                //UTS- 5977: Fetch the company details from company name from python file
                companyDetailsViewModel = await GetCompanyBasicDetailsFromPython(companyurl);
                if (companyDetailsViewModel != null)
                {
                    //clientSignUp.CompanySize = companyDetailsViewModel.Company_Size;
                    clientSignUp.CompanySize_RangeorAdhoc = companyDetailsViewModel.Company_Size;
                    clientSignUp.CompanyIndustryType = companyDetailsViewModel.Company_Industry_Type;
                    clientSignUp.BriefAboutCompany = companyDetailsViewModel.Brief_About_Company;
                    clientSignUp.Culture = companyDetailsViewModel.Brief_About_Culture;
                    clientSignUp.FoundedYear = companyDetailsViewModel.Founded_In;
                    clientSignUp.CompanyType = companyDetailsViewModel.Company_Type;
                    clientSignUp.Headquaters = companyDetailsViewModel.Headquarters;

                    object[] param = new object[] { CompanyID, clientSignUp.CompanySize_RangeorAdhoc, clientSignUp.CompanyIndustryType, clientSignUp.BriefAboutCompany, clientSignUp.Culture, clientSignUp.FoundedYear, clientSignUp.CompanyType, clientSignUp.Headquaters, LogedInUserIdval };
                    string paramString = CommonLogic.ConvertToParamString(param);
                    _iCompany.sproc_Update_Company_Details_From_Scrapping_Result(paramString);
                }



                #region Fetch Logo
                if (genCompany != null)
                {
                    if (!string.IsNullOrEmpty(genCompany.Result.CompanyLogo))
                    {
                        try
                        {
                            string CompanyLogoName = await CompanyLogo(companyurl);
                            if (!string.IsNullOrEmpty(CompanyLogoName))
                            {
                                clientSignUp.CompanyLogo = CompanyLogoName;
                            }
                            if (CompanyLogoName != "")
                            {
                                //EmailBinder email = new EmailBinder(_iConfiguration, _iDBContext);
                                //var emailres = email.SendEmailToInternalTeamWhenCompanyLogoScrap(clientSignUp.FullName, clientSignUp.CompanyName, clientSignUp.WorkEmail, true, CompanyLogoName, companyurl);

                                _iCompany.UpdateCompanyLogo(CompanyID, CompanyLogoName);
                                AddUpdateCompanyViewModel addUpdateCompanyViewModel = BindAddUpdateCompany(CompanyID, clientSignUp.CompanyName, CompanyLogoName);
                                try
                                {
                                    var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                                    ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                                    aTSCall.SendAddEditCompanyData(json, 1);
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "", Details = companyDetailsViewModel });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Method GetCompanyBasicDetailsFromPython
        private async Task<BasicCompanyDetails> GetCompanyBasicDetailsFromPython(string Companyurl)
        {
            string pythonURL = _iConfiguration["CompanyBasicDetailsFromName"] + "?CompanyURL=" + Companyurl;
            string content;
            BasicCompanyDetails? companyDetailsViewModel = new BasicCompanyDetails();

            try
            {
                if (!string.IsNullOrWhiteSpace(pythonURL))
                {
                    using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            content = await sr.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                string pattern = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))+(?(Open)(?!))\}";
                                string JsonString = "";
                                Regex regex = new Regex(pattern);
                                MatchCollection matches = regex.Matches(content);
                                foreach (Match match in matches)
                                {
                                    JsonString = match.Value;
                                }
                                if (!string.IsNullOrEmpty(JsonString))
                                {
                                    companyDetailsViewModel = JsonConvert.DeserializeObject<BasicCompanyDetails>(JsonString);
                                }
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                companyDetailsViewModel = new BasicCompanyDetails();
            }

            return companyDetailsViewModel;
        }
        #endregion

        #region Get Company Other details from Company URL        
        [HttpGet("GetCompanyOtherDetailsFromURL")]
        public async Task<ObjectResult> GetCompanyOtherDetailsFromURL(string companyurl, long CompanyID)
        {
            try
            {
                #region PreValidation
                if (companyurl == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company URL is not blank." });
                }

                if (CompanyID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "CompanyID is not null." });
                }
                #endregion
                string LogedInUserID = SessionValues.LoginUserId.ToString();
                long LogedInUserIdval = SessionValues.LoginUserId;
                OtherCompanyDetails companyDetailsViewModel = new OtherCompanyDetails();
                ClientSignUp clientSignUp = new ClientSignUp();
                clientSignUp.CompanyId = CompanyID;

                var genCompany = _iCompany.CompanyDetails(null, CompanyID);

                //UTS- 5977: Fetch the company details from company name from python file
                companyDetailsViewModel = await GetCompanyOtherDetailsFromPython(companyurl);

                short? Portal = (short)AppActionDoneBy.FrontEnd;
                #region Save Funding details Perks Details and Culture details and Other Company Details


                if (companyDetailsViewModel.Funding_Details != null)
                {
                    var genFundingDetails = _iDBContext.GenCompanyFundingDetails.Where(x => x.CompanyId == CompanyID).ToList();
                    if (genFundingDetails == null)
                    {
                        foreach (var item in companyDetailsViewModel.Funding_Details)
                        {
                            object[] param = new object[] { CompanyID, item.Funding_Amount, item.Funding_Round, item.Type, item.Month, item.Year, item.Investors, LogedInUserIdval, Portal, 0 };
                            string paramString = CommonLogic.ConvertToParamString(param);
                            _iCompany.Sproc_Add_Company_Funding_Details_Result(paramString);
                        }
                    }
                }

                if (companyDetailsViewModel.CultureDetails != null)
                {
                    var genCompanyCultureandPerksDetails = _iDBContext.GenCompanyCultureandPerksDetails.Where(x => x.CompanyId == CompanyID).ToList();
                    if (genCompanyCultureandPerksDetails == null)
                    {
                        foreach (var item in companyDetailsViewModel.CultureDetails)
                        {
                            object[] param = new object[] { CompanyID, item.Culture_Images, LogedInUserIdval, Portal, 0 };
                            string paramString = CommonLogic.ConvertToParamString(param);
                            _iCompany.Sproc_Add_Company_CultureandPerksDetails_Result(paramString);
                        }
                    }
                }

                if (companyDetailsViewModel.Perks != null)
                {
                    var genCompanyPerkDetails = _iDBContext.GenCompanyPerkDetails.Where(x => x.CompanyId == CompanyID).ToList();
                    if (genCompanyPerkDetails == null)
                    {
                        string PerksString = string.Join(",", companyDetailsViewModel.Perks);
                        if (!string.IsNullOrEmpty(PerksString))
                        {
                            object[] param = new object[]
                            {
                                CompanyID,
                                PerksString,
                                LogedInUserIdval,
                                Portal
                            };
                            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iCompany.Sproc_Add_Company_PerksDetails_Result(paramString);
                        }
                    }
                }

                #endregion

                #region ATSCalls
                if (!_iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    AddUpdateCompanyViewModel addUpdateCompanyViewModel = null;
                    if (CompanyID > 0)
                    {
                        addUpdateCompanyViewModel = await MakeCompanyModelForATS((long)CompanyID);
                    }

                    if (addUpdateCompanyViewModel != null)
                    {
                        try
                        {
                            var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                            ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                            aTSCall.SendAddEditCompanyData(json, LogedInUserIdval);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Updated Company profile details", Details = null });
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "", Details = companyDetailsViewModel });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Method GetCompanyOtherDetailsFromPython
        private async Task<OtherCompanyDetails> GetCompanyOtherDetailsFromPython(string Companyurl)
        {
            string pythonURL = _iConfiguration["CompanyOtherDetailsFromName"] + "?CompanyURL=" + Companyurl;
            string content;
            OtherCompanyDetails? companyDetailsViewModel = new OtherCompanyDetails();

            try
            {
                if (!string.IsNullOrWhiteSpace(pythonURL))
                {
                    using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            content = await sr.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                string pattern = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))+(?(Open)(?!))\}";
                                string JsonString = "";
                                Regex regex = new Regex(pattern);
                                MatchCollection matches = regex.Matches(content);
                                foreach (Match match in matches)
                                {
                                    JsonString = match.Value;
                                }
                                if (!string.IsNullOrEmpty(JsonString))
                                {
                                    companyDetailsViewModel = JsonConvert.DeserializeObject<OtherCompanyDetails>(JsonString);
                                }
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                companyDetailsViewModel = new OtherCompanyDetails();
            }

            return companyDetailsViewModel;
        }
        #endregion

        #region JobPost Preview
        [HttpGet("Preview")]
        public async Task<ObjectResult> PreviewJobPost(long companyId, long hrId)
        {
            #region Pre-Validation

            if (hrId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "please enter hrId or Guid" });
            }


            long loggedInUserId = SessionValues.LoginUserId;
            long loggedInUserCompanyId = companyId;
            long contactId = 0;
            string CompanyLogoName = "";
            //if (contactId == 0)
            //{
            //    contactId = loggedInUserId;
            //}
            #endregion


            var HRDetails = _iDBContext.GenSalesHiringRequests.Where(x => x.Id == hrId).FirstOrDefault();
            if (HRDetails != null)
            {
                contactId = HRDetails.ContactId ?? 0;
            }



            object[] param = new object[] { contactId, null, hrId };
            string paramString = CommonLogic.ConvertToParamString(param);
            Sp_UTS_PreviewJobPost_ClientPortal_Result jobPreview = await _iCompany.Sp_UTS_PreviewJobPost_ClientPortal_Result(paramString);
            GenCompany genCompany = await _iCompany.CompanyDetails("", loggedInUserCompanyId);
            if (genCompany != null)
            {
                CompanyLogoName = genCompany.CompanyLogo;
            }
            if (jobPreview != null)
            {
                #region GEt HR POC details
                List<Sproc_HR_POC_ClientPortal_Result>? hrPocData = null;
                try
                {
                    object[] pocParam = new object[]
                    {
                                    0,
                                    "",
                                    hrId,
                                    "",
                                    0,
                                    false
                    };
                    string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                    hrPocData = _iHiringRequest.SaveandGetHRPOCDetails(pocParamString);
                }
                catch
                {

                }
                #endregion

                JobPostDetailsViewModel jobPostDetails = new JobPostDetailsViewModel
                {
                    // step-1               

                    RoleName = jobPreview?.RoleName ?? "",
                    ExperienceYears = jobPreview?.ExperienceYears,
                    IsFresherAllowed = jobPreview?.IsFresherAllowed,
                    NoOfTalents = jobPreview?.NoOfTalents,
                    IsHiringLimited = jobPreview?.IsHiringLimited ?? "",
                    ContractDuration = jobPreview?.ContractDuration,
                    CurrentStepId = jobPreview?.CurrentStepId,
                    NextStepId = jobPreview?.NextStepId,
                    Availability = jobPreview?.Availability,

                    // step-2
                    Skills = jobPreview?.Skills ?? "",
                    AllSkills = jobPreview?.AllSkills ?? "",
                    BudgetFrom = jobPreview?.BudgetFrom,
                    BudgetTo = jobPreview?.BudgetTo,
                    IsConfidentialBudget = jobPreview?.IsConfidentialBudget ?? false,
                    //salary = data.Max_Salary,
                    Currency = jobPreview?.Currency ?? "",
                    CurrencySign = jobPreview?.CurrencySign ?? "",
                    BudgetFromStr = jobPreview?.BudgetFromStr ?? "",
                    BudgetToStr = jobPreview?.BudgetToStr ?? "",
                    HRCost = jobPreview?.HRCost ?? "",
                    //RoleName = data.Working_Zone_With_Time_Zone,

                    // step-3
                    EmploymentType = jobPreview?.EmploymentType ?? "",
                    HowSoon = jobPreview?.HowSoon ?? "",
                    WorkingModeId = jobPreview?.WorkingModeID,
                    CompanyLocation = string.IsNullOrEmpty(jobPreview?.CompanyLocation) ? "Remote" : jobPreview?.CompanyLocation,
                    TimezonePreferenceId = jobPreview?.Timezone_Preference_ID,
                    TimeZoneFromTime = jobPreview?.TimeZone_FromTime,
                    TimeZoneEndTime = jobPreview?.TimeZone_EndTime,
                    AchievementId = jobPreview?.AchievementID,
                    Reason = jobPreview?.Reason,
                    TimeZone = jobPreview?.TimeZone,

                    // step-4
                    Requirements = jobPreview?.Requirements,
                    RolesResponsibilities = jobPreview?.RolesResponsibilities,
                    JDFileName = jobPreview?.JDFileName ?? "",
                    GPTJDID = jobPreview?.GPTJDID ?? 0,
                    JobDescription = jobPreview?.JobDescription,
                    RoleOverviewDescription = jobPreview?.JobRoleDescription,
                    Whatweoffer = jobPreview?.Whatweoffer,

                    //Extra info
                    ContactId = jobPreview?.ContactId,
                    ProcessType = jobPreview?.ProcessType,
                    IST_TimeZone_FromTime = jobPreview?.IST_TimeZone_FromTime,
                    IST_TimeZone_EndTime = jobPreview?.IST_TimeZone_EndTime,

                    //location info
                    City = jobPreview?.City,
                    State = jobPreview?.State,
                    Country = jobPreview?.Country,
                    PostalCode = jobPreview?.PostalCode,
                    CountryID = jobPreview?.CountryID,

                    //New budget info
                    BudgetType = jobPreview?.BudgetType,

                    //Transparent Job details
                    HiringTypePricingId = jobPreview?.HiringTypePricingId,
                    PayrollTypeId = jobPreview?.PayrollTypeId,
                    PayrollPartnerName = jobPreview?.PayrollPartnerName,
                    HiringTypePricing = jobPreview?.HiringTypePricing,
                    PayrollType = jobPreview?.PayrollType,
                    CompanyLogo = CompanyLogoName,

                    // vital info
                    CompensationOption = jobPreview?.CompensationOption,
                    IndustryType = jobPreview?.IndustryType,
                    HasPeopleManagementExp = jobPreview?.HasPeopleManagementExp,
                    Prerequisites = jobPreview?.Prerequisites,
                    StringSeparator = jobPreview?.StringSeparator,
                    ToolTipMessage = jobPreview?.ToolTipMessage,
                    HRTypeId = jobPreview?.HRTypeId,
                    HRPOCUserID = hrPocData,
                    ShowHRPOCDetailsToTalents = jobPreview?.ShowHRPOCDetailsToTalents,
                    JobTypeID = jobPreview?.JobTypeID,
                    JobLocation = jobPreview?.JobLocation,
                    FrequencyOfficeVisitID = jobPreview?.FrequencyOfficeVisitID,
                    IsOpenToWorkNearByCities = jobPreview?.IsOpenToWorkNearByCities,
                    NearByCities = jobPreview?.NearByCities,
                    ATS_JobLocation = jobPreview?.ATS_JobLocationID,
                    ATS_NearByCities = jobPreview?.ATS_NearByCities,
                    ScreeningQuestionsExternallyModified = jobPreview?.ScreeningQuestionsExternallyModified,
                    TotalScreeningQuestions = jobPreview?.TotalScreeningQuestions,
                    IsTransparentPricing = jobPreview?.IsTransparentPricing

                };

                param = new object[] { loggedInUserCompanyId, contactId, null };
                paramString = CommonLogic.ConvertToParamString(param);

                Sproc_GetStandOutDetails_ClientPortal_Result standOutDetails = new Sproc_GetStandOutDetails_ClientPortal_Result();

                GetCompanyDetails getCompanyDetails = await FetchCompanyDetails(loggedInUserCompanyId);

                dynamic result = new ExpandoObject();
                result.JobPreview = jobPostDetails;
                result.StandOutDetails = standOutDetails;
                result.CompanyDetails = getCompanyDetails;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = result });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data found." });
            }
        }

        [HttpPost("HRUpdatePreviewDetails")]
        public async Task<ObjectResult> HRUpdatePreviewDetails([FromBody] PreviewJobPostUpdate previewJobPostUpdate)
        {
            #region Validation
            if (previewJobPostUpdate == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
            }
            #endregion

            long loggedInUserId = SessionValues.LoginUserId;

            int? budgetType = null;
            long gptId = 0;
            long hrId = 0;
            if (previewJobPostUpdate.NoBudgetBar != null)
            {
                if (previewJobPostUpdate.NoBudgetBar.Value)
                {
                    budgetType = 3;
                    previewJobPostUpdate.BudgetFrom = 0;
                    previewJobPostUpdate.BudgetTo = 0;
                }
                else
                {
                    budgetType = 2;
                }
            }


            object[] param = new object[]
            {
                previewJobPostUpdate.RoleName,
                previewJobPostUpdate.Currency,
                previewJobPostUpdate.BudgetFrom,
                previewJobPostUpdate.BudgetTo,
                previewJobPostUpdate.ContractDuration,
                previewJobPostUpdate.EmploymentType,
                previewJobPostUpdate.Howsoon,
                previewJobPostUpdate.WorkingModeID,
                previewJobPostUpdate.ExperienceYears,
                previewJobPostUpdate.IsFresherAllowed,
                previewJobPostUpdate.Requirements,
                previewJobPostUpdate.RolesResponsibilities,
                previewJobPostUpdate.Skills,
                previewJobPostUpdate.AllSkills,
                previewJobPostUpdate.NoOfTalents,
                previewJobPostUpdate.CompanySize,
                previewJobPostUpdate.CompanyLocation,
                previewJobPostUpdate.IndustryType,
                previewJobPostUpdate.AboutCompanyDesc != null ? null : previewJobPostUpdate.AboutCompanyDesc,
                previewJobPostUpdate.TimezonePreferenceId,
                previewJobPostUpdate.TimeZoneFromTime,
                previewJobPostUpdate.TimeZoneEndTime,
                previewJobPostUpdate.TimeZoneId,
                loggedInUserId,
                Convert.ToString(previewJobPostUpdate.HRID),
                previewJobPostUpdate.IsHiringLimited,
                previewJobPostUpdate.City,
                previewJobPostUpdate.State,
                previewJobPostUpdate.Country,
                previewJobPostUpdate.PostalCode,
                budgetType,
                previewJobPostUpdate.HiringTypePricingId,
                previewJobPostUpdate.PayrollTypeId,
                previewJobPostUpdate.PayrollPartnerName,
                previewJobPostUpdate.JobDescription != null ? null : previewJobPostUpdate.JobDescription,
                previewJobPostUpdate.IsConfidentialBudget,
                gptId,
                previewJobPostUpdate.RoleOverviewDescription,
                previewJobPostUpdate.Whatweoffer,
                previewJobPostUpdate.UTMCountry,
                SessionValues.LoginUserId,

                // vital info
                previewJobPostUpdate.CompensationOption,
                previewJobPostUpdate.HRIndustryType,
                previewJobPostUpdate.HasPeopleManagementExp,
                previewJobPostUpdate.Prerequisites != null ? null : previewJobPostUpdate.Prerequisites,
                previewJobPostUpdate.StringSeparator,
                previewJobPostUpdate.IsMustHaveSkillschanged,
                previewJobPostUpdate.IsGoodToHaveSkillschanged,
                previewJobPostUpdate.ShowHRPOCDetailsToTalents,
                (short)AppActionDoneBy.UTS,
                previewJobPostUpdate?.JobLocation,
                previewJobPostUpdate?.FrequencyOfficeVisitID,
                previewJobPostUpdate?.IsOpenToWorkNearByCities,
                previewJobPostUpdate?.NearByCities,
                previewJobPostUpdate.JobTypeID,
                previewJobPostUpdate.HRID,
                previewJobPostUpdate?.ATS_JobLocation,
                previewJobPostUpdate?.ATS_NearByCities,
            };

            string paramString = CommonLogic.ConvertToParamStringWithNull(param);

            _iCompany.UpdatePreviewDetails(paramString);

            #region Update Job Description With Unicode Characters
            if (!string.IsNullOrEmpty(previewJobPostUpdate?.JobDescription))
                _iCompany.SaveStepInfoWithUnicode(previewJobPostUpdate?.HRID.ToString(), previewJobPostUpdate?.JobDescription);
            #endregion

            #region Update Prerequisites With Unicode Characters
            if (!string.IsNullOrEmpty(previewJobPostUpdate?.Prerequisites))
                _iCompany.SaveperquisitesWithUnicode(previewJobPostUpdate?.HRID.ToString(), previewJobPostUpdate?.Prerequisites);
            #endregion

            #region Update Company Details About desc With Unicode Characters
            if (!string.IsNullOrEmpty(previewJobPostUpdate.AboutCompanyDesc))
            {
                long HRID = previewJobPostUpdate.HRID.Value;
                GenSalesHiringRequest genSalesHiringRequest = _iDBContext.GenSalesHiringRequests.Where(x => x.Id == HRID).FirstOrDefault();
                if (genSalesHiringRequest != null)
                {
                    GenContact genContact = _iDBContext.GenContacts.Where(x => x.Id == genSalesHiringRequest.ContactId).FirstOrDefault();
                    if (genContact != null)
                    {
                        _iCompany.SaveCompanyDescUnicode(genContact.CompanyId ?? 0, previewJobPostUpdate.AboutCompanyDesc, loggedInUserId);
                    }
                }
            }
            #endregion

            // Get the updated budget in response
            var parampreview = new object[] { loggedInUserId, null, previewJobPostUpdate?.HRID ?? 0 };
            var paramPreviewString = CommonLogic.ConvertToParamStringWithNull(parampreview);

            Sp_UTS_PreviewJobPost_ClientPortal_Result jobPreviewUpdated = await _iCompany.Sp_UTS_PreviewJobPost_ClientPortal_Result(paramPreviewString);

            //UTS-6738 : pay per view - live - when user update Job in frontend it is not updated in in JD link  in UTS admin 
            #region Update JD 
            try
            {
                hrId = previewJobPostUpdate?.HRID ?? 0;
                if (jobPreviewUpdated != null)
                {
                    Sp_UTS_PreviewJobPost_ClientPortal_Result jobPreview = jobPreviewUpdated;

                    if (jobPreview != null)
                    {
                        previewJobPostUpdate.RoleName = jobPreview.RoleName;
                        previewJobPostUpdate.Currency = jobPreview.Currency;
                        previewJobPostUpdate.BudgetFrom = jobPreview.BudgetFrom;
                        previewJobPostUpdate.BudgetTo = jobPreview.BudgetTo;
                        previewJobPostUpdate.ContractDuration = jobPreview.ContractDuration;
                        previewJobPostUpdate.EmploymentType = jobPreview.EmploymentType;
                        previewJobPostUpdate.Howsoon = jobPreview.HowSoon;
                        previewJobPostUpdate.WorkingModeID = jobPreview.WorkingModeID;
                        previewJobPostUpdate.ExperienceYears = jobPreview.ExperienceYears;
                        previewJobPostUpdate.IsFresherAllowed = jobPreview.IsFresherAllowed;
                        previewJobPostUpdate.Requirements = jobPreview.Requirements;
                        previewJobPostUpdate.RolesResponsibilities = jobPreview.RolesResponsibilities;
                        previewJobPostUpdate.Skills = jobPreview.Skills;
                        previewJobPostUpdate.AllSkills = jobPreview.AllSkills;
                        previewJobPostUpdate.NoOfTalents = jobPreview.NoOfTalents;
                        previewJobPostUpdate.CompanySize = previewJobPostUpdate.CompanySize;
                        previewJobPostUpdate.CompanyLocation = jobPreview.CompanyLocation;
                        previewJobPostUpdate.IndustryType = previewJobPostUpdate.IndustryType;
                        previewJobPostUpdate.AboutCompanyDesc = previewJobPostUpdate.AboutCompanyDesc;
                        previewJobPostUpdate.TimezonePreferenceId = jobPreview.Timezone_Preference_ID;
                        previewJobPostUpdate.TimeZoneFromTime = jobPreview.TimeZone_FromTime;
                        previewJobPostUpdate.TimeZoneEndTime = jobPreview.TimeZone_EndTime;
                        previewJobPostUpdate.TimeZoneId = jobPreview.TimeZoneId;
                        previewJobPostUpdate.IsHiringLimited = jobPreview.IsHiringLimited;
                        previewJobPostUpdate.City = jobPreview.City;
                        previewJobPostUpdate.State = jobPreview.State;
                        previewJobPostUpdate.Country = jobPreview.Country;
                        previewJobPostUpdate.PostalCode = jobPreview.PostalCode;
                        previewJobPostUpdate.HiringTypePricingId = jobPreview.HiringTypePricingId;
                        previewJobPostUpdate.PayrollTypeId = jobPreview.PayrollTypeId;
                        previewJobPostUpdate.PayrollPartnerName = jobPreview.PayrollPartnerName;
                        previewJobPostUpdate.JobDescription = jobPreview.JobDescription;
                        previewJobPostUpdate.IsConfidentialBudget = jobPreview.IsConfidentialBudget;
                        previewJobPostUpdate.RoleOverviewDescription = jobPreview.JobRoleDescription;
                        previewJobPostUpdate.Whatweoffer = jobPreview.Whatweoffer;
                        // vital info
                        previewJobPostUpdate.CompensationOption = jobPreview?.CompensationOption;
                        previewJobPostUpdate.HRIndustryType = jobPreview?.IndustryType;
                        previewJobPostUpdate.HasPeopleManagementExp = jobPreview?.HasPeopleManagementExp;
                        previewJobPostUpdate.Prerequisites = jobPreview?.Prerequisites;

                        //previewJobPostUpdate.JDFileName = $"HR_{previewJobPostUpdate.GUID}.pdf";


                        DateTime now = DateTime.Now;
                        string formattedDate = now.ToString("dd/MM/yyyy HH:mm:ss");
                        string fileName = $"{previewJobPostUpdate.RoleName}_{formattedDate}.pdf";
                        string pattern = @"[^a-zA-Z0-9._]";
                        // Replace specified special characters with an empty string
                        string cleanName = Regex.Replace(fileName, pattern, "");

                        previewJobPostUpdate.JDFileName = cleanName; // $"{previewJobPostUpdate.RoleName}_{System.DateTime.Now.ToString("dd-MM-yyHH:MM:ss")}.pdf";

                        gptId = await GenerateJD(previewJobPostUpdate, true);


                    }
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region Save & get HR POC users
            List<Sproc_HR_POC_ClientPortal_Result>? hrPocData = null;
            try
            {
                StringBuilder POcDetails = new();
                string pocDetailsString = string.Empty;
                if (previewJobPostUpdate?.HRPOCUserDetails != null && previewJobPostUpdate.HRPOCUserDetails.Any())
                {
                    param = null;
                    foreach (var item in previewJobPostUpdate.HRPOCUserDetails)
                    {
                        //Update contact Number in gen_contact
                        if (!string.IsNullOrEmpty(item.ContactNo))
                        {
                            param = new object[] { item.POCUserID, item.ContactNo };
                            _iUniversalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_HR_EditPOC, param);
                        }

                        POcDetails.Append(item.POCUserID + "&");
                        POcDetails.Append(item.ShowEmailToTalent + "&");
                        POcDetails.Append(item.ShowContactNumberToTalent);
                        POcDetails.Append("^");
                    }
                    pocDetailsString = POcDetails.ToString();
                }

                if (!string.IsNullOrEmpty(pocDetailsString))
                {
                    object[] pocParam = new object[]
                    {
                            0,
                            "",
                            previewJobPostUpdate.HRID,
                            pocDetailsString,
                            0,
                            true
                    };
                    string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                    hrPocData = _iHiringRequest.SaveandGetHRPOCDetails(pocParamString);
                }
            }
            catch
            {

            }
            #endregion           

            #region GEt HR POC details
            if (hrPocData == null)
            {
                try
                {
                    object[] pocParam = new object[] { 0, "", hrId, "", 0, false };
                    string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                    hrPocData = _iHiringRequest.SaveandGetHRPOCDetails(pocParamString);
                }
                catch { }
            }
            #endregion            

            #region Get full preview update details in response
            param = new object[] { 0, null, hrId };
            paramString = CommonLogic.ConvertToParamString(param);
            jobPreviewUpdated = await _iCompany.Sp_UTS_PreviewJobPost_ClientPortal_Result(paramString);
            JobPostDetailsViewModel jobPostDetails = new JobPostDetailsViewModel
            {
                // step-1               

                RoleName = jobPreviewUpdated?.RoleName ?? "",
                ExperienceYears = jobPreviewUpdated?.ExperienceYears,
                IsFresherAllowed = jobPreviewUpdated?.IsFresherAllowed,
                NoOfTalents = jobPreviewUpdated?.NoOfTalents,
                IsHiringLimited = jobPreviewUpdated?.IsHiringLimited ?? "",
                ContractDuration = jobPreviewUpdated?.ContractDuration,
                CurrentStepId = jobPreviewUpdated?.CurrentStepId,
                NextStepId = jobPreviewUpdated?.NextStepId,
                Availability = jobPreviewUpdated?.Availability,

                // step-2
                Skills = jobPreviewUpdated?.Skills ?? "",
                AllSkills = jobPreviewUpdated?.AllSkills ?? "",
                BudgetFrom = jobPreviewUpdated?.BudgetFrom,
                BudgetTo = jobPreviewUpdated?.BudgetTo,
                IsConfidentialBudget = jobPreviewUpdated?.IsConfidentialBudget ?? false,
                //salary = data.Max_Salary,
                Currency = jobPreviewUpdated?.Currency ?? "",
                CurrencySign = jobPreviewUpdated?.CurrencySign ?? "",
                BudgetFromStr = jobPreviewUpdated?.BudgetFromStr ?? "",
                BudgetToStr = jobPreviewUpdated?.BudgetToStr ?? "",
                HRCost = jobPreviewUpdated?.HRCost ?? "",
                //RoleName = data.Working_Zone_With_Time_Zone,

                // step-3
                EmploymentType = jobPreviewUpdated?.EmploymentType ?? "",
                HowSoon = jobPreviewUpdated?.HowSoon ?? "",
                WorkingModeId = jobPreviewUpdated?.WorkingModeID,
                CompanyLocation = string.IsNullOrEmpty(jobPreviewUpdated?.CompanyLocation) ? "Remote" : jobPreviewUpdated?.CompanyLocation,
                TimezonePreferenceId = jobPreviewUpdated?.Timezone_Preference_ID,
                TimeZoneFromTime = jobPreviewUpdated?.TimeZone_FromTime,
                TimeZoneEndTime = jobPreviewUpdated?.TimeZone_EndTime,
                AchievementId = jobPreviewUpdated?.AchievementID,
                Reason = jobPreviewUpdated?.Reason,
                TimeZone = jobPreviewUpdated?.TimeZone,

                // step-4
                Requirements = jobPreviewUpdated?.Requirements,
                RolesResponsibilities = jobPreviewUpdated?.RolesResponsibilities,
                JDFileName = jobPreviewUpdated?.JDFileName ?? "",
                GPTJDID = jobPreviewUpdated?.GPTJDID ?? 0,
                JobDescription = jobPreviewUpdated?.JobDescription,
                RoleOverviewDescription = jobPreviewUpdated?.JobRoleDescription,
                Whatweoffer = jobPreviewUpdated?.Whatweoffer,

                //Extra info
                ContactId = jobPreviewUpdated?.ContactId,
                ProcessType = jobPreviewUpdated?.ProcessType,
                IST_TimeZone_FromTime = jobPreviewUpdated?.IST_TimeZone_FromTime,
                IST_TimeZone_EndTime = jobPreviewUpdated?.IST_TimeZone_EndTime,

                //location info
                City = jobPreviewUpdated?.City,
                State = jobPreviewUpdated?.State,
                Country = jobPreviewUpdated?.Country,
                PostalCode = jobPreviewUpdated?.PostalCode,
                CountryID = jobPreviewUpdated?.CountryID,

                //New budget info
                BudgetType = jobPreviewUpdated?.BudgetType,

                //Transparent Job details
                HiringTypePricingId = jobPreviewUpdated?.HiringTypePricingId,
                PayrollTypeId = jobPreviewUpdated?.PayrollTypeId,
                PayrollPartnerName = jobPreviewUpdated?.PayrollPartnerName,
                HiringTypePricing = jobPreviewUpdated?.HiringTypePricing,
                PayrollType = jobPreviewUpdated?.PayrollType,

                // vital info
                CompensationOption = jobPreviewUpdated?.CompensationOption,
                IndustryType = jobPreviewUpdated?.IndustryType,
                HasPeopleManagementExp = jobPreviewUpdated?.HasPeopleManagementExp,
                Prerequisites = jobPreviewUpdated?.Prerequisites,
                StringSeparator = jobPreviewUpdated?.StringSeparator,
                ToolTipMessage = jobPreviewUpdated?.ToolTipMessage,
                HRTypeId = jobPreviewUpdated?.HRTypeId,

                HRPOCUserID = hrPocData,
                ShowHRPOCDetailsToTalents = jobPreviewUpdated?.ShowHRPOCDetailsToTalents,
                JobTypeID = jobPreviewUpdated?.JobTypeID,
                ScreeningQuestionsExternallyModified = jobPreviewUpdated?.ScreeningQuestionsExternallyModified,
                TotalScreeningQuestions = jobPreviewUpdated?.TotalScreeningQuestions,
                IsTransparentPricing = jobPreviewUpdated?.IsTransparentPricing
            };
            #endregion

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Details updated successfully.", Details = jobPostDetails });
        }

        [HttpGet("SendATSUpdateOnEditHR")]
        public async Task<ObjectResult> SendATSUpdateOnEditHR(long hrId, bool? isAutogenerateQuestions)
        {
            #region ATS call           
            if (hrId != 0)
            {
                try
                {
                    if (!_iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {
                        var HRData_Json = _iHiringRequest.GetAllHRDataForAdmin(hrId, isAutogenerateQuestions);
                        string? HRJsonData = Convert.ToString(HRData_Json);
                        if (!string.IsNullOrEmpty(HRJsonData))
                        {
                            bool isAPIResponseSuccess = true;

                            ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                            if (HRJsonData != "")
                                aTSCall.SendHRDataToPMS(HRJsonData, hrId);
                        }
                    }
                }
                catch
                {

                }
            }
            #endregion

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Details updated successfully." });
        }

        [HttpGet("GetHiringTypePricingDetails")]
        public ObjectResult GetHiringTypePricingDetails([FromQuery] long hrId)
        {
            try
            {
                #region Pre-Validation
                if (hrId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass hrId" });
                }
                #endregion

                object[] param = new object[]
                {
                    0,
                    0,
                    hrId
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                var hiringTypePricingList = _iCompany.GetHiringTypePricingDetails(paramString);

                if (hiringTypePricingList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HiringType Pricing List", Details = hiringTypePricingList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<long> GenerateJD(PreviewJobPostUpdate previewJobPostUpdate, bool isFromEdit = false)
        {
            long loggedInUserId = SessionValues.LoginUserId;
            long gptId = 0;
            //string? fileName = previewJobPostUpdate.JDFileName;
            string content, chatGPTPrompt = string.Empty;
            string pythonURL = _iConfiguration["PythonScrapeCreateJD"] + "?ContactID=" + loggedInUserId;

            string JDcreateText = "";

            // If from Edit mode then check if HR already exists or not.
            bool generateJd = true;

            if (generateJd)
            {


                Parser parser = new Parser(_iConfiguration, _iDBContext);

                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("dd/MM/yyyy HH:mm:ss");
                string fileName = $"{previewJobPostUpdate.RoleName}_{formattedDate}.pdf";
                string pattern = @"[^a-zA-Z0-9._]";
                // Replace specified special characters with an empty string
                string cleanName = Regex.Replace(fileName, pattern, "");

                content = GetJDPdfContent(previewJobPostUpdate);
                string pdfPath = System.IO.Path.Combine(_iConfiguration["AdminAPIProjectURL"], "Media\\JDParsing\\JDFiles", cleanName);
                parser.GenerateJDPdf(pdfPath, content);
                previewJobPostUpdate.JDFileName = cleanName;


                if (!string.IsNullOrEmpty(JDcreateText))
                {
                    GenGptJdresponse gptJdresponse = new()
                    {
                        Url = "",
                        Jdtext = JDcreateText,
                        ResponseData = content,
                        CreatedById = loggedInUserId,
                        CreatedDateTime = System.DateTime.Now,
                        Gptprompt = JDcreateText
                    };

                    _iCompany.SaveGptJdresponse(gptJdresponse).ConfigureAwait(false);

                    gptId = gptJdresponse.Id;
                }
            }


            return gptId;
        }

        private string GetJDPdfContent(PreviewJobPostUpdate previewJobPostUpdate)
        {

            string workingMode = string.Empty;


            System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

            sbBody.Append("<!DOCTYPE html>");
            sbBody.Append("<html>");
            sbBody.Append("<body style=\"font-size: 12px;\">");
            sbBody.Append($"<strong>Job Title: </strong> {previewJobPostUpdate.RoleName}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Years of Experience: </strong> {previewJobPostUpdate.ExperienceYears}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Employment Type: </strong> {previewJobPostUpdate.EmploymentType}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Working mode: </strong> {previewJobPostUpdate.ModeOfWork}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Currency: </strong> {previewJobPostUpdate.Currency}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Must have skills: </strong> {previewJobPostUpdate.Skills}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Good to have skills: </strong> {previewJobPostUpdate.AllSkills}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");
            sbBody.Append($"<strong>Job Description </strong>");
            sbBody.Append($"{previewJobPostUpdate.JobDescription}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");

            sbBody.Append("</body>");
            sbBody.Append("</html>");

            return sbBody.ToString();
        }

        #endregion

        #region Fetch Country based on City
        [HttpPost("FetchCountriesBasedonCity")]
        public ObjectResult FetchCountriesBasedonCity([FromBody] GetCitybyCountryViewModel cityName)
        {
            if (cityName == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request Object is empty" });
            }
            long LogedInUserIdval = SessionValues.LoginUserId;
            string pythonParsingURLForCountry = _iConfiguration["PythonParsingURLForCountry"] + "?Name=" + cityName?.City;

            string content = string.Empty;

            using (WebResponse wr = WebRequest.Create(pythonParsingURLForCountry).GetResponse())
            {
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                    content = content.Trim();
                }
            }

            List<PrgCountryRegion> regions = new List<PrgCountryRegion>();
            string message = "List of countries";
            if (!string.IsNullOrEmpty(content))
            {
                string[] countries = content.Split(',');
                for (int i = 0; i < countries.Length; i++)
                {
                    PrgCountryRegion? region = _iDBContext.PrgCountryRegions.Where(x => x.Country.ToLower() == countries[i].ToLower()).FirstOrDefault();
                    if (region != null)
                    {
                        regions.Add(region);
                    }
                }
            }

            if (!regions.Any())
            {
                message = "city not registered with us please select the country manually";
                regions = _iDBContext.PrgCountryRegions.ToList();
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = message, Details = regions });
        }
        #endregion

        #region CreateWhatsappGroup

        [HttpPost("CreateWhatsappGroup")]
        public async Task<ObjectResult> CreateWhatsappGroup([FromBody] CompanyWhatsappDetails whatsappDetails)
        {
            try
            {
                dynamic responseObj = new ExpandoObject();
                string errorMessage = "";
                string? POCName = "";
                bool isAdmin = false;

                //SessionValues.LoginUserId = 190;
                #region Validation
                if (whatsappDetails == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                #region Get Default Users
                string defaultNumbers = Convert.ToString(_iConfiguration["DefaultWhatsappNumbers"]);
                #endregion

                #region GetPOChierarchy
                string param = Convert.ToString(whatsappDetails.POCUserID) ?? "";
                List<Sproc_Get_Hierarchy_For_Email_Result> pocDetails = await _iClient.GetHierarchyForEmail(param);
                #endregion

                if (pocDetails != null && pocDetails.Count > 0)
                {
                    WhatsappViewModel whatsapp = new WhatsappViewModel()
                    {
                        name = whatsappDetails.CompanyName,
                        description = "Group created by Uplers POC for better communication"
                    };

                    whatsapp.permissions = new Permissions()
                    {
                        approval = false,
                        edit = "admins",
                        send = "all",
                        invite = "admins"
                    };

                    List<Participant> participants = new List<Participant>();

                    foreach (var i in pocDetails)
                    {
                        if (!string.IsNullOrEmpty(i.ContactNumber))
                        {
                            if(i.UserId == whatsappDetails.POCUserID)
                            {
                                isAdmin = true;
                                POCName = i.UserName;
                            }
                            participants.Add(new Participant() { phone = i.ContactNumber, admin = isAdmin });
                        }
                    }

                    if (!string.IsNullOrEmpty(defaultNumbers))
                    {
                        string[] defaultNumbersarray = defaultNumbers.Split(",");
                        foreach (string number in defaultNumbersarray)
                        {
                            participants.Add(new Participant() { phone = number, admin = false });
                        }
                    }

                    whatsapp.participants = participants;

                    if (participants.Count > 0)
                    {
                        string json = JsonConvert.SerializeObject(whatsapp);
                        string url = Convert.ToString(_iConfiguration["WhatsppAPIURL"]);
                        string deviceId = Convert.ToString(_iConfiguration["WhatsappDeviceID"]);
                        var client = new RestClient(url); // Replace with your API endpoint
                        var request = new RestRequest($"/devices/{deviceId}/groups", Method.Post); // Replace with your specific endpoint and HTTP method

                        request.AddJsonBody(json);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Token", Convert.ToString(_iConfiguration["WhatsppAPIToken"]));

                        // Execute the request
                        var response = client.Execute(request);
                        if (response.IsSuccessStatusCode && response.Content != null)
                        {
                            //response.Content = "{\"wid\":\"120363332020293955@g.us\",\"kind\":\"group\",\"name\":\"Riya Uplers 3\",\"description\":\"This is a group sample description\",\"lastSyncAt\":\"2024-09-06T11:57:07.630Z\",\"isPinned\":false,\"isArchive\":false,\"isReadOnly\":false,\"unreadCount\":0,\"createdAt\":\"2024-09-06T11:57:07.630Z\",\"lastMessageAt\":\"2024-09-06T11:56:57.000Z\",\"isCommunityAnnounce\":false,\"muteExpiration\":null,\"totalParticipants\":2}";
                            dynamic? whatsappResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                            if (whatsappResponse != null)
                            {
                                #region SendDefaultMessage
                                string wid = Convert.ToString(whatsappResponse.wid);                               
                                string defaultMsg = $"Welcome to our Uplers' hiring platform group!\n\nThis group has been set up to make it easier and faster for you to get updates, alerts, and support while using our platform to find the most relevant candidates for your job openings. You’ll receive:\n\nReal-time notifications when new, highly relevant profiles are available for review.Summaries highlighting key strengths of the candidates for quick assessment.A direct line of communication with {POCName}, our platform admin, who is here to help you navigate the platform and make sure you get the best possible results.\n\nFeel free to use this group to share any feedback, ask questions, or request support as needed. Your input will help us continue to improve the platform to better serve your recruitment needs.\n\nLooking forward to helping you source the best talent faster!";

                                WhatsappViewModelForMessage message = new WhatsappViewModelForMessage()
                                {
                                    group = wid,
                                    message = defaultMsg
                                };

                                SendMessageToWhatsappGroup(message);
                                #endregion

                                object[] whatsapaParam = new object[]
                                {
                                    whatsappDetails.CompanyID ?? 0,
                                    wid,
                                    Convert.ToString(whatsappResponse.name),
                                    SessionValues.LoginUserId
                                };

                                string paramString = CommonLogic.ConvertToParamStringWithNull(whatsapaParam);
                                Sproc_UTS_SaveCompanyWhatsappDetails_Result whatsappDetail = _iCompany.Sproc_UTS_SaveCompanyWhatsappDetails(paramString);

                                if (whatsappDetail != null && whatsappDetail.WhatsappDetailID > 0)
                                {
                                    for (int i = 0; i < participants.Count; i++)
                                    {
                                        var particpantObj = pocDetails.Where(x => x.ContactNumber == participants[i].phone).ToList().FirstOrDefault();
                                        if (particpantObj != null)
                                        {
                                            whatsapaParam = new object[]
                                            {
                                                whatsappDetail.WhatsappDetailID,
                                                particpantObj?.UserId,
                                                participants[i].admin,
                                                SessionValues.LoginUserId
                                            };

                                            paramString = CommonLogic.ConvertToParamStringWithNull(whatsapaParam);
                                            _iCompany.Sproc_UTS_SaveCompanyWhatsappMemberDetails(paramString);
                                        }
                                    }
                                }
                            }

                            var whatsappDetails1 = _iCompany.Sproc_UTS_GetCompanyWhatsappDetails(whatsappDetails.CompanyID.Value);
                            responseObj.WhatsappDetails = whatsappDetails1;
                            if (whatsappDetails1 != null && whatsappDetails1.Count > 0)
                            {
                                responseObj.ShowWhatsappCTA = false;
                            }
                            else
                            {
                                responseObj.ShowWhatsappCTA = true;
                            }
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success", Details = responseObj });
                        }
                        else
                        {
                            dynamic? errorResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                            if (errorResponse != null)
                            {
                                errorMessage = Convert.ToString(errorResponse.message);
                            }
                        }

                    }
                    else
                    {
                        errorMessage = "No participants found to add in group";
                    }
                }
                else
                {
                    errorMessage = "No POC found";
                }

                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = errorMessage });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SendMessageToWhatsappGroup(WhatsappViewModelForMessage message)
        {
            string json = JsonConvert.SerializeObject(message);
            string url = Convert.ToString(_iConfiguration["WhatsppAPIURL"]);
            var client = new RestClient(url); // Replace with your API endpoint
            var request = new RestRequest("/messages", Method.Post); // Replace with your specific endpoint and HTTP method

            request.AddJsonBody(json);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Token", Convert.ToString(_iConfiguration["WhatsppAPIToken"]));

            // Execute the request
            var response = client.Execute(request);            
        }

        #endregion

        #region Cron Job For ATS to Send Company Data
        [HttpPost("SendCompanyDataToATSCronJob")]
        public async Task<ObjectResult> SendCompanyDataToATSCronJob()
        {
            #region ATSCalls
            if (!_iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                AddUpdateCompanyViewModel addUpdateCompanyViewModel = null;
                long CompanyID = 0;
                object[] param = new object[]
                   {
                       "Get",
                        0,
                        0,
                        0,
                        false
                   };
                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                var CompanyList = _iCompany.Sproc_Get_Add_CompanyData_Send_Details_To_ATS(paramString);
                if (CompanyList != null)
                {
                    foreach (var company in CompanyList)
                    {
                        CompanyID = company.CompanyID ?? 0;
                        if (CompanyID > 0)
                        {
                            addUpdateCompanyViewModel = await MakeCompanyModelForATS((long)CompanyID);
                        }

                        if (addUpdateCompanyViewModel != null)
                        {
                            try
                            {
                                var json = JsonConvert.SerializeObject(addUpdateCompanyViewModel);
                                ATSCall aTSCall = new ATSCall(_iConfiguration, _iDBContext);
                                bool IsSend = aTSCall.SendAddEditCompanyData(json, LoggedInUserId);

                                if (IsSend)
                                {
                                    object[] param1 = new object[]
                                   {
                                       "Update",
                                        CompanyID,
                                        LoggedInUserId,
                                        1,
                                        true
                                   };
                                    string paramString1 = CommonLogic.ConvertToParamStringWithNull(param1);
                                    var CompanyList1 = _iCompany.Sproc_Get_Add_CompanyData_Send_Details_To_ATS(paramString1);
                                }
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error", Details = null });
                            }
                        }

                    }
                }


            }
            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully send Company profile details", Details = null });
            #endregion
        }
        #endregion
    }
}
