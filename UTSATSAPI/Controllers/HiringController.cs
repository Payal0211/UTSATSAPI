namespace UTSATSAPI.Controllers
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.Results;
    using Google.Apis.Auth.AspNetCore3;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Download;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.ChatGPTCalls;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
    using UTSATSAPI.Models.ViewModels.Validators;
    using UTSATSAPI.Repositories.Interfaces;
    using UTSATSAPI.Repositories.Interfaces.UpChat;
    using static UTSATSAPI.Helpers.Constants;
    using static UTSATSAPI.Helpers.Enum;

    [ApiController]
    [Authorize]
    [Route("Hiring/")]
    public class HiringController : ControllerBase
    {
        #region Variables

        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IUniversalProcRunner _universalProcRunner;
        private IWebHostEnvironment webHostEnvironment;
        private readonly ICommonInterface _commonInterface;
        private IConfiguration _configuration;
        private IMapper _mapper;
        private IChannel _iChannel;
        private readonly IEmail _iUpChatEmail;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJDParse _iJDParse;
        private readonly IClient _iClient;
        private readonly IMasters _iMasters;
        private readonly IUpChatCall _iUpChatCall;
        private readonly IHiringRequest _iHiringRequest;
        public int count = 0;
        #endregion

        #region Constructors

        public HiringController(TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IWebHostEnvironment _webHostEnvironment, ICommonInterface commonInterface, IConfiguration configuration, IMapper mapper, IChannel channel, IEmail upChatEmail, IUpChatCall iUpChatCall, IHttpContextAccessor httpContextAccessor, IJDParse iJDParse, IClient iClient, IMasters iMasters, IHiringRequest iHiringRequest)
        {
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            webHostEnvironment = _webHostEnvironment;
            _commonInterface = commonInterface;
            _configuration = configuration;
            _mapper = mapper;
            _iChannel = channel;
            _iUpChatEmail = upChatEmail;
            _iUpChatCall = iUpChatCall;
            _httpContextAccessor = httpContextAccessor;
            _iJDParse = iJDParse;
            _iClient = iClient;
            _iMasters = iMasters;
            _iHiringRequest = iHiringRequest;
        }

        #endregion

        #region Public Methods
        [HttpGet("AutoComplete/Contact")]
        public ObjectResult GetAutoCompleteContacts([FromQuery] string search, long CompanyID = 0)
        {
            try
            {
                #region PreValidation

                if (!string.IsNullOrEmpty(search) || !string.IsNullOrWhiteSpace(search))
                {
                    bool breakValidation = false;

                    breakValidation = search.StartsWith('.') || search.EndsWith(".") ||
                        search.StartsWith('_') || search.EndsWith("_") ||
                        search.StartsWith('@') || search.EndsWith("@") ||
                        !Regex.IsMatch(search.ToString(), @"^[a-z]|[A-Z]|[0-9]|@|.|_|$");

                    if (breakValidation)
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper text for search" });
                }

                #endregion

                long LoggedInUserTypeId = SessionValues.LoginUserTypeId;

                object[] param = new object[]
                {
                      search,
                      LoggedInUserTypeId,
                      CompanyID
                 };
                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_UTS_GetAutoCompleteContacts_Result> searchData = _commonInterface.hiringRequest.sproc_UTS_GetAutoCompleteContacts_Result(paramasString);

                if (searchData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = searchData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No contact Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("AutoComplete/Company")]
        public ObjectResult GetAutoCompleteContacts([FromQuery] string search)
        {
            try
            {
                #region PreValidation

                if (!string.IsNullOrEmpty(search) || !string.IsNullOrWhiteSpace(search))
                {
                    bool breakValidation = false;

                    breakValidation = search.StartsWith('.') || search.EndsWith(".") ||
                        search.StartsWith('_') || search.EndsWith("_") ||
                        search.StartsWith('@') || search.EndsWith("@") ||
                        !Regex.IsMatch(search.ToString(), @"^[a-z]|[A-Z]|[0-9]|@|.|_|$");

                    if (breakValidation)
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide proper text for search" });
                }

                #endregion

                object[] param = new object[] { search };
                string paramasString = CommonLogic.ConvertToParamString(param);
                List<sproc_UTS_GetAutoCompleteCompany_Result> searchData = _commonInterface.hiringRequest.sproc_UTS_GetAutoCompleteCompany_Result(paramasString);

                if (searchData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = searchData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No company Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Create")]
        public async Task<ObjectResult> CreateHiringRequest(HiringReqeustModel hrModel, long LoggedInUserId)
        {
            try
            {
                string Message = string.Empty;

                #region Pre Validation                

                LoggedInUserId = SessionValues.LoginUserId;
                int LoginUserTypeID = SessionValues.LoginUserTypeId;

                bool IsDirectHR = false;
                bool IsBDR_MDRUser = false;

                if (hrModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                bool IsPayPerHire = false;
                bool IsPayPerCredit = false;
                if (hrModel.PayPerType == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Pay per type is required" });
                }
                else
                {
                    if (hrModel.PayPerType == 1)
                        IsPayPerHire = true;
                    else if (hrModel.PayPerType == 2)
                        IsPayPerCredit = true;
                    else
                        IsPayPerHire = true;
                }

                IsDirectHR = hrModel.isDirectHR ?? false;
                IsBDR_MDRUser = LoginUserTypeID == 11 || LoginUserTypeID == 12 ? true : false;

                if (!hrModel.issaveasdraft)
                {
                    if (hrModel.directPlacement == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                    }
                    if (IsPayPerHire) //Pay per hire only we get this details
                    {
                        if (hrModel.isHRTypeDP)
                        {
                            if (string.IsNullOrEmpty(hrModel.directPlacement.DpPercentage.ToString()))
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Uplers fees is mandatory when HR type is Direct Placement" });
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(hrModel.NRMargin.ToString()))
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Uplers fees is mandatory when HR type is Contruatual" });
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(hrModel.directPlacement.ModeOfWork))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Mode of Work is mandatory" });
                    }
                    if (hrModel.directPlacement.ModeOfWork == "Hybrid" || hrModel.directPlacement.ModeOfWork == "Office")
                    {
                        DirectPlacementValidator validationRules = new DirectPlacementValidator();
                        ValidationResult validationResult = validationRules.Validate(hrModel.directPlacement);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseObject()
                                {
                                    statusCode = StatusCodes.Status400BadRequest,
                                    Message = "Validation Error",
                                    Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestModel")
                                });
                        }
                    }
                }
                #endregion

                #region Validation

                if (!hrModel.issaveasdraft)
                {
                    if (IsDirectHR && IsBDR_MDRUser || IsPayPerCredit)
                    {
                        DirectHR_or_CreditHR_ModelValidator validationRules = new DirectHR_or_CreditHR_ModelValidator();
                        ValidationResult validationResult = validationRules.Validate(hrModel);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestModel") });
                        }
                    }
                    else
                    {
                        HiringReqeustModelValidator validationRules = new HiringReqeustModelValidator();
                        ValidationResult validationResult = validationRules.Validate(hrModel);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestModel") });
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(hrModel.clientName))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill Client Name" });
                    }

                    //UTS-3885: Sales userID should be a required field while adding Draft HR.    
                    if (Convert.ToInt32(hrModel.salesPerson) == 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select Sales Person" });
                    }
                }

                #endregion

                #region  Validation based on Add Hr / Edit HR
                if (!string.IsNullOrEmpty(hrModel.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(hrModel.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(hrModel.en_Id)) > 0)
                {
                    Message = "Hiring reqest updated successfully";
                    hrModel.Id = Convert.ToInt64(CommonLogic.Decrypt(Convert.ToString(hrModel.en_Id)));

                    #region commented unneccsary code
                    //GenSalesHiringRequest hiringrequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hrModel.Id);

                    //#region conditional validation
                    //if (hiringrequest != null)
                    //{
                    //    if ((IsDirectHR && IsBDR_MDRUser) || IsPayPerCredit || !string.IsNullOrEmpty(hiringrequest.Guid))
                    //    {
                    //        if (IsPayPerCredit && hrModel.IsHiringLimited == null)
                    //        {
                    //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select Hiring limited" });
                    //        }
                    //    }
                    //}
                    //#endregion

                    //#region UpdateTalentContractualDPConversion
                    //bool isHrTypeDP = false;

                    //if (hiringrequest != null)
                    //{
                    //    isHrTypeDP = hiringrequest.IsHrtypeDp; //store the previous HR type before editing.
                    //}

                    //if (Convert.ToBoolean(hrModel.AllowSpecialEdit))
                    //{
                    //    //check if the type of HR is changed or not.
                    //    bool isHrTypeChanged = isHrTypeDP != hrModel.isHRTypeDP;

                    //    //Save the HR type conversion.
                    //    if (isHrTypeChanged)
                    //    {
                    //        ConvertTalentsToContractualORDP(hrModel, hiringrequest.Id);
                    //    }
                    //}
                    //#endregion
                    #endregion
                }
                else
                {
                    Message = "Hiring request created successfully";
                    hrModel.Id = 0;

                    #region conditional validation
                    if ((IsDirectHR && IsBDR_MDRUser || IsPayPerCredit))
                    {
                        //TODO
                        if (IsPayPerCredit)//Required condition only in Create HR 
                        {
                            hrModel.IsPostaJob = hrModel.IsPostaJob ?? false;
                            hrModel.IsProfileView = hrModel.IsProfileView ?? false;
                            if (!(bool)hrModel.IsPostaJob && !(bool)hrModel.IsProfileView)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please check any one of them, IsPostaJob or IsProfileView" });
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                #region SP call : Sproc_UTS_AddEdit_HR

                int contractDuration = 0;
                if (hrModel.contractDuration == "-1")
                    contractDuration = -1;
                else
                    contractDuration = string.IsNullOrEmpty(hrModel.contractDuration) ? 0 : Convert.ToInt32(hrModel.contractDuration); // Represents contract duration months.

                object[] AddEditHR_Param = new object[]
                {
                    hrModel?.Id,
                    hrModel?.contactId,
                    hrModel?.salesPerson,
                    hrModel?.availability,
                    contractDuration,
                    hrModel?.Currency,
                    hrModel?.AdhocBudgetCost,
                    hrModel?.minimumBudget,
                    hrModel?.maximumBudget,
                    hrModel?.IsConfidentialBudget,
                    hrModel?.directPlacement?.ModeOfWork,
                    hrModel?.directPlacement?.City,
                    hrModel?.directPlacement?.Country,
                    hrModel?.jDFilename,
                    hrModel?.jdURL,
                    hrModel?.JDDump_ID,
                    hrModel?.years,
                    hrModel?.talentsNumber,
                    hrModel?.timeZone,
                    hrModel?.TimeZoneFromTime,
                    hrModel?.TimeZoneEndTime,
                    hrModel?.OverlapingHours,
                    hrModel?.howsoon,
                    hrModel?.PartialEngagementTypeID,
                    hrModel?.NoofHoursworking,
                    hrModel?.DurationType,
                    hrModel?.isHRTypeDP,
                    hrModel?.directPlacement?.DpPercentage,
                    hrModel?.NRMargin,
                    hrModel?.IsTransparentPricing,
                    hrModel?.HrTypePricingId,
                    hrModel?.PayrollTypeId,
                    hrModel?.PayrollPartnerName,
                    hrModel?.IsVettedProfile,
                    hrModel?.IsHiringLimited,
                    IsPayPerHire,
                    IsPayPerCredit,
                    hrModel?.IsPostaJob,
                    hrModel?.IsProfileView,
                    hrModel?.isDirectHR,
                    hrModel?.issaveasdraft,
                    hrModel?.bqFormLink,
                    hrModel?.discoveryCallLink,
                    hrModel?.dealID,
                    hrModel?.AllowSpecialEdit,
                    hrModel?.ChildCompanyName,
                    hrModel?.IsFresherAllowed,
                    LoggedInUserId,
                    hrModel?.CompensationOption,
                    hrModel?.HRIndustryType,
                    hrModel?.HasPeopleManagementExp,
                    hrModel?.Prerequisites,
                    hrModel?.StringSeparator,
                    hrModel?.ShowHRPOCDetailsToTalents,
                    hrModel?.JobLocation,
                    hrModel?.FrequencyOfficeVisitID,
                    hrModel?.IsOpenToWorkNearByCities,
                    hrModel?.NearByCities,
                    hrModel?.JobTypeID,
                    hrModel?.ATS_JobLocationID,
                    hrModel?.ATS_NearByCities,
                    (short)AppActionDoneBy.UTS
                };

                string ParamString = CommonLogic.ConvertToParamStringWithNull(AddEditHR_Param);
                Sproc_UTS_AddEdit_HR_Result result = _iHiringRequest.AddEdit_HR(ParamString);
                if (result != null && result.Id > 0)
                {
                    hrModel.en_Id = CommonLogic.Encrypt(result.Id.ToString());
                    hrModel.Id = (long)result.Id;

                    #region Save Jd Description in case of Copy Paste in Save as Draft and Create HR
                    if (hrModel?.ParsingType == "CopyPaste")
                    {
                        GenSalesHiringRequestDetail genSalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == hrModel.Id).FirstOrDefault();
                        if (genSalesHiringRequestDetail != null)
                        {
                            genSalesHiringRequestDetail.JobDescription = hrModel?.jDDescription;
                            _talentConnectAdminDBContext.Update(genSalesHiringRequestDetail);
                            _talentConnectAdminDBContext.SaveChanges();
                        }
                    }
                    #endregion
                }
                #endregion

                #region Save HR POC users
                try
                {
                    StringBuilder POcDetails = new();
                    string pocDetailsString = string.Empty;
                    if (hrModel?.HRPOCUserDetails != null && hrModel.HRPOCUserDetails.Any())
                    {
                        object[] param = null;
                        foreach (var item in hrModel.HRPOCUserDetails)
                        {
                            //Update contact Number in gen_contact
                            if (!string.IsNullOrEmpty(item.ContactNo))
                            {
                                param = new object[] { item.POCUserID, item.ContactNo };
                                _universalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_HR_EditPOC, param);
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
                            hrModel.Id,
                            //string.Join(",", hrModel?.HRPOCUserID),
                            pocDetailsString,
                            0,
                            true
                        };
                        string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                        _iHiringRequest.SaveandGetHRPOCDetails(pocParamString);
                    }
                }
                catch
                {

                }
                #endregion

                #region get Basic details for 2nd tab

                GenCompany _Company = null;
                GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == hrModel.contactId).FirstOrDefault();
                if (_Contact != null && _Contact.CompanyId > 0)
                {
                    _Company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == _Contact.CompanyId).FirstOrDefault();
                }

                #region CompanyInfo
                hrModel.companyInfo = null;
                if (_Company != null)
                {
                    hrModel.companyInfo = new CompanyInfo();
                    hrModel.companyInfo.companyID = _Company.Id;
                    hrModel.companyInfo.companyName = _Company.Company ?? "";
                    hrModel.companyInfo.industry = _Company.Industry ?? _Company.IndustryType ?? "";
                    hrModel.companyInfo.website = _Company.Website ?? "";
                    hrModel.companyInfo.linkedInURL = _Company.LinkedInProfile ?? "";
                    hrModel.companyInfo.companySize = _Company.CompanySize ?? 0;
                    hrModel.companyInfo.aboutCompanyDesc = _Company.AboutCompanyDesc ?? "";
                }
                #endregion

                #region commned code https://uplerstalentsolution.atlassian.net/browse/UTS-7594
                //#region Get InterviewerDetails(Primary)

                //hrModel.interviewerDetails = new();
                //hrModel.interviewerDetails.primaryInterviewer = new();
                //hrModel.interviewerDetails.secondaryinterviewerList = null;

                //if (_Contact != null)// Get default primary interviewer details from gen_contact
                //{
                //    hrModel.interviewerDetails.primaryInterviewer.interviewerId = 0;
                //    hrModel.interviewerDetails.primaryInterviewer.fullName = _Contact.FullName;
                //    hrModel.interviewerDetails.primaryInterviewer.emailID = _Contact.EmailId;
                //    hrModel.interviewerDetails.primaryInterviewer.linkedin = _Contact.LinkedIn;
                //    hrModel.interviewerDetails.primaryInterviewer.designation = _Contact.Designation;
                //    hrModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                //}
                //#endregion

                //var SalesHiringRequestInterviewerList =
                //    _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                //    .Where(z => z.HiringRequestId == hrModel.Id).ToList();

                //if (SalesHiringRequestInterviewerList.Any())
                //{
                //    int count = 1;
                //    foreach (var HiringRequestInterviwerDetailList in SalesHiringRequestInterviewerList)
                //    {
                //        if (count == 1)
                //        {
                //            hrModel.interviewerDetails.primaryInterviewer.interviewerId = HiringRequestInterviwerDetailList.Id;
                //            hrModel.interviewerDetails.primaryInterviewer.fullName = HiringRequestInterviwerDetailList.InterviewerName;
                //            hrModel.interviewerDetails.primaryInterviewer.emailID = HiringRequestInterviwerDetailList.InterviewerEmailId;
                //            hrModel.interviewerDetails.primaryInterviewer.linkedin = HiringRequestInterviwerDetailList.InterviewLinkedin;
                //            hrModel.interviewerDetails.primaryInterviewer.designation = HiringRequestInterviwerDetailList.InterviewerDesignation;
                //            hrModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                //        }
                //        else
                //        {
                //            hrModel.interviewerDetails.secondaryinterviewerList
                //            .Add(new InterviewerClass
                //            {
                //                interviewerId = HiringRequestInterviwerDetailList.Id,
                //                fullName = HiringRequestInterviwerDetailList.InterviewerName,
                //                linkedin = HiringRequestInterviwerDetailList.InterviewLinkedin,
                //                emailID = HiringRequestInterviwerDetailList.InterviewerEmailId,
                //                designation = HiringRequestInterviwerDetailList.InterviewerDesignation,
                //                isUserAddMore = true
                //            });
                //        }
                //        count++;
                //    }
                //}
                //else
                //{
                //    if (_Contact != null)// Get default primary interviewer details from gen_contact
                //    {
                //        hrModel.interviewerDetails.primaryInterviewer.interviewerId = 0;
                //        hrModel.interviewerDetails.primaryInterviewer.fullName = _Contact.FullName;
                //        hrModel.interviewerDetails.primaryInterviewer.emailID = _Contact.EmailId;
                //        hrModel.interviewerDetails.primaryInterviewer.linkedin = _Contact.LinkedIn;
                //        hrModel.interviewerDetails.primaryInterviewer.designation = _Contact.Designation;
                //        hrModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                //    }
                //}

                #endregion

                #endregion

                #region ATSCall

                GenSalesHiringRequest getHr = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hrModel.Id);
                if (getHr != null)
                {
                    getHr.IsActive = getHr.IsActive ?? false;
                }
                if (getHr != null && (bool)getHr.IsActive && hrModel.Id > 0)
                {
                    if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {
                        long HR_ID = getHr.Id;
                        string HR_Number = getHr.HrNumber ?? "";
                        if (HR_ID != 0 && !string.IsNullOrEmpty(HR_Number))
                        {
                            var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(HR_ID).ConfigureAwait(false);
                            string HRJsonData = Convert.ToString(HRData_Json);
                            if (!string.IsNullOrEmpty(HRJsonData))
                            {
                                bool isAPIResponseSuccess = true;

                                try
                                {
                                    ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                    if (HRJsonData != "")
                                        isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HR_ID);
                                }
                                catch (Exception)
                                {
                                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                                }
                            }
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message, Details = hrModel });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Debriefing/Create")]
        public async Task<ObjectResult> AddDebriefingHR(HiringRequestDebriefModel hrDebriefModel, int LoggedInUserId = 0)
        {
            LoggedInUserId = Convert.ToInt32(SessionValues.LoginUserId);
            int LoginUserTypeID = SessionValues.LoginUserTypeId;

            bool IsDirectHR = false;
            bool IsBDR_MDRUser = LoginUserTypeID == 11 || LoginUserTypeID == 12 ? true : false;

            GenSalesHiringRequestDetail hiringrequestdetails = new GenSalesHiringRequestDetail();
            GenSalesHiringRequest genSalesHiringRequestUpdated = new GenSalesHiringRequest();

            #region PreValidation

            if (hrDebriefModel == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
            }
            IsDirectHR = hrDebriefModel.isDirectHR ?? false;

            bool IsPayPerHire = false;
            bool IsPayPerCredit = false;
            if (hrDebriefModel.PayPerType == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Pay per type is required" });
            }
            else
            {
                if (hrDebriefModel.PayPerType == 1)
                    IsPayPerHire = true;
                else if (hrDebriefModel.PayPerType == 2)
                    IsPayPerCredit = true;
                else
                    IsPayPerHire = true;
            }
            #endregion

            #region Validation
            if (IsDirectHR && IsBDR_MDRUser || IsPayPerCredit)
            {
                DirectHR_or_CreditHR_Debrief_ModelValidator validationRules = new DirectHR_or_CreditHR_Debrief_ModelValidator();
                ValidationResult validationResult = validationRules.Validate(hrDebriefModel);

                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestDebriefModel") });
                }
            }
            else
            {
                HiringRequestDebriefModelValidator validationRules = new HiringRequestDebriefModelValidator();
                ValidationResult validationResult = validationRules.Validate(hrDebriefModel);

                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestDebriefModel") });
                }
            }

            #endregion

            #region SP call - Sproc_UTS_AddEdit_HRDebrief

            if (!string.IsNullOrEmpty(hrDebriefModel.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(hrDebriefModel.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(hrDebriefModel.en_Id)) > 0)
            {
                hrDebriefModel.hrID = Convert.ToInt64(CommonLogic.Decrypt(hrDebriefModel.en_Id));

                string aboutCompanyDescription = string.Empty;

                if (hrDebriefModel.companyInfo == null && string.IsNullOrEmpty(hrDebriefModel.companyInfo.aboutCompanyDesc))
                    aboutCompanyDescription = hrDebriefModel?.aboutCompanyDesc;
                else
                    aboutCompanyDescription = hrDebriefModel?.companyInfo?.aboutCompanyDesc;

                object[] AddEditHRDebrief_Param = new object[]
                {
                        hrDebriefModel?.hrID,
                        hrDebriefModel?.hrTitle,
                        hrDebriefModel?.role,
                        aboutCompanyDescription,
                        hrDebriefModel?.JobDescription != null ? null : hrDebriefModel?.JobDescription,
                        hrDebriefModel?.skills,
                        hrDebriefModel?.Allskills,
                        hrDebriefModel?.IsHrfocused,
                        hrDebriefModel?.interviewerDetails?.primaryInterviewer?.fullName,
                        hrDebriefModel?.interviewerDetails?.primaryInterviewer?.emailID,
                        hrDebriefModel?.interviewerDetails?.primaryInterviewer?.linkedin,
                        hrDebriefModel?.interviewerDetails?.primaryInterviewer?.designation,
                        hrDebriefModel?.companyInfo?.companyName,
                        hrDebriefModel?.companyInfo?.industry,
                        hrDebriefModel?.companyInfo?.website,
                        hrDebriefModel?.companyInfo?.linkedInURL,
                        hrDebriefModel?.AllowSpecialEdit,
                        LoggedInUserId,
                        hrDebriefModel?.ActionType,
                        hrDebriefModel?.IsMustHaveSkillschanged,
                        hrDebriefModel?.IsGoodToHaveSkillschanged,
                        (short)AppActionDoneBy.UTS
                };

                _universalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_UTS_AddEdit_HRDebrief, AddEditHRDebrief_Param);

                #region Update Job Description With Unicode Characters
                if (!string.IsNullOrEmpty(hrDebriefModel?.JobDescription))
                    _iJDParse.SaveStepInfoWithUnicode(hrDebriefModel?.hrID.ToString(), hrDebriefModel?.JobDescription);
                #endregion
            }

            #endregion

            #region Create JD in case of Create_HRDBrifing & Update_HRDBrifing
            bool IsNewJDCreate = false;
            if (hrDebriefModel?.ActionType == "Save")
            {
                //if (hrDebriefModel?.ParsingType != "FileUpload")
                //{
                IsNewJDCreate = true;
                //}
            }
            if (IsNewJDCreate)
            {
                try
                {
                    PreviewJobPostUpdate previewJobPostUpdate = new PreviewJobPostUpdate();

                    var ObjHR = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrDebriefModel.hrID).FirstOrDefault();
                    var ObjHRDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == hrDebriefModel.hrID).FirstOrDefault();
                    var ObjDirectPlacement = _talentConnectAdminDBContext.GenDirectPlacements.Where(x => x.HiringRequestId == hrDebriefModel.hrID).FirstOrDefault();
                    if (ObjHR != null && ObjHRDetail != null)
                    {
                        previewJobPostUpdate.Currency = ObjHRDetail.Currency;
                        previewJobPostUpdate.RoleName = ObjHR.RequestForTalent;
                        if (ObjDirectPlacement != null)
                            previewJobPostUpdate.ModeOfWork = ObjDirectPlacement.ModeOfWork;
                        previewJobPostUpdate.Skills = hrDebriefModel?.skills;
                        previewJobPostUpdate.AllSkills = hrDebriefModel?.Allskills;
                        previewJobPostUpdate.JobDescription = hrDebriefModel?.JobDescription;
                        if (ObjHR.HiringTypePricingId == 1)
                            previewJobPostUpdate.EmploymentType = "Hire a Contractor";
                        if (ObjHR.HiringTypePricingId == 2)
                            previewJobPostUpdate.EmploymentType = "Hire an employee on Uplers Payroll";
                        if (ObjHR.HiringTypePricingId == 3)
                            previewJobPostUpdate.EmploymentType = "Direct-hire on your payroll";
                        previewJobPostUpdate.ExperienceYears = ObjHRDetail.YearOfExp ?? 0;

                        string content = string.Empty;
                        string fileName = string.Empty;
                        Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);
                        DateTime now = DateTime.Now;
                        string formattedDate = now.ToString("dd/MM/yyyy HH:mm:ss");
                        if (hrDebriefModel?.ParsingType == "FileUpload")
                        {
                            string sourcePath = System.IO.Path.Combine(_configuration["ClientPortalAbsolutePath"], "Media/JDParsing/JDFiles/" + ObjHR.Jdfilename);
                            string destinationPath = System.IO.Path.Combine("Media\\JDParsing\\JDFiles", ObjHR.Jdfilename);

                            if (!System.IO.File.Exists(destinationPath))
                            {
                                System.IO.File.Copy(sourcePath, destinationPath);
                            }

                            if (System.IO.File.Exists(System.IO.Path.Combine(sourcePath)))
                                System.IO.File.Delete(System.IO.Path.Combine(sourcePath));
                        }
                        else
                        {
                            fileName = $"{previewJobPostUpdate.RoleName}_{formattedDate}.pdf";
                            string pattern = @"[^a-zA-Z0-9._]";
                            // Replace specified special characters with an empty string
                            string cleanName = Regex.Replace(fileName, pattern, "");

                            content = GetJDPdfContent(previewJobPostUpdate);
                            string pdfPath = System.IO.Path.Combine(_configuration["AdminAPIProjectURL"], "Media\\JDParsing\\JDFiles", cleanName);
                            parser.GenerateJDPdf(pdfPath, content);
                            ObjHR.Jdfilename = cleanName;
                            _talentConnectAdminDBContext.Update(ObjHR);
                            _talentConnectAdminDBContext.SaveChanges();
                        }
                    }

                }
                catch (Exception)
                {

                }
            }
            #endregion

            #region Insert/Update Interviewer Details

            //if (hrDebriefModel.interviewerDetails != null)
            //{
            //    #region Primary Interviewer Details
            //    if (hrDebriefModel.interviewerDetails.primaryInterviewer != null)
            //    {
            //        if (hrDebriefModel.interviewerDetails.primaryInterviewer.interviewerId > 0) //Update
            //        {
            //            var checkexistInterviewerDetailsPrimary = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
            //                .Where(x => x.HiringRequestId == hrDebriefModel.hrID
            //                && x.HiringRequestDetailId == hiringrequestdetails.Id
            //                && x.Id == hrDebriefModel.interviewerDetails.primaryInterviewer.interviewerId).FirstOrDefault();

            //            if (checkexistInterviewerDetailsPrimary != null)
            //            {
            //                checkexistInterviewerDetailsPrimary.HiringRequestDetailId = hiringrequestdetails.Id;
            //                checkexistInterviewerDetailsPrimary.HiringRequestId = hrDebriefModel.hrID;
            //                checkexistInterviewerDetailsPrimary.InterviewerName = hrDebriefModel.interviewerDetails.primaryInterviewer.fullName;
            //                checkexistInterviewerDetailsPrimary.InterviewerEmailId = hrDebriefModel.interviewerDetails.primaryInterviewer.emailID;
            //                checkexistInterviewerDetailsPrimary.InterviewLinkedin = hrDebriefModel.interviewerDetails.primaryInterviewer.linkedin;
            //                checkexistInterviewerDetailsPrimary.InterviewerDesignation = hrDebriefModel.interviewerDetails.primaryInterviewer.designation;
            //                checkexistInterviewerDetailsPrimary.ContactId = genSalesHiringRequestUpdated.ContactId;
            //                checkexistInterviewerDetailsPrimary.IsUsedAddMore = false;
            //                _talentConnectAdminDBContext.Update(checkexistInterviewerDetailsPrimary);
            //                _talentConnectAdminDBContext.SaveChanges();
            //            }
            //        }
            //        else //Insert
            //        {
            //            GenSalesHiringRequestInterviewerDetail genSalesHiringRequestInterviewerDetails = new GenSalesHiringRequestInterviewerDetail();

            //            var checkIfAlreadyExists = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
            //                    .Where(x => x.HiringRequestId == hrDebriefModel.hrID
            //                    && x.HiringRequestDetailId == hiringrequestdetails.Id
            //                    && x.InterviewerName == hrDebriefModel.interviewerDetails.primaryInterviewer.fullName
            //                    && x.InterviewerEmailId == hrDebriefModel.interviewerDetails.primaryInterviewer.emailID
            //                    && x.InterviewLinkedin == hrDebriefModel.interviewerDetails.primaryInterviewer.linkedin
            //                    && x.InterviewerDesignation == hrDebriefModel.interviewerDetails.primaryInterviewer.designation).FirstOrDefault();

            //            if (checkIfAlreadyExists == null)
            //            {
            //                genSalesHiringRequestInterviewerDetails.HiringRequestDetailId = hiringrequestdetails.Id;
            //                genSalesHiringRequestInterviewerDetails.HiringRequestId = hrDebriefModel.hrID;
            //                genSalesHiringRequestInterviewerDetails.InterviewerName = hrDebriefModel.interviewerDetails.primaryInterviewer.fullName;
            //                genSalesHiringRequestInterviewerDetails.InterviewerEmailId = hrDebriefModel.interviewerDetails.primaryInterviewer.emailID;
            //                genSalesHiringRequestInterviewerDetails.InterviewLinkedin = hrDebriefModel.interviewerDetails.primaryInterviewer.linkedin;
            //                genSalesHiringRequestInterviewerDetails.InterviewerDesignation = hrDebriefModel.interviewerDetails.primaryInterviewer.designation;
            //                genSalesHiringRequestInterviewerDetails.ContactId = genSalesHiringRequestUpdated.ContactId;
            //                genSalesHiringRequestInterviewerDetails.IsUsedAddMore = false;
            //                _talentConnectAdminDBContext.Add(genSalesHiringRequestInterviewerDetails);
            //                _talentConnectAdminDBContext.SaveChanges();
            //            }
            //        }
            //    }
            //    #endregion
            //}

            #endregion

            #region Send Emails while adding HR.
            //Send Email only when we are adding HR.
            if (hrDebriefModel.ActionType == "Save")
            {
                genSalesHiringRequestUpdated = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hrDebriefModel.hrID);
                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                if (hrDebriefModel.isClient)
                {
                    if (genSalesHiringRequestUpdated != null)
                    {
                        if (!genSalesHiringRequestUpdated.IsEmailSend)
                        {
                            var IsActiveDetail = genSalesHiringRequestUpdated.IsActive;
                            if (IsActiveDetail != null)
                            {
                                var isActiveValue = genSalesHiringRequestUpdated.IsActive ?? false;
                                if (isActiveValue)
                                {
                                    bool email = emailBinder.SendEmailForHRCreation(hrDebriefModel.hrID, "New");

                                    //bool clientmailNotification = false;

                                    #region Send Email Notification For Client LogIn
                                    //GenContact contactdetails = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == genSalesHiringRequestUpdated.ContactId && x.IsPrimary == true);
                                    //if (contactdetails != null)
                                    //{
                                    //var password = contactdetails.FirstName.ToLower() + "@123";
                                    //var password = "Uplers@123";
                                    //var ClientName = (contactdetails.FirstName + " " + contactdetails.LastName);

                                    //string RoleNames = string.Join(",", (from hr in _talentConnectAdminDBContext.GenSalesHiringRequests
                                    //                                     join hrd in _talentConnectAdminDBContext.GenSalesHiringRequestDetails on genSalesHiringRequestUpdated.Id equals hrd.HiringRequestId
                                    //                                     join tr in _talentConnectAdminDBContext.PrgTalentRoles on hrd.RoleId equals tr.Id
                                    //                                     where hr.ContactId == genSalesHiringRequestUpdated.ContactId
                                    //                                     select tr.TalentRole).ToList());
                                    //if (contactdetails.IsClientNotificationSend)
                                    //{
                                    //    clientmailNotification = emailBinder.SendEmailNotificationForLogIn(contactdetails.EmailId, password, ClientName, RoleNames, Convert.ToInt64(genSalesHiringRequestUpdated.ContactId));
                                    //}
                                    //}

                                    //if (email && clientmailNotification)
                                    //if (email)
                                    //{
                                    //    genSalesHiringRequestUpdated.IsEmailSend = true;
                                    //    CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestUpdated, EntityState.Modified);
                                    //}

                                    #endregion
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (genSalesHiringRequestUpdated != null)
                    {
                        if (!genSalesHiringRequestUpdated.IsEmailSend)
                        {
                            var IsActiveDetail = genSalesHiringRequestUpdated.IsActive;
                            if (IsActiveDetail != null)
                            {
                                var isActiveValue = genSalesHiringRequestUpdated.IsActive ?? false;
                                if (isActiveValue)
                                {
                                    bool salesemail = emailBinder.SendEmailForHRCreation(genSalesHiringRequestUpdated.Id, "Old");
                                    if (salesemail)
                                    {
                                        genSalesHiringRequestUpdated.IsEmailSend = true;
                                        CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestUpdated, EntityState.Modified);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region ATSCall
            genSalesHiringRequestUpdated = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hrDebriefModel.hrID);
            if (genSalesHiringRequestUpdated != null)
            {
                genSalesHiringRequestUpdated.IsActive = genSalesHiringRequestUpdated.IsActive ?? false;
            }
            if (genSalesHiringRequestUpdated != null && (bool)genSalesHiringRequestUpdated.IsActive && genSalesHiringRequestUpdated.Id > 0)
            {
                if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    long HR_ID = genSalesHiringRequestUpdated.Id;
                    string HR_Number = genSalesHiringRequestUpdated.HrNumber ?? "";
                    if (HR_ID != 0 && !string.IsNullOrEmpty(HR_Number))
                    {
                        var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(HR_ID, hrDebriefModel.IsAutogenerateQuestions).ConfigureAwait(false);
                        string HRJsonData = Convert.ToString(HRData_Json);
                        if (!string.IsNullOrEmpty(HRJsonData))
                        {
                            bool isAPIResponseSuccess = true;

                            try
                            {
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                if (HRJsonData != "")
                                    isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HR_ID);

                                #region HR Status updates to ATS 

                                //New Status Change
                                if (hrDebriefModel.ActionType == "Save")
                                {
                                    object[] param = new object[] { HR_ID, 0, 0, LoggedInUserId, (short)AppActionDoneBy.UTS, false };
                                    string strParam = CommonLogic.ConvertToParamString(param);
                                    var HRStatus_Json = _iHiringRequest.GetUpdateHrStatus(strParam);
                                    if (HRStatus_Json != null)
                                    {
                                        //string JsonData = Convert.ToString(HRStatus_Json);
                                        var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                        if (!string.IsNullOrEmpty(JsonData))
                                        {
                                            aTSCall.SendHrStatusToATS(JsonData, LoggedInUserId, HR_ID);
                                        }
                                    }
                                }

                                #endregion
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                            }
                        }
                    }
                }
            }
            #endregion

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
        }

        [HttpGet("GetHRDetails")]
        public async Task<ObjectResult> GetHRDetails(long HRId, long DealId = 0)
        {
            try
            {
                #region Pre-Validation 
                if ((HRId == null || HRId == 0) && DealId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide HRId"));
                }
                #endregion
                HiringRequest HiringRequestViewModel = new HiringRequest();
                if (HRId > 0)
                {
                    GenSystemConfiguration _objSysConfDPPercetage = _talentConnectAdminDBContext.GenSystemConfigurations.FirstOrDefault(x => x.Key == "DP Percentage" && x.IsActive == true);
                    GenSystemConfiguration _objSystemConfigurationName = _talentConnectAdminDBContext.GenSystemConfigurations.FirstOrDefault(x => x.Key == "TalentCostCalculationPercentage" && x.IsActive == true);

                    GenSalesHiringRequest SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == HRId);
                    GenSalesHiringRequestDetail SalesHiringRequestDetail = new();

                    GenDirectPlacement directPlacement = _talentConnectAdminDBContext.GenDirectPlacements.FirstOrDefault(x => x.HiringRequestId == HRId);

                    GenJobPostVitalInfoClientPortal vitalInfoClientPortal = _talentConnectAdminDBContext.GenJobPostVitalInfoClientPortals.FirstOrDefault(x => x.Hrid == HRId);


                    GenContact contactDetails = new();
                    if (SalesHiringRequest != null && SalesHiringRequest.Id > 0)
                    {
                        SalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.HiringRequestId == SalesHiringRequest.Id);

                        contactDetails = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == SalesHiringRequest.ContactId);

                        HiringRequestViewModel.addHiringRequest = SalesHiringRequest;

                        HiringRequestViewModel.en_Id = CommonLogic.Encrypt(Convert.ToString(HRId));
                        HiringRequestViewModel.NameOfHiringRequest = SalesHiringRequest.RequestForTalent;

                        HiringRequestViewModel.addHiringRequest.JddumpId = SalesHiringRequest.JddumpId != null ? SalesHiringRequest.JddumpId : 0;
                        HiringRequestViewModel.addHiringRequest.PreviousHiringRequestId = SalesHiringRequest.PreviousHiringRequestId != null ? SalesHiringRequest.PreviousHiringRequestId : 0;
                        HiringRequestViewModel.addHiringRequest.Dppercentage = (SalesHiringRequest.Dppercentage == null || SalesHiringRequest.Dppercentage == 0) ? Convert.ToDecimal(_objSysConfDPPercetage.Value) : SalesHiringRequest.Dppercentage;// Added By Reeena Jain(12-Nov-2022)
                        HiringRequestViewModel.addHiringRequest.TalentCostCalcPercentage = (SalesHiringRequest.TalentCostCalcPercentage == null || SalesHiringRequest.TalentCostCalcPercentage == 0) ? Convert.ToDecimal(_objSystemConfigurationName.Value) : SalesHiringRequest.TalentCostCalcPercentage;
                        HiringRequestViewModel.IsAdHocExistsHR = SalesHiringRequest.IsAdHocHr;
                        HiringRequestViewModel.contact = contactDetails == null ? "" : contactDetails.EmailId;
                        HiringRequestViewModel.SalesHiringRequest_Details = SalesHiringRequestDetail;
                        HiringRequestViewModel.RoleID = HiringRequestViewModel.SalesHiringRequest_Details.RoleId;
                        HiringRequestViewModel.Skillmulticheckbox = new List<SkillOptionVM>();
                        HiringRequestViewModel.AllSkillmulticheckbox = new List<SkillOptionVM>();

                        if (!string.IsNullOrEmpty(SalesHiringRequest.Jdfilename))
                        {
                            string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", SalesHiringRequest.Jdfilename);
                            if (System.IO.File.Exists(filePath))
                            {
                                Byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                                String file = Convert.ToBase64String(bytes);
                                HiringRequestViewModel.jdfileExtension = Path.GetExtension(filePath);
                                HiringRequestViewModel.jdfile = string.Format("data:image/{0};base64,{1}", Path.GetExtension(filePath).Replace(".", ""), file);
                            }
                        }

                        // Fetch the skills from the GenSalesHiringRequestSkillDetails table.
                        var salesHiringSkillDetailList = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(x => x.HiringRequestId == HRId && x.Proficiency == "Strong").ToList();
                        if (salesHiringSkillDetailList != null)
                        {
                            foreach (var s in salesHiringSkillDetailList)
                            {
                                if (s.SkillId != null && s.SkillId != 0)
                                {
                                    var SkillDetails = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Id == s.SkillId).ToList();
                                    if (SkillDetails != null)
                                    {
                                        foreach (var d in SkillDetails)
                                        {
                                            HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = d.Id.ToString(), IsSelected = Convert.ToBoolean(d.IsActive), Text = d.Skill, Proficiency = "Strong" });
                                        }
                                    }
                                }
                                // show the other added skills in the list when displaying HR.
                                else
                                {
                                    var tempSkillDetails = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkillId == s.TempSkillId).ToList();
                                    if (tempSkillDetails != null)
                                    {
                                        foreach (var d in tempSkillDetails)
                                        {
                                            HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = d.TempSkillId.ToString(), TempSkill_ID = d.Id.ToString(), IsSelected = true, Text = d.TempSkill, Proficiency = "Strong" });
                                        }
                                    }
                                }
                            }
                        }

                        //UTS-4752: Fetch good to have skills.
                        var salesHiringAllSkillDetailList = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(x => x.HiringRequestId == HRId && x.Proficiency == "Basic").ToList();
                        if (salesHiringAllSkillDetailList != null)
                        {
                            foreach (var s in salesHiringAllSkillDetailList)
                            {
                                if (s.SkillId != null && s.SkillId != 0)
                                {
                                    var SkillDetails = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Id == s.SkillId).ToList();
                                    if (SkillDetails != null)
                                    {
                                        foreach (var d in SkillDetails)
                                        {
                                            HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = d.Id.ToString(), IsSelected = Convert.ToBoolean(d.IsActive), Text = d.Skill, Proficiency = "Basic" });
                                        }
                                    }
                                }
                                // show the other added skills in the list when displaying HR.
                                else
                                {
                                    var tempSkillDetails = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkillId == s.TempSkillId).ToList();
                                    if (tempSkillDetails != null)
                                    {
                                        foreach (var d in tempSkillDetails)
                                        {
                                            HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = d.TempSkillId.ToString(), TempSkill_ID = d.Id.ToString(), IsSelected = true, Text = d.TempSkill, Proficiency = "Basic" });
                                        }
                                    }
                                }
                            }
                        }


                        if (directPlacement != null && directPlacement.Id > 0)
                        {
                            if (!string.IsNullOrEmpty(directPlacement.Country) && directPlacement.Country != "0")
                            {
                                var ctRegion = _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.Id == Convert.ToInt32(directPlacement.Country)).FirstOrDefault();
                                if (ctRegion != null)
                                    HiringRequestViewModel.ModeofWorkAddress = directPlacement.Address + "," + directPlacement.City + "," + directPlacement.State + "," + ctRegion.Country + " " + ctRegion.CountryRegion + "," + directPlacement.PostalCode;
                                else
                                    HiringRequestViewModel.ModeofWorkAddress = directPlacement.Address + "," + directPlacement.City + "," + directPlacement.State + "," + directPlacement.PostalCode;
                            }

                            HiringRequestViewModel.directPlacement.Address = directPlacement.Address;
                            HiringRequestViewModel.directPlacement.City = directPlacement.City;
                            HiringRequestViewModel.directPlacement.State = directPlacement.State;
                            HiringRequestViewModel.directPlacement.PostalCode = directPlacement.PostalCode;
                            HiringRequestViewModel.directPlacement.Country = directPlacement.Country;
                            HiringRequestViewModel.directPlacement.JobLocation = directPlacement.JobLocation;
                            HiringRequestViewModel.directPlacement.IsOpenToWorkNearByCities = directPlacement.IsOpenToWorkNearByCities;
                            HiringRequestViewModel.directPlacement.FrequencyOfficeVisitId = directPlacement.FrequencyOfficeVisitId;
                            HiringRequestViewModel.directPlacement.NearByCities = directPlacement.NearByCities;
                            HiringRequestViewModel.directPlacement.AtsJobLocationId = directPlacement.AtsJobLocationId;
                            HiringRequestViewModel.directPlacement.AtsNearByCities = directPlacement.AtsNearByCities;


                            HiringRequestViewModel.hdnModeOfWork = directPlacement.ModeOfWork;
                            if (directPlacement.ModeOfWork != null && directPlacement.ModeOfWork != "")
                                HiringRequestViewModel.ModeOfWorkingId = _talentConnectAdminDBContext.PrgModeOfWorkings.FirstOrDefault(x => x.ModeOfWorking == directPlacement.ModeOfWork).Id.ToString();
                        }


                        if (contactDetails != null && contactDetails.Id > 0 && contactDetails.CompanyId > 0)
                        {
                            //HiringRequestViewModel.IsClientNotificationSent = contactDetails.IsClientNotificationSend;
                            HiringRequestViewModel.IsClientNotificationSent = false;
                            HiringRequestViewModel.clientName = contactDetails.FullName;
                            HiringRequestViewModel.fullClientName = contactDetails.FullName + "(" + contactDetails.EmailId + ")";
                            GenCompany _Company = _talentConnectAdminDBContext.GenCompanies.FirstOrDefault(x => x.Id == contactDetails.CompanyId);
                            if (_Company != null)
                            {
                                HiringRequestViewModel.company = _Company.Company;

                                #region CompanyInfo for pay per credit
                                HiringRequestViewModel.CompanyInfo = new CompanyInfo();
                                HiringRequestViewModel.CompanyInfo.companyID = _Company.Id;
                                HiringRequestViewModel.CompanyInfo.companyName = _Company.Company ?? "";
                                HiringRequestViewModel.CompanyInfo.industry = _Company.Industry ?? _Company.IndustryType ?? "";
                                HiringRequestViewModel.CompanyInfo.website = _Company.Website ?? "";
                                HiringRequestViewModel.CompanyInfo.linkedInURL = _Company.LinkedInProfile ?? "";
                                HiringRequestViewModel.CompanyInfo.companySize = _Company.CompanySize ?? 0;
                                HiringRequestViewModel.CompanyInfo.aboutCompanyDesc = _Company.AboutCompanyDesc ?? "";
                                HiringRequestViewModel.CompanyInfo.IsPostaJob = false;
                                HiringRequestViewModel.CompanyInfo.IsProfileView = false;

                                if (SalesHiringRequest.HrtypeId == (short)PayPerCredit.PostaJobViewBasedCredit)
                                {
                                    HiringRequestViewModel.CompanyInfo.IsPostaJob = true;
                                    HiringRequestViewModel.CompanyInfo.IsProfileView = true;
                                }
                                if (SalesHiringRequest.HrtypeId == (short)PayPerCredit.PostaJobCreditBased)
                                {
                                    HiringRequestViewModel.CompanyInfo.IsPostaJob = true;
                                    HiringRequestViewModel.CompanyInfo.IsProfileView = false;
                                }
                                if (SalesHiringRequest.HrtypeId == (short)PayPerCredit.PostajobWithViewCreditsButnoJobPostCredits)
                                {
                                    HiringRequestViewModel.CompanyInfo.IsPostaJob = false;
                                    HiringRequestViewModel.CompanyInfo.IsProfileView = true;
                                }
                                #endregion

                                //if hr level IsTransparentPricing is null then at the time of edit we will get from company level (Pay per hire case)
                                if (_Company.CompanyTypeId == 1 && HiringRequestViewModel.addHiringRequest.IsTransparentPricing == null)
                                {
                                    HiringRequestViewModel.addHiringRequest.IsTransparentPricing = _Company.IsTransparentPricing;
                                }
                            }
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
                        }

                        var DebriefSkillData = (from tdata in _talentConnectAdminDBContext.GenSalesHrJddumpSkillDetails
                                                where tdata.JddumpId == SalesHiringRequest.JddumpId
                                                select tdata).Distinct().ToList();
                        if (DebriefSkillData.Count > 0)
                            HiringRequestViewModel.IsDebriefSkillsAvailable = true;
                        else
                            HiringRequestViewModel.IsDebriefSkillsAvailable = false;

                        #region GEt HR POC details
                        List<Sproc_HR_POC_ClientPortal_Result>? hrPocData = null;
                        try
                        {
                            object[] pocParam = new object[]
                            {
                                    0,
                                    "",
                                    HRId,
                                    "",
                                    0,
                                    false
                            };
                            string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                            hrPocData = _iHiringRequest.SaveandGetHRPOCDetails(pocParamString);
                            if (hrPocData != null && hrPocData.Any())
                            {
                                HiringRequestViewModel.HRPOCUserID = hrPocData;
                            }
                            else
                            {
                                HiringRequestViewModel.HRPOCUserID = null;
                            }
                        }
                        catch
                        {

                        }
                        HiringRequestViewModel.ShowHRPOCDetailsToTalents = SalesHiringRequest.ShowHrpocdetailsToTalents ?? false;
                        #endregion

                        #region Vital Information
                        if (vitalInfoClientPortal != null)
                        {
                            HiringRequestViewModel.CompensationOption = vitalInfoClientPortal.CompensationOption;
                            HiringRequestViewModel.HRIndustryType = vitalInfoClientPortal.IndustryType;
                            HiringRequestViewModel.HasPeopleManagementExp = vitalInfoClientPortal.HasPeopleManagementExp;
                            HiringRequestViewModel.Prerequisites = vitalInfoClientPortal.Prerequisites;
                            HiringRequestViewModel.StringSeparator = vitalInfoClientPortal.StringSeparator;

                        }
                        #endregion

                        #region GetTheRR&RolesforDraftHR

                        if (HiringRequestViewModel.IsDebriefSkillsAvailable && !Convert.ToBoolean(HiringRequestViewModel.addHiringRequest.IsActive))
                        {
                            var jdDumpDetails = _talentConnectAdminDBContext.GenSalesHrJddumps.Where(x => x.Id == SalesHiringRequest.JddumpId).FirstOrDefault();

                            if (jdDumpDetails != null)
                            {
                                HiringRequestViewModel.SalesHiringRequest_Details.Requirement = jdDumpDetails.Requirement;
                                HiringRequestViewModel.SalesHiringRequest_Details.RolesResponsibilities = jdDumpDetails.RolesResponsibilities;

                                //UTS-5009: If already skillcheck box have the data then do not populate from dump table.
                                if (jdDumpDetails.Jdskills != null && HiringRequestViewModel.Skillmulticheckbox != null && HiringRequestViewModel.Skillmulticheckbox.Count() == 0)
                                {
                                    List<string> skillList = new List<string>();
                                    skillList = jdDumpDetails.Jdskills.ToString().Split(',').ToList();
                                    foreach (var s in skillList)
                                    {
                                        var SkillDetails = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                                        if (SkillDetails != null)
                                        {
                                            HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillDetails.Id.ToString(), IsSelected = Convert.ToBoolean(SkillDetails.IsActive), Text = SkillDetails.Skill, Proficiency = "Strong" });
                                        }
                                        // show the other added skills in the list when displaying HR.
                                        else
                                        {
                                            var tempSkillDetails = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                                            if (tempSkillDetails != null)
                                            {
                                                HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = tempSkillDetails.TempSkillId.ToString(), TempSkill_ID = tempSkillDetails.Id.ToString(), IsSelected = true, Text = tempSkillDetails.TempSkill, Proficiency = "Strong" });
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Get InterviewerDetails(Primary, Secondary)

                        HiringRequestViewModel.interviewerDetails = new();
                        HiringRequestViewModel.interviewerDetails.primaryInterviewer = new();
                        HiringRequestViewModel.interviewerDetails.secondaryinterviewerList = new();

                        var SalesHiringRequestInterviewerList =
                            _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                            .Where(z => z.HiringRequestId == HRId).ToList();

                        if (SalesHiringRequestInterviewerList.Any())
                        {
                            int count = 1;
                            foreach (var HiringRequestInterviwerDetailList in SalesHiringRequestInterviewerList)
                            {
                                if (count == 1)
                                {
                                    HiringRequestViewModel.interviewerDetails.primaryInterviewer.interviewerId = HiringRequestInterviwerDetailList.Id;
                                    HiringRequestViewModel.interviewerDetails.primaryInterviewer.fullName = HiringRequestInterviwerDetailList.InterviewerName;
                                    HiringRequestViewModel.interviewerDetails.primaryInterviewer.emailID = HiringRequestInterviwerDetailList.InterviewerEmailId;
                                    HiringRequestViewModel.interviewerDetails.primaryInterviewer.linkedin = HiringRequestInterviwerDetailList.InterviewLinkedin;
                                    HiringRequestViewModel.interviewerDetails.primaryInterviewer.designation = HiringRequestInterviwerDetailList.InterviewerDesignation;
                                    HiringRequestViewModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                                }
                                else
                                {
                                    HiringRequestViewModel.interviewerDetails.secondaryinterviewerList
                                    .Add(new InterviewerClass
                                    {
                                        interviewerId = HiringRequestInterviwerDetailList.Id,
                                        fullName = HiringRequestInterviwerDetailList.InterviewerName,
                                        linkedin = HiringRequestInterviwerDetailList.InterviewLinkedin,
                                        emailID = HiringRequestInterviwerDetailList.InterviewerEmailId,
                                        designation = HiringRequestInterviwerDetailList.InterviewerDesignation,
                                        isUserAddMore = true
                                    });
                                }
                                count++;
                            }
                        }
                        else
                        {
                            if (contactDetails != null)// Get default primary interviewer details from gen_contact
                            {
                                HiringRequestViewModel.interviewerDetails.primaryInterviewer.interviewerId = 0;
                                HiringRequestViewModel.interviewerDetails.primaryInterviewer.fullName = contactDetails.FullName;
                                HiringRequestViewModel.interviewerDetails.primaryInterviewer.emailID = contactDetails.EmailId;
                                HiringRequestViewModel.interviewerDetails.primaryInterviewer.linkedin = contactDetails.LinkedIn;
                                HiringRequestViewModel.interviewerDetails.primaryInterviewer.designation = contactDetails.Designation;
                                HiringRequestViewModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                            }
                        }

                        #endregion

                        HiringRequestViewModel.CompanyCategory = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == contactDetails.CompanyId).FirstOrDefault().Category;

                        #region Get months and Overlapping hours.

                        if (SalesHiringRequestDetail != null)
                        {
                            // UTS-3459: Added the missing properties to the corresponding model properties. 
                            HiringRequestViewModel.DurationType = SalesHiringRequestDetail.DurationType; //Represent Duration Type
                            HiringRequestViewModel.contractDuration = Convert.ToString(SalesHiringRequestDetail.SpecificMonth); // Represents contract duration in months                                       
                            HiringRequestViewModel.Currency = SalesHiringRequestDetail.Currency;
                            HiringRequestViewModel.minimumBudget = Convert.ToDecimal(SalesHiringRequestDetail.BudgetFrom);
                            HiringRequestViewModel.maximumBudget = Convert.ToDecimal(SalesHiringRequestDetail.BudgetTo);
                            HiringRequestViewModel.SalesHiringRequest_Details.OverlapingHours = Convert.ToDecimal(SalesHiringRequestDetail.OverlapingHours);
                            HiringRequestViewModel.SalesHiringRequest_Details.TimeZoneFromTime = SalesHiringRequestDetail.TimeZoneFromTime;
                            HiringRequestViewModel.SalesHiringRequest_Details.TimeZoneEndTime = SalesHiringRequestDetail.TimeZoneEndTime;
                            HiringRequestViewModel.SalesHiringRequest_Details.AdhocBudgetCost = Convert.ToDecimal(SalesHiringRequestDetail.AdhocBudgetCost);

                            if (HiringRequestViewModel.minimumBudget > 0 && HiringRequestViewModel.maximumBudget > 0 && HiringRequestViewModel.SalesHiringRequest_Details.AdhocBudgetCost == 0)
                            {
                                HiringRequestViewModel.BudgetType = "2";
                            }
                            if (HiringRequestViewModel.minimumBudget == 0 && HiringRequestViewModel.maximumBudget == 0 && HiringRequestViewModel.SalesHiringRequest_Details.AdhocBudgetCost > 0)
                            {
                                HiringRequestViewModel.BudgetType = "1";
                            }

                            HiringRequestViewModel.SalesHiringRequest_Details.IsHrfocused = SalesHiringRequestDetail.IsHrfocused;
                        }
                        #endregion

                        #region CompanyTypes

                        var companyType = _talentConnectAdminDBContext.PrgCompanyTypes.ToList();

                        List<CompanyType> companyTypes = new();
                        HiringRequestViewModel.CompanyTypes = null;

                        PrgHiringRequestType prgHiringRequestType = _talentConnectAdminDBContext.PrgHiringRequestTypes.FirstOrDefault(x => x.Id == SalesHiringRequest.HrtypeId);

                        foreach (var item in companyType)
                        {
                            CompanyType type = new();
                            type.ID = item.Id;
                            type.Name = item.CompanyType;

                            if (SalesHiringRequest.HrtypeId == null && item.Id == 1)//default pay per hire
                            {
                                type.IsActive = true;
                            }
                            else if (prgHiringRequestType != null && prgHiringRequestType.CompanyTypeId == item.Id)
                            {
                                type.IsActive = true;
                            }
                            else
                            {
                                type.IsActive = false;
                            }
                            companyTypes.Add(type);
                        }

                        if (companyTypes.Any())
                        {
                            HiringRequestViewModel.CompanyTypes = companyTypes;
                        }

                        #endregion

                        #region Direct HR
                        HiringRequestViewModel.DirectHiringInfo_edit = new LoginUserHRInfo_Edit();
                        long LoggedInUserTypeId = SessionValues.LoginUserTypeId;

                        HiringRequestViewModel.DirectHiringInfo_edit.isDirectHR = SalesHiringRequest.IsDirectHr ?? false;

                        if (LoggedInUserTypeId == 11 || LoggedInUserTypeId == 12)
                            HiringRequestViewModel.DirectHiringInfo_edit.isBDR_MDRUser = true;
                        else
                            HiringRequestViewModel.DirectHiringInfo_edit.isBDR_MDRUser = false;

                        if (HiringRequestViewModel.DirectHiringInfo_edit.isDirectHR && HiringRequestViewModel.DirectHiringInfo_edit.isBDR_MDRUser)
                        {
                            HiringRequestViewModel.DirectHiringInfo_edit.disabledFields = new DisabledFields();
                            HiringRequestViewModel.DirectHiringInfo_edit.removeFields = new RemoveFields();
                        }
                        #endregion
                    }
                }
                if (DealId != 0)
                {
                    var ObjDeal = _talentConnectAdminDBContext.GenDeals.Where(x => x.DealId == DealId).FirstOrDefault();
                    if (ObjDeal != null)
                    {
                        HiringRequestViewModel.addHiringRequest.DealId = ObjDeal.DealId;
                        object[] param = new object[] { ObjDeal.DealId };
                        string paramasString = CommonLogic.ConvertToParamString(param);
                        var data = _commonInterface.hiringRequest.Sproc_Get_HRDetails_From_DealId_Result(paramasString);
                        if (data != null)
                        {
                            HiringRequestViewModel.contact = data.ClientEmail;
                            HiringRequestViewModel.company = data.Company;
                            HiringRequestViewModel.SalesHiringRequest_Details.AdhocBudgetCost = data.Adhoc_BudgetCost;
                            HiringRequestViewModel.addHiringRequest.SalesUserId = data.SalesUserID;
                            HiringRequestViewModel.SalesHiringRequest_Details.Currency = data.CurrencyCode;
                            HiringRequestViewModel.addHiringRequest.DiscoveryCall = data.Discovery_Call;

                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, CommonLogic.ReturnObject(StatusCodes.Status404NotFound, "No records founds", HiringRequestViewModel));
                    }
                }

                #region AllowSpecialEdit

                var loggedInUserEmployeeID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == SessionValues.LoginUserId).Select(a => a.EmployeeId).FirstOrDefault();

                // If job is not closed then show the Edit HR COE CTA.
                if (HiringRequestViewModel.addHiringRequest.JobStatusId != 2)
                {
                    if (loggedInUserEmployeeID != null)
                    {
                        object[] param = new object[] { SessionValues.LoginUserId, loggedInUserEmployeeID };
                        string paramString = CommonLogic.ConvertToParamString(param);
                        Sproc_UTS_FetchUsersWithSpecialEditAccess_Result result = _commonInterface.ViewAllHR.CheckSpecialEdits(paramString);

                        if (result != null)
                        {
                            HiringRequestViewModel.AllowSpecialEdit = result.AllowSpecialEdit.HasValue ? result.AllowSpecialEdit.Value : false;
                        }
                    }
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "HR Details", HiringRequestViewModel));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("EditHRPOC")]
        public ObjectResult EditHRPOC(EditHRPocViewModel editPOC)
        {
            try
            {
                #region PreValidation
                if (editPOC == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty." });
                }
                if (editPOC.HRID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "HRID must be greater than 0" });
                }
                if (editPOC.ContactID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "ContactID must be greater than 0" });
                }
                #endregion

                #region update POC & flag
                object[] param = null;
                if (!string.IsNullOrEmpty(editPOC.ContactNo))
                {
                    param = new object[] { editPOC.ContactID, editPOC.ContactNo };
                    _universalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_HR_EditPOC, param);
                }

                param = new object[] { editPOC.HRID, editPOC.ContactID, editPOC.ShowEmailToTalent, editPOC.ShowContactNumberToTalent, string.Empty };
                _universalProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_Update_POC_ShowHideFlag, param);
                #endregion

                #region ATS call

                if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    if (editPOC.HRID > 0)
                    {
                        var HRData_Json = _iHiringRequest.GetAllHRDataForAdmin(editPOC.HRID).ConfigureAwait(false);
                        string HRJsonData = Convert.ToString(HRData_Json);
                        if (!string.IsNullOrEmpty(HRJsonData))
                        {
                            bool isAPIResponseSuccess = true;

                            try
                            {
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                if (HRJsonData != "")
                                    isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), editPOC.HRID);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                            }
                        }
                    }
                }
                #endregion

                #region GEt HR POC details
                List<Sproc_HR_POC_ClientPortal_Result>? hrPocData = null;
                try
                {
                    object[] pocParam = new object[]
                    {
                                    0,
                                    "",
                                    editPOC.HRID,
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

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = hrPocData });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("DeleteHRPOC")]
        public ObjectResult DeleteHRPOC(long pocId, long HRId)
        {
            try
            {
                #region PreValidation

                if (HRId == 0 || pocId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty." });
                }

                #endregion

                #region Delete HR POC
                List<Sproc_HR_POC_ClientPortal_Result>? hrPocData = null;
                try
                {
                    if (pocId > 0)
                    {
                        object[] pocParam = new object[]
                        {
                            SessionValues.LoginUserId,
                            "",
                            HRId,
                            "",
                            pocId,
                            false
                        };
                        string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                        hrPocData = _iHiringRequest.SaveandGetHRPOCDetails(pocParamString);
                    }
                }
                catch
                {

                }
                #endregion

                #region ATS call

                if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    if (HRId > 0)
                    {
                        var HRData_Json = _iHiringRequest.GetAllHRDataForAdmin(HRId).ConfigureAwait(false);
                        string HRJsonData = Convert.ToString(HRData_Json);
                        if (!string.IsNullOrEmpty(HRJsonData))
                        {
                            bool isAPIResponseSuccess = true;

                            try
                            {
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                if (HRJsonData != "")
                                    isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HRId);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                            }
                        }
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = hrPocData });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("ViewHRDetails")]
        public async Task<ObjectResult> ViewHRDetails(long HRId)
        {
            #region Pre-Validation 
            if (HRId == null || HRId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide HRId"));
            }
            #endregion

            GenSalesHiringRequest SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == HRId);
            GenSalesHiringRequestDetail SalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == HRId).FirstOrDefault();
            var SalesHiringRequestInterviewerList = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(z => z.HiringRequestId == HRId).ToList();
            GenDirectPlacement directPlacement = _talentConnectAdminDBContext.GenDirectPlacements.FirstOrDefault(x => x.HiringRequestId == HRId);
            ViewHRDetailsViewModel viewHRDetailsViewModel = new ViewHRDetailsViewModel();
            if (directPlacement != null)
            {
                viewHRDetailsViewModel.DirectHRModeOfWork = directPlacement.ModeOfWork ?? "";

                if (directPlacement.ModeOfWork != "Remote")
                {
                    viewHRDetailsViewModel.Address = ""; // directPlacement.Address ?? "";
                    viewHRDetailsViewModel.City = directPlacement.JobLocation ?? "";
                    viewHRDetailsViewModel.State = ""; // directPlacement.State ?? "";
                                                       //if (directPlacement.Country != "0" && directPlacement.Country != null)
                                                       //{
                                                       //    PrgCountryRegion country = _talentConnectAdminDBContext.PrgCountryRegions.FirstOrDefault(x => x.Id == Convert.ToInt32(directPlacement.Country));
                                                       //    if (country != null)
                                                       //    {
                                                       //        viewHRDetailsViewModel.Country = ""; // country.Country + " (" + country.CountryRegion + ")" ?? "";
                                                       //    }
                                                       //}
                                                       //else
                                                       //{
                    viewHRDetailsViewModel.Country = "";
                    //}
                    viewHRDetailsViewModel.PostalCode = directPlacement.PostalCode ?? "";
                }
            }
            if (SalesHiringRequest != null)
            {
                viewHRDetailsViewModel.isDirectHR = SalesHiringRequest.IsDirectHr ?? false;
                viewHRDetailsViewModel.hrNumber = SalesHiringRequest.HrNumber;
                GenContact _Contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == SalesHiringRequest.ContactId);
                if (_Contact != null && _Contact.Id > 0 && _Contact.CompanyId > 0)
                {
                    viewHRDetailsViewModel.clientName = _Contact.FullName;
                    GenCompany _Company = _talentConnectAdminDBContext.GenCompanies.FirstOrDefault(x => x.Id == _Contact.CompanyId);
                    if (_Company != null)
                    {
                        viewHRDetailsViewModel.company = _Company.Company;
                        viewHRDetailsViewModel.AboutCompanyDesc = _Company.AboutCompanyDesc;

                        #region CompanyInfo for pay per credit
                        viewHRDetailsViewModel.CompanyInfo = new CompanyInfo();
                        viewHRDetailsViewModel.CompanyInfo.companyID = _Company.Id;
                        viewHRDetailsViewModel.CompanyInfo.companyName = _Company.Company ?? "NA";
                        viewHRDetailsViewModel.CompanyInfo.industry = _Company.Industry ?? _Company.IndustryType ?? "NA";
                        viewHRDetailsViewModel.CompanyInfo.website = _Company.Website ?? "NA";
                        viewHRDetailsViewModel.CompanyInfo.linkedInURL = _Company.LinkedInProfile ?? "NA";
                        viewHRDetailsViewModel.CompanyInfo.companySize = _Company.CompanySize ?? 0;
                        viewHRDetailsViewModel.CompanyInfo.aboutCompanyDesc = _Company.AboutCompanyDesc ?? "NA";
                        #endregion
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
                }

                UsrUser usrUser = _talentConnectAdminDBContext.UsrUsers.Where(xz => xz.Id == SalesHiringRequest.SalesUserId).FirstOrDefault();
                if (usrUser != null)
                {
                    viewHRDetailsViewModel.salesPerson = usrUser.FirstName + " " + usrUser.LastName;
                }
                if (SalesHiringRequestDetail.RoleId != 0 && SalesHiringRequestDetail.RoleId != null)
                {
                    PrgTalentRole roles = _talentConnectAdminDBContext.PrgTalentRoles.FirstOrDefault(x => x.Id == SalesHiringRequestDetail.RoleId);
                    if (roles != null)
                    {
                        viewHRDetailsViewModel.hiringRequestRole = roles.TalentRole;
                    }
                }

                viewHRDetailsViewModel.hiringRequestTitle = SalesHiringRequest.RequestForTalent;
                viewHRDetailsViewModel.jobDescription = SalesHiringRequest.Jdfilename;
                viewHRDetailsViewModel.JDURL = SalesHiringRequest.Jdurl;
                viewHRDetailsViewModel.currency = SalesHiringRequestDetail.Currency;
                viewHRDetailsViewModel.contractType = SalesHiringRequest.IsHrtypeDp == true ? "Direct Placement" : "Contractual";
                viewHRDetailsViewModel.hiringCost = SalesHiringRequestDetail.Cost;
                viewHRDetailsViewModel.budgetFrom = SalesHiringRequestDetail.BudgetFrom;
                viewHRDetailsViewModel.budgetTo = SalesHiringRequestDetail.BudgetTo;
                viewHRDetailsViewModel.IsConfidentialBudget = SalesHiringRequestDetail.IsConfidentialBudget ?? false; //UTS-6304 Confidential budget

                viewHRDetailsViewModel.NRPercetange = SalesHiringRequest.TalentCostCalcPercentage;
                viewHRDetailsViewModel.contractDuration = SalesHiringRequestDetail.SpecificMonth;
                viewHRDetailsViewModel.requiredExperienceYear = SalesHiringRequestDetail.YearOfExp;
                viewHRDetailsViewModel.term = SalesHiringRequestDetail.DurationType;
                viewHRDetailsViewModel.availability = SalesHiringRequest.Availability;
                //viewHRDetailsViewModel.noOfTalents = SalesHiringRequestDetail.NoofEmployee;
                if (!string.IsNullOrEmpty(viewHRDetailsViewModel.availability))
                {
                    if (viewHRDetailsViewModel.availability.Equals(HiringRequestAvailability.PartTime))
                    {
                        viewHRDetailsViewModel.noOfTalents = Convert.ToDecimal(SalesHiringRequestDetail.NoofEmployee / 2.0);
                    }
                    else
                    {
                        viewHRDetailsViewModel.noOfTalents = SalesHiringRequestDetail.NoofEmployee;
                    }
                }
                viewHRDetailsViewModel.region = SalesHiringRequestDetail.TimezonePreference?.Split('|')[0];

                PrgContactTimeZone? prgContactTimeZone = _talentConnectAdminDBContext.PrgContactTimeZones.Where(y => y.Id == SalesHiringRequestDetail.TimezoneId).FirstOrDefault();
                if (prgContactTimeZone != null)
                {
                    viewHRDetailsViewModel.timeZone = prgContactTimeZone.TimeZoneTitle;
                }
                viewHRDetailsViewModel.howSoon = SalesHiringRequestDetail.HowSoon;
                viewHRDetailsViewModel.dealID = SalesHiringRequest.DealId;
                viewHRDetailsViewModel.bqLink = SalesHiringRequest.Bqlink;
                viewHRDetailsViewModel.discoveryCall = SalesHiringRequest.DiscoveryCall;

                // If BqLink and Discovery Call data is not a link then display this as blank.
                if (!string.IsNullOrEmpty(viewHRDetailsViewModel.bqLink) &&
                    !(viewHRDetailsViewModel.bqLink.Contains("http") || viewHRDetailsViewModel.bqLink.Contains("https")))
                {
                    viewHRDetailsViewModel.bqLink = string.Empty;
                }

                if (!string.IsNullOrEmpty(viewHRDetailsViewModel.discoveryCall) &&
                    !(viewHRDetailsViewModel.discoveryCall.Contains("http") || viewHRDetailsViewModel.discoveryCall.Contains("https")))
                {
                    viewHRDetailsViewModel.discoveryCall = string.Empty;
                }

                GenSalesHiringRequestSkillDetail SalesHiringRequestSkillDetail = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(yz => yz.HiringRequestId == HRId).FirstOrDefault();
                if (SalesHiringRequestSkillDetail != null)
                {
                    viewHRDetailsViewModel.modeOfWork = SalesHiringRequestSkillDetail.Proficiency;
                }
                viewHRDetailsViewModel.dpPercentage = SalesHiringRequest.Dppercentage;
                viewHRDetailsViewModel.rolesResponsibilites = SalesHiringRequestDetail.RolesResponsibilities;
                viewHRDetailsViewModel.aboutCompany = SalesHiringRequest.AboutCompanyDesc;
                viewHRDetailsViewModel.requirments = SalesHiringRequestDetail.Requirement;
                viewHRDetailsViewModel.IsHrfocused = SalesHiringRequestDetail.IsHrfocused;
                viewHRDetailsViewModel.IsFresherAllowed = SalesHiringRequestDetail.IsFresherAllowed;

                viewHRDetailsViewModel.Job_Description = SalesHiringRequestDetail.JobDescription;

                #region Check Model IsPayPerHire / IsPayPerCredit
                //default set false
                viewHRDetailsViewModel.IsPayPerHire = false;
                viewHRDetailsViewModel.IsPayPerCredit = false;

                if (SalesHiringRequest.HrtypeId == (short)PayPerHire.SalesHR)
                {
                    viewHRDetailsViewModel.IsPayPerHire = true;
                }
                if (SalesHiringRequest.HrtypeId == (short)PayPerCredit.PostaJobViewBasedCredit || SalesHiringRequest.HrtypeId == (short)PayPerCredit.PostaJobCreditBased || SalesHiringRequest.HrtypeId == (short)PayPerCredit.PostajobWithViewCreditsButnoJobPostCredits)
                {
                    viewHRDetailsViewModel.IsPayPerCredit = true;
                }
                #endregion

                HRCommonOperation commonOperation = new HRCommonOperation(_talentConnectAdminDBContext);
                #region Pay Per Hire Model

                if ((bool)viewHRDetailsViewModel.IsPayPerHire &&
                    SalesHiringRequest.IsTransparentPricing != null
                    && SalesHiringRequest.HiringTypePricingId > 0)//Pay per Hire
                {
                    var getprgHiringTypePricing = _talentConnectAdminDBContext.PrgHiringTypePricings.FirstOrDefault(x => x.Id == SalesHiringRequest.HiringTypePricingId);
                    //var _CurrencyExchangeRate = _talentConnectAdminDBContext.PrgCurrencyExchangeRates.Where(x => x.CurrencyCode == SalesHiringRequestDetail.Currency).FirstOrDefault();
                    var GetprgPayrollType = _talentConnectAdminDBContext.PrgPayrollTypes.FirstOrDefault(x => x.Id == SalesHiringRequest.PayrollTypeId);

                    if (getprgHiringTypePricing != null)
                    {
                        viewHRDetailsViewModel.transparentModel = commonOperation.GetTransparentModelData(SalesHiringRequest, SalesHiringRequestDetail, getprgHiringTypePricing, GetprgPayrollType, viewHRDetailsViewModel.company);
                    }
                }
                else
                {
                    if (SalesHiringRequest.IsHrtypeDp != null && SalesHiringRequest.IsHrtypeDp)
                    {
                        viewHRDetailsViewModel.EngagementType = "Direct Placement";
                    }
                    else
                    {
                        string month = string.Empty;
                        if (SalesHiringRequestDetail.SpecificMonth == -1)
                        {
                            month = "Indefinite Months";
                        }
                        else if (SalesHiringRequestDetail.SpecificMonth > 0)
                        {
                            month = SalesHiringRequestDetail.SpecificMonth.ToString() + " Months"; ;
                        }
                        else
                        {
                            month = "0 Months";
                        }
                        viewHRDetailsViewModel.EngagementType = "Contract - " + month;
                    }
                    viewHRDetailsViewModel.BudgetTitle = "Budget";
                    viewHRDetailsViewModel.BudgetText = !string.IsNullOrEmpty(SalesHiringRequestDetail.Cost) ? SalesHiringRequestDetail.Cost : string.Empty;
                }
                #endregion

                #region Pay per Credit
                string Availability = SalesHiringRequest.Availability ?? "Full Time";

                if ((bool)viewHRDetailsViewModel.IsPayPerCredit)
                {
                    viewHRDetailsViewModel.PayPerCreditModel = new();
                    viewHRDetailsViewModel.PayPerCreditModel.EngagementType = "";

                    Sp_UTS_GetCreditUtilization_BasedOnHR_Result obj = new();
                    obj = _iHiringRequest.GetCreditUtilization_BasedOnHR(HRId);
                    if (obj != null)
                    {
                        viewHRDetailsViewModel.PayPerCreditModel.EngagementType = obj.CreditUtilization ?? "";
                    }

                    viewHRDetailsViewModel.PayPerCreditModel.JobType = commonOperation.PayPerCredit_JobType(SalesHiringRequest.IsHiringLimited ?? false, SalesHiringRequestDetail.SpecificMonth ?? 0, Availability);
                    viewHRDetailsViewModel.PayPerCreditModel.BudgetTitle = "Salary Budget";
                    viewHRDetailsViewModel.PayPerCreditModel.BudgetText = !string.IsNullOrEmpty(SalesHiringRequestDetail.Cost) ? SalesHiringRequestDetail.Cost : string.Empty;
                    viewHRDetailsViewModel.PayPerCreditModel.IsVettedProfile = SalesHiringRequest.IsVettedProfile;
                }
                #endregion

                #region HiringRequestSkillDetailList

                var SalesHiringRequestSkillDetailList = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(yz => yz.HiringRequestId == HRId && yz.Proficiency == "Strong").ToList();
                if (SalesHiringRequestSkillDetailList != null)
                {
                    viewHRDetailsViewModel.requiredSkillList = new List<SkillListVM>();
                    viewHRDetailsViewModel.interviewerlList = new List<InterviewerlList>();
                    foreach (var HiringRequestSkills in SalesHiringRequestSkillDetailList)
                    {
                        if (HiringRequestSkills.SkillId != null && HiringRequestSkills.SkillId != 0)
                        {
                            var SkillDetails = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Id == HiringRequestSkills.SkillId).ToList();
                            if (SkillDetails != null)
                            {
                                foreach (var skills in SkillDetails)
                                {
                                    viewHRDetailsViewModel.requiredSkillList.Add(new SkillListVM { Text = skills.Skill, IsSelected = Convert.ToBoolean(skills.IsActive), Proficiency = "Strong", ID = skills.Id.ToString(), TempSkill_ID = "" });
                                }
                            }
                        }
                        // show the other added skills in the list when displaying HR.
                        else
                        {
                            if (HiringRequestSkills.TempSkillId != null)
                            {
                                var tempSkillDetails = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkillId == HiringRequestSkills.TempSkillId).ToList();
                                if (tempSkillDetails != null)
                                {
                                    foreach (var d in tempSkillDetails)
                                    {
                                        viewHRDetailsViewModel.requiredSkillList.Add(new SkillListVM { ID = d.Id.ToString(), TempSkill_ID = d.TempSkillId.ToString(), IsSelected = true, Text = d.TempSkill, Proficiency = "Strong" });
                                    }
                                }
                            }
                        }
                    }
                }

                //UTS-4752: Fetch the good to have skills.
                var SalesHiringRequestAllSkillDetailList = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(yz => yz.HiringRequestId == HRId && yz.Proficiency == "Basic").ToList();
                if (SalesHiringRequestAllSkillDetailList != null)
                {
                    viewHRDetailsViewModel.GoodToHaveSkillList = new List<SkillListVM>();

                    viewHRDetailsViewModel.interviewerlList = new List<InterviewerlList>();
                    foreach (var HiringRequestSkills in SalesHiringRequestAllSkillDetailList)
                    {
                        if (HiringRequestSkills.SkillId != null && HiringRequestSkills.SkillId != 0)
                        {
                            var SkillDetails = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Id == HiringRequestSkills.SkillId).ToList();
                            if (SkillDetails != null)
                            {
                                foreach (var skills in SkillDetails)
                                {
                                    viewHRDetailsViewModel.GoodToHaveSkillList.Add(new SkillListVM { Text = skills.Skill, IsSelected = Convert.ToBoolean(skills.IsActive), Proficiency = "Basic", ID = skills.Id.ToString(), TempSkill_ID = "" });
                                }
                            }
                        }
                        // show the other added skills in the list when displaying HR.
                        else
                        {
                            if (HiringRequestSkills.TempSkillId != null)
                            {
                                var tempSkillDetails = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkillId == HiringRequestSkills.TempSkillId).ToList();
                                if (tempSkillDetails != null)
                                {
                                    foreach (var d in tempSkillDetails)
                                    {
                                        viewHRDetailsViewModel.GoodToHaveSkillList.Add(new SkillListVM { ID = d.Id.ToString(), TempSkill_ID = d.TempSkillId.ToString(), IsSelected = true, Text = d.TempSkill, Proficiency = "Basic" });
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                #region interviewerlList

                if (SalesHiringRequestInterviewerList.Count > 0)
                {
                    foreach (var HiringRequestInterviwerDetailList in SalesHiringRequestInterviewerList)
                    {
                        viewHRDetailsViewModel.interviewerlList.Add(new InterviewerlList { interviewerFullName = HiringRequestInterviwerDetailList.InterviewerName, interviewerLinkedin = HiringRequestInterviwerDetailList.InterviewLinkedin, interviewerEmail = HiringRequestInterviwerDetailList.InterviewerEmailId, interviewerDesignation = HiringRequestInterviwerDetailList.InterviewerDesignation });
                    }
                }
                else
                {
                    GenContact? contactDetails = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == SalesHiringRequest.ContactId);
                    if (contactDetails != null)
                    {
                        viewHRDetailsViewModel.interviewerlList.Add(new InterviewerlList { interviewerFullName = contactDetails.FullName, interviewerLinkedin = contactDetails.LinkedIn, interviewerEmail = contactDetails.EmailId, interviewerDesignation = contactDetails.Designation });
                    }
                }
                #endregion

                #region hr status
                if (SalesHiringRequest.StatusId != null && SalesHiringRequest.StatusId > 0)
                {
                    var prgHiringRequestStatus = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == SalesHiringRequest.StatusId).FirstOrDefault();
                    if (prgHiringRequestStatus != null)
                    {
                        viewHRDetailsViewModel.hrStatus = prgHiringRequestStatus.HiringRequestStatus;
                    }
                }
                #endregion

                //Fetch the GUID.
                viewHRDetailsViewModel.Guid = SalesHiringRequest.Guid;

                //UTS-5426: Get the Additinal details
                viewHRDetailsViewModel.AdditionalDetails = _iHiringRequest.sp_UTS_GetStandOutDetails(HRId.ToString());
            }
            return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "View HR Details", viewHRDetailsViewModel));
        }

        [HttpPost("CloneHR")]
        public async Task<ObjectResult> CloneHR(CloneHRViewModel cloneHRViewModel)
        {
            if (cloneHRViewModel != null)
            {
                long LoggedInUserID = SessionValues.LoginUserId;
                object[] param = null;
                sproc_CloneHRFromExistHR_Result obj_sproc_CloneHRFromExistHR_Result = new();

                param = new object[]
                {
                    cloneHRViewModel.hrid, LoggedInUserID, 0,
                    null, null, null, null, null, null
                };
                var findUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserID).FirstOrDefault();
                var findHrId = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == cloneHRViewModel.hrid).FirstOrDefault();

                if (cloneHRViewModel.companyId > 0)
                {
                    bool IsHybrid = false;
                    var findCompany = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == cloneHRViewModel.companyId).FirstOrDefault();

                    if (findCompany != null && findCompany.CompanyTypeId > 0 && findCompany.AnotherCompanyTypeId > 0)
                    {
                        IsHybrid = true;
                    }

                    if (IsHybrid)
                    {
                        bool IsPayPerHire = false;
                        if (cloneHRViewModel.hybridModel == null)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "hybrid model object is not proper"));
                        }
                        if (cloneHRViewModel.hybridModel.PayPerType == 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please select any on of the model, Pay per Hire / Pay per credit"));
                        }
                        if (cloneHRViewModel.hybridModel.PayPerType == 1)
                        {
                            IsPayPerHire = true;
                        }
                        param = new object[]
                        {
                            cloneHRViewModel.hrid, LoggedInUserID, 0,
                            IsHybrid, IsPayPerHire,
                            cloneHRViewModel.hybridModel.IsTransparentPricing ?? null,
                            cloneHRViewModel.hybridModel.IsPostaJob ?? null,
                            cloneHRViewModel.hybridModel.IsProfileView ?? null,
                            cloneHRViewModel.hybridModel.IsVettedProfile ?? null
                        };
                    }
                }

                if (findUser != null && findHrId != null)
                {
                    obj_sproc_CloneHRFromExistHR_Result = await _commonInterface.hiringRequest.sproc_CloneHRFromExistHR(CommonLogic.ConvertToParamString(param));

                    if (obj_sproc_CloneHRFromExistHR_Result != null)
                    {
                        try
                        {
                            ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);

                            #region HR Status updates to ATS 

                            //New Status Change
                            object[] ats_param = new object[] { obj_sproc_CloneHRFromExistHR_Result.CloneHRID, 0, 0, LoggedInUserID, (short)AppActionDoneBy.UTS, false };
                            string strParam = CommonLogic.ConvertToParamString(param);
                            var HRStatus_Json = _commonInterface.hiringRequest.GetUpdateHrStatus(strParam);
                            if (HRStatus_Json != null)
                            {
                                var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                if (!string.IsNullOrEmpty(JsonData))
                                {
                                    aTSCall.SendHrStatusToATS(JsonData, LoggedInUserID, obj_sproc_CloneHRFromExistHR_Result.CloneHRID);
                                }
                            }

                            #endregion
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "HR Cloned Successfully.", obj_sproc_CloneHRFromExistHR_Result.CloneHRID));
                        }
                        return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "HR Cloned Successfully.", obj_sproc_CloneHRFromExistHR_Result.CloneHRID));
                    }
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Invalid Request"));
        }

        [HttpGet("CloseHR_WarningMsg")]
        public async Task<ObjectResult> CloseHRWarning(long HR_ID)
        {
            try
            {
                if (HR_ID == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HiringRequestID" });

                sproc_UTS_HRClose_CheckTalentOfferHiredwithMessage_Result warningObj = _commonInterface.hiringRequest.GetWarningMsg(HR_ID);

                if (warningObj != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = warningObj });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No MSG Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("CloseHR")]
        public async Task<ObjectResult> CloseHR(DeleteHR deleteHR)
        {
            long loggedinuserid = SessionValues.LoginUserId;
            #region Pre Validation

            if (deleteHR == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convanation", Details = deleteHR });

            if (deleteHR.Id == 0)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide HiringRequestID" });

            if (string.IsNullOrEmpty(deleteHR.Remark))
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Remarks" });

            if (deleteHR.ReasonId == 0)
                deleteHR.ReasonId = 2;

            #endregion            

            try
            {
                //Check if HR is already closed.
                var salesHiringRequest = await _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == deleteHR.Id).FirstOrDefaultAsync().ConfigureAwait(false);

                if (salesHiringRequest != null)
                {
                    //validate that HR is in not closed.
                    //if (salesHiringRequest.StatusId != 3 && salesHiringRequest.StatusId != 4 && salesHiringRequest.StatusId != 6)
                    if (salesHiringRequest.JobStatusId != (short)prg_JobStatus_ClientPortal.Closed)
                    {
                        object[] param = new object[] { deleteHR.Id, deleteHR.Remark, deleteHR.ReasonId, loggedinuserid, (int)AppActionDoneBy.UTS };
                        string paramasString = CommonLogic.ConvertToParamString(param);

                        Sproc_Update_Status_For_Closed_HR_Result closeResult = _commonInterface.hiringRequest.sproc_Update_Status_For_Clsoed_HR(paramasString);

                        if (closeResult != null)
                        {
                            //UTS-3182: Send Email to the internal team when HR is closed/cancelled/loss.
                            EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                            binder.SendEmailForHRStatusUpdateToInternalTeam(deleteHR.Id, deleteHR.Remark, closeResult.Message, true);

                            //UTS-3208: Send close HR update to ATS.
                            #region ATS Call
                            if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                            {
                                var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(deleteHR.Id).ConfigureAwait(false);
                                string? HRJsonData = Convert.ToString(HRData_Json);
                                if (!string.IsNullOrEmpty(HRJsonData))
                                {
                                    bool isAPIResponseSuccess = true;

                                    try
                                    {
                                        ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                        isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRJsonData, deleteHR.Id);


                                        #region HR Status updates to ATS 

                                        //New Status Change
                                        object[] atsParam = new object[] { deleteHR.Id, 0, 0, loggedinuserid, (short)AppActionDoneBy.UTS, false };
                                        string strParam = CommonLogic.ConvertToParamString(atsParam);
                                        var HRStatus_Json = _iHiringRequest.GetUpdateHrStatus(strParam);
                                        if (HRStatus_Json != null)
                                        {
                                            var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                            //string JsonData = Convert.ToString(HRStatus_Json);
                                            if (!string.IsNullOrEmpty(JsonData))
                                            {
                                                aTSCall.SendHrStatusToATS(JsonData, loggedinuserid, deleteHR.Id);
                                            }
                                        }

                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error in ATS API." });
                                    }
                                }
                                //if (deleteHR.Id != 0 && deleteHR.Id > 0)
                                //{
                                //    ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new ContactTalentPriorityResponseModel();
                                //    long HRID = Convert.ToInt64(deleteHR.Id);
                                //    var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HRID).Select(x => x.TalentId).ToList();
                                //    contactTalentPriorityResponseModel.HRID = HRID;
                                //    GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HRID).FirstOrDefault();
                                //    _talentConnectAdminDBContext.Entry(_ObjSalesHiringRequest).Reload();

                                //    if (_ObjSalesHiringRequest != null && talentList != null && talentList.Count != 0)
                                //    {
                                //        contactTalentPriorityResponseModel.HRStatusID = _ObjSalesHiringRequest.StatusId ?? 0;
                                //        string HiringRequestStatus = "";
                                //        var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == contactTalentPriorityResponseModel.HRStatusID).FirstOrDefault();
                                //        if (HRStatusData != null)
                                //            HiringRequestStatus = HRStatusData.HiringRequestStatus;

                                //        contactTalentPriorityResponseModel.HRStatus = HiringRequestStatus;

                                //        if (talentList.Count != 0 && talentList != null)
                                //        {
                                //            foreach (var talentID in talentList)
                                //            {
                                //                GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();
                                //                _talentConnectAdminDBContext.Entry(_Talent).Reload();
                                //                outTalentDetail talentDetail = new outTalentDetail();
                                //                talentDetail.ATS_TalentID = Convert.ToInt64(_Talent.AtsTalentId);
                                //                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == _Talent.Id && x.HiringRequestId == _ObjSalesHiringRequest.Id).FirstOrDefault();
                                //                _talentConnectAdminDBContext.Entry(TalentCTP_Details).Reload();
                                //                if (TalentCTP_Details != null)
                                //                {
                                //                    var TalStatusClientSelectionDetail = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                //                    _talentConnectAdminDBContext.Entry(TalStatusClientSelectionDetail).Reload();
                                //                    if (TalStatusClientSelectionDetail != null)
                                //                        talentDetail.TalentStatus = TalStatusClientSelectionDetail.TalentStatus;
                                //                    else
                                //                        talentDetail.TalentStatus = "";
                                //                }
                                //                else
                                //                    talentDetail.TalentStatus = "";
                                //                talentDetail.UTS_TalentID = Convert.ToInt64(_Talent.Id);
                                //                talentDetail.Talent_USDCost = _Talent.FinalCost ?? 0;
                                //                contactTalentPriorityResponseModel.TalentDetails.Add(talentDetail);
                                //            }
                                //        }
                                //        try
                                //        {
                                //            var json = JsonConvert.SerializeObject(contactTalentPriorityResponseModel);

                                //            ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                                //            aTSCall.SendMatchMakingRecords(json, loggedinuserid, HRID);

                                //        }
                                //        catch (Exception)
                                //        {
                                //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error in ATS API." });
                                //        }
                                //    }
                                //}
                            }
                            #endregion
                        }
                        return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "Close HR", closeResult));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR is already closed" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Invalid Hiring Request" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpGet("HrDetailsForShortlisted")]
        public ObjectResult HrDetailsForShortlisted(long hrid)
        {
            if (hrid == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "please enter hrId." });
            }

            List<Sproc_Get_ShortListedDetails_ForCreditFlow_ClientPortal_Result> dataModel = _commonInterface.hiringRequest.Get_ShortListedDetails_ForCreditFlow_ClientPortals(hrid);

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = dataModel });
        }

        [HttpGet("ReopenHR")]
        public async Task<ObjectResult> ReopenHR(long hrId, decimal updatedTR)
        {
            long loggedinuserid = SessionValues.LoginUserId;
            #region Pre-Validation 
            if (hrId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide HRId"));
            }
            #endregion      

            try
            {
                var salesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrId).FirstOrDefault();
                long? ContactID = 0;
                long? CompanyID = 0;
                int hrStatus = 0;
                decimal originalTR = 0;

                if (salesHiringRequest != null)
                {
                    hrStatus = Convert.ToInt32(salesHiringRequest.StatusId);

                    //UTS-4214: If HR is partime then Original TR will be calculated as per the logic.
                    if (salesHiringRequest.Availability == "Part Time")
                    {
                        originalTR = Decimal.Divide(Convert.ToInt32(salesHiringRequest.NoofTalents), 2);
                    }
                    else
                    {
                        originalTR = Convert.ToInt32(salesHiringRequest.NoofTalents);
                    }

                    //if (hrStatus == 3 || hrStatus == 4 || hrStatus == 6)
                    if (salesHiringRequest.JobStatusId == (short)prg_JobStatus_ClientPortal.Closed)
                    {
                        #region Validation
                        // If HR is completed and updated TR is not supplied then throw error.
                        if (hrStatus == 3 && updatedTR == 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide the updated TR count"));
                        }

                        if (hrStatus == 3 && updatedTR > 0 && updatedTR <= originalTR)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "TR cannot be same or reduced"));
                        }

                        // If new/updated TR is less than equal to original TR, throw error.
                        if ((hrStatus == 4 || hrStatus == 6) && updatedTR > 0 && updatedTR < originalTR)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "TR cannot be reduced"));
                        }
                        #endregion

                        Sproc_Update_Status_For_Reopen_HR_Result reopenResult = null;
                        Sproc_RepostedJob_ClientPortal_Result reopenResult_PayPerCredit = null;

                        //if Pay Per hire HR
                        if (salesHiringRequest.HrtypeId == (short)PayPerHire.SalesHR)
                        {
                            object[] param = new object[] { hrId, updatedTR, loggedinuserid, (int)AppActionDoneBy.UTS };
                            string paramasString = CommonLogic.ConvertToParamString(param);

                            reopenResult = _commonInterface.hiringRequest.sproc_Update_Status_For_Reopen_HR(paramasString);
                        }
                        else
                        {
                            ContactID = salesHiringRequest.ContactId;

                            GenContact contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactID).FirstOrDefault();
                            if (contact != null)
                            {
                                CompanyID = contact.CompanyId;
                            }

                            object[] param = new object[] { 0, hrId, ContactID, ContactID, CompanyID, false, 0, true, loggedinuserid };
                            string paramasString = CommonLogic.ConvertToParamString(param);

                            reopenResult_PayPerCredit = _commonInterface.hiringRequest.Sproc_RepostedJob_ClientPortal(paramasString);
                            reopenResult = new();
                            if (reopenResult_PayPerCredit != null)
                            {
                                reopenResult.Status = reopenResult_PayPerCredit.Status;
                                reopenResult.IsReopen = reopenResult_PayPerCredit.IsReopen;
                                reopenResult.Message = reopenResult_PayPerCredit.Message;
                            }
                        }

                        if (reopenResult != null && reopenResult.Status == 0)
                        {
                            if (Convert.ToBoolean(reopenResult.IsReopen))
                            {
                                //Send Email to the internal team when HR is reopened.
                                EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                                bool sendUpdatedTREmail = false;
                                binder.SendEmailForHRReopeningToInternalTeam(hrId, originalTR, hrStatus, ref sendUpdatedTREmail);

                                //send updated TR email.
                                if (sendUpdatedTREmail)
                                {
                                    binder.SendEmailForHRReopeningToInternalTeam(hrId, originalTR, hrStatus, ref sendUpdatedTREmail);
                                }
                            }

                            //Send Re-open HR update to ATS.
                            #region ATS Call
                            if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                            {
                                var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(hrId).ConfigureAwait(false);
                                string? HRJsonData = Convert.ToString(HRData_Json);
                                if (!string.IsNullOrEmpty(HRJsonData))
                                {
                                    bool isAPIResponseSuccess = true;

                                    try
                                    {
                                        ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                        isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRJsonData, hrId);

                                        #region HR Status updates to ATS 

                                        //New Status Change
                                        object[] atsParam = new object[] { hrId, 0, 0, loggedinuserid, (short)AppActionDoneBy.UTS, false };
                                        string strParam = CommonLogic.ConvertToParamString(atsParam);
                                        var HRStatus_Json = _iHiringRequest.GetUpdateHrStatus(strParam);
                                        if (HRStatus_Json != null)
                                        {
                                            var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                            //string JsonData = Convert.ToString(HRStatus_Json);
                                            if (!string.IsNullOrEmpty(JsonData))
                                            {
                                                aTSCall.SendHrStatusToATS(JsonData, loggedinuserid, hrId);
                                            }
                                        }

                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error in ATS API." });
                                    }
                                }

                                //if (hrId != 0 && hrId > 0)
                                //{
                                //    ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new ContactTalentPriorityResponseModel();
                                //    long HRID = Convert.ToInt64(hrId);
                                //    var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HRID).Select(x => x.TalentId).ToList();
                                //    contactTalentPriorityResponseModel.HRID = HRID;
                                //    GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HRID).FirstOrDefault();
                                //    _talentConnectAdminDBContext.Entry(_ObjSalesHiringRequest).Reload();

                                //    if (_ObjSalesHiringRequest != null && talentList != null && talentList.Count != 0)
                                //    {
                                //        contactTalentPriorityResponseModel.HRStatusID = _ObjSalesHiringRequest.StatusId ?? 0;
                                //        string HiringRequestStatus = "";
                                //        var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == contactTalentPriorityResponseModel.HRStatusID).FirstOrDefault();
                                //        if (HRStatusData != null)
                                //            HiringRequestStatus = HRStatusData.HiringRequestStatus;

                                //        contactTalentPriorityResponseModel.HRStatus = HiringRequestStatus;

                                //        if (talentList.Count != 0 && talentList != null)
                                //        {
                                //            foreach (var talentID in talentList)
                                //            {
                                //                GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();
                                //                _talentConnectAdminDBContext.Entry(_Talent).Reload();
                                //                outTalentDetail talentDetail = new outTalentDetail();
                                //                talentDetail.ATS_TalentID = Convert.ToInt64(_Talent.AtsTalentId);
                                //                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == _Talent.Id && x.HiringRequestId == _ObjSalesHiringRequest.Id).FirstOrDefault();
                                //                _talentConnectAdminDBContext.Entry(TalentCTP_Details).Reload();
                                //                if (TalentCTP_Details != null)
                                //                {
                                //                    var TalStatusClientSelectionDetail = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                //                    _talentConnectAdminDBContext.Entry(TalStatusClientSelectionDetail).Reload();
                                //                    if (TalStatusClientSelectionDetail != null)
                                //                        talentDetail.TalentStatus = TalStatusClientSelectionDetail.TalentStatus;
                                //                    else
                                //                        talentDetail.TalentStatus = "";

                                //                }
                                //                else
                                //                    talentDetail.TalentStatus = "";
                                //                talentDetail.UTS_TalentID = Convert.ToInt64(_Talent.Id);
                                //                talentDetail.Talent_USDCost = _Talent.FinalCost ?? 0;
                                //                contactTalentPriorityResponseModel.TalentDetails.Add(talentDetail);

                                //            }
                                //        }
                                //        try
                                //        {
                                //            var json = JsonConvert.SerializeObject(contactTalentPriorityResponseModel);

                                //            ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                                //            aTSCall.SendMatchMakingRecords(json, loggedinuserid, HRID);

                                //        }
                                //        catch (Exception)
                                //        {
                                //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error in ATS API." });
                                //        }
                                //    }
                                //}
                            }
                            #endregion
                        }

                        return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "Reopen HR", reopenResult));
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "HR Cannot be re-opened as it is already in process" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Invalid Hiring Request" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpPost("SyncHRUtsToAts")]
        public async Task<ObjectResult> SyncHRUtsToAts(long HRid)
        {
            try
            {
                long LoginUserId = SessionValues.LoginUserId;

                UsrUser varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(LoginUserId);


                if (HRid > 0)
                {
                    var varHRdata = _talentConnectAdminDBContext.GenSalesHiringRequests.AsNoTracking().Where(t => t.Id == HRid).FirstOrDefault();
                    if (varHRdata != null)
                    {
                        //SendHRDetailsToPMS api call
                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, _configuration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(HRid);

                            #region HR Status updates to ATS 

                            //New Status Change
                            object[] param = new object[] { HRid, 0, 0, LoginUserId, (short)AppActionDoneBy.UTS, false };
                            string strParam = CommonLogic.ConvertToParamString(param);
                            var HRStatus_Json = _iHiringRequest.GetUpdateHrStatus(strParam);
                            if (HRStatus_Json != null)
                            {
                                //string JsonData = Convert.ToString(HRStatus_Json);
                                var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                if (!string.IsNullOrEmpty(JsonData))
                                {
                                    ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                    aTSCall.SendHrStatusToATS(JsonData, LoginUserId, HRid);
                                }
                            }

                            #endregion
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                        }

                        var varTalentList = _talentConnectAdminDBContext.GenContactTalentPriorities.AsNoTracking().Where(x => x.HiringRequestId == HRid).ToList();

                        if (varTalentList != null && varTalentList.Count() > 0)
                        {
                            ContactTalentPriorityModel objCTPModel = new();
                            objCTPModel.HRStatusID = varHRdata.StatusId ?? 0;
                            string HiringRequestStatus = "";
                            var varHRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.AsNoTracking().Where(x => x.Id == objCTPModel.HRStatusID).FirstOrDefault();
                            if (varHRStatusData != null)
                                HiringRequestStatus = varHRStatusData.HiringRequestStatus;

                            objCTPModel.HRStatus = HiringRequestStatus;
                            objCTPModel.HRID = HRid;
                            //CallAtsAPIToSendTalentAndHRStatus api call
                            objCTPModel.TalentDetails = new();
                            foreach (var talentID in varTalentList)
                            {
                                var varGenTalent = _talentConnectAdminDBContext.GenTalents.AsNoTracking().Where(x => x.Id == talentID.TalentId).FirstOrDefault();

                                TalentDetail talentDetail = new();
                                talentDetail.TalentStatus = "";

                                talentDetail.ATS_TalentID = Convert.ToInt64(varGenTalent.AtsTalentId);
                                var TalentCTP_Details = varTalentList.Where(x => x.TalentId == varGenTalent.Id && x.HiringRequestId == HRid).FirstOrDefault();
                                if (TalentCTP_Details != null)
                                {
                                    bool IsPayPerHire = false;
                                    bool IsPayPerCredit = false;

                                    if (varHRdata.HrtypeId == (short)PayPerHire.SalesHR)
                                    {
                                        IsPayPerHire = true;
                                    }
                                    if (varHRdata.HrtypeId == (short)PayPerCredit.PostaJobViewBasedCredit || varHRdata.HrtypeId == (short)PayPerCredit.PostaJobCreditBased || varHRdata.HrtypeId == (short)PayPerCredit.PostajobWithViewCreditsButnoJobPostCredits)
                                    {
                                        IsPayPerCredit = true;
                                    }

                                    if (IsPayPerHire)
                                    {
                                        if (TalentCTP_Details != null && TalentCTP_Details.TalentStatusIdBasedOnHr > 0)
                                        {
                                            var TalentStatusMasterDetails = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                            _talentConnectAdminDBContext.Entry(TalentStatusMasterDetails).Reload();
                                            if (TalentStatusMasterDetails != null)
                                            {
                                                talentDetail.TalentStatus = TalentStatusMasterDetails.TalentStatus ?? "";
                                            }
                                        }
                                    }

                                    if (IsPayPerCredit)
                                    {
                                        if (TalentCTP_Details != null && TalentCTP_Details.TalentStatusIdClientPortal > 0)
                                        {
                                            var TalentStatusMasterDetails = _talentConnectAdminDBContext.PrgTalentStatusClientPortals.Where(x => x.Id == TalentCTP_Details.TalentStatusIdClientPortal).FirstOrDefault();
                                            _talentConnectAdminDBContext.Entry(TalentStatusMasterDetails).Reload();
                                            if (TalentStatusMasterDetails != null)
                                            {
                                                talentDetail.TalentStatus = TalentStatusMasterDetails.TalentStatus ?? "";
                                            }
                                        }
                                    }
                                }
                                talentDetail.UTS_TalentID = varGenTalent.Id;
                                talentDetail.Talent_USDCost = varGenTalent.FinalCost ?? 0;

                                object[] objParam = new object[] { HRid, varGenTalent.Id };
                                string strParamas = CommonLogic.ConvertToParamString(objParam);

                                var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas);

                                string? actualReason = string.Empty;
                                string? rejectionComments = string.Empty;

                                if (varTalent_RejectReason != null)
                                {
                                    actualReason = Convert.ToString(varTalent_RejectReason.ActualReason);
                                    rejectionComments = Convert.ToString(varTalent_RejectReason.RejectionComments);
                                }

                                talentDetail.Talent_RejectReason = actualReason;
                                talentDetail.RejectionComments = rejectionComments;

                                try
                                {
                                    if (varUsrUserById != null)
                                    {
                                        talentDetail.RejectedBy = varUsrUserById.EmployeeId;
                                        talentDetail.ActionUserName = varUsrUserById.FullName;
                                        talentDetail.ActionUserEmail = varUsrUserById.EmailId;
                                        talentDetail.ActionBy = Convert.ToString(StatusChangeAction.Sales);
                                    }
                                }
                                catch
                                {

                                }

                                // UTS-7093: Fetch the round details and send to ATS.
                                try
                                {
                                    // UTS-7093: Fetch the round details and send to ATS.
                                    object[] param = new object[] { HRid, 0, varGenTalent.Id };
                                    string paramString = CommonLogic.ConvertToParamString(param);

                                    ATSCall aTSCallforRound = new ATSCall(_configuration, _talentConnectAdminDBContext);

                                    sproc_Get_InterviewRoundDetails_Result roundDetails = aTSCallforRound.sproc_Get_InterviewRoundDetails(paramString);
                                    if (roundDetails != null)
                                    {
                                        string? talentStatusRoundDetails = roundDetails.StrInterviewRound;
                                        if (!string.IsNullOrEmpty(talentStatusRoundDetails))
                                        {
                                            talentDetail.TalentStatusRoundDetails = talentStatusRoundDetails;
                                        }
                                    }
                                }
                                catch
                                {

                                }

                                objCTPModel.TalentDetails.Add(talentDetail);
                            }
                            try
                            {
                                var json = JsonConvert.SerializeObject(objCTPModel);
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                aTSCall.SaveContactTalentPriority(json, LoginUserId, HRid);
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                            }
                        }

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully" });
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Id not found" });

                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "bad request" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetLoginHrInfo")]
        public ObjectResult GetLoginHrInfo()
        {
            try
            {
                LoginUserHRInfo_Add hiringInfo = new LoginUserHRInfo_Add();
                long LoggedInUserTypeId = SessionValues.LoginUserTypeId;

                if (LoggedInUserTypeId == 11 || LoggedInUserTypeId == 12)
                {
                    hiringInfo.isDirectHR = true;
                }
                else
                {
                    hiringInfo.isDirectHR = false;
                }

                if (hiringInfo.isDirectHR)
                {
                    hiringInfo.disabledFields = new DisabledFields();
                    hiringInfo.defaultProperties = new DefaultProperties();
                    hiringInfo.removeFields = new RemoveFields();

                    var defaultSalesUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.EmployeeId == "DC0001").FirstOrDefault();//Darshan Modi as sales user
                    if (defaultSalesUser != null)
                    {
                        hiringInfo.defaultProperties.salesPerson = defaultSalesUser.Id;
                    }
                }

                if (hiringInfo != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully", Details = hiringInfo });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "User is in valid" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("CheckClientEmail")]
        public ObjectResult CheckClientEmail(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var contactFound = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.EmailId.ToLower() == email.ToLower() || x.Username.ToLower() == email.ToLower());

                    if (contactFound != null)
                    {
                        var company = _talentConnectAdminDBContext.GenCompanies.FirstOrDefault(x => x.Id == contactFound.CompanyId);
                        if (company != null)
                        {
                            dynamic result = new ExpandoObject();
                            result.email = contactFound.EmailId;
                            result.name = company.Company;
                            result.contactid = contactFound.Id;
                            result.salesuserid = 0;
                            result.salesusername = "";
                            result.isHRExists = false;
                            result.isTransparentPricing = null;
                            result.hiringTypePricingId = 0;
                            result.AboutCompanyDesc = company.AboutCompanyDesc;

                            #region CompanyTypes

                            var companyType = _talentConnectAdminDBContext.PrgCompanyTypes.ToList();

                            List<CompanyType> companyTypes = new();
                            result.CompanyTypes = null;

                            foreach (var item in companyType)
                            {
                                CompanyType type = new();
                                type.ID = item.Id;
                                type.Name = item.CompanyType;

                                if (company.CompanyTypeId == null && company.AnotherCompanyTypeId == null && item.Id == 1)//default pay per hire
                                {
                                    type.IsActive = true;
                                }
                                else if (company.CompanyTypeId == item.Id || company.AnotherCompanyTypeId == item.Id)
                                {
                                    type.IsActive = true;
                                }
                                else
                                {
                                    type.IsActive = false;
                                }
                                companyTypes.Add(type);
                            }

                            if (companyTypes.Any())
                            {
                                result.CompanyTypes = companyTypes;
                            }

                            #endregion

                            result.CheckCreditAvailablilty = null;
                            Sproc_UTS_CheckCreditAvailablilty_Result creditObj = _iHiringRequest.CheckCreditAvailablilty(company.Id);
                            if (creditObj != null)
                            {
                                result.CheckCreditAvailablilty = creditObj;
                            }

                            var MavlersEmail = _configuration["MavlersClientEmail"].ToString();
                            var MavlersSalesUserID = Convert.ToInt64(_configuration["MavlersSalesUserID"].ToString());

                            if (contactFound.EmailId.ToLower().Trim() == MavlersEmail.ToLower().Trim())
                            {
                                var UserDetails = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.Id == MavlersSalesUserID);
                                if (UserDetails != null)
                                {
                                    result.salesuserid = UserDetails.Id;
                                    result.salesusername = UserDetails.FullName;
                                }
                                else
                                {
                                    long LoggedInUserTypeId = SessionValues.LoginUserTypeId;

                                    object[] param = new object[]
                                    {
                                      contactFound.EmailId,
                                      LoggedInUserTypeId,
                                      company.Id
                                     };
                                    string paramasString = CommonLogic.ConvertToParamString(param);
                                    List<sproc_UTS_GetAutoCompleteContacts_Result> searchData = _commonInterface.hiringRequest.sproc_UTS_GetAutoCompleteContacts_Result(paramasString);
                                    if (searchData != null && searchData.Any())
                                    {
                                        result.salesuserid = searchData[0].SalesUserID;
                                        result.salesusername = searchData[0].SalesUser;
                                    }
                                }
                            }
                            else
                            {
                                long LoggedInUserTypeId = SessionValues.LoginUserTypeId;

                                object[] param = new object[]
                                {
                                      contactFound.EmailId,
                                      LoggedInUserTypeId,
                                      company.Id
                                 };
                                string paramasString = CommonLogic.ConvertToParamString(param);
                                List<sproc_UTS_GetAutoCompleteContacts_Result> searchData = _commonInterface.hiringRequest.sproc_UTS_GetAutoCompleteContacts_Result(paramasString);
                                if (searchData != null && searchData.Any())
                                {
                                    result.salesuserid = searchData[0].SalesUserID;
                                    result.salesusername = searchData[0].SalesUser;
                                }
                            }

                            result.isTransparentPricing = company.IsTransparentPricing;
                            var IsHRExistsForContact = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.ContactId == contactFound.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (IsHRExistsForContact != null)
                            {
                                result.isHRExists = true;
                                result.hiringTypePricingId = IsHRExistsForContact.HiringTypePricingId;
                            }
                            if (company.HiringTypePricingId > 0)
                            {
                                result.isHRExists = true;
                                result.hiringTypePricingId = company.HiringTypePricingId;
                            }

                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Email is Exists", Details = result });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No company found" });
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Client found" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide email" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("SearchHiringRequestDetail")]
        public ObjectResult SearchHiringRequestDetail(SearchHiringRequestViewModel searchHiringRequestViewModel)
        {
            long LoggedInUserId = SessionValues.LoginUserId;
            HiringRequest hiringrequest = new HiringRequest();
            var addHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == searchHiringRequestViewModel.HiringRequestId).FirstOrDefault();
            if (addHiringRequest != null)
            {
                hiringrequest.addHiringRequest = addHiringRequest;
                var SalesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == hiringrequest.addHiringRequest.Id).FirstOrDefault();
                hiringrequest.SalesHiringRequest_Details = SalesHiringRequest_Details;

                hiringrequest.OnboardId = Convert.ToInt64(0);

                int pageIndex = searchHiringRequestViewModel.page;
                int totalRecords = 0;
                long? Id = 0;
                string Name = string.Empty;
                string EmailID = searchHiringRequestViewModel.EmailId;
                if (hiringrequest.addHiringRequest != null && hiringrequest.addHiringRequest.Id > 0)
                {
                    object[] param = new object[]
                    {
                       pageIndex, 0, "Name", "asc", "", "", "", "", "", "", "", LoggedInUserId, "", Name, EmailID, searchHiringRequestViewModel.HiringRequestId
                    };

                    var talentList = _commonInterface.hiringRequest.sproc_UTS_ViewTalent(CommonLogic.ConvertToParamString(param)).ToList().Select( //0
                        a => new
                        {
                            a.ID,
                            a.Name,
                            a.TalentCost,
                            a.TalentRole,
                            a.EmailID,
                            a.TalentStatus,
                            a.TotalRecords,
                            a.FrontStatusID,
                            a.VersantScore,
                            a.TechScore,
                            a.CurrentCTC //UTS-3904: Bind the current CTC.
                        });

                    if (talentList.Count() > 0)
                    {
                        totalRecords = talentList.ToList().FirstOrDefault().TotalRecords.Value;
                    }

                    var totalPages = (int)Math.Ceiling((float)totalRecords / (float)searchHiringRequestViewModel.rows);

                    sproc_UTS_getCompanyNameByHiringRequestID sproc_UTS_GetCompanyNameByHiringRequestID = _commonInterface.hiringRequest.sproc_UTS_getCompanyNameByHiringRequestID(Convert.ToString(searchHiringRequestViewModel.HiringRequestId));

                    //UTS-3904: Send the respective NR/DP percentage while match-making
                    string headerDPNRPercentage = string.Empty;
                    decimal? valueDPNRPercentage = 0;
                    if (hiringrequest.addHiringRequest.IsHrtypeDp)
                    {
                        valueDPNRPercentage = hiringrequest.addHiringRequest.Dppercentage;
                        headerDPNRPercentage = $"DP {valueDPNRPercentage} %";
                    }
                    else
                    {
                        valueDPNRPercentage = hiringrequest.addHiringRequest.TalentCostCalcPercentage;
                        headerDPNRPercentage = $"NR {valueDPNRPercentage} %";
                    }

                    dynamic result = new ExpandoObject();
                    result.totalPages = totalPages;
                    result.currentPage = searchHiringRequestViewModel.page;
                    result.records = totalRecords;
                    result.HRNumber = hiringrequest.addHiringRequest.HrNumber;
                    if (sproc_UTS_GetCompanyNameByHiringRequestID != null)
                        result.CompanyName = sproc_UTS_GetCompanyNameByHiringRequestID.Company;
                    else
                        result.CompanyName = "Not found";
                    result.rows = talentList;
                    result.DPNRPercentage = valueDPNRPercentage;
                    result.headerDPNRPercentage = headerDPNRPercentage;
                    result.IsHrtypeDp = hiringrequest.addHiringRequest.IsHrtypeDp;
                    result.HRCurrency = hiringrequest?.SalesHiringRequest_Details?.Currency;

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Hiring request found", Details = result });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Hiring request not found" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Hiring request not found" });
            }
        }

        [HttpPost("SetTalentPriorties")]
        public ObjectResult SetTalentPriorties(SelectTalentModel selectTalentModel, long LoggedInUserId = 0)
        {
            try
            {
                LoggedInUserId = SessionValues.LoginUserId;

                var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);
                #region PreValidation

                if (selectTalentModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check your datatype or parameters name" });

                #endregion

                #region Validation

                ListOfTalentsValidators validationRules = new ListOfTalentsValidators();
                if (selectTalentModel.listOfTalents.Count > 0)
                {
                    foreach (var each in selectTalentModel.listOfTalents)
                    {
                        ValidationResult validationResult = validationRules.Validate(each);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "listOfTalents") });
                        }
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide list of talents" });
                }

                if (string.IsNullOrEmpty(selectTalentModel.HRId.ToString()) || selectTalentModel.HRId == 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Hiring request id" });

                #endregion

                #region CheckHRData

                GenSalesHiringRequest hiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == selectTalentModel.HRId);
                GenSalesHiringRequestDetail hiringRequestdetils = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.HiringRequestId == selectTalentModel.HRId);

                if (hiringRequest == null || hiringRequestdetils == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Hiring request not found" });

                GenContact contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == hiringRequest.ContactId);
                if (contact == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Contact Found associated with Hiring Request" });

                GenContactTalentPriority genTalentBinded = new();
                long ContactTalentPriorityID = 0;
                #endregion

                #region WorkOnTalents

                foreach (var eachObject in selectTalentModel.listOfTalents)
                {
                    GenTalent genTalent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == eachObject.talentId);
                    _talentConnectAdminDBContext.Entry(genTalent).Reload();
                    if (genTalent != null)
                    {
                        #region UpdateTalent Status ID

                        genTalent.TalentStatusIdAfterClientSelection = 1;
                        genTalent.Id = eachObject.talentId;
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, genTalent, EntityState.Modified);

                        #endregion

                        #region Save the Talents data
                        var ContactTalentPriorityData = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == selectTalentModel.HRId && x.TalentId == eachObject.talentId).FirstOrDefault();
                        if (ContactTalentPriorityData == null)
                        {
                            genTalentBinded = ModelBinder.BindContactTalentPriority(hiringRequest, hiringRequestdetils, eachObject, Convert.ToInt32(LoggedInUserId));
                            if (genTalentBinded != null)
                            {
                                object[] param = new object[]
                                {
                                    genTalentBinded.ContactId ?? 0,
                                    genTalentBinded.HiringRequestId,
                                    genTalent.Id,
                                    genTalentBinded.TalentPriority,
                                    genTalentBinded.RoleId ?? 0,
                                    genTalentBinded.HiringRequestDetailId,
                                    genTalentBinded.TalentCost ?? 0,
                                    hiringRequest.IsAdHocHr,
                                    LoggedInUserId,
                                    Convert.ToDecimal(genTalentBinded.CurrentCtc ?? genTalent.CurrentCtc),
                                    genTalentBinded.DpPercentage ?? 0,
                                    genTalentBinded.Nrpercentage ?? 0,
                                    false,
                                    hiringRequestdetils.Currency //UTS-4016: Send the dynamic currency.
                                };

                                var ContactTalentPriorityDetails = _talentConnectAdminDBContext.Set<sproc_UTS_InsertContactTalentPriority>()
                                                            .FromSqlRaw($"{Constants.ProcConstant.sproc_TalentClientAssociation_Insert} " + $"{CommonLogic.ConvertToParamString(param)}").ToList().FirstOrDefault();
                                if (ContactTalentPriorityDetails != null)
                                {
                                    ContactTalentPriorityID = ContactTalentPriorityDetails.Id;
                                }
                            }
                        }
                        #endregion

                        #region Insert into gen_ShortlistedTalents table.
                        var ShortListedData = _talentConnectAdminDBContext.GenShortlistedTalents.Where(x => x.HiringRequestId == hiringRequest.Id && x.TalentId == eachObject.talentId).FirstOrDefault();
                        if (ShortListedData == null)
                        {
                            GenShortlistedTalent genShortlistedTalentBinded = ModelBinder.BindGenShortlistedTalent(hiringRequest, hiringRequestdetils, selectTalentModel, eachObject, Convert.ToInt32(LoggedInUserId), genTalentBinded);
                            if (genShortlistedTalentBinded != null)
                            {
                                _talentConnectAdminDBContext.GenShortlistedTalents.Add(genShortlistedTalentBinded);
                                _talentConnectAdminDBContext.SaveChanges();
                            }
                        }
                        #endregion

                        #region SendEmail
                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);

                        if (genTalent != null)
                        {
                            //var NotifyToTalent = genTalent.IsTalentNotificationSend ?? false;
                            //var NotifyToTalent = false;
                            //if (NotifyToTalent)
                            //{
                            //    //Email to Talent.
                            //    emailBinder.SendEmailNotificationForAssociateclient(genTalent.Id, selectTalentModel.HRId);
                            //}

                            if (hiringRequest.IsHrtypeDp == true)
                            {
                                //var IsClientNotificationSend = contact.IsClientNotificationSend;

                                //if (IsClientNotificationSend)
                                //{
                                //    //Email to Client for DP HR
                                //    emailBinder.SendEmailtoClientDirectPlacement(hiringRequest.ContactId, selectTalentModel.HRId, genTalent.Id);
                                //}

                                //Email to Sales Team for DP HR
                                emailBinder.SendEmailtoSalesDirectPlacement(genTalent.Id, selectTalentModel.HRId);
                            }
                            else
                            {
                                //Email To Sales Team for Contractual HR
                                emailBinder.SendEmailNotificationToSalesTeam(genTalent.Id, selectTalentModel.HRId);
                            }
                        }
                        #endregion


                        #region History Table data insert

                        object[] param2 = new object[]
                        {
                            Action_Of_History.Talent_Matchmacking, selectTalentModel.HRId, eachObject.talentId, false, LoggedInUserId, ContactTalentPriorityID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);

                        #endregion

                        if (ContactTalentPriorityData == null)
                        {
                            #region ATS Call
                            if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                            {
                                ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, _configuration, _httpContextAccessor.HttpContext);

                                object[] objParam = new object[] { hiringRequest.Id, genTalent.Id };
                                string paramasString = CommonLogic.ConvertToParamString(objParam);

                                var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(paramasString).ActualReason;

                                _talentConnectAdminDBContext.Entry(hiringRequest).Reload();
                                _talentConnectAdminDBContext.Entry(genTalent).Reload();
                                _talentConnectAdminDBContext.Entry(genTalentBinded).Reload();

                                commonAPI.MakeMatchMakingAPICallToSendToATS(hiringRequest.Id, hiringRequest.StatusId, genTalent.Id, genTalent.AtsTalentId, genTalent.FinalCost, genTalentBinded.CreatedByDatetime, varTalent_RejectReason, varUsrUserById.EmployeeId);
                            }
                            #endregion
                        }
                    }
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Talent's priority has been saved !" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Delete")]
        public ObjectResult DeleteHiringRequest(DeleteHR deleteHR, long Loggedinuserid = 0)
        {
            Loggedinuserid = SessionValues.LoginUserId;
            var varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(SessionValues.LoginUserId);
            #region Pre Validation

            if (deleteHR == null)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convanation", Details = deleteHR });

            #endregion

            #region Validation

            DeleteHRValidator validationRules = new DeleteHRValidator();
            ValidationResult validationResult = validationRules.Validate(deleteHR);
            if (!validationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "deleteHR") });
            }

            #endregion

            string OnHoldRemark = string.Empty;
            string OnLossRemark = string.Empty;
            if (deleteHR.DeleteType == (short)DeleteHRType.OnHold)
                OnHoldRemark = deleteHR.Remark;
            else
                OnLossRemark = deleteHR.Remark;

            #region OnBoardWork

            if (deleteHR.OnBoardId > 0)
            {
                object[] param1 = new object[]
                {
                    deleteHR.OnBoardId
                };
                sproc_GetOnBoardData_Result sproc_GetOnBoardData_Result = _commonInterface.hiringRequest.sproc_GetOnBoardData_Result(CommonLogic.ConvertToParamString(param1));
                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                emailBinder.SendEmailOnDeleteHRAfterOnBoard(deleteHR.OnBoardId, deleteHR.Reason, sproc_GetOnBoardData_Result);

                object[] param = new object[]
                {
                    deleteHR.Id,deleteHR.Reason,deleteHR.ReasonId,deleteHR.DeleteType,OnHoldRemark,OnLossRemark,Loggedinuserid
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_Delete_HR_AfterOnBoard, param);
                object[] param2 = new object[]
                        {
                            Action_Of_History.Delete_HR_AfterOnBoard, deleteHR.Id, 0, false, Loggedinuserid, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);
            }
            else
            {
                object[] param = new object[] { deleteHR.Id, deleteHR.Reason, deleteHR.ReasonId, deleteHR.DeleteType, OnHoldRemark, OnLossRemark, Loggedinuserid };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_DeleteSalesHiringRequestAndDetails, param);
            }

            #endregion

            if (deleteHR.DeleteType == (short)DeleteHRType.OnHold)
            {
                object[] param = new object[] { Action_Of_History.HR_OnHold, deleteHR.Id, 0, false, Loggedinuserid, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
            }
            else
            {
                ContactTalentPriorityModel objCTPModel = new();
                object[] param = new object[]
                       {
                            Action_Of_History.Cancel_HR, deleteHR.Id, 0, false, Loggedinuserid, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                       };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);

                //#region ATS call

                //if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                //{
                //    var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == deleteHR.Id).Select(x => x.TalentId).ToList();
                //    objCTPModel.HRID = deleteHR.Id;

                //    string TalentStatus = string.Empty;
                //    int TalentStatusID = (short)prg_TalentStatus_AfterClientSelection.Cancelled;

                //    if (TalentStatusID > 0)
                //    {
                //        TalentStatus = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentStatusID && x.IsActive == true).FirstOrDefault()?.TalentStatus;
                //    }

                //    var hiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == deleteHR.Id);
                //    if (hiringRequestData != null)
                //    {
                //        objCTPModel.HRStatusID = hiringRequestData.StatusId ?? 0;
                //        string HiringRequestStatus = "";
                //        var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.FirstOrDefault(x => x.Id == objCTPModel.HRStatusID);
                //        if (HRStatusData != null)
                //            HiringRequestStatus = HRStatusData.HiringRequestStatus;

                //        objCTPModel.HRStatus = HiringRequestStatus;

                //        if (talentList.Count != 0 && talentList != null)
                //        {
                //            foreach (var talentID in talentList)
                //            {
                //                GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == talentID);
                //                _talentConnectAdminDBContext.Entry(_Talent).Reload();

                //                TalentDetail talentDetail = new();
                //                talentDetail.ATS_TalentID = Convert.ToInt64(_Talent.AtsTalentId);
                //                talentDetail.TalentStatus = TalentStatus;
                //                talentDetail.UTS_TalentID = Convert.ToInt64(_Talent.Id);
                //                talentDetail.Talent_USDCost = _Talent.FinalCost ?? 0;

                //                object[] objParam = new object[] { objCTPModel.HRID, Convert.ToInt64(_Talent.Id) };
                //                string strParamas = CommonLogic.ConvertToParamString(objParam);
                //                var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;

                //                talentDetail.Talent_RejectReason = varTalent_RejectReason;
                //                talentDetail.RejectedBy = varUsrUserById.EmployeeId;
                //                objCTPModel.TalentDetails.Add(talentDetail);
                //            }
                //        }
                //        try
                //        {
                //            var json = JsonConvert.SerializeObject(objCTPModel);
                //            //Call ATS API
                //            ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                //            aTSCall.SaveContactTalentPriority(json, Loggedinuserid, deleteHR.Id);
                //        }
                //        catch (Exception)
                //        {
                //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data save successfully" });
                //        }
                //    }
                //}

                //if (deleteHR.DeleteType == (short)DeleteHRType.Loss)
                //{
                //    if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                //    {
                //        List<outTalentDetailATS> talentCancelledResponsesList = new List<outTalentDetailATS>();
                //        outTalentDetailATS talentCancelledResponseModel = new outTalentDetailATS();

                //        #region Add call for find and send status to ATS for HR used at other place : Added by Divya

                //        var hiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == deleteHR.Id).FirstOrDefault();
                //        if (hiringRequestData != null)
                //        {
                //            object[] param4 = new object[] { hiringRequestData.Id, 0 };
                //            string paramasString = CommonLogic.ConvertToParamString(param4);
                //            var talentListATS = _commonInterface.hiringRequest.sproc_TalentHR_CancelledHR_List(paramasString);
                //            foreach (var list in talentListATS)
                //            {
                //                talentCancelledResponseModel = new outTalentDetailATS();
                //                talentCancelledResponseModel.HRID = list.HRID;
                //                talentCancelledResponseModel.HRStatusID = list.HRStatusID;

                //                var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == list.HRStatusID).FirstOrDefault();
                //                if (HRStatusData != null)
                //                    talentCancelledResponseModel.HRStatus = HRStatusData.HiringRequestStatus;

                //                object[] objParam = new object[] { list.HRID, list.TalentID };
                //                string strParamas = CommonLogic.ConvertToParamString(objParam);

                //                var varTalent_RejectReason = _commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;


                //                talentCancelledResponseModel.ATS_TalentID = Convert.ToInt64(list.ATS_Talent_ID);
                //                talentCancelledResponseModel.TalentStatus = list.TalentStatus;
                //                talentCancelledResponseModel.UTS_TalentID = list.TalentID;
                //                talentCancelledResponseModel.Talent_USDCost = Convert.ToInt64(list.FinalCost);
                //                talentCancelledResponseModel.Reason = list.Reason;
                //                talentCancelledResponseModel.Talent_RejectReason = varTalent_RejectReason;
                //                talentCancelledResponseModel.RejectedBy = varUsrUserById.EmployeeId;
                //                talentCancelledResponsesList.Add(talentCancelledResponseModel);
                //            }

                //            if (talentCancelledResponsesList.Count > 0)
                //            {
                //                try
                //                {
                //                    var jsonList = JsonConvert.SerializeObject(talentCancelledResponsesList);
                //                    ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                //                    aTSCall.SendAutoCancelledTalentHRStatusToATS(jsonList, Loggedinuserid, deleteHR.Id);
                //                }
                //                catch (Exception)
                //                {
                //                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Request has been deleted" });
                //                }
                //            }
                //        }
                //        #endregion
                //    }
                //}

                //#endregion
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Request has been deleted" });
        }

        [HttpPost("UploadDriveFile")]
        public ObjectResult UploadDriveFile(UploadDriveFileModel uploadDriveFileModel)
        {
            try
            {
                #region Pre Validation

                if (uploadDriveFileModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty, please check the datatype or naming convanation", Details = uploadDriveFileModel });

                #endregion

                #region Validation

                UploadDriveFileValidator validationRules = new UploadDriveFileValidator();
                ValidationResult validationResult = validationRules.Validate(uploadDriveFileModel);
                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "UploadDriveFile") });
                }

                #endregion

                //string drivePath = "https://drive.google.com/uc?export=download&id=" + uploadDriveFileModel.FileId;
                string drivePath = string.Empty;

                string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", uploadDriveFileModel.FileName);
                string psTime = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
                long loggedInUserID = SessionValues.LoginUserId;
                string DocfileName = uploadDriveFileModel.FileName;
                string FileExtension = System.IO.Path.GetExtension(filePath);
                string AllJDText = "";

                if (FileExtension.ToLower() == "pdf")
                {
                    drivePath = "https://docs.google.com/document/export?format=pdf&id=" + uploadDriveFileModel.FileId;
                }
                else
                    drivePath = "https://docs.google.com/document/export?format=docx&id=" + uploadDriveFileModel.FileId;


                List<SkillOptionVM> Skills = new List<SkillOptionVM>();
                List<string> responsibilityList = new List<string>();
                List<string> requirementsList = new List<string>();
                List<string> skillList = new List<string>();

                Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);

                GoogleFunction.DownloadGoogleDriveFile(drivePath, filePath);

                if (FileExtension.ToLower() == ".jpg" || FileExtension.ToLower() == ".jpeg" || FileExtension.ToLower() == ".png")
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Image Uploaded", Details = null });
                }

                if (FileExtension.ToLower() == ".pdf")
                {
                    byte[] bindata = System.Text.Encoding.ASCII.GetBytes(filePath);
                    //var content = parser.ExtractPDFText(filePath);

                    var paragraphList = parser.ExtractTextFromPDF(filePath);
                    var pdfText = string.Join(" ", paragraphList);
                    AllJDText = pdfText;

                    if (paragraphList != null && paragraphList.Count > 0)
                    {
                        responsibilityList = parser.ExtractRoleAndResponsbillity(paragraphList);
                        requirementsList = parser.ExtractRequirement(paragraphList);
                        //skillList = GetSkills(pdfText);
                    }
                }
                else
                {
                    string textContent = parser.ExtractTextFromDocument(filePath);

                    AllJDText = textContent;

                    List<string> paragraphList = textContent.Split("\n").ToList();
                    paragraphList = paragraphList.Select(s => s.Trim()).ToList();

                    if (paragraphList != null && paragraphList.Count > 0)
                    {
                        responsibilityList = parser.ExtractRoleAndResponsbillity(paragraphList);
                        requirementsList = parser.ExtractRequirement(paragraphList);
                    }

                    //#region Skill Extract

                    //string content = parser.ExtractTextUsingPython(textContent);

                    //skillList = GetSkills(content);

                    //#endregion
                }

                string SkillListString = string.Empty;
                if (skillList != null && skillList.Count > 0)
                {
                    SkillListString = string.Join("$", skillList);

                    Skills = new List<SkillOptionVM>();

                    if (skillList != null)
                    {
                        foreach (var s in skillList)
                        {
                            var SkillDetails = _talentConnectAdminDBContext.PrgSkills.FirstOrDefault(x => x.Skill.ToLower().Trim() == s.ToLower().Trim());
                            if (SkillDetails != null)
                            {
                                Skills.Add(
                                    new SkillOptionVM
                                    {
                                        ID = SkillDetails.Id.ToString(),
                                        IsSelected = Convert.ToBoolean(SkillDetails.IsActive),
                                        Text = SkillDetails.Skill,
                                        Proficiency = "Basic"
                                    }
                                );
                            }
                        }
                    }
                }

                string responsibilityText = responsibilityList != null && responsibilityList.Count > 0 ? String.Join("$", responsibilityList) : string.Empty;
                string requirementsText = requirementsList != null && requirementsList.Count > 0 ? String.Join("$", requirementsList) : string.Empty;

                if (responsibilityText.Contains("'"))
                    responsibilityText = responsibilityText.Replace("'", "''");
                if (requirementsText.Contains("'"))
                    requirementsText = requirementsText.Replace("'", "''");

                if (AllJDText.Contains("'"))
                    AllJDText = AllJDText.Replace("'", "''");

                //UTS-3474: Save the JD All Skills fields as existing flow.
                string JDAllSkills = SkillListString.Replace("$", ",");

                object[] param = new object[] { loggedInUserID, "", "", "", "", SkillListString, "", "", "", "", "", DocfileName, AllJDText, true, 0, JDAllSkills, false, responsibilityText, requirementsText };
                Sproc_DumpJDDetailsintoTempTable_Result JDDumpObj = _iJDParse.Sproc_DumpJDDetailsintoTempTable(CommonLogic.ConvertToParamString(param));

                string? requirementsContent = requirementsList != null && requirementsList.Count > 0 ? string.Join(" ", requirementsList) : null;
                string? responsibilityContent = responsibilityList != null && responsibilityList.Count > 0 ? string.Join(" ", responsibilityList) : null;

                dynamic objResult = new ExpandoObject();
                objResult.Skills = Skills;
                objResult.FileName = DocfileName;
                objResult.Requirements = requirementsContent;
                objResult.Responsibility = responsibilityContent;
                objResult.JDDumpID = JDDumpObj == null ? 0 : JDDumpObj.Id;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Details = objResult
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("UploadFile")]
        public async Task<ObjectResult> UploadFile([FromForm] IFormFile file, string clientEmail)
        {
            try
            {
                #region Validation
                if (string.IsNullOrEmpty(clientEmail))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please select client Email/Name"));
                }
                if (file == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Required", Details = null });
                }
                else if (file.Length == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "You are uploading corrupt file", Details = null });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                string[] allowedFileExtension = { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".jpg", ".jpeg", ".png" };

                if (!allowedFileExtension.Contains(fileExtension))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Your file format is incorrect.", Details = null });
                }

                var fileSize = (file.Length / 1024) / 1024;
                if (fileSize >= 0.5)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File size must be less than 500 KB", Details = null });
                }

                //UTS-4848: Do not allow user to enter the filename with special characters.
                if (!string.IsNullOrEmpty(file.FileName))
                {
                    char[] specialCharacters = { '#', '/', '?', '&', '%' };
                    if (file.FileName.IndexOfAny(specialCharacters) != -1)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File Name should not contain '#', '/', '?', '&', and '%'", Details = null });
                    }
                }

                #endregion

                long loggedInUserID = SessionValues.LoginUserId;

                long ContactID = 0;

                string pattern = @"[^a-zA-Z0-9_]";
                string FileExtension = System.IO.Path.GetExtension(file.FileName);
                string FileNameWithoutExte = "";
                if (FileExtension == ".pdf")
                    FileNameWithoutExte = file.FileName.Replace(".pdf", "");
                else if (FileExtension == ".doc")
                    FileNameWithoutExte = file.FileName.Replace(".doc", "");
                else if (FileExtension == ".docx")
                    FileNameWithoutExte = file.FileName.Replace(".docx", "");
                else
                    FileNameWithoutExte = "";

                // Replace specified special characters with an empty string
                string cleanName = Regex.Replace(FileNameWithoutExte, pattern, "");
                string DocfileName = cleanName; //file.FileName;
                string FrontOfficeURL = _configuration["FrontOfficeAPIURL"];

                if (FileExtension == ".pdf")
                    DocfileName = cleanName + ".pdf";
                else if (FileExtension == ".doc")
                    DocfileName = cleanName + ".doc";
                else if (FileExtension == ".docx")
                    DocfileName = cleanName + ".docx";

                string filePath = System.IO.Path.Combine(FrontOfficeURL, "JDFiles\\" + DocfileName);

                string AllJDText = "";


                string content = "";

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                List<SkillResponseModel> Skills = new List<SkillResponseModel>();
                List<string> responsibilityList = new List<string>();
                List<string> jdDescriptionList = new List<string>();
                List<string> requirementsList = new List<string>();
                List<string> skillList = new List<string>();
                string Title = "";

                Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);

                if (FileExtension.ToLower() == ".jpg" || FileExtension.ToLower() == ".jpeg" || FileExtension.ToLower() == ".png")
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Image Uploaded", Details = null });
                }

                if (FileExtension.ToLower() == ".pdf")
                {
                    string pythonURLForPDF = _configuration["PDFFileTextParsing"] + "?FileName=" + DocfileName;

                    byte[] bindata = System.Text.Encoding.ASCII.GetBytes(filePath);
                    //var content = parser.ExtractPDFText(filePath);

                    //var paragraphList = parser.ExtractTextFromPDF(filePath);
                    //var pdfText = string.Join(" ", paragraphList);
                    //AllJDText = pdfText;

                    //using (WebResponse wr = WebRequest.Create(pythonURLForPDF).GetResponse())
                    //{
                    //    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    //    {
                    //        content = sr.ReadToEnd();
                    //    }
                    //}

                    using (WebResponse wr = WebRequest.Create(pythonURLForPDF).GetResponse())
                    {
                        // Try to detect the encoding from the response
                        Encoding encoding = DetectEncoding(wr.ContentType);

                        using (Stream stream = wr.GetResponseStream())
                        {
                            // First, read the raw bytes
                            using (MemoryStream ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                byte[] rawData = ms.ToArray();
                                encoding = Encoding.GetEncoding("Windows-1252");  // or whatever the correct encoding is
                                content = encoding.GetString(rawData);
                            }
                        }
                    }


                    AllJDText = content;

                    if (AllJDText != null)  // && paragraphList.Count > 0
                    {
                        dynamic obj = new ExpandoObject();
                        obj.username = clientEmail;
                        obj.IsFromAdmin = true;

                        var json = JsonConvert.SerializeObject(obj);
                        ChatGPTCommonAPI chatGPTCommonAPI = new ChatGPTCommonAPI(_configuration, _talentConnectAdminDBContext);

                        dynamic pdfTextobj = new ExpandoObject();
                        pdfTextobj.PdfText = AllJDText;
                        var fileText = JsonConvert.SerializeObject(pdfTextobj);
                        ExtractTextFromFile extractTextFromFile = new ExtractTextFromFile()
                        {
                            PdfText = AllJDText
                        };
                        GenContact genContact = await _iJDParse.GetGenContact(clientEmail);
                        if (genContact != null)
                        {
                            ContactID = genContact.Id;
                            //JobPostDetailsViewModel result = await ExtraCtJDBasedOnTextClaudAI(extractTextFromFile, ProcessType.Text_Parsing.ToString(), ContactID);
                            JobPostDetailsViewModel result = await ExtraCtJDBasedOnTextchatGPTMini4o(extractTextFromFile, ProcessType.Text_Parsing.ToString(), ContactID);

                            if (result != null)
                            {
                                responsibilityList = result.RolesResponsibilitiesList ?? new();
                                requirementsList = result.RequirementsList ?? new();
                                jdDescriptionList = result.JobDesriptionList ?? new();
                                skillList = string.IsNullOrEmpty(result.Skills) ? new() : result.Skills.Split(',').ToList();
                                Title = result.RoleName;
                            }
                        }
                        //ClientAPIResult result = _
                        //ClientAPIResult result = chatGPTCommonAPI.LoginfromAdminToClient(json, fileText, false, false, true);

                        //if (result != null && result.RRRWithSkillsData != null)
                        //{
                        //    responsibilityList = result.RRRWithSkillsData.RolesResponsibilities ?? new();
                        //    requirementsList = result.RRRWithSkillsData.Requirements ?? new();
                        //    jdDescriptionList = result.RRRWithSkillsData.JobDescription ?? new();
                        //    skillList = string.IsNullOrEmpty(result.RRRWithSkillsData.ChatGptSkills) ? new() : result.RRRWithSkillsData.ChatGptSkills.Split(',').ToList();
                        //}

                        //skillList = GetSkills(pdfText);
                    }


                }
                else
                {
                    string pythonURLForDoc = _configuration["DocFileTextParsing"] + "?FileName=" + DocfileName;
                    //string textContent = parser.ExtractTextFromDocument(filePath);

                    //AllJDText = textContent;

                    //using (WebResponse wr = WebRequest.Create(pythonURLForDoc).GetResponse())
                    //{
                    //    using (StreamReader sr = new StreamReader(wr.GetResponseStream(),Encoding.UTF8))
                    //    {
                    //        content = sr.ReadToEnd();
                    //    }
                    //}

                    using (WebResponse wr = WebRequest.Create(pythonURLForDoc).GetResponse())
                    {
                        // Try to detect the encoding from the response
                        Encoding encoding = DetectEncoding(wr.ContentType);

                        using (Stream stream = wr.GetResponseStream())
                        {
                            // First, read the raw bytes
                            using (MemoryStream ms = new MemoryStream())
                            {
                                stream.CopyTo(ms);
                                byte[] rawData = ms.ToArray();
                                encoding = Encoding.GetEncoding("Windows-1252");  // or whatever the correct encoding is
                                content = encoding.GetString(rawData);
                            }
                        }
                    }





                    AllJDText = content;

                    //List<string> paragraphList = textContent.Split("\n").ToList();
                    //paragraphList = paragraphList.Select(s => s.Trim()).ToList();

                    if (AllJDText != null) //&& paragraphList.Count > 0
                    {
                        dynamic obj = new ExpandoObject();
                        obj.username = clientEmail;
                        obj.IsFromAdmin = true;

                        var json = JsonConvert.SerializeObject(obj);
                        ChatGPTCommonAPI chatGPTCommonAPI = new ChatGPTCommonAPI(_configuration, _talentConnectAdminDBContext);

                        dynamic pdfText = new ExpandoObject();
                        pdfText.PdfText = AllJDText;
                        var fileText = JsonConvert.SerializeObject(pdfText);
                        ExtractTextFromFile extractTextFromFile = new ExtractTextFromFile()
                        {
                            PdfText = AllJDText
                        };
                        GenContact genContact = await _iJDParse.GetGenContact(clientEmail);
                        if (genContact != null)
                        {
                            ContactID = genContact.Id;
                            //JobPostDetailsViewModel result = await ExtraCtJDBasedOnTextClaudAI(extractTextFromFile, ProcessType.Text_Parsing.ToString(), ContactID);
                            JobPostDetailsViewModel result = await ExtraCtJDBasedOnTextchatGPTMini4o(extractTextFromFile, ProcessType.Text_Parsing.ToString(), ContactID);

                            if (result != null)
                            {
                                responsibilityList = result.RolesResponsibilitiesList ?? new();
                                requirementsList = result.RequirementsList ?? new();
                                jdDescriptionList = result.JobDesriptionList ?? new();
                                skillList = string.IsNullOrEmpty(result.Skills) ? new() : result.Skills.Split(',').ToList();
                                Title = result.RoleName;
                            }
                        }

                        //ClientAPIResult result = chatGPTCommonAPI.LoginfromAdminToClient(json, fileText, false, false, true);

                        //if (result != null && result.RRRWithSkillsData != null)
                        //{
                        //    responsibilityList = result.RRRWithSkillsData.RolesResponsibilities ?? new();
                        //    requirementsList = result.RRRWithSkillsData.Requirements ?? new();
                        //    jdDescriptionList = result.RRRWithSkillsData.JobDescription ?? new();
                        //    skillList = string.IsNullOrEmpty(result.RRRWithSkillsData.ChatGptSkills) ? new() : result.RRRWithSkillsData.ChatGptSkills.Split(',').ToList();
                        //}
                    }

                    //#region Skill Extract

                    //string content = parser.ExtractTextUsingPython(textContent);

                    //skillList = GetSkills(content);

                    //#endregion
                }

                string SkillListString = string.Empty;

                if (skillList != null && skillList.Count > 0)
                {
                    SkillListString = string.Join(",", skillList);

                    Skills = new List<SkillResponseModel>();

                    if (skillList != null)
                    {
                        foreach (var s in skillList)
                        {
                            var SkillDetails = _talentConnectAdminDBContext.PrgSkills.FirstOrDefault(x => x.Skill.ToLower().Trim() == s.ToLower().Trim());
                            if (SkillDetails != null)
                            {
                                Skills.Add(
                                    new SkillResponseModel() { Id = SkillDetails.Id.ToString(), Value = SkillDetails.Skill }
                                                                    );
                            }
                            else
                            {
                                var TempSkillDetails = _talentConnectAdminDBContext.PrgTempSkills.FirstOrDefault(x => x.TempSkill.ToLower().Trim() == s.ToLower().Trim());
                                if (TempSkillDetails != null)
                                {
                                    Skills.Add(
                                    new SkillResponseModel() { Id = TempSkillDetails.TempSkillId.ToString(), Value = TempSkillDetails.TempSkill }
                                                                    );
                                }
                                else
                                {
                                    PrgTempSkill tempSkills = new();
                                    tempSkills.TempSkill = s.ToLower().Trim();
                                    tempSkills.CreatedByDatetime = DateTime.Now;
                                    tempSkills.CreatedById = Convert.ToInt32(loggedInUserID);
                                    tempSkills.AddedByTalent = false;
                                    tempSkills.SkillSourceId = 1;
                                    tempSkills.Guid = System.Guid.NewGuid().ToString();

                                    _talentConnectAdminDBContext.PrgTempSkills.Add(tempSkills);
                                    _talentConnectAdminDBContext.SaveChanges();

                                    var TempSkillInsertedID = tempSkills.Id;

                                    _talentConnectAdminDBContext.Entry(tempSkills).Reload();

                                    tempSkills.TempSkillId = "O_" + Convert.ToString(TempSkillInsertedID);
                                    _talentConnectAdminDBContext.Entry(tempSkills).State = EntityState.Modified;

                                    _talentConnectAdminDBContext.SaveChanges();


                                    Skills.Add(
                                    new SkillResponseModel() { Id = tempSkills.TempSkillId, Value = tempSkills.TempSkill }
                                                                    );

                                }
                            }
                        }
                    }
                }

                string responsibilityText = responsibilityList != null && responsibilityList.Count > 0 ? String.Join("$", responsibilityList) : string.Empty;
                string requirementsText = requirementsList != null && requirementsList.Count > 0 ? String.Join("$", requirementsList) : string.Empty;
                string JobDescriptionText = jdDescriptionList != null && jdDescriptionList.Count > 0 ? String.Join("$", jdDescriptionList) : string.Empty;


                if (responsibilityText.Contains("'"))
                    responsibilityText = responsibilityText.Replace("'", "''");
                if (requirementsText.Contains("'"))
                    requirementsText = requirementsText.Replace("'", "''");
                if (JobDescriptionText.Contains("'"))
                    JobDescriptionText = JobDescriptionText.Replace("'", "''");

                if (AllJDText.Contains("'"))
                    AllJDText = AllJDText.Replace("'", "''");
                if (AllJDText.Contains("\r"))
                    AllJDText = AllJDText.Replace("\r", "");
                if (AllJDText.Contains("\n"))
                    AllJDText = AllJDText.Replace("\n", "");

                //UTS-3474: Save the JD All Skills fields as existing flow.
                string JDAllSkills = SkillListString.Replace("$", ",");

                object[] param = new object[] { ContactID, "", "", "", "", SkillListString, "", "", "", "", "", DocfileName, "", true, 0, JDAllSkills, false, responsibilityText, requirementsText };
                Sproc_DumpJDDetailsintoTempTable_Result JDDumpObj = _iJDParse.Sproc_DumpJDDetailsintoTempTable(CommonLogic.ConvertToParamString(param));

                string? requirementsContent = requirementsList != null && requirementsList.Count > 0 ? string.Join(" ", requirementsList) : null;
                string? responsibilityContent = responsibilityList != null && responsibilityList.Count > 0 ? string.Join(" ", responsibilityList) : null;
                string? JobDescriptionContent = jdDescriptionList != null && jdDescriptionList.Count > 0 ? string.Join(" ", jdDescriptionList) : null;

                dynamic objResult = new ExpandoObject();
                objResult.Skills = Skills;
                objResult.FileName = DocfileName;
                objResult.Requirements = requirementsContent;
                objResult.Responsibility = responsibilityContent;
                objResult.JDDumpID = JDDumpObj == null ? 0 : JDDumpObj.Id;
                //objResult.JobDescription = JobDescriptionContent;
                objResult.JobDescription = AllJDText;
                objResult.Title = Title;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Details = objResult
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("UploadGoogleFileLink")]
        [GoogleScopedAuthorize(DriveService.ScopeConstants.DriveReadonly)]
        public async Task<ObjectResult> UploadGoogleFileLink([FromServices] IGoogleAuthProvider auth, string url)
        {
            try
            {
                #region Validation

                if (string.IsNullOrWhiteSpace(url))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Url must be required", Details = null });
                }

                string[] validUrlDomain = { "docs.google.com", "drive.google.com" };
                if (!validUrlDomain.Any(s => url.ToLower().Contains(s.ToLower())))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please enter a valid url", Details = null });
                }

                #endregion

                #region Google File Download Logic

                GoogleCredential credential = await auth.GetCredentialAsync();
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "UTS"
                });

                Regex r = new Regex(@"\/d\/(.+)\/", RegexOptions.IgnoreCase);
                Match m = r.Match(url);

                var fileId = m.ToString().TrimStart('/', 'd').Trim('/');
                var googleFileList = await service.Files.List().ExecuteAsync();
                var googleFile = googleFileList.Files.FirstOrDefault(x => x.Id == fileId);
                string filePath = string.Empty;

                long loggedInUserID = SessionValues.LoginUserId;

                if (googleFile == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "File not found", Details = null });
                }

                string psTime = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
                string DocfileName = googleFile.Name;

                filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", DocfileName);

                string FileExtension = System.IO.Path.GetExtension(filePath);
                if (string.IsNullOrEmpty(FileExtension))
                {
                    FileExtension = ".pdf";
                    filePath = filePath + FileExtension;
                    DocfileName = DocfileName + FileExtension;
                }


                if (url.Contains("docs.google.com"))
                {
                    DownloadDocsFile(service, googleFile, filePath);
                }
                else if (url.Contains("drive.google.com"))
                {
                    await DownloadDriveFile(service, googleFile, filePath);
                }

                if (filePath == string.Empty || !System.IO.File.Exists(filePath))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Link File not download", Details = null });
                }

                #endregion

                List<SkillOptionVM> Skills = new List<SkillOptionVM>();
                List<string> responsibilityList = new List<string>();
                List<string> requirementsList = new List<string>();
                List<string> skillList = new List<string>();
                string AllJDText = "";

                Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);


                if (FileExtension.ToLower() == ".jpg" || FileExtension.ToLower() == ".jpeg" || FileExtension.ToLower() == ".png")
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Image Uploaded", Details = null });
                }

                if (FileExtension.ToLower() == ".pdf")
                {
                    byte[] bindata = System.Text.Encoding.ASCII.GetBytes(filePath);
                    //var content = parser.ExtractPDFText(filePath);

                    var paragraphList = parser.ExtractTextFromPDF(filePath);
                    var pdfText = string.Join(" ", paragraphList);

                    AllJDText = pdfText;

                    if (paragraphList != null && paragraphList.Count > 0)
                    {
                        responsibilityList = parser.ExtractRoleAndResponsbillity(paragraphList);
                        requirementsList = parser.ExtractRequirement(paragraphList);
                        //skillList = GetSkills(pdfText);
                    }

                }
                else
                {
                    string textContent = parser.ExtractTextFromDocument(filePath);

                    AllJDText = textContent;

                    List<string> paragraphList = textContent.Split("\n").ToList();
                    paragraphList = paragraphList.Select(s => s.Trim()).ToList();

                    if (paragraphList != null && paragraphList.Count > 0)
                    {
                        responsibilityList = parser.ExtractRoleAndResponsbillity(paragraphList);
                        requirementsList = parser.ExtractRequirement(paragraphList);
                    }

                    //#region Skill Extract

                    //string content = parser.ExtractTextUsingPython(textContent);

                    //skillList = GetSkills(content);

                    //#endregion
                }

                string SkillListString = string.Empty;
                if (skillList != null && skillList.Count > 0)
                {
                    SkillListString = string.Join("$", skillList);
                    Skills = new List<SkillOptionVM>();

                    if (skillList != null)
                    {
                        foreach (var s in skillList)
                        {
                            var SkillDetails = _talentConnectAdminDBContext.PrgSkills.FirstOrDefault(x => x.Skill.ToLower().Trim() == s.ToLower().Trim());
                            if (SkillDetails != null)
                            {
                                Skills.Add(
                                    new SkillOptionVM
                                    {
                                        ID = SkillDetails.Id.ToString(),
                                        IsSelected = Convert.ToBoolean(SkillDetails.IsActive),
                                        Text = SkillDetails.Skill,
                                        Proficiency = "Basic"
                                    }
                                );
                            }
                        }
                    }
                }

                string responsibilityText = responsibilityList != null && responsibilityList.Count > 0 ? String.Join("$", responsibilityList) : string.Empty;
                string requirementsText = requirementsList != null && requirementsList.Count > 0 ? String.Join("$", requirementsList) : string.Empty;

                if (responsibilityText.Contains("'"))
                    responsibilityText = responsibilityText.Replace("'", "''");
                if (requirementsText.Contains("'"))
                    requirementsText = requirementsText.Replace("'", "''");
                if (AllJDText.Contains("'"))
                    AllJDText = AllJDText.Replace("'", "''");

                //UTS-3474: Save the JD All Skills fields as existing flow.
                string JDAllSkills = SkillListString.Replace("$", ",");

                object[] param = new object[] { loggedInUserID, "", "", "", "", SkillListString, "", "", "", "", "", DocfileName, AllJDText, true, 0, JDAllSkills, false, responsibilityText, requirementsText };
                Sproc_DumpJDDetailsintoTempTable_Result JDDumpObj = _iJDParse.Sproc_DumpJDDetailsintoTempTable(CommonLogic.ConvertToParamString(param));

                string? requirementsContent = requirementsList != null && requirementsList.Count > 0 ? string.Join(" ", requirementsList) : null;
                string? responsibilityContent = responsibilityList != null && responsibilityList.Count > 0 ? string.Join(" ", responsibilityList) : null;

                dynamic objResult = new ExpandoObject();
                objResult.Skills = Skills;
                objResult.FileName = DocfileName;
                objResult.Requirements = requirementsContent;
                objResult.Responsibility = responsibilityContent;
                objResult.JDDumpID = JDDumpObj == null ? 0 : JDDumpObj.Id;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Details = objResult
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("UploadLinkForParsing")]
        public async Task<ObjectResult> UploadLinkForParsing(string url)
        {
            try
            {
                dynamic objResult = new ExpandoObject();

                long LoggedInUserID = SessionValues.LoginUserId;
                string LinkParsingURL = _configuration["LinkParsingURL"];
                string content;
                if (string.IsNullOrWhiteSpace(LinkParsingURL) || string.IsNullOrWhiteSpace(url))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Enter URL",
                        Details = ""
                    });
                }
                else
                {

                    //Regex r = new Regex(@"\/d\/(.+)\/", RegexOptions.IgnoreCase);
                    //Match m = r.Match(url);

                    //string fileId = m.ToString().TrimStart('/', 'd').Trim('/');
                    string LinkParsingFileIDPath = System.IO.Path.Combine("Media/JDParsing/JDFiles", "LinkParsingFileID.txt");
                    if (System.IO.File.Exists(LinkParsingFileIDPath))
                        System.IO.File.Delete(LinkParsingFileIDPath);

                    System.IO.File.WriteAllText(LinkParsingFileIDPath, url);

                    using (WebResponse wr = WebRequest.Create(LinkParsingURL).GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            content = await sr.ReadToEndAsync();
                            content = content.Replace("\r\n", "");
                        }
                    }

                    if (content.EndsWith(".pdf"))
                    {
                        objResult = GetSkillsAndRolesAndRR(content, LoggedInUserID);

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Success",
                            Details = objResult
                        });

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                        {
                            statusCode = StatusCodes.Status404NotFound,
                            Message = "Enter URL",
                            Details = ""
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("CheckSalesUserIsPartner")]
        public async Task<ObjectResult> CheckSalesUserIsPartner(long salesPersonId, long ContactID)
        {
            try
            {
                if (salesPersonId == 0 || ContactID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "salesPersonId or ContactID are not correct" });
                }

                dynamic dObject = new ExpandoObject();

                dObject.SaleUserIsPartner = false;

                UsrUser user = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == salesPersonId && x.IsPartnerUser == true).FirstOrDefault();
                if (user != null)
                {
                    List<sproc_UTS_GetChildCompanyList_Result> ChildCompanyList = new List<sproc_UTS_GetChildCompanyList_Result>();

                    ChildCompanyList = _commonInterface.hiringRequest.sproc_UTS_GetChildCompanyList(ContactID.ToString());
                    dObject.ChildCompanyList = ChildCompanyList;

                    dObject.SaleUserIsPartner = true;

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                }
                else
                {
                    dObject.ChildCompanyList = null;
                    dObject.SaleUserIsPartner = false;
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("HRAccept")]
        public async Task<ObjectResult> HRAccept([FromBody] AcceptHRViewModel model)
        {
            var LoggedInUserId = SessionValues.LoginUserId;
            string EmailMsg = "";

            var HiringRequestData = await _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefaultAsync(x => x.Id == model.HRID).ConfigureAwait(false);
            if (HiringRequestData == null)
                return StatusCode(StatusCodes.Status404NotFound, CommonLogic.ReturnObject(StatusCodes.Status404NotFound, "Hiring Request Not Found."));

            if (HiringRequestData.IsActive == true)
            {
                HiringRequestData.IsAccepted = model.AcceptValue;
                HiringRequestData.AcceptedById = LoggedInUserId;
                HiringRequestData.AcceptedByDateTime = DateTime.Now;
                HiringRequestData.NotAcceptedHrreason = (model.AcceptValue == 2) ? model.Reason : null;

                CommonLogic.DBOperator(_talentConnectAdminDBContext, HiringRequestData, EntityState.Modified);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Hiring Request is not Active."));
            }

            if (model.AcceptValue != 2)
            {
                HiringRequestData.TrAccepted = HiringRequestData.NoofTalents;
                HiringRequestData.LastModifiedById = Convert.ToInt32(LoggedInUserId);
                HiringRequestData.LastModifiedDatetime = DateTime.Now;

                CommonLogic.DBOperator(_talentConnectAdminDBContext, HiringRequestData, EntityState.Modified);

                GenSalesHrTracceptedDetail tracceptedDetail = new()
                {
                    HiringRequestId = model.HRID,
                    TrAccepted = HiringRequestData.NoofTalents,
                    CreatedById = Convert.ToInt32(LoggedInUserId),
                    CreatedByDateTime = DateTime.Now
                };

                await _talentConnectAdminDBContext.GenSalesHrTracceptedDetails.AddAsync(tracceptedDetail).ConfigureAwait(false);
                await _talentConnectAdminDBContext.SaveChangesAsync().ConfigureAwait(false);
            }

            if (model.AcceptValue == 1)
            {
                object[] param = new object[]
                        {
                            Action_Of_History.HR_Acceptance, model.HRID, 0, false, LoggedInUserId, 0, 0, HiringRequestData.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);


                param = new object[]
                        {
                            Action_Of_History.TR_Accepted, model.HRID, 0, false, LoggedInUserId, 0, 0, HiringRequestData.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
            }
            //else if (model.AcceptValue == 2)
            //{
            //    object[] param = new object[]
            //            {
            //                Action_Of_History.WaitFor_More_Info, model.HRID, 0, false, LoggedInUserId, 0, 0, HiringRequestData.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
            //            };
            //    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
            //}

            EmailBinder emailBinder = new(_configuration, _talentConnectAdminDBContext);
            EmailMsg = emailBinder.SendEmailForHRAcceptanceToInternalTeam(model.HRID, model.AcceptValue, model.Reason);

            if (model.AcceptValue == 1)
            {
                string EmailForTRAcceptance = "";
                EmailForTRAcceptance = emailBinder.SendEmailForTRAcceptanceToInternalTeam(webHostEnvironment.WebRootPath, model.HRID, HiringRequestData.NoofTalents ?? 0, 0);
            }

            if (EmailMsg.ToLower().Trim() == "success")
            {
                #region ATS Call to Send data
                if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(model.HRID).ConfigureAwait(false);
                    string HRJsonData = Convert.ToString(HRData_Json);

                    if (!string.IsNullOrEmpty(HRJsonData))
                    {
                        bool isAPIResponseSuccess = true;

                        ATSCall aTSCall = new(_configuration, _talentConnectAdminDBContext);
                        if (HRJsonData != "")
                            isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), model.HRID);
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "HR Accepted successfully"));
            }
            else if (EmailMsg.ToLower().Trim() == "error")
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, "Email Error"));
            else
                return StatusCode(StatusCodes.Status404NotFound, CommonLogic.ReturnObject(StatusCodes.Status404NotFound, EmailMsg));
        }

        [HttpPost("GetCountryListByCountryCodeOrPostalcode")]
        public async Task<ObjectResult> GetCountryListByCountryCodeOrPostalcode(string? CountryCode, string? postalcode)
        {

            int intCountryCode, intPostalcode;
            if (int.TryParse(CountryCode, out intCountryCode) || int.TryParse(postalcode, out intPostalcode))
            {
                string strCountryCode = null;
                if (!string.IsNullOrEmpty(CountryCode))
                {
                    strCountryCode = _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.Id == Convert.ToInt32(CountryCode)).Select(x => x.CountryRegion).FirstOrDefault();
                }

                List<GenPostalCodeswithDetail> objGenPostalCodeswithDetails = new List<GenPostalCodeswithDetail>();

                if (string.IsNullOrEmpty(strCountryCode) && string.IsNullOrEmpty(postalcode))
                {
                    var GetCountry = _talentConnectAdminDBContext.PrgCountryRegions.Select(x => new MastersResponseModel { Value = x.Country + " (" + x.CountryRegion + ")", Id = x.Id }).ToList();
                    if (GetCountry.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Country List", Details = new { GetCountry = GetCountry, StateCityData = "" } });
                }
                else if (!string.IsNullOrEmpty(postalcode) && string.IsNullOrEmpty(strCountryCode))
                {
                    objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode).ToList();

                    if (!objGenPostalCodeswithDetails.Any())
                    {
                        string result = SaveCountryStateCityPostalCodeData(strCountryCode, Convert.ToInt64(postalcode));
                        if (!string.IsNullOrEmpty(result))
                        {
                            objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode).ToList();
                            if (objGenPostalCodeswithDetails.Any())
                            {
                                if (objGenPostalCodeswithDetails.Count() != 1)
                                {
                                    var GetCountry = (from Postal in _talentConnectAdminDBContext.GenPostalCodeswithDetails
                                                      join C in _talentConnectAdminDBContext.PrgCountryRegions on Postal.CountryCode equals C.CountryRegion
                                                      where Postal.PostalCode == postalcode
                                                      select new
                                                      {
                                                          ID = C.Id,
                                                          Country = C.Country + " (" + C.CountryRegion + ")"
                                                      }).Distinct().ToList();


                                    if (GetCountry.Count() == 1)
                                    {
                                        objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode).ToList();
                                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = objGenPostalCodeswithDetails.FirstOrDefault() } });
                                    }
                                    else
                                    {
                                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = "" } });
                                    }

                                }
                                else
                                {
                                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = objGenPostalCodeswithDetails });
                                }
                            }
                        }
                        else
                        {
                            //postal code not find
                            var GetCountry = _talentConnectAdminDBContext.PrgCountryRegions.Select(x => new MastersResponseModel { Value = x.Country + " (" + x.CountryRegion + ")", Id = x.Id }).ToList();
                            if (GetCountry.Any())

                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Postal Code not found", Details = new { GetCountry = GetCountry, StateCityData = "postal code not find" } });
                        }

                    }
                    else
                    {
                        var GetCountry = (from Postal in _talentConnectAdminDBContext.GenPostalCodeswithDetails
                                          join C in _talentConnectAdminDBContext.PrgCountryRegions on Postal.CountryCode equals C.CountryRegion
                                          where Postal.PostalCode == postalcode
                                          select new
                                          {
                                              ID = C.Id,
                                              Country = C.Country + " (" + C.CountryRegion + ")"
                                          }).Distinct().ToList();
                        if (GetCountry.Count() == 1)
                        {
                            objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode).ToList();
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = objGenPostalCodeswithDetails.FirstOrDefault() } });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = "" } });
                        }

                    }

                }

                else if (!string.IsNullOrEmpty(strCountryCode) && !string.IsNullOrEmpty(postalcode))
                {
                    //int _country = Convert.ToInt32(countryCode);
                    var varCountryRegion = _talentConnectAdminDBContext.PrgCountryRegions.Where(x => x.CountryRegion.ToLower() == strCountryCode.ToLower()).Select(x => x.CountryRegion).FirstOrDefault();

                    objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode && x.CountryCode.ToLower() == varCountryRegion.ToLower()).ToList();
                    if (!objGenPostalCodeswithDetails.Any())
                    {
                        string result = SaveCountryStateCityPostalCodeData(varCountryRegion, Convert.ToInt64(postalcode));
                        if (!string.IsNullOrEmpty(result))
                        {
                            objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode && x.CountryCode.ToLower() == varCountryRegion.ToLower()).ToList();
                            if (objGenPostalCodeswithDetails.Any())
                            {
                                if (objGenPostalCodeswithDetails.Count() != 1)
                                {
                                    var GetCountry = (from Postal in _talentConnectAdminDBContext.GenPostalCodeswithDetails
                                                      join C in _talentConnectAdminDBContext.PrgCountryRegions on Postal.CountryCode equals C.CountryRegion
                                                      where Postal.PostalCode == postalcode && Postal.CountryCode == varCountryRegion
                                                      select new
                                                      {
                                                          ID = C.Id,
                                                          Country = C.Country + " (" + C.CountryRegion + ")"
                                                      }).Distinct().ToList();

                                    if (GetCountry.Count() == 1)
                                    {
                                        objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode).ToList();
                                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = objGenPostalCodeswithDetails.FirstOrDefault() } });
                                    }
                                    else
                                    {
                                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = "" } });
                                    }

                                }
                                else
                                {
                                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = objGenPostalCodeswithDetails });
                                }
                            }
                        }
                        else
                        {
                            //postal code not find
                            var GetCountry = _talentConnectAdminDBContext.PrgCountryRegions.Select(x => new MastersResponseModel { Value = x.Country + " (" + x.CountryRegion + ")", Id = x.Id }).ToList();
                            if (GetCountry.Any())
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Postal Code not found", Details = new { GetCountry = GetCountry, StateCityData = "postal code not find" } });
                        }
                    }
                    else
                    {
                        var GetCountry = (from Postal in _talentConnectAdminDBContext.GenPostalCodeswithDetails
                                          join C in _talentConnectAdminDBContext.PrgCountryRegions on Postal.CountryCode equals C.CountryRegion
                                          where Postal.PostalCode == postalcode && Postal.CountryCode == varCountryRegion
                                          select new
                                          {
                                              ID = C.Id,
                                              Country = C.Country + " (" + C.CountryRegion + ")"
                                          }).Distinct().ToList();
                        if (GetCountry.Count() == 1)
                        {
                            objGenPostalCodeswithDetails = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == postalcode && x.CountryCode.ToLower() == strCountryCode.ToLower()).ToList();
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = objGenPostalCodeswithDetails.FirstOrDefault() } });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = new { GetCountry = GetCountry.Select(x => new MastersResponseModel { Value = x.Country, Id = x.ID }).ToList(), StateCityData = "" } });
                        }
                    }
                }

                else
                {
                    var GetCountry = _talentConnectAdminDBContext.PrgCountryRegions.Select(x => new MastersResponseModel { Value = x.Country + " (" + x.CountryRegion + ")", Id = x.Id }).ToList();
                    if (GetCountry.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Country List", Details = new { GetCountry = GetCountry, StateCityData = "" } });
                }
            }
            else
            {
                var GetCountry = _talentConnectAdminDBContext.PrgCountryRegions.Select(x => new MastersResponseModel { Value = x.Country + " (" + x.CountryRegion + ")", Id = x.Id }).ToList();
                if (GetCountry.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Country List", Details = new { GetCountry = GetCountry, StateCityData = "" } });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Found", Details = null });
        }

        [HttpGet("DeleteInterviewDetails")]
        public async Task<ObjectResult> DeleteInterviewerDetails(long Id, long HRId)
        {
            #region Pre-Validation 
            if (Id == null || Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide interviewDetails Id"));
            }

            if (HRId == null || HRId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide HRId"));
            }
            #endregion

            try
            {
                GenSalesHiringRequestInterviewerDetail genSalesHiringRequestInterviewerDetail = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Where(x => x.Id == Id && x.HiringRequestId == HRId).FirstOrDefault();

                if (genSalesHiringRequestInterviewerDetail != null)
                {
                    _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails.Remove(genSalesHiringRequestInterviewerDetail);
                    _talentConnectAdminDBContext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "Interview Details Deleted successfully"));
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, CommonLogic.ReturnObject(StatusCodes.Status404NotFound, "Interview Details not found"));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, "Error occur please try again after sometime"));
            }
        }

        [HttpGet("CloseHRValidation")]
        public ObjectResult CloseHRValidation(long hrId)
        {
            #region Pre-Validation 
            if (hrId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide HRId"));
            }
            #endregion

            try
            {
                HiringRequest hiringRequest = new HiringRequest();
                hiringRequest.addHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hrId);
                if (hiringRequest.addHiringRequest != null && hiringRequest.addHiringRequest.Id > 0)
                {
                    hiringRequest.CloseEmailName = new List<string>();
                    object[] param = new object[] { hrId };
                    string paramasString = CommonLogic.ConvertToParamString(param);
                    hiringRequest.Close_HR_Result = _commonInterface.hiringRequest.Sproc_Check_Validation_Message_For_Close_HR(paramasString);

                    if (hiringRequest.Close_HR_Result.Count() > 0)
                    {
                        for (int i = 0; i < hiringRequest.Close_HR_Result.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(hiringRequest.Close_HR_Result[i].NameEmail))
                            {
                                hiringRequest.CloseEmailName = hiringRequest.Close_HR_Result[i].NameEmail.Split(',').ToList();
                            }
                        }
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, CommonLogic.ReturnObject(StatusCodes.Status404NotFound, "Hiring request not found"));
                }

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "Close HR Validation", hiringRequest));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, CommonLogic.ReturnObject(StatusCodes.Status500InternalServerError, ex.Message));
            }
        }

        [HttpGet("ExtractTextUsingPython")]
        public async Task<ObjectResult> ExtractTextUsingPython(string clientEmail, string psUrl)
        {
            try
            {
                #region Pre-Validation
                if (string.IsNullOrEmpty(clientEmail) || string.IsNullOrEmpty(psUrl))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please select client Email/Name or psUrl"));
                }
                #endregion
                long ContactID = 0;
                GenContact genContact = await _iJDParse.GetGenContact(clientEmail);
                if (genContact != null)
                {
                    ContactID = genContact.Id;
                    return await ExtractTextUsingclaudAI(psUrl, clientEmail, ProcessType.URL_Parsing.ToString());

                    //var responseJObject = JsonConvert.DeserializeObject<ExtractTextFromURl>(urlResponse.ToString());
                    //var responseJObject = ((Microsoft.AspNetCore.Mvc.ObjectResult)urlResponse).Value.ToString();
                    //return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" ,Details = responseJObject });
                }

                #region Old Code
                //long loggedinuserid = ContactID;//SessionValues.LoginUserId;
                //dynamic obj = new ExpandoObject();
                //obj.username = clientEmail;
                //obj.IsFromAdmin = true;

                //var json = JsonConvert.SerializeObject(obj);
                //ChatGPTCommonAPI chatGPTCommonAPI = new ChatGPTCommonAPI(_configuration, _talentConnectAdminDBContext);

                //ClientAPIResult result = chatGPTCommonAPI.LoginfromAdminToClient(json, psUrl, true);
                //if (result != null && result.UrlParsingData != null)
                //{
                //    HiringRequest HiringRequestViewModel = new HiringRequest();
                //    HiringRequestViewModel.addHiringRequest.RequestForTalent = result.UrlParsingData.details?.roleName;
                //    HiringRequestViewModel.addHiringRequest.NoofTalents = result.UrlParsingData.details?.noOfTalents;
                //    HiringRequestViewModel.addHiringRequest.IsHiringLimited = result.UrlParsingData.details?.isHiringLimited == "Temporary";
                //    HiringRequestViewModel.addHiringRequest.Availability = result.UrlParsingData.details?.employmentType == "PartTime" ? "Part Time" : "Full Time";
                //    HiringRequestViewModel.addHiringRequest.Guid = Guid.NewGuid().ToString();
                //    HiringRequestViewModel.ModeOfWorkingId = Convert.ToString(result.UrlParsingData.details?.workingModeId);
                //    HiringRequestViewModel.ChatGptSkills = result.UrlParsingData.details?.skills;
                //    HiringRequestViewModel.ChatGptAllSkills = result.UrlParsingData.details?.allSkills;

                //    var MustHaveSkills = string.IsNullOrEmpty(result.UrlParsingData.details?.skills) ? new List<string>() : result.UrlParsingData.details?.skills.Split(',').ToList();
                //    var GoodToHaveSkills = string.IsNullOrEmpty(result.UrlParsingData.details?.allSkills) ? new List<string>() : result.UrlParsingData.details?.allSkills.Split(',').ToList(); ;

                //    foreach (var s in MustHaveSkills)
                //    {
                //        string? SkillID = string.Empty;

                //        var skillInMainData = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                //        if (skillInMainData != null)
                //        {
                //            SkillID = skillInMainData.Id.ToString();
                //            HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, IsSelected = true, Text = s, Proficiency = "Strong" });
                //        }

                //        var skillInTempData = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                //        if (skillInTempData != null)
                //        {
                //            SkillID = skillInTempData.TempSkillId;
                //            HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = skillInTempData.Id.ToString(), IsSelected = true, Text = s, Proficiency = "Strong" });
                //        }


                //        // If skills does not exist then insert in the temp table.
                //        if (string.IsNullOrEmpty(SkillID))
                //        {
                //            object[] param = new object[] { s, loggedinuserid, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
                //            var objresult = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
                //            if (objresult != null)
                //            {
                //                SkillID = objresult.TempSkill_ID;
                //                var TempSkill_PrimaryID = _talentConnectAdminDBContext.PrgTempSkills.FirstOrDefault(x => x.TempSkillId == SkillID).Id;

                //                HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = TempSkill_PrimaryID.ToString(), IsSelected = true, Text = s, Proficiency = "Strong" });
                //            }
                //        }
                //    }


                //    foreach (var s in GoodToHaveSkills)
                //    {
                //        string? SkillID = string.Empty;

                //        var skillInMainData = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                //        if (skillInMainData != null)
                //        {
                //            SkillID = skillInMainData.Id.ToString();
                //            HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, IsSelected = true, Text = s, Proficiency = "Basic" });
                //        }

                //        var skillInTempData = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                //        if (skillInTempData != null)
                //        {
                //            SkillID = skillInTempData.TempSkillId;
                //            HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = skillInTempData.Id.ToString(), IsSelected = true, Text = s, Proficiency = "Basic" });
                //        }


                //        // If skills does not exist then insert in the temp table.
                //        if (string.IsNullOrEmpty(SkillID))
                //        {
                //            object[] param = new object[] { s, loggedinuserid, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
                //            var objresult = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
                //            if (objresult != null)
                //            {
                //                SkillID = objresult.TempSkill_ID;
                //                var TempSkill_PrimaryID = _talentConnectAdminDBContext.PrgTempSkills.FirstOrDefault(x => x.TempSkillId == SkillID).Id;

                //                HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = TempSkill_PrimaryID.ToString(), IsSelected = true, Text = s, Proficiency = "Basic" });
                //            }
                //        }
                //    }

                //    HiringRequestViewModel.SalesHiringRequest_Details.YearOfExp = result.UrlParsingData.details?.experienceYears;
                //    HiringRequestViewModel.SalesHiringRequest_Details.SpecificMonth = Convert.ToInt32(result.UrlParsingData.details?.contractDuration);
                //    HiringRequestViewModel.SalesHiringRequest_Details.Currency = result.UrlParsingData.details?.currency;
                //    HiringRequestViewModel.SalesHiringRequest_Details.BudgetFrom = result.UrlParsingData.details?.budgetFrom;
                //    HiringRequestViewModel.SalesHiringRequest_Details.BudgetTo = result.UrlParsingData.details?.budgetTo;
                //    HiringRequestViewModel.SalesHiringRequest_Details.HowSoon = Convert.ToString(result.UrlParsingData.details?.howSoon);
                //    HiringRequestViewModel.SalesHiringRequest_Details.TimezonePreferenceId = result.UrlParsingData.details?.timezonePreferenceId;
                //    HiringRequestViewModel.SalesHiringRequest_Details.TimeZoneFromTime = result.UrlParsingData.details?.timeZoneFromTime;
                //    HiringRequestViewModel.SalesHiringRequest_Details.TimeZoneEndTime = result.UrlParsingData.details?.timeZoneEndTime;
                //    HiringRequestViewModel.SalesHiringRequest_Details.RolesResponsibilities = result.UrlParsingData.details?.rolesResponsibilities;
                //    HiringRequestViewModel.SalesHiringRequest_Details.Requirement = result.UrlParsingData.details?.requirements;
                //    HiringRequestViewModel.SalesHiringRequest_Details.JobDescription = result.UrlParsingData.details?.JobDescription;

                //    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HiringRequestViewModel });
                //}

                //return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "No data found" });

                #endregion

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "No Client Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("ExtractSkillsUsingPython")]
        public ObjectResult ExtractSkillsUsingPython(string clientEmail, string roleName, string experience)
        {
            try
            {
                #region Pre-Validation
                if (string.IsNullOrEmpty(clientEmail) || string.IsNullOrEmpty(roleName) || string.IsNullOrEmpty(experience))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide clientemail or roleName or experience"));
                }
                #endregion

                dynamic obj = new ExpandoObject();
                obj.username = clientEmail;
                obj.IsFromAdmin = true;

                var json = JsonConvert.SerializeObject(obj);
                ChatGPTCommonAPI chatGPTCommonAPI = new ChatGPTCommonAPI(_configuration, _talentConnectAdminDBContext);

                dynamic skillsobj = new ExpandoObject();
                skillsobj.RoleName = roleName;
                skillsobj.YearsofExp = experience;
                var payload = JsonConvert.SerializeObject(skillsobj);

                ClientAPIResult result = chatGPTCommonAPI.LoginfromAdminToClient(json, payload, false, true);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success", Details = result.SkillsResponseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("GetHRCloseActionHistory")]
        public ObjectResult GetHRCloseActionHistory(long hrId)
        {
            try
            {
                #region Pre-Validation
                if (hrId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide hiring request id"));
                }
                #endregion

                var result = _iHiringRequest.Sproc_UTS_GetCloseHRHistory(hrId.ToString());
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success.", Details = result });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data found." });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Method
        private void SaveJDDetailsAndSkills(long ContactID, HiringRequestDebriefModel model, bool isEdit, long HR_ID)
        {
            //string JDSkills = "";

            //var HRModel = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(x => x.Id == HR_ID);
            //var HRDetailModel = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefault(x => x.HiringRequestId == HR_ID);

            //string CompanyName = "";

            //var ContactModel = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == ContactID);
            //var CompanyID = ContactModel.CompanyId;

            //var CompanyModel = _talentConnectAdminDBContext.GenCompanies.FirstOrDefault(x => x.Id == CompanyID);
            //if (CompanyModel != null)
            //    CompanyName = CompanyModel.Company;

            //Hashtable htable = new Hashtable();
            //if (model.skills != null)
            //{
            //    foreach (var sk in model.skills)
            //    {
            //        if (sk.skillsID != null && sk.skillsID != "0")
            //        {
            //            var sId = sk.skillsID.Split('-')[0];
            //            var Sproficiency = "Strong";

            //            if (!htable.ContainsKey(sId))
            //                htable.Add(sId, Sproficiency);

            //            if (sId.StartsWith("O_"))
            //            {
            //                var TempSkillDetail = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkillId == sId).FirstOrDefault();
            //                if (TempSkillDetail != null)
            //                {
            //                    JDSkills = JDSkills + TempSkillDetail.TempSkill + ",";
            //                }
            //            }
            //            else
            //            {
            //                long skillID = Convert.ToInt64(sId);
            //                var skillDetail = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Id == skillID).FirstOrDefault();
            //                if (skillDetail != null)
            //                {
            //                    JDSkills = JDSkills + skillDetail.Skill + ",";
            //                }
            //            }
            //        }
            //    }
            //}

            ////UTS-4752: Add Good to have skills
            //if (model.Allskills != null)
            //{
            //    foreach (var sk in model.Allskills)
            //    {
            //        if (sk.skillsID != null && sk.skillsID != "0")
            //        {
            //            var sId = sk.skillsID.Split('-')[0];
            //            var Sproficiency = "Basic";

            //            if (!htable.ContainsKey(sId))
            //                htable.Add(sId, Sproficiency);

            //            if (sId.StartsWith("O_"))
            //            {
            //                var TempSkillDetail = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkillId == sId).FirstOrDefault();
            //                if (TempSkillDetail != null)
            //                {
            //                    JDSkills = JDSkills + TempSkillDetail.TempSkill + ",";
            //                }
            //            }
            //            else
            //            {
            //                long skillID = Convert.ToInt64(sId);
            //                var skillDetail = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Id == skillID).FirstOrDefault();
            //                if (skillDetail != null)
            //                {
            //                    JDSkills = JDSkills + skillDetail.Skill + ",";
            //                }
            //            }
            //        }
            //    }
            //}

            //if (JDSkills.EndsWith(","))
            //    JDSkills = JDSkills.Remove(JDSkills.Length - 1);

            //string JDAllSkills = JDSkills.Replace("$", ",");
            //string TimeZoneValue = "";
            //var TimeZoneData = _talentConnectAdminDBContext.PrgTalentTimeZones.Where(x => x.Id == HRDetailModel.TimezoneId).FirstOrDefault();
            //if (TimeZoneData != null)
            //    TimeZoneValue = TimeZoneData.TalentTimeZone ?? string.Empty;

            //string RoleValue = "";
            //var RoleData = _talentConnectAdminDBContext.PrgTalentRoles.Where(x => x.Id == HRDetailModel.RoleId).FirstOrDefault();
            //if (RoleData != null)
            //    RoleValue = RoleData.TalentRole ?? string.Empty;

            //string JDDescription = string.Empty;
            //string HowSoonValue = HRDetailModel.HowSoon ?? string.Empty;
            //string HowLongValue = HRModel.Availability ?? string.Empty;
            //string YearsOfExp = HRDetailModel.YearOfExp.HasValue ? Convert.ToString(HRDetailModel.YearOfExp.Value) : "";
            //string HowMany = HRModel.NoofTalents.ToString();
            //string JDFileName = HRModel.Jdfilename ?? string.Empty;

            //string JDData = "";

            //object[] param = null;

            //Sproc_DumpJDDetailsintoTempTable_Result sproc_DumpJDDetailsintoTempTable_Result = null;
            //long? psID = 0;

            //if (model.roleAndResponsibilites.Contains("'"))
            //    model.roleAndResponsibilites = model.roleAndResponsibilites.Replace("'", "''");
            //String res = Regex.Replace(model.roleAndResponsibilites, @"<[^>]*>", String.Empty);

            //if (model.requirements.Contains("'"))
            //    model.requirements = model.requirements.Replace("'", "''");

            //if (string.IsNullOrEmpty(HRModel.Jdurl) && !string.IsNullOrEmpty(HRModel.Jdfilename))
            //{
            //    if (!(HRModel.Jdfilename.EndsWith(".jpg") || HRModel.Jdfilename.EndsWith(".png") || HRModel.Jdfilename.EndsWith(".jpeg")))
            //    {
            //        if (model.JDDumpID > 0)
            //            param = new object[] { ContactID, CompanyName, TimeZoneValue, RoleValue, JDDescription, JDSkills, HowSoonValue, HowLongValue, YearsOfExp, HowMany, "", JDFileName, JDData, true, model.JDDumpID, JDAllSkills, true, model.roleAndResponsibilites, model.requirements };
            //        else
            //            param = new object[] { ContactID, CompanyName, TimeZoneValue, RoleValue, JDDescription, JDSkills, HowSoonValue, HowLongValue, YearsOfExp, HowMany, "", JDFileName, JDData, true, 0, JDAllSkills, isEdit, model.roleAndResponsibilites, model.requirements };

            //        sproc_DumpJDDetailsintoTempTable_Result = _iJDParse.Sproc_DumpJDDetailsintoTempTable(CommonLogic.ConvertToParamString(param));
            //    }

            //}

            //if (sproc_DumpJDDetailsintoTempTable_Result != null)
            //    psID = sproc_DumpJDDetailsintoTempTable_Result.Id;

            //if (HR_ID > 0 && psID > 0)
            //{
            //    GenSalesHiringRequest HRDetails = new GenSalesHiringRequest();
            //    HRDetails = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
            //    if (HRDetails != null)
            //    {
            //        HRDetails.JddumpId = psID;
            //        _talentConnectAdminDBContext.Entry(HRDetails).State = EntityState.Modified;
            //        _talentConnectAdminDBContext.SaveChanges();
            //    }
            //}
        }

        [NonAction]
        private List<string> GetSkills(string content)
        {
            List<string> skillList = new List<string>();
            Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);

            if (!string.IsNullOrWhiteSpace(content))
            {
                if (!content.ToLower().Contains("filenotfounderror"))
                {
                    if (content.Length > 0)
                    {
                        content = content.Replace("'", "");
                        content = content.Replace("'", "");
                        content = content.Replace("[", "");
                        content = content.Replace("]", "");
                        content = content.Replace("\r\n", "");
                        content = content.Trim();

                        content = parser.CompareWordsWithSkills(content);

                        object[] skillsParam = new object[] { content, 1 };

                        List<Sproc_GetExtractedSkills_FromJD_Result> psApprovedSkills = _iJDParse.Sproc_GetExtractedSkills_FromJD(CommonLogic.ConvertToParamString(skillsParam));
                        if (psApprovedSkills != null && psApprovedSkills.Count > 0)
                        {
                            var tempSkillList = psApprovedSkills.Where(s => !string.IsNullOrWhiteSpace(s.AllValues)).Select(x => x.AllValues).ToList();
                            if (tempSkillList != null && tempSkillList.Count > 0)
                            {
                                skillList = tempSkillList.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                            }
                        }
                        else
                        {
                            skillList = content.Split(',').ToList();
                            skillList = skillList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                        }
                    }
                }
            }
            return skillList;
        }

        [NonAction]
        private async Task DownloadDriveFile(DriveService service, Google.Apis.Drive.v3.Data.File googleFile, string filePath)
        {
            try
            {
                var downloader = new MediaDownloader(service);
                downloader.ChunkSize = 500;
                // add a delegate for the progress changed event for writing to console on changes
                var downloadfile = service.Files.Get(googleFile.Id);

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    var progress = await downloadfile.DownloadAsync(fileStream);
                    if (progress.Status == DownloadStatus.Completed) { }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [NonAction]
        private void DownloadDocsFile(DriveService service, Google.Apis.Drive.v3.Data.File googleFile, string filePath)
        {
            try
            {
                var str1 = new MemoryStream();
                var req1 = service.Files.Export(googleFile.Id, "application/pdf");
                req1.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    Console.WriteLine(progress.BytesDownloaded);

                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    Console.WriteLine("Download complete.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    Console.WriteLine("Download failed.");
                                    break;
                                }
                        }
                    };

                req1.Download(str1);

                using (FileStream fs = new FileStream(filePath, FileMode.CreateNew))
                {
                    str1.WriteTo(fs);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private dynamic GetSkillsAndRolesAndRR(string FileName, long loggedInUserID = 0)
        {
            loggedInUserID = SessionValues.LoginUserId;

            List<SkillOptionVM> Skills = new List<SkillOptionVM>();
            List<string> responsibilityList = new List<string>();
            List<string> requirementsList = new List<string>();
            List<string> skillList = new List<string>();

            Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);

            string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", FileName);

            byte[] bindata = System.Text.Encoding.ASCII.GetBytes(filePath);
            //var content = parser.ExtractPDFText(filePath);

            var paragraphList = parser.ExtractTextFromPDF(filePath);
            var pdfText = string.Join(" ", paragraphList);

            if (paragraphList != null && paragraphList.Count > 0)
            {
                responsibilityList = parser.ExtractRoleAndResponsbillity(paragraphList);
                requirementsList = parser.ExtractRequirement(paragraphList);
                //skillList = GetSkills(pdfText);
            }



            string SkillListString = string.Empty;
            if (skillList != null && skillList.Count > 0)
            {
                SkillListString = string.Join("$", skillList);
                Skills = new List<SkillOptionVM>();

                if (skillList != null)
                {
                    foreach (var s in skillList)
                    {
                        var SkillDetails = _talentConnectAdminDBContext.PrgSkills.FirstOrDefault(x => x.Skill.ToLower().Trim() == s.ToLower().Trim());
                        if (SkillDetails != null)
                        {
                            Skills.Add(
                                new SkillOptionVM
                                {
                                    ID = SkillDetails.Id.ToString(),
                                    IsSelected = Convert.ToBoolean(SkillDetails.IsActive),
                                    Text = SkillDetails.Skill,
                                    Proficiency = "Basic"
                                }
                            );
                        }
                    }
                }
            }

            string responsibilityText = responsibilityList != null && responsibilityList.Count > 0 ? String.Join("$", responsibilityList) : string.Empty;
            string requirementsText = requirementsList != null && requirementsList.Count > 0 ? String.Join("$", requirementsList) : string.Empty;

            if (responsibilityText.Contains("'"))
                responsibilityText = responsibilityText.Replace("'", "''");
            if (requirementsText.Contains("'"))
                requirementsText = requirementsText.Replace("'", "''");
            if (pdfText.Contains("'"))
                pdfText = pdfText.Replace("'", "''");

            //UTS-3474: Save the JD All Skills fields as existing flow.
            string JDAllSkills = SkillListString.Replace("$", ",");

            object[] param = new object[] { loggedInUserID, "", "", "", "", SkillListString, "", "", "", "", "", FileName, pdfText, true, 0, JDAllSkills, false, responsibilityText, requirementsText };
            Sproc_DumpJDDetailsintoTempTable_Result JDDumpObj = _iJDParse.Sproc_DumpJDDetailsintoTempTable(CommonLogic.ConvertToParamString(param));

            string? requirementsContent = requirementsList != null && requirementsList.Count > 0 ? string.Join(" ", requirementsList) : null;
            string? responsibilityContent = responsibilityList != null && responsibilityList.Count > 0 ? string.Join(" ", responsibilityList) : null;

            dynamic objResult = new ExpandoObject();
            objResult.Skills = Skills;
            objResult.FileName = FileName;
            objResult.Requirements = requirementsContent;
            objResult.Responsibility = responsibilityContent;
            objResult.JDDumpID = JDDumpObj == null ? 0 : JDDumpObj.Id;

            return objResult;
        }
        //Comments by anit 06Nov2023
        /*
        private object _GetAllHRDataForAdmin(TalentConnectAdminDBContext tcw, long HiringRequest_ID)
        {
            string HRData = "";

            if (HiringRequest_ID == 0)
            {
                var table = tcw.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{null}, {true}")
                                .ToList();

                if (table.Count == 0)
                {
                    return string.Empty;
                }

                HRData = JsonConvert.SerializeObject(table);
                return HRData;

            }
            else
            {
                var table = tcw.Set<Sproc_GET_ALL_HR_Details_For_PHP_API_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.Sproc_GET_ALL_HR_Details_For_PHP_API} " + $"{HiringRequest_ID}, {false}")
                                .ToList().AsEnumerable().FirstOrDefault();

                if (table == null)
                {
                    return "";
                }

                if (!string.IsNullOrEmpty(table.JDFilename))
                {
                    table.JDPath = System.IO.Path.Combine($"{Request.Scheme}:", Request.Host.Value, "Media", "JDParsing", "JDFiles", table.JDFilename);
                }

                //HRData = JsonConvert.SerializeObject(table);
                HR_Data _data = new HR_Data();

                HRDetailWithSkills hRDetailWithSkills = new HRDetailWithSkills();
                hRDetailWithSkills.HR_Details = table;
                var HR_Skills_Details = tcw.Set<sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI_Result>()
                                .FromSqlRaw($"{Constants.ProcConstant.sproc_GetSkillsAndProficiencyBasedonHR_ForPHPAPI} " + $" {HiringRequest_ID} ")
                                .ToList();

                foreach (var item in HR_Skills_Details)
                {
                    hRDetailWithSkills.skill.Add(new SkillDetail
                    {
                        Proficiency = item.Proficiency,
                        SkillName = item.Skill,
                        TempSkillName = item.TempSkill
                    });
                }

                var HR_InterviewerDetails = tcw.GenSalesHiringRequestInterviewerDetails.Where(x => x.HiringRequestId == HiringRequest_ID).ToList();
                foreach (var item in HR_InterviewerDetails)
                {
                    hRDetailWithSkills.HR_InterviewerDetails.Add(new HR_InterviewerDetail
                    {
                        ID = item.Id,
                        HiringRequestID = item.HiringRequestId ?? 0,
                        InterviewerDesignation = item.InterviewerDesignation,
                        InterviewerEmailID = item.InterviewerEmailId,
                        InterviewerLinkedIn = item.InterviewLinkedin,
                        InterviewerName = item.InterviewerName,
                        InterviewerType = tcw.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault() == null ? ""
                                                            : tcw.PrgCompanyTypeofInterviewers.Where(x => x.Id == item.TypeofInterviewerId).FirstOrDefault().TypeofInterviewer,
                        InterviewerTypeID = item.TypeofInterviewerId ?? 0,
                        InterviewerYearsOfExp = item.InterviewerYearofExperience ?? 0
                    });
                }

                _data.HRData = hRDetailWithSkills;
                _data.Status = "200";


                return JsonConvert.SerializeObject(_data);
            }
        }
        */

        private string SaveCountryStateCityPostalCodeData(string? country, long? pincode)
        {
            try
            {
                string apikey = "550c4850-d2af-11ed-8fb0-f3aa3820f89f";
                string APIUrl = "";

                if (!string.IsNullOrEmpty(pincode.ToString()))
                {
                    APIUrl = string.Format("https://app.zipcodebase.com/api/v1/search?apikey={0}&codes={1}", apikey, pincode);
                }
                else if (!string.IsNullOrEmpty(pincode.ToString()) && !string.IsNullOrEmpty(country))
                {
                    APIUrl = string.Format("https://app.zipcodebase.com/api/v1/search?apikey={0}&codes={1}&country={2}", apikey, pincode, country);
                }
                using (var client = new WebClient()) //WebClient  
                {

                    client.Headers.Add("Content-Type:application/json"); //Content-Type  
                    client.Headers.Add("Accept:application/json");
                    var result = client.DownloadString(APIUrl); //URI  
                    if (!string.IsNullOrEmpty(result))
                    {
                        List<PostalCodeswithDetailsModel> getList = JsonConvert.DeserializeObject<List<PostalCodeswithDetailsModel>>(Tabulate(result.ToString()));
                        if (getList.Any())
                        {
                            var distinctGetList = getList.Select(x => new { x.postal_code, x.country_code, x.state_en, x.province }).Distinct().ToList();

                            foreach (var item in distinctGetList)
                            {

                                var ObjPostalCode = _talentConnectAdminDBContext.GenPostalCodeswithDetails.Where(x => x.PostalCode == item.postal_code && x.CountryCode == item.country_code && x.StateEn == item.state_en && x.Province == item.province).FirstOrDefault();
                                if (ObjPostalCode == null)
                                {
                                    GenPostalCodeswithDetail objGenPostalCodeswithDetail = new();
                                    objGenPostalCodeswithDetail.PostalCode = item.postal_code;
                                    objGenPostalCodeswithDetail.CountryCode = item.country_code;
                                    objGenPostalCodeswithDetail.Province = item.province;
                                    objGenPostalCodeswithDetail.StateEn = item.state_en;
                                    objGenPostalCodeswithDetail.CreatedByDatetime = DateTime.Now;
                                    _talentConnectAdminDBContext.GenPostalCodeswithDetails.Add(objGenPostalCodeswithDetail);
                                    _talentConnectAdminDBContext.SaveChanges();
                                }
                            }
                            return "OK";
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string Tabulate(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the Last array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).Last();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return trgArray.ToString();
        }

        /// <summary>
        /// Common method to save other added role.
        /// </summary>
        /// <param name="genSalesHiringRequestDetailCreated"></param>
        private void SaveOtherRoleWhileAddEditHR(ref GenSalesHiringRequestDetail genSalesHiringRequestDetailCreated)
        {
            //UTS-3959: If other role is added then insert that role in the master table
            if (genSalesHiringRequestDetailCreated.RoleId == 0 && !string.IsNullOrWhiteSpace(genSalesHiringRequestDetailCreated.AddOtherRole))
            {
                PrgTalentRole prgTalentRole = new PrgTalentRole()
                {
                    TalentRole = genSalesHiringRequestDetailCreated.AddOtherRole,
                    IsActive = true,
                    IsAdhoc = true,
                    FrontIconImage = "default-role-icon.png"
                };

                _talentConnectAdminDBContext.PrgTalentRoles.Add(prgTalentRole);
                _talentConnectAdminDBContext.SaveChanges();
                _talentConnectAdminDBContext.Entry(prgTalentRole).Reload();

                string? otherRole = genSalesHiringRequestDetailCreated.AddOtherRole;

                var roleid = _talentConnectAdminDBContext.PrgTalentRoles.FirstOrDefault(x => x.TalentRole == otherRole);
                if (roleid != null) { genSalesHiringRequestDetailCreated.RoleId = roleid.Id; }
            }
        }

        /// <summary>
        /// UTS-4332: Convert Talents to HR or DP based on the HR from Special Edit.
        /// </summary>
        private void ConvertTalentsToContractualORDP(HiringReqeustModel hiringRequestModel, long hrId)
        {
            ViewAllHRController viewAllHRController = new ViewAllHRController(_commonInterface, _configuration, _talentConnectAdminDBContext, _universalProcRunner, _iUpChatEmail, _iUpChatCall, _httpContextAccessor);
            //Convert the Talent into DP.
            if (hiringRequestModel.isHRTypeDP)
            {
                ObjectResult? isSucess = viewAllHRController.GetTalentsDPConversion(hrId);
                if (isSucess != null)
                {
                    ResponseObject? responseObject = (ResponseObject)isSucess.Value;
                    List<Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result> details = (List<Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result>)(responseObject?.Details);
                    if (details != null)
                    {
                        List<ConvertToDP> convertToDPs = new List<ConvertToDP>();
                        foreach (var talent in details)
                        {
                            ConvertToDP convertToDP = new ConvertToDP();
                            convertToDP.HRID = talent.HRID;
                            convertToDP.ContactTalentID = talent.ContactTalentPriorityID ?? 0;
                            convertToDP.TalentID = talent.TalentID;
                            convertToDP.CurrentCTC = talent.CurrentCTC ?? 0;
                            convertToDP.ExpectedCTC = talent.ExpectedCTC ?? 0;
                            convertToDP.DpPercentage = hiringRequestModel.directPlacement.DpPercentage ?? 0;
                            ObjectResult? dpAmountresponse = viewAllHRController.CalculateDPConversionCost(convertToDP.HRID, convertToDP.ContactTalentID, convertToDP.DpPercentage, convertToDP.ExpectedCTC);

                            if (dpAmountresponse != null)
                            {
                                ResponseObject? dpAmountresponseObject = (ResponseObject)dpAmountresponse.Value;
                                convertToDP.DPAmount = Convert.ToDecimal(dpAmountresponseObject?.Details);
                            }
                            convertToDPs.Add(convertToDP);
                        }

                        if (convertToDPs.Count > 0)
                        {
                            viewAllHRController.SaveTalentsDPConversion(convertToDPs);
                        }
                    }
                }
            }
            //Convert the talent into Contractual.
            else
            {
                ObjectResult? isSucess = viewAllHRController.GetTalentsContractualConversion(hrId);
                if (isSucess != null)
                {
                    ResponseObject? responseObject = (ResponseObject)isSucess.Value;
                    List<Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result> details = (List<Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result>)(responseObject?.Details);
                    if (details != null)
                    {
                        List<ConvertToContractual> convertToContractuals = new List<ConvertToContractual>();
                        foreach (var talent in details)
                        {
                            ConvertToContractual convertToContractual = new ConvertToContractual();
                            convertToContractual.HRID = talent.HRID;
                            convertToContractual.ContactTalentID = talent.ContactTalentPriorityID;
                            convertToContractual.TalentID = talent.TalentID;
                            convertToContractual.NRPercentage = hiringRequestModel.NRMargin ?? 0;
                            ObjectResult? hrCostresponse = viewAllHRController.CalculateHRCost(convertToContractual.HRID, convertToContractual.ContactTalentID, talent.BRAmount, convertToContractual.NRPercentage);

                            if (hrCostresponse != null)
                            {
                                ResponseObject? dpAmountresponseObject = (ResponseObject)hrCostresponse.Value;
                                convertToContractual.HR_Cost = Convert.ToDecimal(dpAmountresponseObject?.Details);
                            }
                            convertToContractuals.Add(convertToContractual);
                        }

                        if (convertToContractuals.Count > 0)
                        {
                            viewAllHRController.SaveTalentsContractualConversion(convertToContractuals);
                        }
                    }
                }
            }
        }

        #endregion

        #region Claude AI
        [HttpGet("ExtractTextUsingclaudAI")]
        public async Task<ObjectResult> ExtractTextUsingclaudAI(string psUrl, string clientEmail, string processType = "")
        {
            try
            {
                #region Pre-Validation
                if (string.IsNullOrEmpty(clientEmail) || string.IsNullOrEmpty(psUrl))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please select client Email/Name or psUrl"));
                }
                #endregion
                long ContactID = 0;
                GenContact genContact = await _iJDParse.GetGenContact(clientEmail);
                if (genContact != null)
                {
                    ContactID = genContact.Id;
                }
                string LogedInUserID = ContactID.ToString();
                long LogedInUserIdval = ContactID;
                string pythonURL = _configuration["PythonParsingURL"] + "?ContactID=" + LogedInUserID;
                string FrontOfficeURL = _configuration["FrontOfficeAPIURL"];
                string endpoint = _configuration["AIEndPoint"];
                string apiKey = _configuration["AIAPIKey"];
                string content = "";
                HttpWebRequest webRequest;
                ClaudeAIResponse claudeAIResponse = new();
                string OpenAIChatGPTPrompt = string.Empty;

                if (string.IsNullOrWhiteSpace(pythonURL) || string.IsNullOrWhiteSpace(psUrl))
                {
                    // Return a JSON response
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "Please provide parsing URL"
                    });
                }


                Uri uri = new Uri(psUrl);

                var cleanedUrl = psUrl;//uri.Scheme + "://" + uri.Host + uri.AbsolutePath;
                count += 1;

                // Check if the URL is valid or not.
                if (IsUrlValid(cleanedUrl))
                {
                    // Already exists in table then fetch details from db and set

                    var gptData = await _iJDParse.GetGenGptJdResponseByUrl(cleanedUrl);

                    long gptJdId = 0;

                    if (gptData is not null && gptData.Id > 0)
                    {
                        gptJdId = gptData.Id;
                        JobPostDetailsViewModel jobPostDetails = new JobPostDetailsViewModel();

                        //ProcessType process;
                        //Enum.TryParse<ProcessType>(processType, out process);

                        claudeAIResponse = JsonConvert.DeserializeObject<ClaudeAIResponse>(gptData.ResponseData);

                        jobPostDetails = ConvertStringToJobPostDetailsModel(gptData.ResponseData.ToString().Trim(), gptJdId, processType);

                        var roleData = await _iJDParse.GetGenRoleAndHiringTypeClientPortalByGptJdId(gptData.Id);
                        var skillData = await _iJDParse.GetGenSkillAndBudgetClientPortalByGptJdId(gptData.Id);
                        var empData = await _iJDParse.FetchEmploymentDetailsByGptJdIdAsync(gptData.Id);
                        var jobDescData = await _iJDParse.FetchRolesResponsibilitiesByGptJdIdAsync(gptData.Id);

                        if (roleData != null) // step-1
                        {
                            jobPostDetails.RoleName = roleData?.RoleName;
                            jobPostDetails.GPTJDID = roleData?.Gptjdid;
                            jobPostDetails.GUID = roleData?.Guid;
                            jobPostDetails.ExperienceYears = roleData?.ExperienceYears;
                            jobPostDetails.NoOfTalents = roleData?.NoOfTalents;
                            jobPostDetails.IsHiringLimited = roleData?.IsHiringLimited;
                            jobPostDetails.ContractDuration = roleData?.ContractDuration;

                            //Finished step details
                            var stepData = await _iJDParse.GetGenClientJobInfo(roleData.Guid);
                            jobPostDetails.CurrentStepId = stepData?.CurrentStepId;
                            jobPostDetails.NextStepId = stepData?.NextStepId;
                        }

                        if (skillData != null) // step-2
                        {
                            jobPostDetails.Skills = skillData?.Skills;
                            jobPostDetails.AllSkills = skillData?.AllSkills;
                            jobPostDetails.BudgetFrom = skillData?.BudgetFrom;
                            jobPostDetails.BudgetTo = skillData?.BudgetTo;
                            //jobPostDetails.salary = data.Max_Salary;
                            jobPostDetails.Currency = skillData?.Currency;
                            //jobPostDetails.RoleName = data.Working_Zone_With_Time_Zone;
                        }

                        if (empData != null) // step-3
                        {
                            jobPostDetails.EmploymentType = empData?.EmploymentType;
                            jobPostDetails.WorkingModeId = empData?.WorkingModeId;
                            jobPostDetails.TimezonePreferenceId = empData?.TimezonePreferenceId;
                            jobPostDetails.TimeZoneFromTime = empData?.TimeZoneFromTime;
                            jobPostDetails.TimeZoneEndTime = empData?.TimeZoneEndTime;
                            jobPostDetails.AchievementId = empData?.AchievementId;
                        }

                        if (jobDescData != null)// step-4
                        {
                            jobPostDetails.Requirements = jobDescData?.Requirements;
                            jobPostDetails.RolesResponsibilities = jobDescData?.RolesResponsibilities;
                        }

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Success",
                            Details = jobPostDetails
                        });
                    }


                    if (cleanedUrl.Contains("drive.google.com") || cleanedUrl.Contains("docs.google.com"))
                    {
                        return await UploadLinkForParsingClaudAI(cleanedUrl, processType, ContactID);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(System.IO.Path.Combine(FrontOfficeURL, "File_JDPythonParse_" + LogedInUserID + ".txt"), cleanedUrl);


                        using (WebResponse wr = await WebRequest.Create(pythonURL).GetResponseAsync())
                        {
                            using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                            {
                                content = await sr.ReadToEndAsync();
                                //claudeAIResponse = JsonConvert.DeserializeObject<ClaudeAIResponse>(content);
                                //content = sr.ReadToEnd();
                            }
                        };

                        // if the url has multiple jobs listed then show this msg.
                        if (content.Trim().ToLower().Contains("Multiple Jobs Listed For this URL".ToLower()))
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                            {
                                statusCode = StatusCodes.Status404NotFound,
                                Message = "Multiple Jobs Listed For this URL",
                                Details = ""
                            });
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(FrontOfficeURL, "File_JDPythonParse_" + LogedInUserID + ".txt")))
                            System.IO.File.Delete(System.IO.Path.Combine(FrontOfficeURL, "File_JDPythonParse_" + LogedInUserID + ".txt"));

                        //string filePath = System.IO.Path.Combine(FrontOfficeURL, "JDText_" + LogedInUserID + ".txt");

                        OpenAIChatGPTPrompt = System.IO.File.ReadAllText(Path.Combine(FrontOfficeURL, "GPTPrompt.txt"));
                        content = System.IO.File.ReadAllText(Path.Combine(FrontOfficeURL, "ExtractedJDText_" + LogedInUserID + ".txt"));
                        // Replace with your file path


                        if (!string.IsNullOrEmpty(content))
                        {
                            string content1 = string.Empty;
                            string pythonURL1 = _configuration["PythonScrapeCreateJD"] + "?ContactID=" + LogedInUserID;
                            object[] param1 = new object[] { LogedInUserID, null, 1, content };
                            string paramString1 = CommonLogic.ConvertToParamString(param1);
                            Sproc_CreateJDFromPrompt_ClientPortal_Result jobPreview = _iJDParse.Sproc_CreateJDFromPrompt_ClientPortal_Result(paramString1);
                            string JDcreateText = "";

                            JDcreateText = jobPreview.PromptBody;
                            System.IO.File.WriteAllText(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt"), JDcreateText);


                            using (WebResponse wr = WebRequest.Create(pythonURL1).GetResponse())
                            {
                                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                                {
                                    content1 = sr.ReadToEnd();
                                    //gPTResponse = JsonConvert.DeserializeObject<GPTResponse>(content);
                                }
                            }

                            if (System.IO.File.Exists(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt")))
                                System.IO.File.Delete(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt"));

                            if (System.IO.File.Exists(System.IO.Path.Combine(FrontOfficeURL, "ExtractedJDText_" + LogedInUserID + ".txt")))
                                System.IO.File.Delete(System.IO.Path.Combine(FrontOfficeURL, "ExtractedJDText_" + LogedInUserID + ".txt"));


                            // Check if the file exists
                            if (!string.IsNullOrEmpty(JDcreateText))
                            {
                                // Read all lines from the file
                                //string[] lines = await System.IO.File.ReadAllLinesAsync(filePath);

                                GenGptJdresponse gptJdresponse = new()
                                {
                                    Url = cleanedUrl,
                                    Jdtext = JDcreateText,
                                    ResponseData = content1,
                                    CreatedById = LogedInUserIdval,
                                    CreatedDateTime = System.DateTime.Now,
                                    Gptprompt = JDcreateText //OpenAIChatGPTPrompt + string.Join(" ", lines)
                                };

                                await _iJDParse.SaveGptJdresponse(gptJdresponse).ConfigureAwait(false);

                                gptJdId = gptJdresponse.Id;

                                if (System.IO.File.Exists(System.IO.Path.Combine(FrontOfficeURL, "JDText_" + LogedInUserID + ".txt")))
                                    System.IO.File.Delete(System.IO.Path.Combine(FrontOfficeURL, "JDText_" + LogedInUserID + ".txt"));



                                //Response.WriteAsync("ABCD" + gptJdId.ToString() + "_" + filePath);
                            }
                            else
                            {
                                //Response.WriteAsync("ABCD" + filePath);
                                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                                {
                                    statusCode = StatusCodes.Status404NotFound,
                                    Message = "Error File not found."
                                });
                            }

                            // Return a JSON response
                            //return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                            //{
                            //    statusCode = StatusCodes.Status200OK,
                            //    Message = "Success",
                            //    Details = ConvertStringToClaudeAIResponseModel(content1.ToString().Trim(), gptJdId, processType)
                            //});
                            JobPostDetailsViewModel responseJObject = await ConvertStringToClaudeAIResponseModel(content1.ToString().Trim(), gptJdId, processType);

                            #region MyRegion
                            if (responseJObject != null)
                            {
                                var result = responseJObject;
                                HiringRequest HiringRequestViewModel = new HiringRequest();
                                HiringRequestViewModel.addHiringRequest.RequestForTalent = result.RoleName;
                                HiringRequestViewModel.addHiringRequest.NoofTalents = result.NoOfTalents;
                                HiringRequestViewModel.addHiringRequest.IsHiringLimited = result.IsHiringLimited == "Temporary";
                                HiringRequestViewModel.addHiringRequest.Availability = result.EmploymentType == "PartTime" ? "Part Time" : "Full Time";
                                HiringRequestViewModel.addHiringRequest.Guid = Guid.NewGuid().ToString();
                                HiringRequestViewModel.ModeOfWorkingId = Convert.ToString(result.WorkingModeId);
                                HiringRequestViewModel.ChatGptSkills = result.Skills;
                                HiringRequestViewModel.ChatGptAllSkills = result.AllSkills;

                                var MustHaveSkills = string.IsNullOrEmpty(result.Skills) ? new List<string>() : result.Skills.Split(',').ToList();
                                var GoodToHaveSkills = string.IsNullOrEmpty(result.AllSkills) ? new List<string>() : result.AllSkills.Split(',').ToList(); ;

                                //foreach (var s in MustHaveSkills)
                                //{
                                //    string? SkillID = string.Empty;

                                //    var skillInMainData = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                                //    if (skillInMainData != null)
                                //    {
                                //        SkillID = skillInMainData.Id.ToString();
                                //        HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, IsSelected = true, Text = s, Proficiency = "Strong" });
                                //    }

                                //    var skillInTempData = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                                //    if (skillInTempData != null)
                                //    {
                                //        SkillID = skillInTempData.TempSkillId;
                                //        HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = skillInTempData.Id.ToString(), IsSelected = true, Text = s, Proficiency = "Strong" });
                                //    }


                                //    // If skills does not exist then insert in the temp table.
                                //    if (string.IsNullOrEmpty(SkillID))
                                //    {
                                //        object[] param = new object[] { s, ContactID, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
                                //        var objresult = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
                                //        if (objresult != null)
                                //        {
                                //            SkillID = objresult.TempSkill_ID;
                                //            var TempSkill_PrimaryID = _talentConnectAdminDBContext.PrgTempSkills.FirstOrDefault(x => x.TempSkillId == SkillID).Id;

                                //            HiringRequestViewModel.Skillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = TempSkill_PrimaryID.ToString(), IsSelected = true, Text = s, Proficiency = "Strong" });
                                //        }
                                //    }
                                //}


                                //foreach (var s in GoodToHaveSkills)
                                //{
                                //    string? SkillID = string.Empty;

                                //    var skillInMainData = _talentConnectAdminDBContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                                //    if (skillInMainData != null)
                                //    {
                                //        SkillID = skillInMainData.Id.ToString();
                                //        HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, IsSelected = true, Text = s, Proficiency = "Basic" });
                                //    }

                                //    var skillInTempData = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill.ToLower().Trim() == s.ToLower().Trim()).FirstOrDefault();
                                //    if (skillInTempData != null)
                                //    {
                                //        SkillID = skillInTempData.TempSkillId;
                                //        HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = skillInTempData.Id.ToString(), IsSelected = true, Text = s, Proficiency = "Basic" });
                                //    }


                                //    // If skills does not exist then insert in the temp table.
                                //    if (string.IsNullOrEmpty(SkillID))
                                //    {
                                //        object[] param = new object[] { s, ContactID, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
                                //        var objresult = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
                                //        if (objresult != null)
                                //        {
                                //            SkillID = objresult.TempSkill_ID;
                                //            var TempSkill_PrimaryID = _talentConnectAdminDBContext.PrgTempSkills.FirstOrDefault(x => x.TempSkillId == SkillID).Id;

                                //            HiringRequestViewModel.AllSkillmulticheckbox.Add(new SkillOptionVM { ID = SkillID, TempSkill_ID = TempSkill_PrimaryID.ToString(), IsSelected = true, Text = s, Proficiency = "Basic" });
                                //        }
                                //    }
                                //}

                                HiringRequestViewModel.SalesHiringRequest_Details.YearOfExp = result.ExperienceYears;
                                HiringRequestViewModel.SalesHiringRequest_Details.SpecificMonth = Convert.ToInt32(result.ContractDuration);
                                HiringRequestViewModel.SalesHiringRequest_Details.Currency = result.Currency;
                                HiringRequestViewModel.SalesHiringRequest_Details.BudgetFrom = result.BudgetFrom;
                                HiringRequestViewModel.SalesHiringRequest_Details.BudgetTo = result.BudgetTo;
                                HiringRequestViewModel.SalesHiringRequest_Details.HowSoon = Convert.ToString(result.HowSoon);
                                HiringRequestViewModel.SalesHiringRequest_Details.TimezonePreferenceId = result.TimezonePreferenceId;
                                HiringRequestViewModel.SalesHiringRequest_Details.TimeZoneFromTime = result.TimeZoneFromTime;
                                HiringRequestViewModel.SalesHiringRequest_Details.TimeZoneEndTime = result.TimeZoneEndTime;
                                HiringRequestViewModel.SalesHiringRequest_Details.RolesResponsibilities = result.RolesResponsibilities;
                                HiringRequestViewModel.SalesHiringRequest_Details.Requirement = result.Requirements;
                                HiringRequestViewModel.SalesHiringRequest_Details.JobDescription = result.JobDescription;

                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HiringRequestViewModel });


                            }
                            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "No Data Found" });
                            #endregion
                        }

                        // Return a JSON response
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Success",
                            Details = ""//ConvertStringToJobPostDetailsModel(content.ToString().Trim(), gptJdId, processType)
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid URL"
                    });
                }
            }
            catch (Exception ex)
            {
                // Return a JSON response
                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = ex.Message,
                    Details = new JobPostDetailsViewModel()
                });
            }
        }

        private static bool IsUrlValid(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                Uri uriResult;
                return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            }
            catch (Exception)
            {
                return false;
            }
        }

        private JobPostDetailsViewModel ConvertStringToJobPostDetailsModel(string strContent, long gptId, string processType)
        {
            JobPostDetailsViewModel jobPostDetails = new JobPostDetailsViewModel();
            try
            {
                // The below IF block is only for testing purpose
                if (string.IsNullOrWhiteSpace(strContent))
                {
                    strContent = @"{
                                ""Requirements"": [
                                    ""Bachelor\u2019s/Master\u2019s degree in Engineering, Computer Science (or equivalent experience)"",
                                    ""At least 5+ years of relevant experience as a Product Manager"",
                                    ""Extensive knowledge of developing technology features and/or products for a brokerage company"",
                                    ""In-depth knowledge of trading products, CFDs, and financial markets"",
                                    ""Must have extensive FinTech experience""
                                ],
                                ""Roles_And_Responsibilities"": [
                                    ""Determine the internal stakeholders' insights and personalization requirements, then transform them into product features"",
                                    ""Create wireframes and specifications, present them to key stakeholders for approval, and explain the scope and nature of the work to be done"",
                                    ""Utilize analysis, user input, research, and direct customer observation to determine how insights might enhance user experience"",
                                    ""Be responsible for successful project delivery in accordance with the project's objectives, scope, and timetable by cooperating with QA successfully"",
                                    ""Help with requirement management throughout the project lifecycle, including tracking issues and defects""
                                ],
                                ""Years_Of_Experience"": 5,
                                ""Budget_From"": 0,
                                ""Budget_To"": 0,
                                ""Max_Salary"": 0,
                                ""Salary_Currency"": """",
                                ""Working_Zone_With_Time_Zone"": """",
                                ""Type_Of_Job"": ""Full Time"",
                                ""Opportunity_Type"": ""On Site"",
                                ""Skills"": ""Product Management, Engineering, Computer Science, Brokerage, Trading Products, CFDs, Financial Markets, FinTech"",
                                ""Job_Title"": ""Product Manager"",
                                ""Suggested_Skills"": ""Product Management, Engineering, Computer Science, Brokerage""
                            }";
                }
                int count = 0;

                var data = JsonConvert.DeserializeObject<ChatGptResponseModel>(strContent);

                int WorkingModeId = 1;
                if (string.Equals(data?.Opportunity_Type, "Remote", StringComparison.OrdinalIgnoreCase))
                {
                    WorkingModeId = 1;
                }
                else if (string.Equals(data?.Opportunity_Type, "Hybrid", StringComparison.OrdinalIgnoreCase))
                {
                    WorkingModeId = 2;
                }
                else if (string.Equals(data?.Opportunity_Type, "On Site", StringComparison.OrdinalIgnoreCase))
                {
                    WorkingModeId = 3;
                }

                jobPostDetails.GPTJDID = gptId;

                // step-1
                jobPostDetails.RoleName = data.Job_Title;
                jobPostDetails.ExperienceYears = Convert.ToInt32(data.Years_Of_Experience);

                // step-2
                jobPostDetails.Skills = data.Skills;
                jobPostDetails.AllSkills = (data.Skills == data.Suggested_Skills ? "" : data.Suggested_Skills) ?? "";
                jobPostDetails.BudgetFrom = Convert.ToDecimal(data.Budget_From);
                jobPostDetails.BudgetTo = Convert.ToDecimal(data.Budget_To);
                //jobPostDetails.salary = data.Max_Salary;
                jobPostDetails.Currency = data.Salary_Currency;
                //jobPostDetails.RoleName = data.Working_Zone_With_Time_Zone;

                // step-3
                jobPostDetails.EmploymentType = data.Type_Of_Job;
                jobPostDetails.WorkingModeId = WorkingModeId;

                // step-4

                List<string> description = new List<string>();

                List<string>? requirements = null;
                if (data.Requirements != null && data.Requirements.Count > 0)
                {
                    requirements = data.Requirements;
                }

                if (requirements != null)
                {
                    jobPostDetails.Requirements = System.Text.Json.JsonSerializer.Serialize(requirements);
                    description.AddRange(requirements);
                }

                List<string>? rolesResponsibilities = null;
                if (data.Roles_And_Responsibilities != null && data.Roles_And_Responsibilities.Count > 0)
                {
                    rolesResponsibilities = data.Roles_And_Responsibilities;
                }
                if (data.RolesResponsibilities != null && data.RolesResponsibilities.Count > 0)
                {
                    rolesResponsibilities = data.RolesResponsibilities;
                }

                if (rolesResponsibilities != null)
                {
                    jobPostDetails.RolesResponsibilities = System.Text.Json.JsonSerializer.Serialize(rolesResponsibilities);
                    description.AddRange(rolesResponsibilities);
                }

                jobPostDetails.JobDescription = System.Text.Json.JsonSerializer.Serialize(description);

                // Extra info
                jobPostDetails.ProcessType = processType;

                if (gptId > 0 && processType == ProcessType.URL_Parsing.ToString())
                {
                    if (!string.IsNullOrEmpty(data.Job_Title))
                        count += 1;
                    if (!string.IsNullOrEmpty(data.Skills))
                        count += 1;
                    if (!string.IsNullOrEmpty(jobPostDetails.Requirements))
                        count += 1;
                    if (!string.IsNullOrEmpty(jobPostDetails.RolesResponsibilities))
                        count += 1;
                    if (data.Years_Of_Experience != 0)
                        count += 1;
                    if (data.Budget_From != 0)
                        count += 1;
                    if (data.Budget_To != 0)
                        count += 1;
                    if (data.Max_Salary != 0)
                        count += 1;
                    if (!string.IsNullOrEmpty(data.Salary_Currency))
                        count += 1;
                    if (!string.IsNullOrEmpty(data.Working_Zone_With_Time_Zone))
                        count += 1;
                    if (!string.IsNullOrEmpty(data.Type_Of_Job))
                        count += 1;
                    if (!string.IsNullOrEmpty(data.Opportunity_Type))
                        count += 1;
                    if (!string.IsNullOrEmpty(data.Suggested_Skills))
                        count += 1;

                    _iJDParse.UpdateGptJdresponse(gptId, count).ConfigureAwait(false);
                    jobPostDetails.AchievedCount = count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobPostDetails;
        }

        [HttpGet("UploadLinkForParsingClaudAI")]
        public async Task<ObjectResult> UploadLinkForParsingClaudAI(string url, string processType, long ContactID)
        {
            try
            {
                dynamic objResult = new ExpandoObject();
                string LogedInUserID = ContactID.ToString();
                long LogedInUserIdval = ContactID;
                //long LoggedInUserID = SessionValues.LoginUserId;
                string FrontOfficeURL = _configuration["FrontOfficeAPIURL"];
                string LinkParsingURL = _configuration["LinkParsingURL"] + "?ContactID=" + LogedInUserID;
                string content;
                if (string.IsNullOrWhiteSpace(LinkParsingURL) || string.IsNullOrWhiteSpace(url))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "Enter URL",
                        Details = ""
                    });
                }
                else
                {
                    string LinkParsingFileIDPath = System.IO.Path.Combine(FrontOfficeURL + "\\JDFiles", "LinkParsingFileID_" + LogedInUserID + ".txt");
                    if (System.IO.File.Exists(LinkParsingFileIDPath))
                        System.IO.File.Delete(LinkParsingFileIDPath);

                    System.IO.File.WriteAllText(LinkParsingFileIDPath, url);

                    using (WebResponse wr = WebRequest.Create(LinkParsingURL).GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            content = await sr.ReadToEndAsync();
                            content = content.Replace("\r\n", "");
                        }
                    }

                    if (content.EndsWith(".pdf"))
                    {
                        long gptId = 0;

                        objResult = await GetSkillsAndRolesAndRRClaudAI(content, url, ContactID);

                        if (System.IO.File.Exists(LinkParsingFileIDPath))
                            System.IO.File.Delete(LinkParsingFileIDPath);

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Success",
                            //Details = ConvertStringToJobPostDetailsModel(objResult, gptId, processType)
                            Details = objResult
                        });
                    }
                    else
                    {

                        if (System.IO.File.Exists(LinkParsingFileIDPath))
                            System.IO.File.Delete(LinkParsingFileIDPath);

                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                        {
                            statusCode = StatusCodes.Status404NotFound,
                            Message = "Enter URL",
                            Details = ""
                        });
                    }


                }
            }
            catch (Exception ex)
            {
                // Return a JSON response
                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = ex.Message,
                    Details = new JobPostDetailsViewModel()
                });
            }
        }

        private async Task<dynamic> GetSkillsAndRolesAndRRClaudAI(string FileName, string Url, long ContactID)
        {
            long loggedInUserID = ContactID;
            string FrontOfficeURL = _configuration["FrontOfficeAPIURL"];
            Parser parser = new Parser(_configuration, _talentConnectAdminDBContext);

            string filePath = System.IO.Path.Combine(FrontOfficeURL + "\\JDFiles", FileName);
            string contentJDText;

            byte[] bindata = System.Text.Encoding.ASCII.GetBytes(filePath);
            //var content = parser.ExtractPDFText(filePath);


            string content = System.IO.File.ReadAllText(filePath);
            //var paragraphList = parser.ExtractTextFromPDF(filePath);
            var pdfText = string.Join(" ", content);
            string AllJDText = "";
            AllJDText = pdfText;

            ExtractTextFromFile extractTextFromFile = new ExtractTextFromFile()
            {
                PdfText = AllJDText
            };
            JobPostDetailsViewModel objResult = new JobPostDetailsViewModel();
            //contentJDText = ExtraCtJDBasedOnTextClaudAI(pdfText, Url);
            objResult = await ExtraCtJDBasedOnTextClaudAI(extractTextFromFile, ProcessType.Text_Parsing.ToString(), ContactID);

            return objResult;
        }

        [HttpPost("ExtraCtJDBasedOnTextClaudAI")]
        public async Task<JobPostDetailsViewModel> ExtraCtJDBasedOnTextClaudAI(ExtractTextFromFile extractTextFromFile, string processType, long ContactID)
        {
            try
            {
                string LogedInUserID = ContactID.ToString();
                string content = string.Empty;
                string pythonURL = _configuration["PythonScrapeCreateJD"] + "?ContactID=" + LogedInUserID;
                string FrontOfficeURL = @"" + _configuration["FrontOfficeAPIURL"];
                string PythonWithJDTextPath = System.IO.Path.Combine(FrontOfficeURL, "File_JDPythonParseWithJDText_" + LogedInUserID + ".txt");
                GPTResponse gPTResponse = new();
                long loggedInUserID = ContactID;//SessionValues.LoginUserId;

                object[] param1 = new object[] { LogedInUserID, null, 1, null };
                string paramString1 = CommonLogic.ConvertToParamString(param1);
                Sproc_CreateJDFromPrompt_ClientPortal_Result jobPreview = _iJDParse.Sproc_CreateJDFromPrompt_ClientPortal_Result(paramString1);
                string JDcreateText = "";

                //if (System.IO.File.Exists(PythonWithJDTextPath))
                //    System.IO.File.Delete(PythonWithJDTextPath);

                //System.IO.File.WriteAllText(PythonWithJDTextPath, extractTextFromFile.PdfText);


                //using (WebResponse wr = WebRequest.Create(pythonURL).GetResponse())
                //{
                //    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                //    {
                //        content = sr.ReadToEnd();
                //        //gPTResponse = JsonConvert.DeserializeObject<GPTResponse>(content);
                //    }
                //}

                //if (System.IO.File.Exists(System.IO.Path.Combine("Media/JDParsing", "File_JDPythonParseWithJDText_" + LogedInUserID + ".txt")))
                //    System.IO.File.Delete(System.IO.Path.Combine("Media/JDParsing", "File_JDPythonParseWithJDText_" + LogedInUserID + ".txt"));


                JDcreateText = jobPreview.PromptBody + ' ' + extractTextFromFile.PdfText;
                System.IO.File.WriteAllText(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + loggedInUserID + ".txt"), JDcreateText);


                using (WebResponse wr = WebRequest.Create(pythonURL).GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        content = sr.ReadToEnd();
                        //gPTResponse = JsonConvert.DeserializeObject<GPTResponse>(content);
                    }
                }

                if (System.IO.File.Exists(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt")))
                    System.IO.File.Delete(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt"));



                long gptJdId = 0;

                if (!string.IsNullOrEmpty(JDcreateText))
                {
                    GenGptJdresponse gptJdresponse = new()
                    {
                        Url = "",
                        Jdtext = JDcreateText,
                        ResponseData = content,
                        CreatedById = SessionValues.LoginUserId,
                        CreatedDateTime = System.DateTime.Now,
                        Gptprompt = JDcreateText
                    };

                    _iJDParse.SaveGptJdresponse(gptJdresponse).ConfigureAwait(false);

                    gptJdId = gptJdresponse.Id;
                }

                //var data = ConvertStringToJobPostDetailsModel(content, gptJdId, processType);
                var data = await ConvertStringToClaudeAIResponseModel(content.ToString().Trim(), gptJdId, ProcessType.AIGenerated.ToString());

                return data;
            }
            catch (Exception)
            {
                return new JobPostDetailsViewModel();
            }
        }

        [HttpPost("ExtraCtJDBasedOnTextchatGPTMini4o")]
        public async Task<JobPostDetailsViewModel> ExtraCtJDBasedOnTextchatGPTMini4o(ExtractTextFromFile extractTextFromFile, string processType, long ContactID)
        {
            try
            {
                string LogedInUserID = ContactID.ToString();
                string content = string.Empty;
                string pythonURL = _configuration["PythonScrapeCreateJD"] + "?ContactID=" + LogedInUserID;
                string FrontOfficeURL = @"" + _configuration["FrontOfficeAPIURL"];
                string PythonWithJDTextPath = System.IO.Path.Combine(FrontOfficeURL, "File_JDPythonParseWithJDText_" + LogedInUserID + ".txt");
                GPTResponse gPTResponse = new();
                long loggedInUserID = ContactID;//SessionValues.LoginUserId;
                string endpoint = _configuration["AIEndPoint"];
                string apiKey = _configuration["AIAPIKey"];

                object[] param1 = new object[] { LogedInUserID, null, 1, null };
                string paramString1 = CommonLogic.ConvertToParamString(param1);
                Sproc_CreateJDFromPrompt_ClientPortal_Result jobPreview = _iJDParse.Sproc_CreateJDFromPrompt_ClientPortal_Result(paramString1);
                string JDcreateText = "";

                JDcreateText = jobPreview.PromptBody + ' ' + extractTextFromFile.PdfText;

                if (!string.IsNullOrWhiteSpace(JDcreateText))
                {
                    content = await MakeApiCall(endpoint, apiKey, JDcreateText);
                }

                long gptJdId = 0;

                if (!string.IsNullOrEmpty(JDcreateText))
                {
                    GenGptJdresponse gptJdresponse = new()
                    {
                        Url = "",
                        Jdtext = JDcreateText,
                        ResponseData = content,
                        CreatedById = SessionValues.LoginUserId,
                        CreatedDateTime = System.DateTime.Now,
                        Gptprompt = JDcreateText
                    };

                    _iJDParse.SaveGptJdresponse(gptJdresponse).ConfigureAwait(false);

                    gptJdId = gptJdresponse.Id;
                }

                //var data = await ConvertStringToClaudeAIResponseModel(content.ToString().Trim(), gptJdId, ProcessType.AIGenerated.ToString());
                var data = await ConvertStringToChatGPTAIResponseModel(content.ToString().Trim(), gptJdId, ProcessType.AIGenerated.ToString());

                return data;
            }
            catch (Exception)
            {
                return new JobPostDetailsViewModel();
            }
        }


        private async Task<JobPostDetailsViewModel> ConvertStringToClaudeAIResponseModel(string strContent, long gptId, string processType)
        {
            JobPostDetailsViewModel jobPostDetails = new JobPostDetailsViewModel();
            try
            {
                int count = 0;

                string pattern = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))+(?(Open)(?!))\}";
                string JsonString = "";
                Regex regex = new Regex(pattern);
                MatchCollection matches = regex.Matches(strContent);
                foreach (Match match in matches)
                {
                    JsonString = match.Value;
                }

                if (!string.IsNullOrEmpty(JsonString))
                {
                    var data = JsonConvert.DeserializeObject<ClaudeAIResponseModel>(JsonString);


                    int WorkingModeId = 1;
                    if (string.Equals(data?.Opportunity_Type, "Remote", StringComparison.OrdinalIgnoreCase))
                    {
                        WorkingModeId = 1;
                    }
                    else if (string.Equals(data?.Opportunity_Type, "Hybrid", StringComparison.OrdinalIgnoreCase))
                    {
                        WorkingModeId = 2;
                    }
                    else if (string.Equals(data?.Opportunity_Type, "On Site", StringComparison.OrdinalIgnoreCase))
                    {
                        WorkingModeId = 3;
                    }

                    jobPostDetails.RoleName = data.Title;
                    jobPostDetails.ExperienceYears = Convert.ToInt32(data.YearsofExperienceRequired);

                    // step-2
                    jobPostDetails.Skills = data.Skills;
                    jobPostDetails.AllSkills = (data.Skills == data.Suggested_Skills ? "" : data.Suggested_Skills) ?? "";
                    jobPostDetails.BudgetFrom = Convert.ToDecimal(data.Budget_From);
                    jobPostDetails.BudgetTo = Convert.ToDecimal(data.Budget_To);
                    jobPostDetails.Currency = data.Salary_Currency;


                    // step-3
                    jobPostDetails.EmploymentType = data.Type_Of_Job;
                    jobPostDetails.WorkingModeId = WorkingModeId;

                    jobPostDetails.GPTJDID = gptId;
                    jobPostDetails.WorkingModeId = WorkingModeId;
                    // step-4
                    List<string> description = new List<string>();

                    List<string>? requirements = null;
                    if (data.Requirements != null && data.Requirements.Count > 0)
                    {

                        requirements = ReplacesingleQuotes(data.Requirements);
                    }

                    if (requirements != null)
                    {
                        jobPostDetails.Requirements = System.Text.Json.JsonSerializer.Serialize(requirements);
                        description.AddRange(requirements);
                    }


                    List<string>? offer = null;
                    if (data.Whatweoffer != null && data.Whatweoffer.Count > 0)
                    {
                        offer = ReplacesingleQuotes(data.Whatweoffer);
                    }

                    List<string>? roleoverviewdescription = null;
                    if (data.RoleOverviewDescription != null && data.RoleOverviewDescription.Count > 0)
                    {
                        roleoverviewdescription = ReplacesingleQuotes(data.RoleOverviewDescription);
                    }

                    List<string>? rolesResponsibilities = null;
                    if (data.RolesResponsibilities != null && data.RolesResponsibilities.Count > 0)
                    {
                        rolesResponsibilities = ReplacesingleQuotes(data.RolesResponsibilities);
                    }


                    if (rolesResponsibilities != null)
                    {
                        jobPostDetails.RolesResponsibilities = System.Text.Json.JsonSerializer.Serialize(rolesResponsibilities);
                        description.AddRange(rolesResponsibilities);
                    }

                    if (offer != null)
                    {
                        jobPostDetails.Whatweoffer = System.Text.Json.JsonSerializer.Serialize(offer);
                        description.AddRange(offer);
                    }

                    if (roleoverviewdescription != null)
                    {
                        jobPostDetails.RoleOverviewDescription = System.Text.Json.JsonSerializer.Serialize(roleoverviewdescription);
                        description.AddRange(roleoverviewdescription);
                    }


                    jobPostDetails.RequirementsList = new List<string>();
                    if (requirements != null)
                        jobPostDetails.RequirementsList.AddRange(requirements);


                    jobPostDetails.RolesResponsibilitiesList = new List<string>();
                    if (rolesResponsibilities != null)
                        jobPostDetails.RolesResponsibilitiesList.AddRange(rolesResponsibilities);

                    jobPostDetails.JobDescription = System.Text.Json.JsonSerializer.Serialize(description);
                    jobPostDetails.JobDesriptionList = new List<string>();
                    if (roleoverviewdescription != null)
                        jobPostDetails.JobDesriptionList.AddRange(roleoverviewdescription);
                    if (rolesResponsibilities != null)
                        jobPostDetails.JobDesriptionList.AddRange(rolesResponsibilities);
                    if (requirements != null)
                        jobPostDetails.JobDesriptionList.AddRange(requirements);
                    if (offer != null)
                        jobPostDetails.JobDesriptionList.AddRange(offer);

                    if (data.JobDescription != null)
                        jobPostDetails.JobDescription = data.JobDescription;

                    // Extra info
                    jobPostDetails.ProcessType = processType;
                    if (gptId > 0 && processType == ProcessType.URL_Parsing.ToString())
                    {
                        if (!string.IsNullOrEmpty(data.Title))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Skills))
                            count += 1;
                        if (!string.IsNullOrEmpty(jobPostDetails.Requirements))
                            count += 1;
                        if (!string.IsNullOrEmpty(jobPostDetails.RolesResponsibilities))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.YearsofExperienceRequired))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Budget_From))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Budget_To))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Max_Salary))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Salary_Currency))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Working_Zone_With_Time_Zone))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Type_Of_Job))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Opportunity_Type))
                            count += 1;
                        if (!string.IsNullOrEmpty(data.Suggested_Skills))
                            count += 1;

                        _iJDParse.UpdateGptJdresponse(gptId, count).ConfigureAwait(false);
                        jobPostDetails.AchievedCount = count;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobPostDetails;
        }

        #region For Copy Paste and I Donot Have JD Case
        [HttpPost("GetAllDetailsFromText")]
        public async Task<ObjectResult> GetAllDetailsFromText([FromBody] ExtractTextFromFile extractTextFromFile, string clientEmail)
        {
            try
            {
                #region Validation

                if (extractTextFromFile == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty", Details = null });
                }

                #endregion

                GenContact genContact = await _iJDParse.GetGenContact(clientEmail);
                JobPostDetailsViewModel objResult = new JobPostDetailsViewModel();

                if (genContact != null)
                {
                    long ContactID = genContact.Id;
                    objResult = await ExtraCtJDBasedOnTextClaudAI(extractTextFromFile, ProcessType.Text_Parsing.ToString(), ContactID);

                    objResult.JobDescription = extractTextFromFile.PdfText;

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Success",
                        Details = objResult
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status200OK,
                        Message = "Contact Not Found.",
                        Details = objResult
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("DontHaveJD")]
        public async Task<JobPostDetailsViewModel> DontHaveJD(DontHaveJdViewModel dontHaveJdViewModel)
        {
            try
            {
                if (dontHaveJdViewModel != null)
                {
                    string LogedInUserID = dontHaveJdViewModel.ContactID.ToString();
                    string content = string.Empty;
                    string pythonURL = _configuration["PythonScrapeCreateJD"] + "?ContactID=" + LogedInUserID;
                    string FrontOfficeURL = @"" + _configuration["FrontOfficeAPIURL"];
                    string PythonWithJDTextPath = System.IO.Path.Combine(FrontOfficeURL, "File_JDPythonParseWithJDText_" + LogedInUserID + ".txt");
                    GPTResponse gPTResponse = new();
                    long loggedInUserID = dontHaveJdViewModel.ContactID ?? 0;//SessionValues.LoginUserId;
                    long HRID = dontHaveJdViewModel.HRID ?? 0;
                    string MustHaveSkill = string.Join(",", dontHaveJdViewModel.MustHaveSkill);
                    string GoodHaveSkill = string.Join(",", dontHaveJdViewModel.GoodToHaveSkill);
                    string Title = dontHaveJdViewModel.Title;
                    string Location = dontHaveJdViewModel.Location;

                    string endpoint = _configuration["AIEndPoint"];
                    string apiKey = _configuration["AIAPIKey"];

                    object[] param1 = new object[] { LogedInUserID, HRID, MustHaveSkill, GoodHaveSkill, Title, Location };
                    string paramString1 = CommonLogic.ConvertToParamString(param1);
                    Sproc_CreateJDFromPrompt_UTSAdmin_Result jobPreview = _iJDParse.Sproc_CreateJDFromPrompt_UTSAdmin_Result(paramString1);
                    string JDcreateText = "";

                    JDcreateText = jobPreview.PromptBody;
                    //System.IO.File.WriteAllText(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + loggedInUserID + ".txt"), JDcreateText);


                    //using (WebResponse wr = WebRequest.Create(pythonURL).GetResponse())
                    //{
                    //    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    //    {
                    //        content = sr.ReadToEnd();
                    //    }
                    //}

                    //if (System.IO.File.Exists(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt")))
                    //    System.IO.File.Delete(System.IO.Path.Combine(FrontOfficeURL, "File_JDCreate_" + LogedInUserID + ".txt"));

                    //if (!string.IsNullOrWhiteSpace(JDcreateText))
                    //{
                    //    content = await CallClaudeApi(endpoint, apiKey, JDcreateText);
                    //}

                    if (!string.IsNullOrWhiteSpace(JDcreateText))
                    {
                        content = await MakeApiCall(endpoint, apiKey, JDcreateText);
                    }


                    long gptJdId = 0;

                    if (!string.IsNullOrEmpty(JDcreateText))
                    {
                        GenGptJdresponse gptJdresponse = new()
                        {
                            Url = "",
                            Jdtext = JDcreateText,
                            ResponseData = content,
                            CreatedById = SessionValues.LoginUserId,
                            CreatedDateTime = System.DateTime.Now,
                            Gptprompt = JDcreateText
                        };

                        _iJDParse.SaveGptJdresponse(gptJdresponse).ConfigureAwait(false);

                        gptJdId = gptJdresponse.Id;
                    }

                    //var data = await ConvertStringToClaudeAIResponseModel(content.ToString().Trim(), gptJdId, ProcessType.AIGenerated.ToString());
                    var data = await ConvertStringToChatGPTAIResponseModel(content.ToString().Trim(), gptJdId, ProcessType.AIGenerated.ToString());

                    return data;
                }
                else
                    return new JobPostDetailsViewModel();
            }
            catch (Exception)
            {
                return new JobPostDetailsViewModel();
            }
        }

        private string GetJDPdfContent(PreviewJobPostUpdate previewJobPostUpdate)
        {

            string workingMode = string.Empty;


            System.Text.StringBuilder sbBody = new System.Text.StringBuilder();

            sbBody.Append("<!DOCTYPE html>");
            sbBody.Append("<html>");
            sbBody.Append("<body style=\"font-size: 12px;\">");
            //sbBody.Append($"<strong>Job Title: </strong> {previewJobPostUpdate.RoleName}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            //sbBody.Append($"<strong>Years of Experience: </strong> {previewJobPostUpdate.ExperienceYears}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            //sbBody.Append($"<strong>Employment Type: </strong> {previewJobPostUpdate.EmploymentType}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            //sbBody.Append($"<strong>Working mode: </strong> {previewJobPostUpdate.ModeOfWork}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            //sbBody.Append($"<strong>Currency: </strong> {previewJobPostUpdate.Currency}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            //sbBody.Append($"<strong>Must have skills: </strong> {previewJobPostUpdate.Skills}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            //sbBody.Append($"<strong>Good to have skills: </strong> {previewJobPostUpdate.AllSkills}");
            //sbBody.Append("<br/>");
            //sbBody.Append("<br/>");
            sbBody.Append($"<strong>Job Description </strong>");
            sbBody.Append($"{previewJobPostUpdate.JobDescription}");
            sbBody.Append("<br/>");
            sbBody.Append("<br/>");

            sbBody.Append("</body>");
            sbBody.Append("</html>");

            return sbBody.ToString();
        }

        #endregion
        public static List<string> ReplacesingleQuotes(List<string> items)
        {

            List<string> modifiedStrings = new List<string>();
            foreach (string str in items)
            {
                modifiedStrings.Add(str.Replace("�", "'"));
            }

            return modifiedStrings;
        }
        public class ExtractTextFromFile
        {
            public string? PdfText { get; set; }
        }
        #endregion

        #region Fetch SalesUSers with Heads EmailID while Crating HR
        [HttpGet("GetSalesUserWithHeadAfterHRCreate")]
        public async Task<ObjectResult> GetSalesUserWithHeadAfterHRCreate(string HRID)
        {
            try
            {
                #region Validation
                long HR_ID = Convert.ToInt64(CommonLogic.Decrypt(HRID));
                if (HR_ID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide HR." });
                }
                #endregion


                var varActiveSalesUserList = _iClient.Sproc_Get_SalesUserWithHead_FOr_HiringRequest(HR_ID);

                if (varActiveSalesUserList.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Sales User List", Details = varActiveSalesUserList });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No data" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update HR Details for Activity 
        [HttpPost("GetUpdateHRDetails")]
        public async Task<ObjectResult> GetUpdateHRDetails(long HRId, long HistoryID, long DetailHistoryID)
        {
            try
            {
                #region Pre-Validation 
                if ((HRId == null || HRId == 0) && HistoryID == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide HRId or History ID"));
                }
                #endregion
                List<Sproc_Get_UpdateHR_Details_Result> jobPreview = new List<Sproc_Get_UpdateHR_Details_Result>();
                if (HRId > 0)
                {
                    object[] param1 = new object[] { HRId, HistoryID, DetailHistoryID };
                    string paramString1 = CommonLogic.ConvertToParamString(param1);
                    jobPreview = _iJDParse.Sproc_Get_UpdateHR_Details_Result(paramString1);
                }

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "Update HR Details", jobPreview));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Backup of add/edit HR
        [HttpPost("Create_Backup")]
        public async Task<ObjectResult> CreateHiringRequest_Backup(HiringReqeustModel hiringRequestModel, long LoggedInUserId)
        {
            try
            {
                #region Pre Validation                

                LoggedInUserId = SessionValues.LoginUserId;
                int LoginUserTypeID = SessionValues.LoginUserTypeId;

                bool IsDirectHR = false;
                bool IsBDR_MDRUser = false;

                if (hiringRequestModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                bool IsPayPerHire = false;
                bool IsPayPerCredit = false;
                if (hiringRequestModel.PayPerType == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Pay per type is required" });
                }
                else
                {
                    if (hiringRequestModel.PayPerType == 1)
                        IsPayPerHire = true;
                    else if (hiringRequestModel.PayPerType == 2)
                        IsPayPerCredit = true;
                    else
                        IsPayPerHire = true;
                }

                IsDirectHR = hiringRequestModel.isDirectHR ?? false;
                IsBDR_MDRUser = LoginUserTypeID == 11 || LoginUserTypeID == 12 ? true : false;

                if (!hiringRequestModel.issaveasdraft)
                {
                    if (hiringRequestModel.directPlacement == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                    }
                    if (IsPayPerHire) //Pay per hire only we get this details
                    {
                        if (hiringRequestModel.isHRTypeDP)
                        {
                            if (string.IsNullOrEmpty(hiringRequestModel.directPlacement.DpPercentage.ToString()))
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Uplers fees is mandatory when HR type is Direct Placement" });
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(hiringRequestModel.NRMargin.ToString()))
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Uplers fees is mandatory when HR type is Contruatual" });
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(hiringRequestModel.directPlacement.ModeOfWork))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Mode of Work is mandatory" });
                    }
                    if (hiringRequestModel.directPlacement.ModeOfWork == "Hybrid" || hiringRequestModel.directPlacement.ModeOfWork == "Office")
                    {
                        DirectPlacementValidator validationRules = new DirectPlacementValidator();
                        ValidationResult validationResult = validationRules.Validate(hiringRequestModel.directPlacement);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseObject()
                                {
                                    statusCode = StatusCodes.Status400BadRequest,
                                    Message = "Validation Error",
                                    Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestModel")
                                });
                        }
                    }
                }
                #endregion

                #region Validation

                if (!hiringRequestModel.issaveasdraft)
                {
                    if (IsDirectHR && IsBDR_MDRUser || IsPayPerCredit)
                    {
                        DirectHR_or_CreditHR_ModelValidator validationRules = new DirectHR_or_CreditHR_ModelValidator();
                        ValidationResult validationResult = validationRules.Validate(hiringRequestModel);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestModel") });
                        }
                    }
                    else
                    {
                        HiringReqeustModelValidator validationRules = new HiringReqeustModelValidator();
                        ValidationResult validationResult = validationRules.Validate(hiringRequestModel);
                        if (!validationResult.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestModel") });
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(hiringRequestModel.clientName))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill Client Name" });
                    }

                    //UTS-3885: Sales userID should be a required field while adding Draft HR.    
                    if (Convert.ToInt32(hiringRequestModel.salesPerson) == 0)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select Sales Person" });
                    }
                }

                #endregion

                GenSalesHiringRequest hiringrequest = new GenSalesHiringRequest();

                GenDirectPlacement directPlacement = new GenDirectPlacement();
                DirectPlacementViewModel directplacementModel = new DirectPlacementViewModel();
                GenSalesHiringRequestHistory hiringRequestHistory = new GenSalesHiringRequestHistory();
                if (!string.IsNullOrEmpty(hiringRequestModel.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(hiringRequestModel.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(hiringRequestModel.en_Id)) > 0)
                {
                    hiringRequestModel.Id = Convert.ToInt64(CommonLogic.Decrypt(Convert.ToString(hiringRequestModel.en_Id)));
                    hiringrequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hiringRequestModel.Id);

                    object[] HRID = new object[] { hiringRequestModel.Id };
                    #region conditional validation
                    if (hiringrequest != null)
                    {
                        if ((IsDirectHR && IsBDR_MDRUser) || IsPayPerCredit || !string.IsNullOrEmpty(hiringrequest.Guid))
                        {
                            if (IsPayPerCredit && hiringRequestModel.IsHiringLimited == null)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please select Hiring limited" });
                            }
                        }
                        else
                        {
                            if (!hiringRequestModel.issaveasdraft && string.IsNullOrEmpty(hiringRequestModel.discoveryCallLink))
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill Discovery Call Link" });
                            }
                            if (!hiringRequestModel.issaveasdraft && string.IsNullOrEmpty(hiringRequestModel.bqFormLink))
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill bq Form Link" });
                            }
                        }
                    }
                    #endregion

                    #region UpdateTalentContractualDPConversion
                    bool isHrTypeDP = false;

                    if (hiringrequest != null)
                    {
                        isHrTypeDP = hiringrequest.IsHrtypeDp; //store the previous HR type before editing.
                    }

                    if (Convert.ToBoolean(hiringRequestModel.AllowSpecialEdit))
                    {
                        //check if the type of HR is changed or not.
                        bool isHrTypeChanged = isHrTypeDP != hiringRequestModel.isHRTypeDP;

                        //Save the HR type conversion.
                        if (isHrTypeChanged)
                        {
                            ConvertTalentsToContractualORDP(hiringRequestModel, hiringrequest.Id);
                        }
                    }
                    #endregion

                    #region Update Hiring/Request
                    hiringrequest = _talentConnectAdminDBContext.GenSalesHiringRequests.FirstOrDefault(xy => xy.Id == hiringRequestModel.Id);
                    GenSalesHiringRequest genSalesHiringRequestUpdated = ModelBinder.BindgenSalesHiringRequest(hiringrequest, hiringRequestModel, LoggedInUserId);
                    CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestUpdated, EntityState.Modified);
                    #endregion

                    #region update directPlacement
                    directPlacement = _talentConnectAdminDBContext.GenDirectPlacements.FirstOrDefault(xy => xy.HiringRequestId == hiringRequestModel.Id);
                    GenDirectPlacement genDirectPlacementUpdated = ModelBinder.BindgenDirectPlacement(directPlacement, hiringRequestModel, hiringrequest.Id, LoggedInUserId);
                    if (directPlacement == null)
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, genDirectPlacementUpdated, EntityState.Added);
                    else
                        CommonLogic.DBOperator(_talentConnectAdminDBContext, genDirectPlacementUpdated, EntityState.Modified);
                    #endregion

                    #region Update Hiring/Request Detail

                    GenSalesHiringRequestDetail genSalesHiringRequestDetail = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(xy => xy.HiringRequestId == hiringRequestModel.Id).FirstOrDefault();
                    GenSalesHiringRequestDetail genSalesHiringRequestDetailUpdated = ModelBinder.BindgenSalesHiringRequestDetail(genSalesHiringRequestDetail, hiringRequestModel, hiringrequest.Id, LoggedInUserId, _talentConnectAdminDBContext);

                    //UTS-3959: If other role is added then insert that role in the master table
                    SaveOtherRoleWhileAddEditHR(ref genSalesHiringRequestDetailUpdated);

                    CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestDetailUpdated, EntityState.Modified);

                    #endregion

                    #region Update CalculatedUplersfees through SP

                    if (IsPayPerHire && hiringRequestModel.Id > 0
                        && (hiringRequestModel.BudgetType == "1" || hiringRequestModel.BudgetType == "2"))
                    {

                        _universalProcRunner.Manipulation(Constants.ProcConstant.SP_PayPerHire_Calculation_Update, HRID);
                    }

                    #endregion

                    #region HR Partnership Update Info

                    var IsPartnershipUserDetails = _talentConnectAdminDBContext.UsrUsers
                        .Where(x => x.Id == hiringRequestModel.salesPerson && x.IsPartnerUser == true).FirstOrDefault();

                    if (IsPartnershipUserDetails != null)
                    {
                        object[] param = new object[]
                        {
                            hiringRequestModel.Id, hiringRequestModel.contactId, LoggedInUserId, hiringRequestModel.ChildCompanyName
                        };

                        string paramasString = CommonLogic.ConvertToParamString(param);
                        _commonInterface.hiringRequest.sproc_UTS_UpdatePartnershipDetails_ForHR(paramasString);
                    }

                    #endregion

                    #region Update-DiscoveryCall

                    long ContactId = Convert.ToInt64(hiringRequestModel.contactId);
                    GenCompany _Company = null;
                    GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();
                    if (_Contact != null && _Contact.CompanyId > 0)
                    {
                        _Company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == _Contact.CompanyId).FirstOrDefault();
                    }
                    if (IsPayPerHire && _Company != null)//for pay per hire only
                    {
                        _Company.Company = hiringRequestModel.companyName;
                        if (string.IsNullOrEmpty(_Company.DiscoveryCall) && !string.IsNullOrEmpty(hiringRequestModel.discoveryCallLink))
                        {
                            _Company.DiscoveryCall = hiringRequestModel.discoveryCallLink;
                        }
                        _talentConnectAdminDBContext.Entry(_Company).State = EntityState.Modified;
                        _talentConnectAdminDBContext.SaveChanges();

                        #region UTS-6158 - Maintain history of Gen_Company
                        if (_Company.Id > 0)
                            _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_Company_History, new object[] { _Company.Id });
                        #endregion
                    }
                    #endregion

                    #region Insert History for Update Hiring Request

                    if (hiringRequestModel.issaveasdraft)
                    {
                        object[] param2 = new object[]
                        {
                            Action_Of_History.HR_SaveAsDraft, CommonLogic.Decrypt(hiringRequestModel.en_Id), 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);
                    }

                    else
                    {

                        object[] param3 = new object[]
                        {
                            Action_Of_History.Update_HR, CommonLogic.Decrypt(hiringRequestModel.en_Id), 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param3);
                    }

                    #endregion

                    //UTS-3998: Allowing COE team and Darshan to edit HR
                    #region SpecialUpdate

                    if (Convert.ToBoolean(hiringRequestModel.AllowSpecialEdit))
                    {
                        object[] param = new object[] {

                           hiringRequestModel.Id,
                           genSalesHiringRequestDetailUpdated.Id,
                           LoggedInUserId
                        };
                        string paramasString = CommonLogic.ConvertToParamString(param);
                        _commonInterface.hiringRequest.Sproc_Update_HRDetails_From_Special_User(paramasString);

                        //Maintain History
                        object[] param3 = new object[]
                        {
                            Action_Of_History.EditHR_COETeam, CommonLogic.Decrypt(hiringRequestModel.en_Id), 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param3);
                    }

                    #endregion

                    #region Get InterviewerDetails(Primary, Secondary)

                    hiringRequestModel.interviewerDetails = new();
                    hiringRequestModel.interviewerDetails.primaryInterviewer = new();
                    hiringRequestModel.interviewerDetails.secondaryinterviewerList = new();

                    var SalesHiringRequestInterviewerList =
                        _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                        .Where(z => z.HiringRequestId == hiringRequestModel.Id).ToList();

                    if (SalesHiringRequestInterviewerList.Any())
                    {
                        int count = 1;
                        foreach (var HiringRequestInterviwerDetailList in SalesHiringRequestInterviewerList)
                        {
                            if (count == 1)
                            {
                                hiringRequestModel.interviewerDetails.primaryInterviewer.interviewerId = HiringRequestInterviwerDetailList.Id;
                                hiringRequestModel.interviewerDetails.primaryInterviewer.fullName = HiringRequestInterviwerDetailList.InterviewerName;
                                hiringRequestModel.interviewerDetails.primaryInterviewer.emailID = HiringRequestInterviwerDetailList.InterviewerEmailId;
                                hiringRequestModel.interviewerDetails.primaryInterviewer.linkedin = HiringRequestInterviwerDetailList.InterviewLinkedin;
                                hiringRequestModel.interviewerDetails.primaryInterviewer.designation = HiringRequestInterviwerDetailList.InterviewerDesignation;
                                hiringRequestModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                            }
                            else
                            {
                                hiringRequestModel.interviewerDetails.secondaryinterviewerList
                                .Add(new InterviewerClass
                                {
                                    interviewerId = HiringRequestInterviwerDetailList.Id,
                                    fullName = HiringRequestInterviwerDetailList.InterviewerName,
                                    linkedin = HiringRequestInterviwerDetailList.InterviewLinkedin,
                                    emailID = HiringRequestInterviwerDetailList.InterviewerEmailId,
                                    designation = HiringRequestInterviwerDetailList.InterviewerDesignation,
                                    isUserAddMore = true
                                });
                            }
                            count++;
                        }
                    }
                    else
                    {
                        if (_Contact != null)// Get default primary interviewer details from gen_contact
                        {
                            hiringRequestModel.interviewerDetails.primaryInterviewer.interviewerId = 0;
                            hiringRequestModel.interviewerDetails.primaryInterviewer.fullName = _Contact.FullName;
                            hiringRequestModel.interviewerDetails.primaryInterviewer.emailID = _Contact.EmailId;
                            hiringRequestModel.interviewerDetails.primaryInterviewer.linkedin = _Contact.LinkedIn;
                            hiringRequestModel.interviewerDetails.primaryInterviewer.designation = _Contact.Designation;
                            hiringRequestModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                        }
                    }

                    #endregion

                    #region CompanyInfo
                    hiringRequestModel.companyInfo = null;
                    if (_Company != null)
                    {
                        hiringRequestModel.companyInfo = new CompanyInfo();
                        hiringRequestModel.companyInfo.companyID = _Company.Id;
                        hiringRequestModel.companyInfo.companyName = _Company.Company ?? "";
                        hiringRequestModel.companyInfo.industry = _Company.Industry ?? _Company.IndustryType ?? "";
                        hiringRequestModel.companyInfo.website = _Company.Website ?? "";
                        hiringRequestModel.companyInfo.linkedInURL = _Company.LinkedInProfile ?? "";
                        hiringRequestModel.companyInfo.companySize = _Company.CompanySize ?? 0;
                        hiringRequestModel.companyInfo.aboutCompanyDesc = _Company.AboutCompanyDesc ?? "";
                    }
                    #endregion

                    #region Insert History of HR in gen_SalesHiringRequest_History & gen_SalesHiringRequest_Details_History
                    object[] HistoryObj = new object[] { hiringRequestModel.Id, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_SalesHiringRequest_History, HistoryObj);
                    _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_SalesHiringRequest_Details_History, HistoryObj);
                    #endregion

                    #region ATSCall
                    if (!hiringRequestModel.issaveasdraft)
                    {
                        if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                        {
                            _talentConnectAdminDBContext.Entry(genSalesHiringRequestUpdated).Reload();

                            if (genSalesHiringRequestUpdated != null)
                            {
                                long HR_ID = genSalesHiringRequestUpdated.Id;
                                string HR_Number = genSalesHiringRequestUpdated.HrNumber ?? "";
                                if (HR_ID != 0 && !string.IsNullOrEmpty(HR_Number))
                                {
                                    var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(HR_ID).ConfigureAwait(false);
                                    string HRJsonData = Convert.ToString(HRData_Json);
                                    if (!string.IsNullOrEmpty(HRJsonData))
                                    {
                                        bool isAPIResponseSuccess = true;

                                        try
                                        {
                                            ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                            if (HRJsonData != "")
                                                isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HR_ID);
                                        }
                                        catch (Exception)
                                        {
                                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Hiring reqest updated successfully", Details = hiringRequestModel });
                }
                else
                {
                    #region conditional validation
                    if ((IsDirectHR && IsBDR_MDRUser || IsPayPerCredit))
                    {
                        //TODO
                        if (IsPayPerCredit)//Required condition only in Create HR 
                        {
                            hiringRequestModel.IsPostaJob = hiringRequestModel.IsPostaJob ?? false;
                            hiringRequestModel.IsProfileView = hiringRequestModel.IsProfileView ?? false;
                            if (!(bool)hiringRequestModel.IsPostaJob && !(bool)hiringRequestModel.IsProfileView)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please check any one of them, IsPostaJob or IsProfileView" });
                            }
                        }
                    }
                    else
                    {
                        if (!hiringRequestModel.issaveasdraft && string.IsNullOrEmpty(hiringRequestModel.discoveryCallLink))
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill Discovery Call Link" });
                        }
                        if (!hiringRequestModel.issaveasdraft && string.IsNullOrEmpty(hiringRequestModel.bqFormLink))
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please fill bq Form Link" });
                        }
                    }
                    #endregion

                    #region Insert Hiring/Request

                    GenSalesHiringRequest genSalesHiringRequestCreate = ModelBinder.BindgenSalesHiringRequest(hiringrequest, hiringRequestModel, LoggedInUserId, true);

                    _talentConnectAdminDBContext.GenSalesHiringRequests.Add(genSalesHiringRequestCreate);
                    _talentConnectAdminDBContext.SaveChanges();
                    hiringRequestModel.en_Id = CommonLogic.Encrypt(genSalesHiringRequestCreate.Id.ToString());

                    GenDirectPlacement genDirectPlacementCreate = ModelBinder.BindgenDirectPlacement(directPlacement, hiringRequestModel, hiringrequest.Id, LoggedInUserId);
                    _talentConnectAdminDBContext.GenDirectPlacements.Add(genDirectPlacementCreate);
                    _talentConnectAdminDBContext.SaveChanges();

                    #endregion

                    #region Insert Hiring/Request Detail

                    GenSalesHiringRequestDetail genSalesHiringRequestDetail = new GenSalesHiringRequestDetail();
                    GenSalesHiringRequestDetail genSalesHiringRequestDetailCreated = ModelBinder.BindgenSalesHiringRequestDetail(genSalesHiringRequestDetail, hiringRequestModel, hiringrequest.Id, LoggedInUserId, _talentConnectAdminDBContext);

                    //UTS-3959: If other role is added then insert that role in the master table
                    SaveOtherRoleWhileAddEditHR(ref genSalesHiringRequestDetailCreated);

                    _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Add(genSalesHiringRequestDetailCreated);
                    _talentConnectAdminDBContext.SaveChanges();

                    #endregion

                    #region Update CalculatedUplersfees through SP

                    if (IsPayPerHire && genSalesHiringRequestCreate.Id > 0 &&
                        (hiringRequestModel.BudgetType == "1" || hiringRequestModel.BudgetType == "2"))
                    {
                        object[] HRID = new object[] { genSalesHiringRequestCreate.Id };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.SP_PayPerHire_Calculation_Update, HRID);
                    }
                    #endregion

                    #region HR Partnership Update Info

                    var IsPartnershipUserDetails = _talentConnectAdminDBContext.UsrUsers
                        .Where(x => x.Id == hiringRequestModel.salesPerson && x.IsPartnerUser == true).FirstOrDefault();

                    if (IsPartnershipUserDetails != null)
                    {
                        object[] param = new object[]
                        {
                            genSalesHiringRequestCreate.Id, hiringRequestModel.contactId, LoggedInUserId, hiringRequestModel.ChildCompanyName
                        };

                        string paramasString = CommonLogic.ConvertToParamString(param);
                        _commonInterface.hiringRequest.sproc_UTS_UpdatePartnershipDetails_ForHR(paramasString);
                    }

                    #endregion


                    #region Update-DiscoveryCall & IsTransparentPricing flag in gen_company

                    long ContactId = Convert.ToInt64(hiringRequestModel.contactId);
                    GenContact _Contact = _talentConnectAdminDBContext.GenContacts.Where(x => x.Id == ContactId).FirstOrDefault();
                    GenCompany _Company = null;
                    if (_Contact != null && _Contact.CompanyId > 0)
                    {
                        _Company = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == _Contact.CompanyId).FirstOrDefault();
                        if (_Company != null)
                        {
                            _Company.Company = hiringRequestModel.companyName;
                            if (string.IsNullOrEmpty(_Company.DiscoveryCall) && !string.IsNullOrEmpty(hiringRequestModel.discoveryCallLink))
                            {
                                _Company.DiscoveryCall = hiringRequestModel.discoveryCallLink;
                            }

                            //IsTransparentPricing update in company, only when its not updated yet
                            if (_Company.IsTransparentPricing == null && hiringRequestModel.IsTransparentPricing != null)
                            {
                                _Company.IsTransparentPricing = hiringRequestModel.IsTransparentPricing;
                            }

                            _talentConnectAdminDBContext.Entry(_Company).State = EntityState.Modified;
                            _talentConnectAdminDBContext.SaveChanges();

                            #region UTS-6158 - Maintain history of Gen_Company
                            if (_Company.Id > 0)
                                _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_Company_History, new object[] { _Company.Id });
                            #endregion
                        }
                    }

                    #endregion

                    #region Contact as Active if its not active

                    object[] contact = new object[] { hiringRequestModel.contactId };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.Sproc_UTS_ContactAsActive, contact);

                    #endregion

                    #region Insert History for New Hiring Request

                    if (hiringRequestModel.issaveasdraft)
                    {
                        object[] param2 = new object[]
                        {
                            Action_Of_History.HR_SaveAsDraft, CommonLogic.Decrypt(hiringRequestModel.en_Id) , 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param2);
                    }
                    else
                    {
                        object[] param3 = new object[]
                        {
                            Action_Of_History.Create_HR, CommonLogic.Decrypt(hiringRequestModel.en_Id), 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param3);
                    }

                    #endregion

                    #region Get InterviewerDetails(Primary, Secondary)

                    hiringRequestModel.interviewerDetails = new();
                    hiringRequestModel.interviewerDetails.primaryInterviewer = new();
                    hiringRequestModel.interviewerDetails.secondaryinterviewerList = null;

                    if (_Contact != null)// Get default primary interviewer details from gen_contact
                    {
                        hiringRequestModel.interviewerDetails.primaryInterviewer.interviewerId = 0;
                        hiringRequestModel.interviewerDetails.primaryInterviewer.fullName = _Contact.FullName;
                        hiringRequestModel.interviewerDetails.primaryInterviewer.emailID = _Contact.EmailId;
                        hiringRequestModel.interviewerDetails.primaryInterviewer.linkedin = _Contact.LinkedIn;
                        hiringRequestModel.interviewerDetails.primaryInterviewer.designation = _Contact.Designation;
                        hiringRequestModel.interviewerDetails.primaryInterviewer.isUserAddMore = false;
                    }
                    #endregion

                    #region Insert History of HR in gen_SalesHiringRequest_History & gen_SalesHiringRequest_Details_History
                    object[] HistoryObj = new object[] { genSalesHiringRequestCreate.Id, (short)AppActionDoneBy.UTS };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_SalesHiringRequest_History, HistoryObj);
                    _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_SalesHiringRequest_Details_History, HistoryObj);
                    #endregion

                    #region CompanyInfo
                    hiringRequestModel.companyInfo = null;
                    if (_Company != null)
                    {
                        hiringRequestModel.companyInfo = new CompanyInfo();
                        hiringRequestModel.companyInfo.companyID = _Company.Id;
                        hiringRequestModel.companyInfo.companyName = _Company.Company ?? "";
                        hiringRequestModel.companyInfo.industry = _Company.Industry ?? _Company.IndustryType ?? "";
                        hiringRequestModel.companyInfo.website = _Company.Website ?? "";
                        hiringRequestModel.companyInfo.linkedInURL = _Company.LinkedInProfile ?? "";
                        hiringRequestModel.companyInfo.companySize = _Company.CompanySize ?? 0;
                        hiringRequestModel.companyInfo.aboutCompanyDesc = _Company.AboutCompanyDesc ?? "";
                    }
                    #endregion

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Hiring request created successfully", Details = hiringRequestModel });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Debriefing/Create_Backup")]
        public async Task<ObjectResult> AddDebriefingHR_Backup(HiringRequestDebriefModel hiringRequestDebriefModel, int LoggedInUserId = 0)
        {
            LoggedInUserId = Convert.ToInt32(SessionValues.LoginUserId);
            int LoginUserTypeID = SessionValues.LoginUserTypeId;

            bool IsDirectHR = false;
            bool IsBDR_MDRUser = LoginUserTypeID == 11 || LoginUserTypeID == 12 ? true : false;

            GenSalesHiringRequestDetail hiringrequestdetails = new GenSalesHiringRequestDetail();
            GenSalesHiringRequest genSalesHiringRequestUpdated = new GenSalesHiringRequest();

            #region PreValidation

            if (hiringRequestDebriefModel == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
            }
            IsDirectHR = hiringRequestDebriefModel.isDirectHR ?? false;

            bool IsPayPerHire = false;
            bool IsPayPerCredit = false;
            if (hiringRequestDebriefModel.PayPerType == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Pay per type is required" });
            }
            else
            {
                if (hiringRequestDebriefModel.PayPerType == 1)
                    IsPayPerHire = true;
                else if (hiringRequestDebriefModel.PayPerType == 2)
                    IsPayPerCredit = true;
                else
                    IsPayPerHire = true;
            }
            #endregion

            #region Validation
            if (IsDirectHR && IsBDR_MDRUser || IsPayPerCredit)
            {
                DirectHR_or_CreditHR_Debrief_ModelValidator validationRules = new DirectHR_or_CreditHR_Debrief_ModelValidator();
                ValidationResult validationResult = validationRules.Validate(hiringRequestDebriefModel);

                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestDebriefModel") });
                }
            }
            else
            {
                HiringRequestDebriefModelValidator validationRules = new HiringRequestDebriefModelValidator();
                ValidationResult validationResult = validationRules.Validate(hiringRequestDebriefModel);

                if (!validationResult.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationResult.Errors, "hiringRequestDebriefModel") });
                }

                if (hiringRequestDebriefModel.interviewerDetails != null)
                {
                    //primaryInterviewer Validation
                    if (hiringRequestDebriefModel.interviewerDetails.primaryInterviewer != null)
                    {
                        interviewerValidator validationsforInterviewer = new interviewerValidator();
                        ValidationResult validationResult2 = validationsforInterviewer.Validate(hiringRequestDebriefModel.interviewerDetails.primaryInterviewer);
                        if (!validationResult2.IsValid)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseObject()
                                {
                                    statusCode = StatusCodes.Status400BadRequest,
                                    Message = "Validation Error",
                                    Details = CommonLogic.SerializeErrors(validationResult2.Errors, "primaryInterviewer")
                                });
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                                new ResponseObject()
                                {
                                    statusCode = StatusCodes.Status400BadRequest,
                                    Message = "Primary Interviewer Required",
                                    Details = CommonLogic.SerializeErrors(validationResult.Errors, "primaryInterviewer")
                                });
                    }

                    //secondaryinterviewer Validation
                    if (hiringRequestDebriefModel.interviewerDetails.secondaryinterviewerList.Any())
                    {
                        foreach (var t in hiringRequestDebriefModel.interviewerDetails.secondaryinterviewerList)
                        {
                            interviewerValidator validationsforInterviewer = new interviewerValidator();
                            ValidationResult validationResult3 = validationsforInterviewer.Validate(t);
                            if (!validationResult3.IsValid)
                            {
                                return StatusCode(StatusCodes.Status400BadRequest,
                                    new ResponseObject()
                                    {
                                        statusCode = StatusCodes.Status400BadRequest,
                                        Message = "Validation Error",
                                        Details = CommonLogic.SerializeErrors(validationResult3.Errors, "secondaryInterviewer")
                                    });
                            }
                        }
                    }
                }
            }

            #endregion

            #region Update Debrifing & company info

            if (!string.IsNullOrEmpty(hiringRequestDebriefModel.en_Id) && !string.IsNullOrEmpty(CommonLogic.Decrypt(hiringRequestDebriefModel.en_Id)) && Convert.ToInt64(CommonLogic.Decrypt(hiringRequestDebriefModel.en_Id)) > 0)
            {
                #region Update gen_SalesHiringRequest_Details table

                hiringRequestDebriefModel.hrID = Convert.ToInt64(CommonLogic.Decrypt(hiringRequestDebriefModel.en_Id));
                hiringrequestdetails = await _talentConnectAdminDBContext.GenSalesHiringRequestDetails.FirstOrDefaultAsync(xy => xy.HiringRequestId == hiringRequestDebriefModel.hrID);

                #region RoleID
                //get roleid based on role title if its DirectHr or credit based HR,
                //if its not there automatically created new entry in prg_TalentRoles
                if (hiringRequestDebriefModel.role == null)
                {
                    object[] param = new object[] { LoggedInUserId, hiringRequestDebriefModel.hrTitle };
                    string paramstring = CommonLogic.ConvertToParamString(param);

                    List<Sproc_FetchSimilarRoles_UTS_Result> result = _iHiringRequest.Sproc_FetchSimilarRoles_UTS(paramstring);

                    if (result != null && result.Any())
                    {
                        hiringrequestdetails.RoleId = (int)result.FirstOrDefault().RoleID;
                        hiringRequestDebriefModel.role = (int)result.FirstOrDefault().RoleID;
                    }
                }
                else
                {
                    //Added because the table will not j=have this value as it is moved in the debrief section.
                    hiringrequestdetails.RoleId = hiringRequestDebriefModel.role;
                }
                #endregion

                CommonLogic.DBOperator(_talentConnectAdminDBContext, ModelBinder.BindgenSalesHiringRequestDetailForDebriefing(hiringrequestdetails, hiringRequestDebriefModel, LoggedInUserId), EntityState.Modified);

                #endregion

                #region Update gen_SalesHiringRequest table

                genSalesHiringRequestUpdated = await _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hiringRequestDebriefModel.hrID).FirstOrDefaultAsync();

                if (hiringRequestDebriefModel.companyInfo == null)
                {
                    hiringRequestDebriefModel.companyInfo = new();
                    hiringRequestDebriefModel.companyInfo.aboutCompanyDesc = hiringRequestDebriefModel.aboutCompanyDesc;
                    genSalesHiringRequestUpdated.AboutCompanyDesc = hiringRequestDebriefModel.aboutCompanyDesc;
                }
                else
                {
                    genSalesHiringRequestUpdated.AboutCompanyDesc = hiringRequestDebriefModel.companyInfo.aboutCompanyDesc;
                }

                genSalesHiringRequestUpdated.RequestForTalent = hiringRequestDebriefModel.hrTitle;

                if (!Convert.ToBoolean(hiringRequestDebriefModel.issaveasdraft))
                {
                    genSalesHiringRequestUpdated.IsActive = true;

                    if (hiringRequestDebriefModel.ActionType == "Save")
                    {
                        genSalesHiringRequestUpdated.StatusId = (short)prg_HiringRequestStatus.Open;
                        genSalesHiringRequestUpdated.JobStatusId = (short)prg_JobStatus_ClientPortal.Open;

                        //after dicussion with Soumya we comment this
                        //New Status changes
                        //if (IsPayPerHire)//Pay per hire
                        //{
                        //    genSalesHiringRequestUpdated.StatusId = (short)prg_HiringRequestStatus.Open;
                        //    genSalesHiringRequestUpdated.JobStatusId = (short)prg_JobStatus_ClientPortal.Open;
                        //}
                        //if (IsPayPerCredit)//Pay per credit
                        //{
                        //    genSalesHiringRequestUpdated.StatusId = (short)prg_HiringRequestStatus.Open;
                        //    genSalesHiringRequestUpdated.JobStatusId = (short)prg_JobStatus_ClientPortal.Active;
                        //}
                    }
                }

                genSalesHiringRequestUpdated.LastModifiedById = LoggedInUserId;
                genSalesHiringRequestUpdated.LastModifiedDatetime = DateTime.Now;
                CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestUpdated, EntityState.Modified);

                #endregion

                #region Update Company info
                await _iClient.updateGenCompany(genSalesHiringRequestUpdated.ContactId, hiringRequestDebriefModel.companyInfo);
                #endregion
            }

            #endregion

            #region Add-Skills

            //Remove the previously added skills.
            //var InsertedskillDetails = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(p => p.HiringRequestId == hiringRequestDebriefModel.hrID).ToList();
            //_talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.RemoveRange(InsertedskillDetails);
            //_talentConnectAdminDBContext.SaveChanges();

            //if (hiringRequestDebriefModel.skills.Count() > 0)
            //{
            //    foreach (var s in hiringRequestDebriefModel.skills)
            //    {
            //        // If skill ID is blank then fetch from temp table.
            //        string? skillID = string.Empty;
            //        if (s.skillsID != null && s.skillsID != "0")
            //        {
            //            skillID = s.skillsID;
            //        }
            //        else
            //        {
            //            skillID = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill == s.skillsName).Select(x => x.TempSkillId).FirstOrDefault();
            //            s.skillsID = skillID ?? "";
            //        }

            //        // If skills does not exist then insert in the temp table.
            //        if (string.IsNullOrEmpty(skillID))
            //        {
            //            object[] param = new object[] { s.skillsName, LoggedInUserId, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
            //            var result = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
            //            if (result != null)
            //            {
            //                skillID = result.TempSkill_ID;
            //                s.skillsID = result.TempSkill_ID;
            //            }
            //        }

            //        if (!string.IsNullOrEmpty(skillID))
            //        {
            //            GenSalesHiringRequestSkillDetail newSkill = new GenSalesHiringRequestSkillDetail();
            //            var checkexists = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(x => x.HiringRequestId == hiringRequestDebriefModel.hrID && (skillID.Contains("O_") ? x.TempSkillId == skillID : x.SkillId == Convert.ToInt32(skillID))).FirstOrDefault();
            //            if (checkexists == null)
            //            {
            //                if (skillID.Contains("O_"))
            //                {
            //                    newSkill.TempSkillId = skillID;
            //                }
            //                else
            //                {
            //                    newSkill.SkillId = Convert.ToInt32(skillID);
            //                }
            //                newSkill.HiringRequestId = hiringRequestDebriefModel.hrID;
            //                newSkill.CreatedById = Convert.ToInt32(LoggedInUserId);
            //                newSkill.CreatedByDatetime = DateTime.Now;
            //                newSkill.Proficiency = "Strong";
            //                _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Add(newSkill);
            //                _talentConnectAdminDBContext.SaveChanges();
            //            }
            //        }
            //    }
            //}

            ////UTS-4752: Add Good to have skills
            //if (hiringRequestDebriefModel.Allskills != null)
            //{
            //    if (hiringRequestDebriefModel.Allskills.Count() > 0)
            //    {
            //        foreach (var s in hiringRequestDebriefModel.Allskills)
            //        {
            //            // If skill ID is blank then fetch from temp table.
            //            string? skillID = string.Empty;
            //            if (s.skillsID != null && s.skillsID != "0")
            //            {
            //                skillID = s.skillsID;
            //            }
            //            else
            //            {
            //                skillID = _talentConnectAdminDBContext.PrgTempSkills.Where(x => x.TempSkill == s.skillsName).Select(x => x.TempSkillId).FirstOrDefault();
            //                s.skillsID = skillID ?? "";
            //            }

            //            // If skills does not exist then insert in the temp table.
            //            if (string.IsNullOrEmpty(skillID))
            //            {
            //                object[] param = new object[] { s.skillsName, LoggedInUserId, DateTime.Now.ToString("yyyy-MM-dd"), 0, DateTime.Now.ToString("yyyy-MM-dd"), true };
            //                var result = _iMasters.AddSkills(CommonLogic.ConvertToParamString(param));
            //                if (result != null)
            //                {
            //                    skillID = result.TempSkill_ID;
            //                    s.skillsID = result.TempSkill_ID;
            //                }
            //            }

            //            if (!string.IsNullOrEmpty(skillID))
            //            {
            //                GenSalesHiringRequestSkillDetail newSkill = new GenSalesHiringRequestSkillDetail();
            //                var checkexists = _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Where(x => x.HiringRequestId == hiringRequestDebriefModel.hrID && (skillID.Contains("O_") ? x.TempSkillId == skillID : x.SkillId == Convert.ToInt32(skillID))).FirstOrDefault();
            //                if (checkexists == null)
            //                {
            //                    if (skillID.Contains("O_"))
            //                    {
            //                        newSkill.TempSkillId = skillID;
            //                    }
            //                    else
            //                    {
            //                        newSkill.SkillId = Convert.ToInt32(skillID);
            //                    }
            //                    newSkill.HiringRequestId = hiringRequestDebriefModel.hrID;
            //                    newSkill.CreatedById = Convert.ToInt32(LoggedInUserId);
            //                    newSkill.CreatedByDatetime = DateTime.Now;
            //                    newSkill.Proficiency = "Basic";
            //                    _talentConnectAdminDBContext.GenSalesHiringRequestSkillDetails.Add(newSkill);
            //                    _talentConnectAdminDBContext.SaveChanges();
            //                }
            //            }
            //        }
            //    }
            //}


            #endregion

            #region JD Info
            if (!string.IsNullOrEmpty(genSalesHiringRequestUpdated.Jdfilename) &&
                                    !(genSalesHiringRequestUpdated.Jdfilename.ToLower().EndsWith(".jpg") ||
                                    genSalesHiringRequestUpdated.Jdfilename.ToLower().EndsWith(".png") ||
                                    genSalesHiringRequestUpdated.Jdfilename.ToLower().EndsWith(".jpeg")))
                SaveJDDetailsAndSkills(genSalesHiringRequestUpdated.ContactId.Value, hiringRequestDebriefModel, false, genSalesHiringRequestUpdated.Id);
            else if (hiringRequestDebriefModel.skills != null || hiringRequestDebriefModel.Allskills != null)
                SaveJDDetailsAndSkills(genSalesHiringRequestUpdated.ContactId.Value, hiringRequestDebriefModel, false, genSalesHiringRequestUpdated.Id);
            #endregion

            #region Insert/Update Interviewer Details

            if (hiringRequestDebriefModel.interviewerDetails != null)
            {
                #region Primary Interviewer Details
                if (hiringRequestDebriefModel.interviewerDetails.primaryInterviewer != null)
                {
                    if (hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.interviewerId > 0) //Update
                    {
                        var checkexistInterviewerDetailsPrimary = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                            .Where(x => x.HiringRequestId == hiringRequestDebriefModel.hrID
                            && x.HiringRequestDetailId == hiringrequestdetails.Id
                            && x.Id == hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.interviewerId).FirstOrDefault();

                        if (checkexistInterviewerDetailsPrimary != null)
                        {
                            checkexistInterviewerDetailsPrimary.HiringRequestDetailId = hiringrequestdetails.Id;
                            checkexistInterviewerDetailsPrimary.HiringRequestId = hiringRequestDebriefModel.hrID;
                            checkexistInterviewerDetailsPrimary.InterviewerName = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.fullName;
                            checkexistInterviewerDetailsPrimary.InterviewerEmailId = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.emailID;
                            checkexistInterviewerDetailsPrimary.InterviewLinkedin = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.linkedin;
                            checkexistInterviewerDetailsPrimary.InterviewerDesignation = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.designation;
                            checkexistInterviewerDetailsPrimary.ContactId = genSalesHiringRequestUpdated.ContactId;
                            checkexistInterviewerDetailsPrimary.IsUsedAddMore = false;
                            _talentConnectAdminDBContext.Update(checkexistInterviewerDetailsPrimary);
                            _talentConnectAdminDBContext.SaveChanges();
                        }
                    }
                    else //Insert
                    {
                        GenSalesHiringRequestInterviewerDetail genSalesHiringRequestInterviewerDetails = new GenSalesHiringRequestInterviewerDetail();

                        var checkIfAlreadyExists = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                                .Where(x => x.HiringRequestId == hiringRequestDebriefModel.hrID
                                && x.HiringRequestDetailId == hiringrequestdetails.Id
                                && x.InterviewerName == hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.fullName
                                && x.InterviewerEmailId == hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.emailID
                                && x.InterviewLinkedin == hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.linkedin
                                && x.InterviewerDesignation == hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.designation).FirstOrDefault();

                        if (checkIfAlreadyExists == null)
                        {
                            genSalesHiringRequestInterviewerDetails.HiringRequestDetailId = hiringrequestdetails.Id;
                            genSalesHiringRequestInterviewerDetails.HiringRequestId = hiringRequestDebriefModel.hrID;
                            genSalesHiringRequestInterviewerDetails.InterviewerName = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.fullName;
                            genSalesHiringRequestInterviewerDetails.InterviewerEmailId = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.emailID;
                            genSalesHiringRequestInterviewerDetails.InterviewLinkedin = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.linkedin;
                            genSalesHiringRequestInterviewerDetails.InterviewerDesignation = hiringRequestDebriefModel.interviewerDetails.primaryInterviewer.designation;
                            genSalesHiringRequestInterviewerDetails.ContactId = genSalesHiringRequestUpdated.ContactId;
                            genSalesHiringRequestInterviewerDetails.IsUsedAddMore = false;
                            _talentConnectAdminDBContext.Add(genSalesHiringRequestInterviewerDetails);
                            _talentConnectAdminDBContext.SaveChanges();
                        }
                    }
                }
                #endregion

                #region Secondary Interviewer Details
                if (hiringRequestDebriefModel.interviewerDetails.secondaryinterviewerList != null && hiringRequestDebriefModel.interviewerDetails.secondaryinterviewerList.Any())
                {
                    foreach (var obj in hiringRequestDebriefModel.interviewerDetails.secondaryinterviewerList)
                    {
                        if (obj.interviewerId > 0) //Update
                        {
                            var checkexistInterviewerDetailsPrimary = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                                .Where(x => x.HiringRequestId == hiringRequestDebriefModel.hrID
                                && x.HiringRequestDetailId == hiringrequestdetails.Id
                                && x.Id == obj.interviewerId).FirstOrDefault();

                            if (checkexistInterviewerDetailsPrimary != null)
                            {
                                checkexistInterviewerDetailsPrimary.HiringRequestDetailId = hiringrequestdetails.Id;
                                checkexistInterviewerDetailsPrimary.HiringRequestId = hiringRequestDebriefModel.hrID;
                                checkexistInterviewerDetailsPrimary.InterviewerName = obj.fullName;
                                checkexistInterviewerDetailsPrimary.InterviewerEmailId = obj.emailID;
                                checkexistInterviewerDetailsPrimary.InterviewLinkedin = obj.linkedin;
                                checkexistInterviewerDetailsPrimary.InterviewerDesignation = obj.designation;
                                checkexistInterviewerDetailsPrimary.ContactId = genSalesHiringRequestUpdated.ContactId;
                                checkexistInterviewerDetailsPrimary.IsUsedAddMore = true;
                                _talentConnectAdminDBContext.Update(checkexistInterviewerDetailsPrimary);
                                _talentConnectAdminDBContext.SaveChanges();
                            }
                        }
                        else //Insert
                        {
                            GenSalesHiringRequestInterviewerDetail genSalesHiringRequestInterviewerDetails = new GenSalesHiringRequestInterviewerDetail();

                            var checkIfAlreadyExists = _talentConnectAdminDBContext.GenSalesHiringRequestInterviewerDetails
                                    .Where(x => x.HiringRequestId == hiringRequestDebriefModel.hrID
                                    && x.HiringRequestDetailId == hiringrequestdetails.Id
                                    && x.InterviewerName == obj.fullName
                                    && x.InterviewerEmailId == obj.emailID
                                    && x.InterviewLinkedin == obj.linkedin
                                    && x.InterviewerDesignation == obj.designation).FirstOrDefault();

                            if (checkIfAlreadyExists == null)
                            {
                                genSalesHiringRequestInterviewerDetails.HiringRequestDetailId = hiringrequestdetails.Id;
                                genSalesHiringRequestInterviewerDetails.HiringRequestId = hiringRequestDebriefModel.hrID;
                                genSalesHiringRequestInterviewerDetails.InterviewerName = obj.fullName;
                                genSalesHiringRequestInterviewerDetails.InterviewerEmailId = obj.emailID;
                                genSalesHiringRequestInterviewerDetails.InterviewLinkedin = obj.linkedin;
                                genSalesHiringRequestInterviewerDetails.InterviewerDesignation = obj.designation;
                                genSalesHiringRequestInterviewerDetails.ContactId = genSalesHiringRequestUpdated.ContactId;
                                genSalesHiringRequestInterviewerDetails.IsUsedAddMore = true;
                                _talentConnectAdminDBContext.Add(genSalesHiringRequestInterviewerDetails);
                                _talentConnectAdminDBContext.SaveChanges();
                            }
                        }
                    }
                }
                #endregion
            }

            #endregion

            #region Insert D-brif History Create_HRDBrifing/Update_HRDBrifing

            string action = "";

            if (hiringRequestDebriefModel.ActionType == "EDIT")
                action = Action_Of_History.Update_HRDBrifing.ToString();
            else
                action = Action_Of_History.Create_HRDBrifing.ToString();

            object[] param1 = new object[]
            {
               action, CommonLogic.Decrypt(hiringRequestDebriefModel.en_Id), 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
            };
            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param1);

            #endregion

            #region Send Emails while adding HR.
            //Send Email only when we are adding HR.
            if (hiringRequestDebriefModel.ActionType == "Save")
            {
                EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                if (hiringRequestDebriefModel.isClient)
                {
                    if (genSalesHiringRequestUpdated != null)
                    {
                        if (!genSalesHiringRequestUpdated.IsEmailSend)
                        {
                            var IsActiveDetail = genSalesHiringRequestUpdated.IsActive;
                            if (IsActiveDetail != null)
                            {
                                var isActiveValue = genSalesHiringRequestUpdated.IsActive ?? false;
                                if (isActiveValue)
                                {
                                    bool email = emailBinder.SendEmailForHRCreation(hiringRequestDebriefModel.hrID, "New");

                                    bool clientmailNotification = false;

                                    #region Send Email Notification For Client LogIn
                                    GenContact contactdetails = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == genSalesHiringRequestUpdated.ContactId && x.IsPrimary == true);
                                    if (contactdetails != null)
                                    {
                                        //var password = contactdetails.FirstName.ToLower() + "@123";
                                        var password = "Uplers@123";
                                        var ClientName = (contactdetails.FirstName + " " + contactdetails.LastName);

                                        string RoleNames = string.Join(",", (from hr in _talentConnectAdminDBContext.GenSalesHiringRequests
                                                                             join hrd in _talentConnectAdminDBContext.GenSalesHiringRequestDetails on genSalesHiringRequestUpdated.Id equals hrd.HiringRequestId
                                                                             join tr in _talentConnectAdminDBContext.PrgTalentRoles on hrd.RoleId equals tr.Id
                                                                             where hr.ContactId == genSalesHiringRequestUpdated.ContactId
                                                                             select tr.TalentRole).ToList());
                                        //if (contactdetails.IsClientNotificationSend)
                                        //{
                                        //    clientmailNotification = emailBinder.SendEmailNotificationForLogIn(contactdetails.EmailId, password, ClientName, RoleNames, Convert.ToInt64(genSalesHiringRequestUpdated.ContactId));
                                        //}
                                    }

                                    //if (email && clientmailNotification)
                                    if (email)
                                    {
                                        genSalesHiringRequestUpdated.IsEmailSend = true;
                                        CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestUpdated, EntityState.Modified);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (genSalesHiringRequestUpdated != null)
                    {
                        if (!genSalesHiringRequestUpdated.IsEmailSend)
                        {
                            var IsActiveDetail = genSalesHiringRequestUpdated.IsActive;
                            if (IsActiveDetail != null)
                            {
                                var isActiveValue = genSalesHiringRequestUpdated.IsActive ?? false;
                                if (isActiveValue)
                                {
                                    bool salesemail = emailBinder.SendEmailForHRCreation(genSalesHiringRequestUpdated.Id, "Old");
                                    //GenContact _Contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == genSalesHiringRequestUpdated.ContactId);
                                    //var IsClientNotificationSent = false;
                                    //bool clientemail = false;
                                    //if (_Contact != null)
                                    //{
                                    //    IsClientNotificationSent = _Contact.IsClientNotificationSend;
                                    //}
                                    ////Payal (08-03-22) First HR create Email send if Client IsClientNotificationSend =0
                                    //if (IsClientNotificationSent)
                                    //{
                                    //    Object[] param = new object[] { genSalesHiringRequestUpdated.ContactId };
                                    //    sproc_Get_ContactPointofContact_Result sproc_Get_ContactPointofContact = _commonInterface.interview.sproc_Get_ContactPointofContact(CommonLogic.ConvertToParamString(param));

                                    //    clientemail = emailBinder.SendEmailForHRCreationtoclient(genSalesHiringRequestUpdated.Id, sproc_Get_ContactPointofContact);
                                    //}
                                    //if (salesemail && clientemail)
                                    if (salesemail)
                                    {
                                        genSalesHiringRequestUpdated.IsEmailSend = true;
                                        CommonLogic.DBOperator(_talentConnectAdminDBContext, genSalesHiringRequestUpdated, EntityState.Modified);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            //UTS-3998: Allowing COE team and Darshan to edit HR
            #region SpecialUpdate

            if (Convert.ToBoolean(hiringRequestDebriefModel.AllowSpecialEdit))
            {
                object[] param = new object[] {

                           hiringRequestDebriefModel.hrID,
                           Convert.ToInt64(hiringrequestdetails?.Id),
                           LoggedInUserId
                        };
                string paramasString = CommonLogic.ConvertToParamString(param);
                _commonInterface.hiringRequest.Sproc_Update_HRDetails_From_Special_User(paramasString);


                //Maintain History
                object[] param3 = new object[]
                {
                            Action_Of_History.EditHR_COETeam, hiringRequestDebriefModel.hrID, 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param3);
            }

            #endregion

            #region Insert History for SalesHiringRequestDetail Table

            object[] HistoryObj = new object[] { hiringrequestdetails.HiringRequestId, (short)AppActionDoneBy.UTS };
            _universalProcRunner.Manipulation(Constants.ProcConstant.SPROC_Gen_SalesHiringRequest_Details_History, HistoryObj);

            #endregion

            #region ATSCall
            if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
            {
                _talentConnectAdminDBContext.Entry(genSalesHiringRequestUpdated).Reload();
                if (genSalesHiringRequestUpdated != null)
                {
                    long HR_ID = genSalesHiringRequestUpdated.Id;
                    string HR_Number = genSalesHiringRequestUpdated.HrNumber ?? "";
                    if (HR_ID != 0 && !string.IsNullOrEmpty(HR_Number))
                    {
                        var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(HR_ID).ConfigureAwait(false);
                        string HRJsonData = Convert.ToString(HRData_Json);
                        if (!string.IsNullOrEmpty(HRJsonData))
                        {
                            bool isAPIResponseSuccess = true;

                            try
                            {
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                                if (HRJsonData != "")
                                    isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HR_ID);

                                #region HR Status updates to ATS 

                                //New Status Change
                                if (hiringRequestDebriefModel.ActionType == "Save")
                                {
                                    object[] param = new object[] { HR_ID, 0, 0, LoggedInUserId, (short)AppActionDoneBy.UTS, false };
                                    string strParam = CommonLogic.ConvertToParamString(param);
                                    var HRStatus_Json = _iHiringRequest.GetUpdateHrStatus(strParam);
                                    if (HRStatus_Json != null)
                                    {
                                        //string JsonData = Convert.ToString(HRStatus_Json);
                                        var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                        if (!string.IsNullOrEmpty(JsonData))
                                        {
                                            aTSCall.SendHrStatusToATS(JsonData, LoggedInUserId, HR_ID);
                                        }
                                    }
                                }

                                #endregion
                            }
                            catch (Exception)
                            {
                                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                            }
                        }
                    }
                }

            }
            #endregion

            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
        }
        #endregion

        #region Delete Test HRs

        [HttpGet("DeleteTestHR")]
        public async Task<ObjectResult> DeleteTestHR(long hrId)
        {
            try
            {
                #region Pre-Validation
                if (hrId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Please provide hiring request id"));
                }
                #endregion

                //Check if HR is already closed.
                var salesHiringRequest = await _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrId).FirstOrDefaultAsync().ConfigureAwait(false);

                if (salesHiringRequest != null)
                {
                    long loggedinuserid = SessionValues.LoginUserId;
                    UsrUser varUsrUserById = _commonInterface.TalentStatus.GetUsrUserById(loggedinuserid);

                    _iHiringRequest.Sproc_UTS_DeleteTestHR(hrId.ToString());

                    //UTS-7484: Send Email to the internal team when HR is deleted
                    EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                    binder.SendEmailForHRDeleteToInternalTeam(salesHiringRequest, varUsrUserById);

                    //UTS-7484: Send Delete HR update to ATS.
                    #region ATS Call
                    if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                    {
                        try
                        {
                            ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                            dynamic hrData = new ExpandoObject();
                            hrData.HR_Number = salesHiringRequest.HrNumber;
                            var JsonData = JsonConvert.SerializeObject(hrData);
                            //string JsonData = Convert.ToString(HRStatus_Json);
                            if (!string.IsNullOrEmpty(JsonData))
                            {
                                aTSCall.SendDeleteHRToATS(JsonData, loggedinuserid, hrId);
                            }
                        }
                        catch (Exception ex)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error in ATS API." });
                        }
                    }
                    #endregion
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success." });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("DeleteTestHRs")]
        public async Task<ObjectResult> DeleteTestHRs(string[] hrList)
        {
            try
            {
                long loggedinuserid = SessionValues.LoginUserId;

                #region ATS Call
                if (!_configuration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    try
                    {
                        ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);
                        dynamic hrData = new ExpandoObject();

                        foreach (var hr in hrList)
                        {
                            hrData.HR_Number = hr;
                            var JsonData = JsonConvert.SerializeObject(hrData);

                            if (!string.IsNullOrEmpty(JsonData))
                            {
                                aTSCall.SendDeleteHRToATS(JsonData, loggedinuserid, 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Error in ATS API." });
                    }
                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Demo Account HR's
        [HttpPost("CloneHRDemoAccount")]
        public async Task<ObjectResult> CloneHRDemoAccount(CloneHRViewModelDemoAccount cloneHRViewModel)
        {
            if (cloneHRViewModel != null)
            {
                long LoggedInUserID = SessionValues.LoginUserId;

                var findUser = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserID).FirstOrDefault();

                List<string> HRListsDone = new List<string>();
                List<string> HRListsExcluded = new List<string>();

                foreach (var i in cloneHRViewModel.cloneHRLists)
                {
                    sproc_CloneHRFromExistHR_Result obj_sproc_CloneHRFromExistHR_Result = new();

                    long hrID = i.HRID;

                    object[] param = new object[]
                    {
                        hrID,
                        LoggedInUserID,
                        0,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        0,
                        (short)AppActionDoneBy.UTS,
                        true
                    };


                    var findHrId = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrID).FirstOrDefault();

                    if (i.companyId > 0)
                    {
                        bool IsHybrid = false;
                        var findCompany = _talentConnectAdminDBContext.GenCompanies.Where(x => x.Id == i.companyId).FirstOrDefault();

                        if (findCompany != null && findCompany.CompanyTypeId > 0 && findCompany.AnotherCompanyTypeId > 0)
                        {
                            IsHybrid = true;
                        }

                        if (IsHybrid)
                        {
                            bool IsPayPerHire = findHrId?.HrtypeId == 1 ? true : false;

                            param = new object[]
                            {
                                    hrID,
                                     LoggedInUserID,
                                     0,
                                    IsHybrid, IsPayPerHire,
                                    findHrId?.IsTransparentPricing ?? false,
                                    findCompany?.IsPostaJob ?? false,
                                    findCompany?.IsProfileView ?? false,
                                    findCompany?.IsVettedProfile ?? false,
                                    0,
                                    (short)AppActionDoneBy.UTS,
                                    true
                            };
                        }
                    }

                    if (findUser != null && findHrId != null)
                    {
                        obj_sproc_CloneHRFromExistHR_Result = await _commonInterface.hiringRequest.sproc_CloneHRFromExistHRDemoAccount(CommonLogic.ConvertToParamString(param));

                        if (obj_sproc_CloneHRFromExistHR_Result != null && obj_sproc_CloneHRFromExistHR_Result.CloneHRID > 0)
                        {
                            HRListsDone.Add($"Reference HR# {i.HR_Number} -  New HR# {obj_sproc_CloneHRFromExistHR_Result.CloneHRGuid}");

                            try
                            {
                                ATSCall aTSCall = new ATSCall(_configuration, _talentConnectAdminDBContext);

                                #region HR Status updates to ATS 

                                long HR_ID = obj_sproc_CloneHRFromExistHR_Result.CloneHRID;
                                string HR_Number = i.HR_Number ?? "";
                                if (HR_ID != 0 && !string.IsNullOrEmpty(HR_Number))
                                {
                                    var HRData_Json = await _iHiringRequest.GetAllHRDataForAdmin(HR_ID).ConfigureAwait(false);
                                    string HRJsonData = Convert.ToString(HRData_Json);
                                    if (!string.IsNullOrEmpty(HRJsonData))
                                    {
                                        bool isAPIResponseSuccess = true;

                                        try
                                        {
                                            if (HRJsonData != "")
                                                isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRData_Json.ToString(), HR_ID);

                                            #region HR Status updates to ATS 

                                            //New Status Change
                                            object[] atsParam = new object[] { HR_ID, 0, 0, LoggedInUserID, (short)AppActionDoneBy.UTS, false };
                                            string strParam = CommonLogic.ConvertToParamString(atsParam);
                                            var HRStatus_Json = _iHiringRequest.GetUpdateHrStatus(strParam);
                                            if (HRStatus_Json != null)
                                            {
                                                //string JsonData = Convert.ToString(HRStatus_Json);
                                                var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                                if (!string.IsNullOrEmpty(JsonData))
                                                {
                                                    aTSCall.SendHrStatusToATS(JsonData, LoggedInUserID, HR_ID);
                                                }
                                            }

                                            #endregion
                                        }
                                        catch (Exception)
                                        {
                                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                                        }
                                    }
                                }

                                #endregion
                            }
                            catch (Exception)
                            {

                            }
                        }
                        else
                        {
                            HRListsExcluded.Add(i.HR_Number);
                        }
                    }
                }

                dynamic result = new ExpandoObject();
                result.HRListsDone = HRListsDone;
                result.HRListsExcluded = HRListsExcluded;

                //if (findUser != null)
                //{
                //    EmailBinder binder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                //    binder.SendEmailWhileAddingHRToDemoAccount(findUser?.EmailId, findUser?.FullName, HRListsDone, HRListsExcluded);
                //}

                return StatusCode(StatusCodes.Status200OK, CommonLogic.ReturnObject(StatusCodes.Status200OK, "HR Cloned Successfully.", result));
            }
            return StatusCode(StatusCodes.Status400BadRequest, CommonLogic.ReturnObject(StatusCodes.Status400BadRequest, "Invalid Request"));
        }
        #endregion

        #region HRTalentNotes
        [HttpPost("AddDeleteNotesData")]
        public ObjectResult AddDeleteNotesData([FromBody] HRTalentNotesViewModel notesViewModel)
        {
            try
            {
                #region Pre-Validation

                if (notesViewModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                #endregion

                object[] param1 = new object[] {
                                notesViewModel.ContactID,
                                notesViewModel.HRID,
                                notesViewModel.ATSTalentID,
                                notesViewModel.CreatedByDateTime,
                                notesViewModel.Notes,
                                notesViewModel.ATSNoteID,
                                notesViewModel.Flag,
                                SessionValues.LoginUserId,
                                3
                            };
                string paramString2 = CommonLogic.ConvertToParamStringWithNull(param1);

                var result = _iHiringRequest.Sproc_AddUpdate_TalentNotes_ClientPortal(paramString2);

                if (notesViewModel.Flag == "Edit" || notesViewModel.Flag == "Delete")
                {
                    // If for the edited note email is sent then only send the email at run time
                    if (result != null && Convert.ToBoolean(result.IsEmailSentToClient))
                    {
                        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
                        emailBinder.SendEmailForNotesAddedToInternalTeam(notesViewModel, SessionValues.LoginUserId);
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "success." });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        static Encoding DetectEncoding(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return Encoding.GetEncoding("ISO-8859-1"); // Default to ISO-8859-1 if no content type is specified
            }

            // Try to extract charset from content type
            var charsetIndex = contentType.IndexOf("charset=", StringComparison.OrdinalIgnoreCase);
            if (charsetIndex != -1)
            {
                var charset = contentType.Substring(charsetIndex + 8).Trim('"', ' ');
                try
                {
                    return Encoding.GetEncoding(charset);
                }
                catch (ArgumentException)
                {
                    // If the specified charset is not recognized, fall back to ISO-8859-1
                }
            }

            return Encoding.GetEncoding("ISO-8859-1"); // Fallback to ISO-8859-1
        }

        static string ConvertToUtf8(byte[] data, Encoding sourceEncoding)
        {
            // Convert from the source encoding to UTF-8
            string decodedString = sourceEncoding.GetString(data);
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(decodedString);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        [Authorize]
        [HttpGet("GetFrequency")]
        public ObjectResult GetFrequency()
        {
            try
            {
                var frequencyList = _iHiringRequest.Sproc_Get_Frequency_Office_Visit().ToList();

                if (frequencyList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Frequency List", Details = frequencyList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region GetAutoCompleteCityStateWise
        [Authorize]
        [HttpGet("GetAutoCompleteCityStateWise")]
        public ObjectResult GetAutoCompleteCityStateWise(string city)
        {
            try
            {
                object[] pocParam = new object[]
                   {
                        city
                   };
                string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                var CityList = _iHiringRequest.sproc_UTS_GetAutoComplete_CityStateWise(pocParamString).ToList();

                if (CityList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "City List", Details = CityList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAutoCompleteCityStateWise
        [Authorize]
        [HttpGet("GetNearByCities")]
        public ObjectResult GetNearByCities(long LocationID)
        {
            try
            {
                object[] pocParam = new object[]
                   {
                        LocationID
                   };
                string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                var CityList = _iHiringRequest.sproc_UTS_GetNearByCities(pocParamString).ToList();

                if (CityList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "City List", Details = CityList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetAutoCompleteCityWise
        [Authorize]
        [HttpGet("GetAutoCompleteCityWise")]
        public ObjectResult GetAutoCompleteCityWise(string city)
        {
            try
            {
                object[] pocParam = new object[]
                   {
                        city
                   };
                string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                var CityList = _iHiringRequest.sproc_UTS_GetAutoComplete_CityWise(pocParamString).ToList();

                if (CityList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "City List", Details = CityList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Call Chat GPT API call
        static async Task<string> MakeApiCall(string endpoint, string apiKey, string copiedText)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", apiKey);

                // Payload for the request
                var payload = new
                {
                    messages = new[]
                    {
                    new
                    {
                        role = "system",
                        content = new[]
                        {
                            new
                            {
                                type = "text",
                                text = copiedText
                            }
                        }
                    }
                },
                    temperature = 0.1,
                    top_p = 1.0,
                    max_tokens = 800
                };

                try
                {
                    var response = await client.PostAsJsonAsync(endpoint, payload);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Step 1: Decode Unicode escape sequences
                    string unescapedContent = Regex.Unescape(responseBody);

                    // Step 2: Decode HTML entities
                    string fullyDecodedContent = HttpUtility.HtmlDecode(unescapedContent);


                    return fullyDecodedContent;
                }
                catch (HttpRequestException e)
                {
                    return "";

                }
            }
        }

        private async Task<JobPostDetailsViewModel> ConvertStringToChatGPTAIResponseModel(string strContent, long gptId, string processType)
        {
            JobPostDetailsViewModel jobPostDetails = new JobPostDetailsViewModel();
            try
            {
                //int count = 0;
                //string pattern = @"(?<=<result>)(.*?)(?=</result>)";
                //string JsonString = "";
                //// Use Regex.Match to find the first match of the pattern
                //Match match = Regex.Match(strContent, pattern, RegexOptions.Singleline);

                //if (match.Success)
                ////{

                //    string pattern1 = @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))+(?(Open)(?!))\}";
                //    Regex regex = new Regex(pattern1);
                //    MatchCollection matches = regex.Matches(match.Value);
                //    foreach (Match match1 in matches)
                //    {
                //        JsonString = match1.Value;
                //    }
                string JsonString = "";
                string pattern = @"```json\n(?<json>.+?)\n```";
                Match match = Regex.Match(strContent, pattern, RegexOptions.Singleline);

                if (match.Success)
                {
                    // Extracted JSON
                    JsonString = match.Groups["json"].Value;
                    JsonString = JsonString.Replace(@"\\n", "").Replace(@"\n", "").Replace(@"\'", "");


                    if (!string.IsNullOrEmpty(JsonString))
                    {
                        var data = JsonConvert.DeserializeObject<ClaudeAIResponseModel>(JsonString);


                        int WorkingModeId = 1;
                        if (string.Equals(data?.Opportunity_Type, "Remote", StringComparison.OrdinalIgnoreCase))
                        {
                            WorkingModeId = 1;
                        }
                        else if (string.Equals(data?.Opportunity_Type, "Hybrid", StringComparison.OrdinalIgnoreCase))
                        {
                            WorkingModeId = 2;
                        }
                        else if (string.Equals(data?.Opportunity_Type, "On Site", StringComparison.OrdinalIgnoreCase))
                        {
                            WorkingModeId = 3;
                        }

                        jobPostDetails.RoleName = data.Title;
                        jobPostDetails.ExperienceYears = Convert.ToInt32(data.YearsofExperienceRequired);

                        // step-2
                        jobPostDetails.Skills = data.Skills;
                        jobPostDetails.AllSkills = (data.Skills == data.Suggested_Skills ? "" : data.Suggested_Skills) ?? "";
                        jobPostDetails.BudgetFrom = Convert.ToDecimal(data.Budget_From);
                        jobPostDetails.BudgetTo = Convert.ToDecimal(data.Budget_To);
                        jobPostDetails.Currency = data.Salary_Currency;


                        // step-3
                        jobPostDetails.EmploymentType = data.Type_Of_Job;
                        jobPostDetails.WorkingModeId = WorkingModeId;

                        jobPostDetails.GPTJDID = gptId;
                        jobPostDetails.WorkingModeId = WorkingModeId;
                        // step-4
                        List<string> description = new List<string>();

                        List<string>? requirements = null;
                        if (data.Requirements != null && data.Requirements.Count > 0)
                        {

                            requirements = ReplacesingleQuotes(data.Requirements);
                        }

                        if (requirements != null)
                        {
                            jobPostDetails.Requirements = System.Text.Json.JsonSerializer.Serialize(requirements);
                            description.AddRange(requirements);
                        }


                        List<string>? offer = null;
                        if (data.Whatweoffer != null && data.Whatweoffer.Count > 0)
                        {
                            offer = ReplacesingleQuotes(data.Whatweoffer);
                        }

                        List<string>? roleoverviewdescription = null;
                        if (data.RoleOverviewDescription != null && data.RoleOverviewDescription.Count > 0)
                        {
                            roleoverviewdescription = ReplacesingleQuotes(data.RoleOverviewDescription);
                        }

                        List<string>? rolesResponsibilities = null;
                        if (data.RolesResponsibilities != null && data.RolesResponsibilities.Count > 0)
                        {
                            rolesResponsibilities = ReplacesingleQuotes(data.RolesResponsibilities);
                        }


                        if (rolesResponsibilities != null)
                        {
                            jobPostDetails.RolesResponsibilities = System.Text.Json.JsonSerializer.Serialize(rolesResponsibilities);
                            description.AddRange(rolesResponsibilities);
                        }

                        if (offer != null)
                        {
                            jobPostDetails.Whatweoffer = System.Text.Json.JsonSerializer.Serialize(offer);
                            description.AddRange(offer);
                        }

                        if (roleoverviewdescription != null)
                        {
                            jobPostDetails.RoleOverviewDescription = System.Text.Json.JsonSerializer.Serialize(roleoverviewdescription);
                            description.AddRange(roleoverviewdescription);
                        }


                        jobPostDetails.RequirementsList = new List<string>();
                        if (requirements != null)
                            jobPostDetails.RequirementsList.AddRange(requirements);


                        jobPostDetails.RolesResponsibilitiesList = new List<string>();
                        if (rolesResponsibilities != null)
                            jobPostDetails.RolesResponsibilitiesList.AddRange(rolesResponsibilities);

                        jobPostDetails.JobDescription = System.Text.Json.JsonSerializer.Serialize(description);
                        jobPostDetails.JobDesriptionList = new List<string>();
                        if (roleoverviewdescription != null)
                            jobPostDetails.JobDesriptionList.AddRange(roleoverviewdescription);
                        if (rolesResponsibilities != null)
                            jobPostDetails.JobDesriptionList.AddRange(rolesResponsibilities);
                        if (requirements != null)
                            jobPostDetails.JobDesriptionList.AddRange(requirements);
                        if (offer != null)
                            jobPostDetails.JobDesriptionList.AddRange(offer);

                        if (data.JobDescription != null)
                            jobPostDetails.JobDescription = data.JobDescription;

                        // Extra info
                        jobPostDetails.ProcessType = processType;
                        if (gptId > 0 && processType == ProcessType.URL_Parsing.ToString())
                        {
                            if (!string.IsNullOrEmpty(data.Title))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Skills))
                                count += 1;
                            if (!string.IsNullOrEmpty(jobPostDetails.Requirements))
                                count += 1;
                            if (!string.IsNullOrEmpty(jobPostDetails.RolesResponsibilities))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.YearsofExperienceRequired))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Budget_From))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Budget_To))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Max_Salary))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Salary_Currency))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Working_Zone_With_Time_Zone))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Type_Of_Job))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Opportunity_Type))
                                count += 1;
                            if (!string.IsNullOrEmpty(data.Suggested_Skills))
                                count += 1;

                            _iJDParse.UpdateGptJdresponse(gptId, count).ConfigureAwait(false);
                            jobPostDetails.AchievedCount = count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobPostDetails;
        }

        #endregion

        #region call Claude AI API call

        static async Task<string> CallClaudeApi(string endpoint, string apiKey, string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set up request headers
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                // Create the payload for the API request
                var requestBody = new
                {
                    model = "claude-v1",  // Specify the version of Claude
                    prompt = prompt,
                    max_tokens_to_sample = 100
                };

                // Serialize request body to JSON
                string jsonRequestBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                // Send POST request
                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                // Ensure success status code
                response.EnsureSuccessStatusCode();

                // Read response content
                string responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize response (assuming it's in JSON format)
                dynamic responseObject = JsonConvert.DeserializeObject(responseContent);

                // Output the response
                return responseObject;
            }
        }

        #endregion
    }
}
