namespace UTSATSAPI.Controllers
{
    using Amazon.S3.Model;
    using Amazon.S3;
    using ClosedXML.Excel;
    using HtmlAgilityPack;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Dynamic;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModel;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Repositories.Interfaces;
    using UTSATSAPI.Repositories.Interfaces.UpChat;
    using static UTSATSAPI.Helpers.Enum;
    using Amazon;
    using System.Diagnostics.Metrics;

    /// <summary>
    /// ViewAllHRController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Authorize]
    //[Route("api/[controller]")]
    [Route("ViewAllHR/", Name = "View All HR API's")]
    [ApiController]
    public class ViewAllHRController : ControllerBase
    {
        #region Variables
        private readonly ICommonInterface commonInterface;
        private readonly IConfiguration iConfiguration;
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IEmail _iUpChatEmail;
        private readonly IUpChatCall _iUpChatCall;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHRInterviewerDetail _iHRInterviewerDetail;
        #endregion

        #region Constructors

        public ViewAllHRController(ICommonInterface _commonInterface, IConfiguration _iConfiguration, TalentConnectAdminDBContext talentConnectAdminDBContext, IUniversalProcRunner universalProcRunner, IEmail iUpChatEmail, IUpChatCall iUpChatCall, IHttpContextAccessor httpContextAccessor)
        {
            commonInterface = _commonInterface;
            iConfiguration = _iConfiguration;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
            _universalProcRunner = universalProcRunner;
            _iUpChatEmail = iUpChatEmail;
            _iUpChatCall = iUpChatCall;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Public APIs

        [HttpPost]
        [Route("GetAllHiringRequests")]
        public IActionResult GetAllHiringRequests([FromBody] ViewAllHRViewModel viewAllHRViewModel, [FromQuery] bool IsExportToExcel = false)
        {
            try
            {
                #region PreValidation
                if (viewAllHRViewModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });

                if (viewAllHRViewModel.FilterFields_ViewAllHRs == null)
                    viewAllHRViewModel.FilterFields_ViewAllHRs = new();
                #endregion

                var LoggedInUserTypeID = SessionValues.LoginUserTypeId;
                var LoggedInUserID = SessionValues.LoginUserId;

                List<sproc_ViewAllHRs_Result> allHRListData = commonInterface.ViewAllHR.GetAllHRs(viewAllHRViewModel, LoggedInUserTypeID, LoggedInUserID);

                #region Export to excel
                if (allHRListData.Any() && IsExportToExcel)
                {
                    try
                    {
                        return Export_HrList(allHRListData);
                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Some data issue in Export to excel" });
                    }
                }
                #endregion

                if (allHRListData.Count > 0)
                {
                    // UTS-7517: Show Clone to demo account CTA in HR listing page.
                    var result = CustomRendering.ListingResponses(allHRListData, allHRListData[0].TotalRecords, viewAllHRViewModel.Pagesize, viewAllHRViewModel.Pagenum);
                    if (result != null)
                    {
                        var loggedInUserEmployeeID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserID).Select(a => a.EmployeeId).FirstOrDefault();

                        if (loggedInUserEmployeeID != null)
                        {
                            object[] param = new object[] { LoggedInUserID, loggedInUserEmployeeID };
                            string paramString = CommonLogic.ConvertToParamString(param);
                            Sproc_UTS_FetchUsersWithSpecialEditAccess_Result specialAccess = commonInterface.ViewAllHR.CheckSpecialEdits(paramString);

                            if (result != null)
                            {
                                result.ShowCloneToDemoAccount = specialAccess.AllowHRAddInDemoAccount;
                            }
                        }
                    }
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Hiring Requests Available", Details = CustomRendering.EmptyRows() });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetHRDetail")]
        public ObjectResult GetHRDetail(long id, long? WhatToaddClick = 0)
        {
            try
            {
                HRDetailViewModel hRDetailsViewModel = new();
                hRDetailsViewModel = commonInterface.ViewAllHR.ShowHRDetail(id, WhatToaddClick);
                if (hRDetailsViewModel != null && hRDetailsViewModel.ClientDetail != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = JsonConvert.SerializeObject(hRDetailsViewModel) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("DownloadResume")]
        public async Task<IActionResult> DownloadFile([FromBody] DownloadResume FileObj)
        {
            try
            {
                if (FileObj == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                if (string.IsNullOrEmpty(FileObj.resumeFile))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "resumeFile is empty" });
                }

                string BucketName = iConfiguration["BucketName"].ToString();
                string KeyName = iConfiguration["KeyNameResumeDownload"].ToString();
                string AccessKey = iConfiguration["AccessKey"].ToString();
                string SecretKey = iConfiguration["SecretKey"].ToString();

                string fileName = Path.GetFileName(FileObj.resumeFile);

                var credentials = new Amazon.Runtime.BasicAWSCredentials(AccessKey, SecretKey);

                using (var client = new AmazonS3Client(credentials, RegionEndpoint.USEast1)) // Replace YOUR_REGION with the appropriate AWS region
                {
                    var getObjectRequest = new GetObjectRequest
                    {
                        BucketName = BucketName,
                        Key = KeyName + fileName
                    };

                    using (GetObjectResponse response = await client.GetObjectAsync(getObjectRequest))
                    {
                        var memoryStream = new MemoryStream();
                        await response.ResponseStream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        return File(memoryStream, response.Headers.ContentType, fileName);
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                return StatusCode(500, $"Error downloading file: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("SaveHRNotes")]
        public async Task<ObjectResult> SaveHRNotes([FromBody] NotesViewRequestModel notesViewRequestModel)
        {
            try
            {
                if (notesViewRequestModel.Id < 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });

                string userEmployeeID = (string)HttpContext.Items["User"];
                if (userEmployeeID == null)
                    return StatusCode(StatusCodes.Status401Unauthorized, new ResponseObject() { statusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized" });

                string emailSuccessMsg = "";
                string hdnUserValues = "";
                var doc = new HtmlDocument();
                doc.LoadHtml(notesViewRequestModel.Note);
                var tags = doc.DocumentNode.Descendants("span");
                var idList = new List<string>();
                foreach (var tag in tags)
                {
                    if (idList.Contains(tag.Id) == false)
                    {
                        if (!string.IsNullOrEmpty(hdnUserValues))
                        {
                            idList.Add(tag.Id.ToString());
                            hdnUserValues = string.Format("{0},{1}", hdnUserValues, tag.Id);
                        }
                        else
                        {
                            hdnUserValues = tag.Id;
                            idList.Add(tag.Id.ToString());
                        }
                    }
                }

                NotesViewModel notesViewModel = new();
                notesViewModel.HiringRequest_ID = notesViewRequestModel.Id;
                notesViewModel.hdnNotes = notesViewRequestModel.Note;
                notesViewModel.hdnUserValues = hdnUserValues;

                notesViewModel = commonInterface.ViewAllHR.SaveHRNotes(notesViewModel, userEmployeeID);
                emailSuccessMsg = await commonInterface.SendEmailNotes.SendEmailForAddingNotes(notesViewModel.HiringRequest_ID, notesViewModel.hdnUserValues, notesViewModel.hdnNotes, userEmployeeID);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = notesViewModel });
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("UpChatSaveHRNotes")]
        public async Task<ObjectResult> UpChatSaveHRNotes([FromBody] NotesViewRequestModel notesViewRequestModel)
        {
            try
            {
                #region Pre-validation
                if (notesViewRequestModel.Id < 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Not found" });

                string userEmployeeID = (string)HttpContext.Items["User"];
                if (userEmployeeID == null)
                    return StatusCode(StatusCodes.Status401Unauthorized, new ResponseObject() { statusCode = StatusCodes.Status401Unauthorized, Message = "Unauthorized" });
                #endregion

                #region EMPid to userId converation in html
                string emailSuccessMsg = "";
                string hdnUserValues = "";

                var doc = new HtmlDocument();
                doc.LoadHtml(notesViewRequestModel.Note);
                var tags = doc.DocumentNode.Descendants("span");
                var idList = new List<string>();

                var empIDList = new List<string>();

                foreach (var tag in tags)
                {
                    if (empIDList.Contains(tag.Id) == false)
                    {
                        empIDList.Add(tag.Id.ToString());
                    }
                }

                foreach (var id in empIDList)
                {
                    HtmlNode spanNode = doc.GetElementbyId(id);

                    if (spanNode != null)
                    {
                        UsrUser User = _talentConnectAdminDBContext.UsrUsers.FirstOrDefault(x => x.EmployeeId == id);
                        if (User != null)
                        {
                            spanNode.Id = User.Id.ToString();
                        }
                    }
                }
                notesViewRequestModel.Note = doc.DocumentNode.OuterHtml;
                #endregion

                #region set hdnUserValues
                doc = new HtmlDocument();
                doc.LoadHtml(notesViewRequestModel.Note);
                tags = doc.DocumentNode.Descendants("span");

                foreach (var tag in tags)
                {
                    if (idList.Contains(tag.Id) == false)
                    {
                        if (!string.IsNullOrEmpty(hdnUserValues))
                        {
                            idList.Add(tag.Id.ToString());
                            hdnUserValues = string.Format("{0},{1}", hdnUserValues, tag.Id);
                        }
                        else
                        {
                            hdnUserValues = tag.Id;
                            idList.Add(tag.Id.ToString());
                        }
                    }
                }
                #endregion

                NotesViewModel notesViewModel = new();
                notesViewModel.HiringRequest_ID = notesViewRequestModel.Id;
                notesViewModel.hdnNotes = notesViewRequestModel.Note;
                notesViewModel.hdnUserValues = hdnUserValues;

                notesViewModel = commonInterface.ViewAllHR.UpChatSaveHRNotes(notesViewModel, userEmployeeID);

                emailSuccessMsg = await _iUpChatEmail.SendEmail_UserTagInChat(notesViewModel.HiringRequest_ID, notesViewModel.hdnUserValues, notesViewModel.hdnNotes, userEmployeeID);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = notesViewModel });
            }
            catch
            {
                throw;
            }

        }

        [HttpGet("GetChannelLibrary")]
        public async Task<ObjectResult> GetChannelLibrary(int Type, string ChannelID)
        {
            try
            {
                if (string.IsNullOrEmpty(ChannelID) || Type == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Folder Type" });
                }

                string LibraryType = string.Empty;

                if ((int)Upchat_ChannelLibrary.images == Type)
                {
                    LibraryType = Upchat_ChannelLibrary.images.ToString();
                }
                else if ((int)Upchat_ChannelLibrary.videos == Type)
                {
                    LibraryType = Upchat_ChannelLibrary.videos.ToString();
                }
                else if ((int)Upchat_ChannelLibrary.document == Type)
                {
                    LibraryType = Upchat_ChannelLibrary.document.ToString();
                }

                var FileObject = await _iUpChatCall.GetChannelLibrary(LibraryType, ChannelID).ConfigureAwait(false);

                if (FileObject != null)
                {
                    return StatusCode(StatusCodes.Status200OK, FileObject);
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No such File Found" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("IsCurrentUserMapWithAnyChannel")]
        public async Task<ObjectResult> IsCurrentUserMapWithAnyChannel(string UserEmpID)
        {
            try
            {
                GenChannelHistory genChannelHistory =
                    _talentConnectAdminDBContext.GenChannelHistories.FirstOrDefault(x => x.UserEmpId == UserEmpID && x.ChannelActionId == 1);

                if (genChannelHistory != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Exist", Details = true });
                }
                else
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Not exist", Details = false });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = ex.Message });
            }
        }

        [HttpPost("SetPriorityForNextWeek")]
        public async Task<ObjectResult> SetPriorityForNextWeek([FromBody] PriorityForNextWeekViewModel priorityForNextWeekViewModel)
        {
            try
            {
                if (priorityForNextWeekViewModel.HRID < 0 || priorityForNextWeekViewModel.HRID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR ID Not found" });

                long count = 0;
                var HR_ID = Convert.ToInt64(priorityForNextWeekViewModel.HRID);
                var HRPriorityToSet = priorityForNextWeekViewModel.IsNextWeekStarMarked == "1" ? true : false;
                GenSalesHiringRequestPriority HRPriorityData = commonInterface.ViewAllHR.GetHRPriorityData(HR_ID);
                GenSalesHiringRequest HRData = commonInterface.ViewAllHR.GetHRData(HR_ID);
                long LoggedInUserId = SessionValues.LoginUserId;
                if (HRData == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });

                if (HRPriorityData == null)
                {
                    if (HRPriorityToSet == true)
                    {
                        var PStartDate = CommonFunction.GetNextWeekday(DateTime.Today, DayOfWeek.Monday);
                        var PendDate = PStartDate.AddDays(Convert.ToDouble(iConfiguration["Date:PriorityDaysDifference"]));

                        object[] param = new object[]
                        {
                            LoggedInUserId, HRData.SalesUserId, HRData.Id, false, PStartDate.ToString("yyyy-MM-dd"), PendDate.ToString("yyyy-MM-dd"), HRPriorityToSet
                        };

                        count = commonInterface.ViewAllHR.HiringRequestPriority(CommonLogic.ConvertToParamString(param));
                    }
                }
                else
                {
                    if (HRPriorityToSet)
                    {
                        var PStartDate = CommonFunction.GetNextWeekday(DateTime.Today, DayOfWeek.Monday);
                        var PendDate = PStartDate.AddDays(Convert.ToDouble(iConfiguration["Date:PriorityDaysDifference"]));
                        object[] param = new object[]
                        {
                            LoggedInUserId, HRData.SalesUserId, HRData.Id, false, PStartDate.ToString("yyyy-MM-dd"), PendDate.ToString("yyyy-MM-dd"), HRPriorityToSet
                        };

                        count = commonInterface.ViewAllHR.HiringRequestPriority(CommonLogic.ConvertToParamString(param));
                    }
                    else
                    {
                        object[] param = new object[]
                        {
                            LoggedInUserId, HRData.SalesUserId, HRData.Id, false, null, null, HRPriorityToSet
                        };
                        count = commonInterface.ViewAllHR.HiringRequestPriority(CommonLogic.ConvertToParamString(param));
                    }
                }

                if (count == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Remaining Priority Count left to set is 0" });
                else
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = priorityForNextWeekViewModel.HRID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllFilterDataForHR")]
        public async Task<ObjectResult> GetAllFilterDataForHR()
        {
            try
            {
                HRFilterListViewModel model = new();
                model = await commonInterface.ViewAllHR.GetFiltersLists();

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(model) });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetHRCompanyDetails")]
        public ObjectResult GetHRCompanyDetails(long id)
        {
            try
            {
                HRDetailViewModel hRDetailsViewModel = new()
                {
                    ClientDetail = commonInterface.ViewAllHR.GetClientCompanyDetail(id)
                };

                List<SalesHiringRequestInterviewerDetailViewModel> salesHRInterviewerDetailList = new();

                if (hRDetailsViewModel.ClientDetail == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "No Company Details Found",
                        Details = CustomRendering.EmptyRows()
                    }
                                     );
                }
                else
                {
                    salesHRInterviewerDetailList = _iHRInterviewerDetail.GetHRInterviewerDetails(id);
                }


                #region Dynamic Object

                var responseJson = new
                {
                    CompanyDetails = hRDetailsViewModel.ClientDetail,
                    InterviewerDetails = salesHRInterviewerDetailList
                };
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = responseJson });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("NeedMoreHelp")]
        public ObjectResult NeedMoreHelp(long HiringRequestID, int AcceptValue, string Reason)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = commonInterface.ViewAllHR.GetHRData(HiringRequestID);
                if (_SalesHiringRequest == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });

                var LoggedInUserId = Convert.ToInt64(_SalesHiringRequest.CreatedById);

                if (_SalesHiringRequest.IsActive == true)
                {
                    _SalesHiringRequest.IsAccepted = AcceptValue;
                    _SalesHiringRequest.AcceptedById = LoggedInUserId;
                    _SalesHiringRequest.AcceptedByDateTime = DateTime.Now;
                    _SalesHiringRequest.NotAcceptedHrreason = (AcceptValue == 2) ? Reason : null;

                    _SalesHiringRequest = commonInterface.ViewAllHR.SaveSalesHiringRequest(_SalesHiringRequest);
                }

                //If Qty 1 then Update TR_Accepted in gen_SalesHiringRequest
                if (_SalesHiringRequest.NoofTalents == 1 && AcceptValue != 2)
                {
                    _SalesHiringRequest.TrAccepted = _SalesHiringRequest.NoofTalents;
                    _SalesHiringRequest.LastModifiedById = (int)LoggedInUserId;
                    _SalesHiringRequest.LastModifiedDatetime = DateTime.Now;

                    _SalesHiringRequest = commonInterface.ViewAllHR.SaveSalesHiringRequest(_SalesHiringRequest);

                    // Insert TR_Accepted in gen_SalesHR_TRAccepted_Details
                    GenSalesHrTracceptedDetail _gen_SalesHR_TRAccepted_Details = new GenSalesHrTracceptedDetail();
                    _gen_SalesHR_TRAccepted_Details.HiringRequestId = HiringRequestID;
                    _gen_SalesHR_TRAccepted_Details.TrAccepted = _SalesHiringRequest.NoofTalents;
                    _gen_SalesHR_TRAccepted_Details.CreatedById = (int)LoggedInUserId;
                    _gen_SalesHR_TRAccepted_Details.CreatedByDateTime = DateTime.Now;

                    _gen_SalesHR_TRAccepted_Details = commonInterface.ViewAllHR.SaveSalesHRTracceptedDetail(_gen_SalesHR_TRAccepted_Details);
                }
                if (AcceptValue == 1)
                {
                    string query = "";

                    //HR_Acceptance
                    query = commonInterface.ViewAllHR.SaveHiringRequestHistory("HR_Acceptance", HiringRequestID, 0, false, LoggedInUserId, 0, 0, _SalesHiringRequest.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false);

                    //TR_Accepted
                    query = commonInterface.ViewAllHR.SaveHiringRequestHistory("TR_Accepted", HiringRequestID, 0, false, LoggedInUserId, 0, 0, _SalesHiringRequest.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false);
                }
                if (AcceptValue == 2)
                {
                    commonInterface.ViewAllHR.SaveHiringRequestHistory("WaitFor_More_Info", HiringRequestID, 0, false, LoggedInUserId, 0, 0, _SalesHiringRequest.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false);
                }

                var EmailSuccessMsg = commonInterface.SendEmailNotes.SendEmailForHRAcceptanceToInternalTeam(HiringRequestID, AcceptValue, Reason);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("AddUpdateTRAcceptance")]
        public ObjectResult AddUpdateTRAcceptance(long HiringRequestID, int TR_Accepted, int AcceptValue)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = commonInterface.ViewAllHR.GetHRData(HiringRequestID);
                if (_SalesHiringRequest == null)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });

                var LoggedInUserId = Convert.ToInt64(_SalesHiringRequest.CreatedById);

                string EmailSuccessMsg = "";

                // Update TR_Accepted in  gen_SalesHiringRequest
                if (_SalesHiringRequest.IsActive == true)
                {
                    if (_SalesHiringRequest.IsAccepted == 1)
                    {
                        _SalesHiringRequest.TrAccepted = _SalesHiringRequest.NoofTalents;
                        _SalesHiringRequest.LastModifiedById = (int)LoggedInUserId;
                        _SalesHiringRequest.LastModifiedDatetime = DateTime.Now;

                        _SalesHiringRequest = commonInterface.ViewAllHR.SaveSalesHiringRequest(_SalesHiringRequest);
                    }
                    else
                    {
                        _SalesHiringRequest.IsAccepted = 1;
                        _SalesHiringRequest.TrAccepted = _SalesHiringRequest.NoofTalents;
                        _SalesHiringRequest.AcceptedById = LoggedInUserId;
                        _SalesHiringRequest.AcceptedByDateTime = DateTime.Now;

                        _SalesHiringRequest = commonInterface.ViewAllHR.SaveSalesHiringRequest(_SalesHiringRequest);

                        // Insert History for HR Acceptance.
                        commonInterface.ViewAllHR.SaveHiringRequestHistory("HR_Acceptance", HiringRequestID, 0, false, LoggedInUserId, 0, 0, _SalesHiringRequest.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false);

                        // Send Email to Internal Team
                        EmailSuccessMsg = commonInterface.SendEmailNotes.SendEmailForHRAcceptanceToInternalTeam(HiringRequestID, 1, "");
                    }
                }

                // Insert TR_Accepted in gen_SalesHR_TRAccepted_Details
                GenSalesHrTracceptedDetail _gen_SalesHR_TRAccepted_Details = new GenSalesHrTracceptedDetail();
                _gen_SalesHR_TRAccepted_Details.HiringRequestId = HiringRequestID;
                _gen_SalesHR_TRAccepted_Details.TrAccepted = AcceptValue;
                _gen_SalesHR_TRAccepted_Details.CreatedById = (int)LoggedInUserId;
                _gen_SalesHR_TRAccepted_Details.CreatedByDateTime = DateTime.Now;

                _gen_SalesHR_TRAccepted_Details = commonInterface.ViewAllHR.SaveSalesHRTracceptedDetail(_gen_SalesHR_TRAccepted_Details);

                TRAcceptanceViewModel tRAcceptanceViewModel = new TRAcceptanceViewModel();
                tRAcceptanceViewModel.TR_Parked = _SalesHiringRequest.NoofTalents - (_SalesHiringRequest.TrAccepted != null ? _SalesHiringRequest.TrAccepted : 0);

                //Insert History for this : Added By Reena Jain
                if (TR_Accepted >= 1)
                {
                    commonInterface.ViewAllHR.SaveHiringRequestHistory("TR_Accepted", HiringRequestID, 0, false, LoggedInUserId, 0, 0, _SalesHiringRequest.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss"), 0, false, false);
                }
                EmailSuccessMsg = commonInterface.SendEmailNotes.SendEmailForTRAcceptanceToInternalTeam(HiringRequestID, TR_Accepted, tRAcceptanceViewModel.TR_Parked);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("UpdateODRPoolStatus")]
        public ObjectResult UpdateODRPoolStatus(long? HiringRequestID, bool IsPool, bool IsODR)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = null;
                if (HiringRequestID == null && HiringRequestID <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide HiringRequestID" });
                }
                else
                {
                    _SalesHiringRequest = commonInterface.ViewAllHR.GetHRData((long)HiringRequestID);
                    if (_SalesHiringRequest == null)
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                }

                var LoggedInUserId = SessionValues.LoginUserId;

                _SalesHiringRequest.IsAdHocHr = IsODR;
                _SalesHiringRequest.IsPoolHr = IsPool;
                if (IsODR)
                {
                    _SalesHiringRequest.AdhocPassBy = (int)LoggedInUserId;
                    _SalesHiringRequest.AdhocPassDate = DateTime.Now;
                }
                else
                {
                    _SalesHiringRequest.AdhocPassBy = 0;
                    _SalesHiringRequest.AdhocPassDate = null;
                }
                _talentConnectAdminDBContext.Entry(_SalesHiringRequest).State = EntityState.Modified;
                _talentConnectAdminDBContext.SaveChanges();

                if (IsODR == true)
                {
                    #region Fire email to Ad-hoc team with CC : Soumya, Bhuvan sir
                    EmailBinder binder = new EmailBinder(iConfiguration, _talentConnectAdminDBContext);
                    binder.SendEmailNotificationForPassToAdhoc((long)HiringRequestID);
                    #endregion
                }

                string acceptedByDateTime = _SalesHiringRequest.AcceptedByDateTime != null ? _SalesHiringRequest.AcceptedByDateTime.Value.ToString("MM-dd-yyyy hh:mm:ss") : string.Empty;

                if (IsODR && !IsPool)
                    commonInterface.ViewAllHR.SaveHiringRequestHistory("HR_PasstoAdhoc", (long)HiringRequestID, 0, false, LoggedInUserId, 0, 0, acceptedByDateTime, 0, false, false);
                else if (IsPool && !IsODR)
                    commonInterface.ViewAllHR.SaveHiringRequestHistory("HR_PasstoPool", (long)HiringRequestID, 0, false, LoggedInUserId, 0, 0, acceptedByDateTime, 0, false, false);
                else if (IsODR && IsPool)
                    commonInterface.ViewAllHR.SaveHiringRequestHistory("HR_PassToPoolAndODR", (long)HiringRequestID, 0, false, LoggedInUserId, 0, 0, acceptedByDateTime, 0, false, false);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetHRCostDetails")]
        public ObjectResult GetHRCostDetails(long HR_ID, long ContactPriorityID, string? BillRate, string? PayRate)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HR_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                GetHRCost getHRCost = new GetHRCost();
                getHRCost.ContactPriorityID = ContactPriorityID;
                getHRCost.HrCost = BillRate;
                getHRCost.HR_ID = HR_ID;

                var CTPDetails = _talentConnectAdminDBContext.GenContactTalentPriorities.FirstOrDefault(c => c.Id == ContactPriorityID);

                if (CTPDetails != null)
                    getHRCost.FinalHRCost = CTPDetails.FinalHrCost ?? 0;

                if (_SalesHiringRequest != null)
                {
                    GenSalesHiringRequestDetail _SalesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.HiringRequestId == HR_ID).FirstOrDefault();
                    if (_SalesHiringRequest_Details != null)
                    {
                        getHRCost.Currency_Sign = _SalesHiringRequest_Details.Currency;
                        getHRCost.Talent_Fees = PayRate;
                        //getHRCost.HR_Percentage = _SalesHiringRequest.TalentCostCalcPercentage ?? 0;
                        getHRCost.HR_Percentage = CTPDetails.Nrpercentage ?? 0;
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = getHRCost });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("UpdateHRCost")]
        public ObjectResult UpdateHRCost(long ContactPriorityID, decimal Hr_Cost, decimal HR_Percentage)
        {
            try
            {
                long TalentId = 0;
                long HR_ID = 0;
                GenContactTalentPriority? contactTalentPriority = null;
                var LoggedInUserId = SessionValues.LoginUserId;

                #region Validation
                if (ContactPriorityID == 0 || ContactPriorityID < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data is not valid" });
                }
                if (Hr_Cost == 0 || Hr_Cost < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Add HR Cost" });
                }
                if (HR_Percentage == 0 || HR_Percentage < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Add HR Percentage" });
                }
                #endregion

                if (ContactPriorityID > 0)
                {
                    contactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.FirstOrDefault(x => x.Id == ContactPriorityID);
                    if (contactTalentPriority != null)
                    {
                        //UTS-4570: Calculate the HR cost
                        decimal Talent_Cost = Convert.ToDecimal(contactTalentPriority.TalentCost);
                        object[] param = new object[] { ContactPriorityID, Talent_Cost, 0, TalentId, HR_Percentage };
                        //object[] param = new object[] { ContactPriorityID, 0, Hr_Cost, TalentId, HR_Percentage };
                        string paramasString = CommonLogic.ConvertToParamString(param);
                        commonInterface.ViewAllHR.sproc_UTS_gen_ContactTalentPriorityupdate(paramasString);

                        #region Insert HR History 
                        param = new object[]
                        {
                            Action_Of_History.HR_Cost_Update, contactTalentPriority.HiringRequestId, TalentId, false, LoggedInUserId, ContactPriorityID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                        #endregion
                    }
                }
                #region API CAll to send to ATS Team for Dp and Contractual HR
                if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    if (contactTalentPriority != null)
                    {
                        _talentConnectAdminDBContext.Entry(contactTalentPriority).Reload();

                        HR_ID = (long)contactTalentPriority.HiringRequestId;

                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(HR_ID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }


                        ContractualDpViewModel ObjcontractualDpView = new();

                        var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HR_ID && x.Id == ContactPriorityID).Select(x => x.TalentId).ToList();

                        var User_UPID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserId).Select(x => x.EmployeeId).FirstOrDefault();
                        GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HR_ID).FirstOrDefault();
                        GenSalesHiringRequestDetail _ObjSalesHiringrequestDetails = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(X => X.HiringRequestId == HR_ID).FirstOrDefault();

                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringRequest).Reload();
                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringrequestDetails).Reload();

                        if (_ObjSalesHiringRequest != null && _ObjSalesHiringrequestDetails != null)
                        {
                            ObjcontractualDpView.HRID = HR_ID;
                            ObjcontractualDpView.CreatedByDateTime = System.DateTime.Now.ToString("yyy-mm-dd HH:mm:ss");
                            ObjcontractualDpView.CreatedByID = User_UPID;
                            ObjcontractualDpView.HR_Currency = _ObjSalesHiringrequestDetails.Currency == null ? "" : _ObjSalesHiringrequestDetails.Currency;
                        }
                        if (talentList.Count != 0 && talentList != null)
                        {
                            foreach (var talentID in talentList)
                            {
                                HRTalentDetail talentDetail = new();
                                GenTalent Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();

                                _talentConnectAdminDBContext.Entry(Talent).Reload();

                                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == HR_ID && x.Id == ContactPriorityID).FirstOrDefault();
                                if (TalentCTP_Details != null && Talent != null)
                                {
                                    talentDetail.CTPID = TalentCTP_Details.Id;
                                    talentDetail.Talent_current_fee = TalentCTP_Details.CurrentCtc;
                                    talentDetail.Talent_Expected_fee = TalentCTP_Details.TalentCost;
                                    talentDetail.HR_Cost = TalentCTP_Details.FinalHrCost;
                                    talentDetail.HR_Cost_With_Currency = TalentCTP_Details.HrCost;
                                    talentDetail.HR_Type = TalentCTP_Details.IsHrtypeDp == true ? "DP" : "Contractual";
                                    talentDetail.DPAmount = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.Dpamount : 0;
                                    talentDetail.TalentCurrency = TalentCTP_Details.TalentCurrencyCode == null ? "USD" : TalentCTP_Details.TalentCurrencyCode;
                                    talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                                    talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                                    talentDetail.ExchangeRateUTS = TalentCTP_Details.ExchangeRate == null ? 1 : TalentCTP_Details.ExchangeRate;
                                    talentDetail.DPorNR_Percent = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.DpPercentage : TalentCTP_Details.Nrpercentage;

                                }
                                ObjcontractualDpView.TalentDetails.Add(talentDetail);

                            }
                        }

                        try
                        {
                            var json = JsonConvert.SerializeObject(ObjcontractualDpView);
                            ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                            aTSCall.SendPayrateBillratetoATS(json, LoggedInUserId, HR_ID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Update HR cost successfully" });
                        }
                    }
                }
                #endregion
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Update HR cost successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("UpdateTalentFees")]
        public ObjectResult UpdateTalentFees(long ContactPriorityID, decimal Cost_AfterAcceptance)
        {
            try
            {
                GenContactTalentPriority contactTalentPriority = null;
                var LoggedInUserId = SessionValues.LoginUserId;


                long HR_ID = 0;
                decimal HR_Percentage = 0;
                long TalentID = 0;

                #region Pre-Validation
                if (ContactPriorityID == 0 || ContactPriorityID < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data is not valid" });
                }
                else
                {
                    contactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.FirstOrDefault(y => y.Id == ContactPriorityID);
                    if (contactTalentPriority == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not exist" });
                    }
                }
                if (Cost_AfterAcceptance == 0 || Cost_AfterAcceptance < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Add Talent Fees" });
                }
                #endregion

                if (contactTalentPriority != null)
                {
                    HR_ID = (long)contactTalentPriority.HiringRequestId;
                    TalentID = (long)contactTalentPriority.TalentId;

                    GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();

                    if (_SalesHiringRequest != null)
                    {

                        HR_Percentage = contactTalentPriority.Nrpercentage ?? Convert.ToDecimal(_SalesHiringRequest.TalentCostCalcPercentage);

                        object[] param = new object[] { ContactPriorityID, Cost_AfterAcceptance, 0, TalentID, HR_Percentage };
                        string paramasString = CommonLogic.ConvertToParamString(param);
                        commonInterface.ViewAllHR.sproc_UTS_gen_ContactTalentPriorityupdate(paramasString);

                        #region Insert HR History 
                        param = new object[]
                        {
                            Action_Of_History.Talent_Fees_Update, contactTalentPriority.HiringRequestId, TalentID, false, LoggedInUserId, ContactPriorityID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                        #endregion
                    }
                }
                #region API CAll to send to ATS Team for Dp and Contractual HR
                if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    if (contactTalentPriority != null)
                    {
                        _talentConnectAdminDBContext.Entry(contactTalentPriority).Reload();

                        HR_ID = (long)contactTalentPriority.HiringRequestId;

                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(HR_ID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }

                        ContractualDpViewModel ObjcontractualDpView = new();
                        _talentConnectAdminDBContext.Entry(contactTalentPriority).Reload();

                        var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == HR_ID && x.Id == ContactPriorityID).Select(x => x.TalentId).ToList();

                        var User_UPID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserId).Select(x => x.EmployeeId).FirstOrDefault();
                        GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == HR_ID).FirstOrDefault();
                        GenSalesHiringRequestDetail _ObjSalesHiringrequestDetails = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(X => X.HiringRequestId == HR_ID).FirstOrDefault();

                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringRequest).Reload();
                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringrequestDetails).Reload();

                        if (_ObjSalesHiringRequest != null && _ObjSalesHiringrequestDetails != null)
                        {
                            ObjcontractualDpView.HRID = HR_ID;
                            ObjcontractualDpView.CreatedByDateTime = System.DateTime.Now.ToString("yyy-mm-dd HH:mm:ss");
                            ObjcontractualDpView.CreatedByID = User_UPID;
                            ObjcontractualDpView.HR_Currency = _ObjSalesHiringrequestDetails.Currency == null ? "" : _ObjSalesHiringrequestDetails.Currency;
                        }
                        if (talentList.Count != 0 && talentList != null)
                        {
                            foreach (var talentID in talentList)
                            {
                                HRTalentDetail talentDetail = new();
                                GenTalent Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();

                                _talentConnectAdminDBContext.Entry(Talent).Reload();

                                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == HR_ID && x.Id == ContactPriorityID).FirstOrDefault();
                                if (TalentCTP_Details != null && Talent != null)
                                {
                                    talentDetail.CTPID = TalentCTP_Details.Id;
                                    talentDetail.Talent_current_fee = TalentCTP_Details.CurrentCtc;
                                    talentDetail.Talent_Expected_fee = TalentCTP_Details.TalentCost;
                                    talentDetail.HR_Cost = TalentCTP_Details.FinalHrCost;
                                    talentDetail.HR_Cost_With_Currency = TalentCTP_Details.HrCost;
                                    talentDetail.HR_Type = TalentCTP_Details.IsHrtypeDp == true ? "DP" : "Contractual";
                                    talentDetail.DPAmount = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.Dpamount : 0;
                                    talentDetail.TalentCurrency = TalentCTP_Details.TalentCurrencyCode == null ? "USD" : TalentCTP_Details.TalentCurrencyCode;
                                    talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                                    talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                                    talentDetail.ExchangeRateUTS = TalentCTP_Details.ExchangeRate == null ? 1 : TalentCTP_Details.ExchangeRate;
                                    talentDetail.DPorNR_Percent = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.DpPercentage : TalentCTP_Details.Nrpercentage;

                                }
                                ObjcontractualDpView.TalentDetails.Add(talentDetail);

                            }
                        }

                        try
                        {
                            var json = JsonConvert.SerializeObject(ObjcontractualDpView);
                            ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                            aTSCall.SendPayrateBillratetoATS(json, LoggedInUserId, HR_ID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Update Talent Fees successfully" });
                        }
                    }
                }
                #endregion
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Update Talent Fees successfully" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Convert To DP

        [Authorize]
        [HttpGet("GetHrDPConversion")]
        public ObjectResult GetHrDPConversion(long? HiringRequest_ID)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HiringRequest_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HiringRequest_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                decimal DPAmount = Convert.ToDecimal(_SalesHiringRequest.Dppercentage);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DPAmount });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("SaveHrDPConversion")]
        public ObjectResult SaveHrDPConversion(long? HrId, decimal? DPAmount)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HrId == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HrId).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                if (DPAmount == null || DPAmount <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid DP Percentage." });
                }
                #endregion

                if (_SalesHiringRequest != null)
                {
                    _SalesHiringRequest.Dppercentage = DPAmount;
                    _SalesHiringRequest.IsHrtypeDp = true;
                    _SalesHiringRequest.TalentCostCalcPercentage = 0;
                    _talentConnectAdminDBContext.Entry(_SalesHiringRequest).State = EntityState.Modified;
                    _talentConnectAdminDBContext.SaveChanges();
                }

                #region Insert HR History 
                object[] param = new object[]
                 {
                     Action_Of_History.Convert_HR_To_DP, HrId, 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                 };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                #endregion


                #region ATSCall
                if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    if (_SalesHiringRequest != null)
                    {
                        _talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();
                        long HR_ID = _SalesHiringRequest.Id;
                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(HR_ID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }
                    }

                }
                #endregion


                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To DP successfully" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentsDPConversion")]
        public ObjectResult GetTalentsDPConversion(long? HiringRequest_ID)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HiringRequest_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HiringRequest_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                List<Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result> data = null;

                object[] param = new object[] { HiringRequest_ID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                data = commonInterface.ViewAllHR.Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion(paramasString);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("CalculateDPConversionCost")]
        public ObjectResult CalculateDPConversionCost(long HR_ID, long ContactPriorityID, decimal DPPercentage, decimal TalentExpectedCTC)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HR_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }

                if (DPPercentage > 100)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please enter proper percentage." });
                else if (DPPercentage <= 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "percentage must more than 0%." });

                #endregion

                GenContactTalentPriority contactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.Id == ContactPriorityID).FirstOrDefault();
                long TalentId = 0;
                decimal Talent_Cost = 0;
                decimal DPAmount = 00;
                long HR_Detail_Id = 0;
                string Currency = "USD";
                decimal CurrencyExchangeRate = 1;
                if (contactTalentPriority != null)
                {
                    HR_Detail_Id = Convert.ToInt64(contactTalentPriority.HiringRequestDetailId);
                    GenSalesHiringRequestDetail _SalesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.Id == HR_Detail_Id).FirstOrDefault();
                    if (_SalesHiringRequest_Details != null)
                    {
                        Currency = _SalesHiringRequest_Details.Currency;
                        PrgCurrencyExchangeRate _CurrencyExchangeRate = _talentConnectAdminDBContext.PrgCurrencyExchangeRates.Where(x => x.CurrencyCode == Currency).FirstOrDefault();
                        if (_CurrencyExchangeRate != null)
                        {
                            CurrencyExchangeRate = Convert.ToDecimal(_CurrencyExchangeRate.ExchangeRate);
                        }
                    }
                    TalentId = Convert.ToInt64(contactTalentPriority.TalentId);
                    GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == TalentId).FirstOrDefault();
                    if (_Talent != null)
                    {
                        Talent_Cost = Convert.ToDecimal(contactTalentPriority.TalentCost);
                        DPAmount = ((TalentExpectedCTC * 12) * DPPercentage) / 100;
                    }

                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DPAmount });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("SaveTalentsDPConversion")]
        public ObjectResult SaveTalentsDPConversion(List<ConvertToDP> convertToDPs)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                #region PreValidation
                if (convertToDPs == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Proper data." });
                }
                else
                {
                    foreach (var ConvertToDP in convertToDPs)
                    {
                        if (ConvertToDP.HRID <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid HR ID" });
                        }
                        if (ConvertToDP.TalentID <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Talent ID" });
                        }
                        if (ConvertToDP.CurrentCTC <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Talent CTC" });
                        }
                        if (ConvertToDP.ExpectedCTC <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Expected CTC" });
                        }
                        if (ConvertToDP.DpPercentage <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Dp Percentage" });
                        }
                        if (ConvertToDP.DPAmount <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Dp Amount" });
                        }
                    }
                }
                #endregion

                if (convertToDPs != null && convertToDPs.Any())
                {
                    foreach (var ConvertToDP in convertToDPs)
                    {
                        object[] param = new object[]
                        {
                            ConvertToDP.HRID, ConvertToDP.ContactTalentID , ConvertToDP.DPAmount,
                            ConvertToDP.DpPercentage, ConvertToDP.TalentID, ConvertToDP.CurrentCTC,ConvertToDP.ExpectedCTC
                        };
                        string paramasString = CommonLogic.ConvertToParamString(param);

                        commonInterface.ViewAllHR.sproc_UTS_UpdateDPAmountDetails(paramasString);

                        GenTalent talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == ConvertToDP.TalentID).FirstOrDefault();
                        if (talent != null)
                        {
                            talent.FinalCost = ConvertToDP.ExpectedCTC;
                            _talentConnectAdminDBContext.Entry(talent).State = EntityState.Modified;
                            _talentConnectAdminDBContext.SaveChanges();
                        }
                        GenOnBoardTalent OnboardTalent = _talentConnectAdminDBContext.GenOnBoardTalents.Where(x => x.HiringRequestId == ConvertToDP.HRID && x.TalentId == ConvertToDP.TalentID).FirstOrDefault();
                        if (OnboardTalent != null)
                        {
                            OnboardTalent.Nrpercentage = 0;
                            _talentConnectAdminDBContext.Entry(OnboardTalent).State = EntityState.Modified;
                            _talentConnectAdminDBContext.SaveChanges();
                        }

                        string PreviousHRType = "Contractual";
                        string CurrentHRType = "Direct Placement";

                        //Comment this code after dicussion with Payal mam on 15-08-24
                        //#region SendEmail
                        //EmailBinder emailBinder = new EmailBinder(iConfiguration, _talentConnectAdminDBContext);
                        ////emailBinder.SendClientEmailHRTypeChanged(PreviousHRType, CurrentHRType, ConvertToDP.HRID, ConvertToDP.TalentID);
                        ////emailBinder.SendTalentEmailHRTypeChanged(PreviousHRType, CurrentHRType, ConvertToDP.HRID, ConvertToDP.TalentID);
                        //bool salesemail = emailBinder.SendSalesPersonEmailHRTypeChanged(PreviousHRType, CurrentHRType, ConvertToDP.HRID, ConvertToDP.TalentID);  //, ConvertToDP.HRID, ConvertToDP.TalentID
                        //#endregion

                        #region Insert HR History 
                        param = new object[]
                        {
                            Action_Of_History.Convert_HR_To_DP, ConvertToDP.HRID, ConvertToDP.TalentID, false, LoggedInUserId, ConvertToDP.ContactTalentID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                        #endregion


                    }
                    #region API CAll to send to ATS Team for Dp and Contractual HR
                    if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                    {
                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(convertToDPs[0].HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }


                        ContractualDpViewModel ObjcontractualDpView = new();

                        var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == convertToDPs[0].HRID).Select(x => x.TalentId).ToList();

                        var User_UPID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserId).Select(x => x.EmployeeId).FirstOrDefault();
                        GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == convertToDPs[0].HRID).FirstOrDefault();
                        GenSalesHiringRequestDetail _ObjSalesHiringrequestDetails = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(X => X.HiringRequestId == convertToDPs[0].HRID).FirstOrDefault();

                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringRequest).Reload();
                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringrequestDetails).Reload();


                        if (_ObjSalesHiringRequest != null && _ObjSalesHiringrequestDetails != null)
                        {
                            ObjcontractualDpView.HRID = convertToDPs[0].HRID;
                            ObjcontractualDpView.CreatedByDateTime = System.DateTime.Now.ToString("yyy-mm-dd HH:mm:ss");
                            ObjcontractualDpView.CreatedByID = User_UPID;
                            ObjcontractualDpView.HR_Currency = _ObjSalesHiringrequestDetails.Currency == null ? "" : _ObjSalesHiringrequestDetails.Currency;
                        }
                        if (talentList.Count != 0 && talentList != null)
                        {
                            foreach (var talentID in talentList)
                            {
                                HRTalentDetail talentDetail = new();
                                GenTalent Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();
                                _talentConnectAdminDBContext.Entry(Talent).Reload();

                                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == convertToDPs[0].HRID).FirstOrDefault();
                                if (TalentCTP_Details != null && Talent != null)
                                {
                                    talentDetail.CTPID = TalentCTP_Details.Id;
                                    talentDetail.Talent_current_fee = TalentCTP_Details.CurrentCtc;
                                    talentDetail.Talent_Expected_fee = TalentCTP_Details.TalentCost;
                                    talentDetail.HR_Cost = TalentCTP_Details.FinalHrCost;
                                    talentDetail.HR_Cost_With_Currency = TalentCTP_Details.HrCost;
                                    talentDetail.HR_Type = TalentCTP_Details.IsHrtypeDp == true ? "DP" : "Contractual";
                                    talentDetail.DPAmount = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.Dpamount : 0;
                                    talentDetail.TalentCurrency = TalentCTP_Details.TalentCurrencyCode == null ? "USD" : TalentCTP_Details.TalentCurrencyCode;
                                    talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                                    talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                                    talentDetail.ExchangeRateUTS = TalentCTP_Details.ExchangeRate == null ? 1 : TalentCTP_Details.ExchangeRate;
                                    talentDetail.DPorNR_Percent = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.DpPercentage : TalentCTP_Details.Nrpercentage;
                                }
                                ObjcontractualDpView.TalentDetails.Add(talentDetail);

                            }
                        }

                        try
                        {
                            var json = JsonConvert.SerializeObject(ObjcontractualDpView);
                            ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                            aTSCall.SendPayrateBillratetoATS(json, LoggedInUserId, convertToDPs[0].HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To DP successfully" });
                        }

                    }
                    #endregion
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To DP successfully" });
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion

        #region Convert to Contractual

        [Authorize]
        [HttpGet("GetHrContractualConversion")]
        public ObjectResult GetHrContractualConversion(long? HiringRequest_ID)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HiringRequest_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HiringRequest_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                decimal NRPercentage = Convert.ToDecimal(_SalesHiringRequest.TalentCostCalcPercentage);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = NRPercentage });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("SaveHrContractualConversion")]
        public ObjectResult SaveHrContractualConversion(long? HrId, decimal? NRpercentage)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HrId == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HrId).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                if (NRpercentage == null || NRpercentage <= 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid DP Percentage." });

                #endregion

                if (_SalesHiringRequest != null)
                {
                    _SalesHiringRequest.TalentCostCalcPercentage = NRpercentage;
                    _SalesHiringRequest.IsHrtypeDp = false;
                    _SalesHiringRequest.Dppercentage = 0;
                    _talentConnectAdminDBContext.Entry(_SalesHiringRequest).State = EntityState.Modified;
                    _talentConnectAdminDBContext.SaveChanges();
                }

                #region Insert HR History 
                object[] param = new object[]
                 {
                     Action_Of_History.Convert_HR_To_Contractual, HrId, 0, false, LoggedInUserId, 0, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                 };
                _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                #endregion

                #region API CAll to send to ATS Team for Dp and Contractual HR
                if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                {
                    if (_SalesHiringRequest != null)
                    {
                        _talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();
                        long HR_ID = _SalesHiringRequest.Id;
                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(HR_ID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }
                    }

                }
                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To Contractual successfully" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetTalentsContractualConversion")]
        public ObjectResult GetTalentsContractualConversion(long? HiringRequest_ID)
        {
            try
            {
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HiringRequest_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HiringRequest_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                List<Sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion_Result> data = null;

                object[] param = new object[] { HiringRequest_ID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                data = commonInterface.ViewAllHR.sproc_UTS_Get_TalentDetails_For_DPToContractual_Conversion(paramasString);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet("CalculateHRCost")]
        public ObjectResult CalculateHRCost(long HR_ID, long ContactPriorityID, decimal Hr_Cost, decimal HR_Percentage)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest _SalesHiringRequest = null;

                #region PreValidation
                if (HR_ID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }

                if (HR_Percentage > 100)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please enter proper percentage." });

                else if (HR_Percentage <= 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "percentage must more than 0%." });

                #endregion

                GenContactTalentPriority contactTalentPriority = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.Id == ContactPriorityID).FirstOrDefault();
                long TalentId = 0;
                decimal Talent_Cost = 0;
                decimal HR_Cost = Hr_Cost;
                long HR_Detail_Id = 0;
                string Currency = "USD";
                decimal CurrencyExchangeRate = 1;

                if (contactTalentPriority != null)
                {
                    HR_Detail_Id = Convert.ToInt64(contactTalentPriority.HiringRequestDetailId);
                    GenSalesHiringRequestDetail _SalesHiringRequest_Details = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(x => x.Id == HR_Detail_Id).FirstOrDefault();
                    if (_SalesHiringRequest_Details != null)
                    {
                        Currency = _SalesHiringRequest_Details.Currency;
                        PrgCurrencyExchangeRate _CurrencyExchangeRate = _talentConnectAdminDBContext.PrgCurrencyExchangeRates.Where(x => x.CurrencyCode == Currency).FirstOrDefault();
                        if (_CurrencyExchangeRate != null)
                        {
                            CurrencyExchangeRate = Convert.ToDecimal(_CurrencyExchangeRate.ExchangeRate);
                        }
                    }
                    TalentId = Convert.ToInt64(contactTalentPriority.TalentId);
                    GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.FirstOrDefault(x => x.Id == TalentId);
                    if (_Talent != null)
                    {
                        Talent_Cost = Convert.ToDecimal(contactTalentPriority.TalentCost);

                        object[] param = new object[] { Talent_Cost, 0, 0, HR_Percentage, _SalesHiringRequest.IsHrtypeDp };
                        string paramasString = CommonLogic.ConvertToParamString(param);
                        sp_UTS_TalentCalculation_PayPerHire_Result HR_CostCalcultaion = commonInterface.ViewAllHR.GetTalentLevelCalculation(paramasString);
                        if (HR_CostCalcultaion != null)
                        {
                            HR_Cost = HR_CostCalcultaion.CalculatedClientPay;
                        }
                        else
                        {
                            HR_Cost = ((Talent_Cost * 100) / (100 - HR_Percentage));
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HR_Cost });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("SaveTalentsContractualConversion")]
        public ObjectResult SaveTalentsContractualConversion(List<ConvertToContractual> convertToContractuals)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                #region PreValidation
                if (convertToContractuals == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide data." });
                }
                else
                {
                    foreach (var ConvertToContractual in convertToContractuals)
                    {
                        if (ConvertToContractual.HRID <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid HR ID" });
                        }
                        if (ConvertToContractual.TalentID <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Talent ID" });
                        }
                        if (ConvertToContractual.NRPercentage <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid NR Percentage" });
                        }
                        if (ConvertToContractual.HR_Cost <= 0)
                        {
                            return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid HR cost" });
                        }
                    }
                }
                #endregion

                if (convertToContractuals != null && convertToContractuals.Any())
                {
                    foreach (var ConvertToContractual in convertToContractuals)
                    {
                        object[] param = new object[]
                        {
                            ConvertToContractual.ContactTalentID, 0, ConvertToContractual.HR_Cost,
                            ConvertToContractual.TalentID, ConvertToContractual.NRPercentage
                        };
                        string paramasString = CommonLogic.ConvertToParamString(param);
                        commonInterface.ViewAllHR.sproc_UTS_gen_ContactTalentPriorityupdate(paramasString);

                        param = new object[]
                        {
                            ConvertToContractual.ContactTalentID, ConvertToContractual.TalentID,
                            ConvertToContractual.HRID, ConvertToContractual.NRPercentage, ConvertToContractual.IsHiringLimited,
                            ConvertToContractual.SpecificMonth, ConvertToContractual.durationType ?? string.Empty
                        };
                        paramasString = CommonLogic.ConvertToParamString(param);
                        commonInterface.ViewAllHR.sproc_UTS_UpdateIsHrtypedpandDPAmount(paramasString);

                        string PreviousHRType = "Direct Placement";
                        string CurrentHRType = "Contractual";

                        //Comment this code after dicussion with Payal mam on 15-08-24
                        //#region SendEmail
                        //EmailBinder emailBinder = new EmailBinder(iConfiguration, _talentConnectAdminDBContext);
                        ////emailBinder.SendClientEmailHRTypeChanged(PreviousHRType, CurrentHRType, ConvertToContractual.HRID, ConvertToContractual.TalentID);
                        ////emailBinder.SendTalentEmailHRTypeChanged(PreviousHRType, CurrentHRType, ConvertToContractual.HRID, ConvertToContractual.TalentID);
                        //bool salesemail = emailBinder.SendSalesPersonEmailHRTypeChanged(PreviousHRType, CurrentHRType, ConvertToContractual.HRID, ConvertToContractual.TalentID); //, ConvertToContractual.HRID, ConvertToContractual.TalentID
                        //#endregion

                        #region Insert HR History 
                        param = new object[]
                        {
                            Action_Of_History.Convert_HR_To_Contractual, ConvertToContractual.HRID, ConvertToContractual.TalentID,
                            false, LoggedInUserId, ConvertToContractual.ContactTalentID, 0, "", 0, false, false, 0, 0, (short)AppActionDoneBy.UTS
                        };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_HiringRequest_History_Insert, param);
                        #endregion
                    }

                    #region API CAll to send to ATS Team for Dp and Contractual HR
                    if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                    {
                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(convertToContractuals[0].HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }
                        ContractualDpViewModel ObjcontractualDpView = new();


                        var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == convertToContractuals[0].HRID).Select(x => x.TalentId).ToList();

                        var User_UPID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserId).Select(x => x.EmployeeId).FirstOrDefault();
                        GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == convertToContractuals[0].HRID).FirstOrDefault();
                        GenSalesHiringRequestDetail _ObjSalesHiringrequestDetails = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(X => X.HiringRequestId == convertToContractuals[0].HRID).FirstOrDefault();

                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringRequest).Reload();
                        _talentConnectAdminDBContext.Entry(_ObjSalesHiringrequestDetails).Reload();

                        if (_ObjSalesHiringRequest != null && _ObjSalesHiringrequestDetails != null)
                        {
                            ObjcontractualDpView.HRID = convertToContractuals[0].HRID;
                            ObjcontractualDpView.CreatedByDateTime = System.DateTime.Now.ToString("yyy-mm-dd HH:mm:ss");
                            ObjcontractualDpView.CreatedByID = User_UPID;
                            ObjcontractualDpView.HR_Currency = _ObjSalesHiringrequestDetails.Currency == null ? "" : _ObjSalesHiringrequestDetails.Currency;
                        }
                        if (talentList.Count != 0 && talentList != null)
                        {
                            foreach (var talentID in talentList)
                            {
                                HRTalentDetail talentDetail = new();
                                GenTalent Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();
                                _talentConnectAdminDBContext.Entry(Talent).Reload();

                                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == convertToContractuals[0].HRID).FirstOrDefault();
                                if (TalentCTP_Details != null && Talent != null)
                                {
                                    talentDetail.CTPID = TalentCTP_Details.Id;
                                    talentDetail.Talent_current_fee = TalentCTP_Details.CurrentCtc;
                                    talentDetail.Talent_Expected_fee = TalentCTP_Details.TalentCost;
                                    talentDetail.HR_Cost = TalentCTP_Details.FinalHrCost;
                                    talentDetail.HR_Cost_With_Currency = TalentCTP_Details.HrCost;
                                    talentDetail.HR_Type = TalentCTP_Details.IsHrtypeDp == true ? "DP" : "Contractual";
                                    talentDetail.DPAmount = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.Dpamount : 0;
                                    talentDetail.TalentCurrency = TalentCTP_Details.TalentCurrencyCode == null ? "USD" : TalentCTP_Details.TalentCurrencyCode;
                                    talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                                    talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                                    talentDetail.ExchangeRateUTS = TalentCTP_Details.ExchangeRate == null ? 1 : TalentCTP_Details.ExchangeRate;
                                    talentDetail.DPorNR_Percent = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.DpPercentage : TalentCTP_Details.Nrpercentage;
                                }
                                ObjcontractualDpView.TalentDetails.Add(talentDetail);
                            }
                        }

                        try
                        {
                            var json = JsonConvert.SerializeObject(ObjcontractualDpView);
                            ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                            aTSCall.SendPayrateBillratetoATS(json, LoggedInUserId, convertToContractuals[0].HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To Contractual successfully" });
                        }

                    }
                    #endregion
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To Contractual successfully" });
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        //UTS-3915: Update the DP Amount if HR is DP.
        #region Update DP Amount

        [Authorize]
        [HttpGet("GetHRDPAmountDetails")]
        public ObjectResult GetHRDPAmountDetails(long hrID, long contactPriorityID, long talentId)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;
                GenSalesHiringRequest _SalesHiringRequest = new();

                #region PreValidation
                if (hrID == 0)
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "please provide HR ID." });
                else
                {
                    _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == hrID).FirstOrDefault();
                    if (_SalesHiringRequest == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "HR Does not exists." });
                    }
                }
                #endregion

                decimal talentCurrentCTC = 0;
                decimal dPPercentage = 10;
                decimal talentExpectedCTC = 0;

                GenTalent talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentId).FirstOrDefault();
                if (talent != null)
                {
                    talentCurrentCTC = Convert.ToDecimal(talent.CurrentCtc);
                    talentExpectedCTC = Convert.ToDecimal(talent.FinalCost);
                }

                if (_SalesHiringRequest != null)
                {
                    dPPercentage = Convert.ToDecimal(_SalesHiringRequest.Dppercentage);
                }

                GenContactTalentPriority ContactTalentPriorityData = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.Id == contactPriorityID).FirstOrDefault();
                dynamic result = new ExpandoObject();

                if (ContactTalentPriorityData != null)
                {
                    result.ContactTalentPriorityID = ContactTalentPriorityData.Id;
                    result.TalentCurrentCTC = ContactTalentPriorityData.CurrentCtc != null && ContactTalentPriorityData.CurrentCtc > 0 ?
                        ContactTalentPriorityData.CurrentCtc : talentCurrentCTC;
                    result.TalentExpectedCTC =
                        ContactTalentPriorityData.TalentCost != null && ContactTalentPriorityData.TalentCost > 0 ?
                        ContactTalentPriorityData.TalentCost : talentExpectedCTC;
                    result.DP_Percentage =
                        ContactTalentPriorityData.DpPercentage != null && ContactTalentPriorityData.DpPercentage > 0 ?
                        ContactTalentPriorityData.DpPercentage : dPPercentage;
                    result.DPAmount = ContactTalentPriorityData.Dpamount;
                    result.HRID = hrID;
                    result.TalentId = talentId;
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }


        [Authorize]
        [HttpPost("UpdateDPAmount")]
        public ObjectResult UpdateDPAmount(ConvertToDP ConvertToDP)
        {
            try
            {
                var LoggedInUserId = SessionValues.LoginUserId;

                #region PreValidation
                if (ConvertToDP == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Proper data." });
                }
                else
                {
                    if (ConvertToDP.HRID <= 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid HR ID" });
                    }
                    if (ConvertToDP.TalentID <= 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Talent ID" });
                    }
                    if (ConvertToDP.CurrentCTC <= 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Talent CTC" });
                    }
                    if (ConvertToDP.ExpectedCTC <= 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Expected CTC" });
                    }
                    if (ConvertToDP.DpPercentage <= 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Dp Percentage" });
                    }
                    if (ConvertToDP.DPAmount <= 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Enter Valid Dp Amount" });
                    }
                }
                #endregion

                if (ConvertToDP != null)
                {
                    object[] param = new object[]
                         {
                            ConvertToDP.HRID,
                            ConvertToDP.ContactTalentID ,
                            ConvertToDP.DPAmount,
                            ConvertToDP.DpPercentage,
                            ConvertToDP.TalentID,
                            ConvertToDP.CurrentCTC,
                            ConvertToDP.ExpectedCTC
                         };

                    string paramasString = CommonLogic.ConvertToParamString(param);

                    commonInterface.ViewAllHR.sproc_UTS_UpdateDPAmountDetails(paramasString);

                    GenTalent talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == ConvertToDP.TalentID).FirstOrDefault();
                    if (talent != null)
                    {
                        talent.FinalCost = ConvertToDP.ExpectedCTC;
                        _talentConnectAdminDBContext.Entry(talent).State = EntityState.Modified;
                        _talentConnectAdminDBContext.SaveChanges();
                    }

                    #region API CAll to send to ATS Team for Dp update.
                    if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                    {
                        try
                        {
                            ATSCommonAPI commonAPI = new(_talentConnectAdminDBContext, iConfiguration, _httpContextAccessor.HttpContext);
                            commonAPI.SendHRDetailsToPMS(ConvertToDP.HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "HR Debriefing has been created" });
                        }

                        ContractualDpViewModel ObjcontractualDpView = new();

                        var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == ConvertToDP.HRID && x.Id == ConvertToDP.ContactTalentID).Select(x => x.TalentId).ToList();

                        var User_UPID = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserId).Select(x => x.EmployeeId).FirstOrDefault();
                        GenSalesHiringRequest _ObjSalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == ConvertToDP.HRID).FirstOrDefault();
                        GenSalesHiringRequestDetail _ObjSalesHiringrequestDetails = _talentConnectAdminDBContext.GenSalesHiringRequestDetails.Where(X => X.HiringRequestId == ConvertToDP.HRID).FirstOrDefault();
                        if (_ObjSalesHiringRequest != null && _ObjSalesHiringrequestDetails != null)
                        {
                            ObjcontractualDpView.HRID = ConvertToDP.HRID;
                            ObjcontractualDpView.CreatedByDateTime = System.DateTime.Now.ToString("yyy-mm-dd HH:mm:ss");
                            ObjcontractualDpView.CreatedByID = User_UPID;
                            ObjcontractualDpView.HR_Currency = _ObjSalesHiringrequestDetails.Currency == null ? "" : _ObjSalesHiringrequestDetails.Currency;
                        }
                        if (talentList.Count != 0 && talentList != null)
                        {
                            foreach (var talentID in talentList)
                            {
                                HRTalentDetail talentDetail = new();
                                GenTalent Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();

                                var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == ConvertToDP.HRID).FirstOrDefault();
                                if (TalentCTP_Details != null && Talent != null)
                                {
                                    talentDetail.CTPID = TalentCTP_Details.Id;
                                    talentDetail.Talent_current_fee = TalentCTP_Details.CurrentCtc;
                                    talentDetail.Talent_Expected_fee = TalentCTP_Details.TalentCost;
                                    talentDetail.HR_Cost = TalentCTP_Details.FinalHrCost;
                                    talentDetail.HR_Cost_With_Currency = TalentCTP_Details.HrCost;
                                    talentDetail.HR_Type = TalentCTP_Details.IsHrtypeDp == true ? "DP" : "Contractual";
                                    talentDetail.DPAmount = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.Dpamount : 0;
                                    talentDetail.TalentCurrency = TalentCTP_Details.TalentCurrencyCode == null ? "USD" : TalentCTP_Details.TalentCurrencyCode;
                                    talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                                    talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                                    talentDetail.ExchangeRateUTS = TalentCTP_Details.ExchangeRate == null ? 1 : TalentCTP_Details.ExchangeRate;
                                    talentDetail.DPorNR_Percent = TalentCTP_Details.IsHrtypeDp == true ? TalentCTP_Details.DpPercentage : TalentCTP_Details.Nrpercentage;
                                }
                                ObjcontractualDpView.TalentDetails.Add(talentDetail);

                            }
                        }

                        try
                        {
                            var json = JsonConvert.SerializeObject(ObjcontractualDpView);
                            ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                            aTSCall.SendPayrateBillratetoATS(json, LoggedInUserId, ConvertToDP.HRID);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Convert To DP successfully" });
                        }

                    }
                    #endregion
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "DP amount updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        #endregion

        [Authorize]
        [HttpPost("SetHrPriority")]
        public async Task<ObjectResult> SetHrPriority(string IsNextWeekStarMarked, string HiringRequestID, string SalesPerson)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(IsNextWeekStarMarked))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Provide Star Marked Details." });

                }

                if (string.IsNullOrWhiteSpace(HiringRequestID))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Provide Hiring Request Id." });

                }
                if (string.IsNullOrWhiteSpace(SalesPerson))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please Provide Sales Person name." });
                }

                long LoggedInUserId = SessionValues.LoginUserId;
                var SalesPersonId = _talentConnectAdminDBContext.UsrUsers.Where(x => x.FullName == SalesPerson).Select(x => x.Id).FirstOrDefault();
                int count = 0;
                var HR_ID = Convert.ToInt64(HiringRequestID);
                var HRPriorityToSet = IsNextWeekStarMarked == "1" ? true : false;
                var SalesPerson_Id = Convert.ToInt64(SalesPersonId);
                CheckValidationMessage dataValidation = new();
                if (HRPriorityToSet) // remove if condition Anit for star check and uncheck
                {
                    dataValidation = commonInterface.ViewAllHR.Sproc_Get_Validation_Message_For_Priority(LoggedInUserId);
                }
                if (dataValidation != null && string.IsNullOrWhiteSpace(dataValidation.Message))
                {
                    Object[] objects = new object[] { LoggedInUserId, SalesPerson_Id, HR_ID, HRPriorityToSet };
                    string paramstring = CommonLogic.ConvertToParamString(objects);
                    commonInterface.ViewAllHR.InsertHiringRequestPriorityReviseFlow(paramstring);
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = "Priority Updated" });
                }

                string strErrorMessage = "Error";
                if (dataValidation != null && !string.IsNullOrWhiteSpace(dataValidation.Message))
                {
                    strErrorMessage = dataValidation.Message;
                }

                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = strErrorMessage });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetRemainingPriorityCount")]
        public async Task<ObjectResult> GetRemainingPriorityCount()
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                var data = await commonInterface.ViewAllHR.GetPrioritySetRemainingCountDetailsResult(LoggedInUserId);
                if (data != null && data.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = data });
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject()
                    {
                        statusCode = StatusCodes.Status404NotFound,
                        Message = "No records found"
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("UpdateTR")]
        public async Task<ObjectResult> UpdateTR(UpdateTRModel updateTRModel)
        {
            try
            {
                Int64 LoggedInUserID = SessionValues.LoginUserId;

                var varUsrUserById = commonInterface.TalentStatus.GetUsrUserById(LoggedInUserID);

                if (updateTRModel != null)
                {
                    Sproc_Check_Validation_Message_For_User_Edit_TR_Result messageresult = new Sproc_Check_Validation_Message_For_User_Edit_TR_Result();
                    if (!updateTRModel.IsFinalSubmit)
                    {
                        object[] param = new object[] { updateTRModel.HiringRequestId, updateTRModel.NoOfTR };
                        string paramasString = CommonLogic.ConvertToParamString(param);

                        var resultMessageresult = await commonInterface.ViewAllHR.Get_Sproc_Check_Validation_Message_For_User_Edit_TR(paramasString).ConfigureAwait(false);
                        messageresult = resultMessageresult.FirstOrDefault();
                    }
                    if (string.IsNullOrEmpty(messageresult.Message) || messageresult.Message == "Success")
                    {
                        object[] param = new object[] { updateTRModel.HiringRequestId, LoggedInUserID, updateTRModel.NoOfTR, updateTRModel.ReasonForLossCancelled, updateTRModel.AddtionalRemarks, (int)AppActionDoneBy.UTS };
                        string paramasString = CommonLogic.ConvertToParamString(param);

                        Sproc_UpdateTR_Result messageResult = new Sproc_UpdateTR_Result();
                        var ResultMessageResult = await commonInterface.ViewAllHR.Get_Sproc_UpdateTR(paramasString).ConfigureAwait(false);
                        messageResult = ResultMessageResult.FirstOrDefault();

                        //UTS-3117: Send email when TR is updated.
                        if (string.IsNullOrEmpty(messageResult.Message) || messageResult.Message == "Success")
                        {
                            EmailBinder binder = new EmailBinder(iConfiguration, _talentConnectAdminDBContext);

                            string reason = updateTRModel.AddtionalRemarks;
                            if (string.IsNullOrEmpty(updateTRModel.AddtionalRemarks))
                            {
                                reason = updateTRModel.ReasonForLossCancelled;
                            }

                            binder.SendEmailForHRStatusUpdateToInternalTeam(updateTRModel.HiringRequestId, reason);

                            #region API Call to Send to ATS Team for TR Update
                            if (iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                            {
                                TRUpdateViewModel objTRUpdateViewModel = new();

                                var talentList = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.HiringRequestId == updateTRModel.HiringRequestId).Select(x => x.TalentId).ToList();
                                objTRUpdateViewModel.HRID = updateTRModel.HiringRequestId;


                                GenSalesHiringRequest _SalesHiringRequest = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(t => t.Id == updateTRModel.HiringRequestId).FirstOrDefault();
                                _talentConnectAdminDBContext.Entry(_SalesHiringRequest).Reload();

                                int hrStatusId = Convert.ToInt32(_SalesHiringRequest.StatusId);

                                var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == hrStatusId).FirstOrDefault();
                                if (HRStatusData != null)
                                {
                                    objTRUpdateViewModel.HRStatusID = HRStatusData.Id;
                                    objTRUpdateViewModel.HRStatus = HRStatusData.HiringRequestStatus;
                                }

                                objTRUpdateViewModel.TR = updateTRModel.NoOfTR;

                                //if (talentList.Count != 0 && talentList != null)
                                //{
                                //    foreach (var talentID in talentList)
                                //    {
                                //        GenTalent Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == talentID).FirstOrDefault();

                                //        _talentConnectAdminDBContext.Entry(Talent).Reload();

                                //        TRUpdateTalentDetail talentDetail = new();
                                //        talentDetail.ATS_TalentID = Convert.ToInt64(Talent.AtsTalentId);
                                //        var TalentCTP_Details = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == Talent.Id && x.HiringRequestId == updateTRModel.HiringRequestId).FirstOrDefault();
                                //        if (TalentCTP_Details != null)
                                //        {
                                //            var TalStatusClientSelectionDetail = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == TalentCTP_Details.TalentStatusIdBasedOnHr).FirstOrDefault();
                                //            if (TalStatusClientSelectionDetail != null)
                                //                talentDetail.TalentStatus = TalStatusClientSelectionDetail.TalentStatus;
                                //            else
                                //                talentDetail.TalentStatus = string.Empty;

                                //            talentDetail.Reason = TalentCTP_Details.OtherRejectReason;
                                //        }
                                //        else
                                //        {
                                //            talentDetail.TalentStatus = string.Empty;
                                //            talentDetail.Reason = string.Empty;
                                //        }

                                //        talentDetail.UTS_TalentID = Convert.ToInt64(Talent.Id);
                                //        talentDetail.Talent_USDCost = Talent.FinalCost ?? 0;

                                //        object[] objParam = new object[] { objTRUpdateViewModel.HRID, Convert.ToInt64(Talent.Id) };
                                //        string strParamas = CommonLogic.ConvertToParamString(objParam);
                                //        var varTalent_RejectReason = commonInterface.TalentStatus.sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;

                                //        talentDetail.Talent_RejectReason = varTalent_RejectReason;
                                //        talentDetail.RejectedBy = varUsrUserById.EmployeeId;
                                //        objTRUpdateViewModel.TalentDetails.Add(talentDetail);
                                //    }
                                //}

                                try
                                {
                                    var json = JsonConvert.SerializeObject(objTRUpdateViewModel);
                                    ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                                    aTSCall.SendUpdatedTRtoATS(json, LoggedInUserID, updateTRModel.HiringRequestId);

                                    #region HR Status updates to ATS 

                                    //New Status Change

                                    object[] paramStatus = new object[] { updateTRModel.HiringRequestId, 0, 0, LoggedInUserID, (short)AppActionDoneBy.UTS, false };
                                    string strParam = CommonLogic.ConvertToParamString(paramStatus);
                                    var HRStatus_Json = commonInterface.hiringRequest.GetUpdateHrStatus(strParam);
                                    if (HRStatus_Json != null)
                                    {
                                        //string JsonData = Convert.ToString(HRStatus_Json);
                                        var JsonData = JsonConvert.SerializeObject(HRStatus_Json);
                                        if (!string.IsNullOrEmpty(JsonData))
                                        {
                                            aTSCall.SendHrStatusToATS(JsonData, LoggedInUserID, updateTRModel.HiringRequestId);
                                        }
                                    }

                                    #endregion
                                }
                                catch (Exception)
                                {
                                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Unexpected error occurs" });
                                }
                            }
                            #endregion

                        }

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = ResultMessageResult.FirstOrDefault().Message, Details = ResultMessageResult.FirstOrDefault().Message });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = messageresult.Message });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    Message = "Bad Request"
                });

            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost("GetAllUnAssignedHiringRequests")]
        public IActionResult GetAllUnAssignedHiringRequests([FromBody] ViewAllUnAssignedHRViewModel viewAllUnAssignedHRViewModel)
        {
            try
            {
                #region PreValidation
                if (viewAllUnAssignedHRViewModel == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });

                if (viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs == null)
                    viewAllUnAssignedHRViewModel.FilterFields_ViewAllUnAssignedHRs = new();
                #endregion

                var LoggedInUserTypeID = SessionValues.LoginUserTypeId;
                var LoggedInUserID = SessionValues.LoginUserId;

                List<sproc_ViewAllUnAssignedHRs_Result> allHRListData = commonInterface.ViewAllHR.GetAllUnAssignedHRs(viewAllUnAssignedHRViewModel, LoggedInUserTypeID, LoggedInUserID);


                if (allHRListData.Count > 0)
                {
                    // UTS-7517: Show Clone to demo account CTA in HR listing page.
                    var result = CustomRendering.ListingResponses(allHRListData, allHRListData[0].TotalRecords, viewAllUnAssignedHRViewModel.Pagesize, viewAllUnAssignedHRViewModel.Pagenum);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Hiring Requests Available", Details = CustomRendering.EmptyRows() });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("AssinedPOCforUnAssignedHRs")]
        public string AssinedPOCforUnAssignedHRs([FromBody] AssinedPOCforUnAssignedHRs assinedPOCforUnAssignedHRs)
        {
            try
            {
                long loggedinUserID = SessionValues.LoginUserId;
                object[] param = new object[] { assinedPOCforUnAssignedHRs.HRID, assinedPOCforUnAssignedHRs.POCID, loggedinUserID };
                string paramasString = CommonLogic.ConvertToParamString(param);

                commonInterface.ViewAllHR.sproc_AssignedPOCToUnAssignedHRs(paramasString);

                if (!iConfiguration["HRDataSendSwitchForPHP"].ToString().ToLower().Equals("local"))
                {
                    long HR_ID = assinedPOCforUnAssignedHRs.HRID;
                    string HR_Number = "";
                    var objHR = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HR_ID).FirstOrDefault();
                    if (objHR != null)
                    {
                        HR_Number = objHR.HrNumber ?? "";
                    }

                    if (HR_ID != 0 && !string.IsNullOrEmpty(HR_Number))
                    {
                        var HRData_Json = commonInterface.ViewAllHR.GetAllHRDataForAdmin(HR_ID);
                        string HRJsonData = Convert.ToString(HRData_Json.Result);
                        if (!string.IsNullOrEmpty(HRJsonData))
                        {
                            bool isAPIResponseSuccess = true;

                            try
                            {
                                ATSCall aTSCall = new ATSCall(iConfiguration, _talentConnectAdminDBContext);
                                if (HRJsonData != "")
                                    isAPIResponseSuccess = aTSCall.SendHRDataToPMS(HRJsonData.ToString(), HR_ID);
                            }
                            catch (Exception)
                            {
                                return "Error";
                            }
                        }
                    }
                }

                return "POC Assigned Successfully.";
            }
            catch (Exception)
            {

                throw;
            }
        }



        #region SLA details, Update, and History

        [HttpGet("GetHiringRequestSLADetails")]
        public async Task<ObjectResult> GetHiringRequestSLADetails(long hrId)
        {
            try
            {
                if (hrId <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide hr id." });
                }

                object[] param = new object[] { hrId };
                string paramasString = CommonLogic.ConvertToParamString(param);

                var HiringRequestSLADetailResult = await commonInterface.ViewAllHR.Get_HiringRequest_SLADetails(paramasString).ConfigureAwait(false);

                var editSLAReasons = await commonInterface.ViewAllHR.GetSLAEditReasons().ConfigureAwait(false);

                var slaUpdateHistoryResult = await commonInterface.ViewAllHR.Get_SLAUpdate_History(paramasString).ConfigureAwait(false);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject()
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Data retrieved Successfully",
                    Details = new
                    {
                        hrSLADetails = HiringRequestSLADetailResult,
                        SLAReasons = editSLAReasons,
                        slaUpdateHistoryResult = slaUpdateHistoryResult
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpGet("GetSLAUpdateHistory")]
        //public async Task<ObjectResult> GetSLAUpdateHistory(long hrId)
        //{
        //    try
        //    {
        //        if (hrId <= 0)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide hr id." });
        //        }

        //        object[] param = new object[] { hrId };
        //        string paramasString = CommonLogic.ConvertToParamString(param);

        //        var slaUpdateHistoryResult = await commonInterface.ViewAllHR.Get_SLAUpdate_History(paramasString).ConfigureAwait(false);

        //        if (slaUpdateHistoryResult is not null)
        //        {
        //            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Data retrieved Successfully", Details = slaUpdateHistoryResult });
        //        }
        //        else
        //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject()
        //            {
        //                statusCode = StatusCodes.Status400BadRequest,
        //                Message = "Bad Request"
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        [HttpPost("UpdateSLADate")]
        public async Task<ObjectResult> UpdateSLADate(UpdateHRSLADateViewModel updateHRSLADateViewModel)
        {
            try
            {
                if (updateHRSLADateViewModel is null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No data found." });

                if (updateHRSLADateViewModel.HrId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please Provide hr id." });

                var actionId = 89;
                object[] param = new object[] {
                    updateHRSLADateViewModel.HrId,
                    updateHRSLADateViewModel.ReasonId,
                    updateHRSLADateViewModel.OtherReason,
                    updateHRSLADateViewModel.SlaRevisedDate,
                    actionId,
                    SessionValues.LoginUserId
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                commonInterface.ViewAllHR.Sproc_Update_SLADate(paramasString);

                #region Send SLA Update emails to Client & Internal Team

                object[] param1 = new object[] {
                    updateHRSLADateViewModel.HrId,
                };
                Sproc_Get_SLAUpdateDetails_ForEmail_Result sproc_Get_SLAUpdateDetails_ForEmail_Result = await commonInterface.ViewAllHR.Sproc_Get_SLAUpdateDetails_ForEmail(CommonLogic.ConvertToParamString(param1));

                if (sproc_Get_SLAUpdateDetails_ForEmail_Result is not null)
                {
                    EmailBinder binder = new EmailBinder(iConfiguration, _talentConnectAdminDBContext);

                    // If contact has the flag on for NotificationSend then only send the emails to client.
                    //GenContact? _Contact = _talentConnectAdminDBContext.GenContacts.FirstOrDefault(x => x.Id == sproc_Get_SLAUpdateDetails_ForEmail_Result.ContactId);
                    //var IsClientNotificationSent = false;

                    //if (_Contact != null)
                    //{
                    //    IsClientNotificationSent = _Contact.IsClientNotificationSend;
                    //}

                    //if (IsClientNotificationSent)
                    //{
                    //    binder.SendSLAUpdateEmailToClient(sproc_Get_SLAUpdateDetails_ForEmail_Result);
                    //}

                    binder.SendSLAUpdateEmailToInternalTeam(sproc_Get_SLAUpdateDetails_ForEmail_Result);
                }

                #endregion

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "SLA Date updated Successfully", Details = null });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Private
        private IActionResult Export_HrList(List<sproc_ViewAllHRs_Result> HRList)
        {
            var ExportFileName = "HiringRequestExport_" + DateTime.Now.ToString("yyyyMMddHHmm") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("HiringRequestList");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Created Date";
            worksheet.Cell(currentRow, 2).Value = "HR ID";
            worksheet.Cell(currentRow, 3).Value = "TR";
            worksheet.Cell(currentRow, 4).Value = "Position";
            worksheet.Cell(currentRow, 5).Value = "Cat";
            worksheet.Cell(currentRow, 6).Value = "Company";
            worksheet.Cell(currentRow, 7).Value = "Time";
            worksheet.Cell(currentRow, 8).Value = "FTE/PTE";
            worksheet.Cell(currentRow, 9).Value = "Sales Rep";
            worksheet.Cell(currentRow, 10).Value = "HR Status";

            foreach (var hr in HRList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = hr.CreatedDateTime;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.HR;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.TR;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.Position;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.CompanyCategory;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.Company;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.TimeZone;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.TypeOfEmployee;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.SalesRep;
                worksheet.Cell(currentRow, currentColumn++).Value = hr.HRStatus;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }

        #endregion

        #region GetHR History Activity List Using Pagination
        [HttpPost("GetHRActivityUsingPagination")]
        public ObjectResult GetHRActivityUsingPagination([FromBody] HRActivityUsingPagination hRActivityUsingPagination)
        {
            try
            {
                var varLoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (hRActivityUsingPagination == null || (hRActivityUsingPagination.pagenumber == 0 || hRActivityUsingPagination.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion


                object[] param = new object[]
                {
                    hRActivityUsingPagination.filterFields.HRID,
                    hRActivityUsingPagination.pagenumber,
                    hRActivityUsingPagination.totalrecord
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                var result = commonInterface.ViewAllHR.sp_UTS_get_HRHistory_UsingPagination_Result(paramasString);

                if (result != null && result.Any())
                {
                    var TotalRecords = result.ToList().FirstOrDefault().TotalRecords.Value;
                    var FinalResult = CustomRendering.ListingResponses(result, TotalRecords, hRActivityUsingPagination.totalrecord, hRActivityUsingPagination.pagenumber);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = FinalResult });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Data not available" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetHR Talent List Using Pagination
        [HttpPost("GetHRTalentsUsingPagination")]
        public ObjectResult GetHRTalentsUsingPagination([FromBody] HRTalentDetailsViewModel hRTalentDetailsViewModel)
        {
            try
            {
                var varLoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (hRTalentDetailsViewModel == null || (hRTalentDetailsViewModel.pagenumber == 0 || hRTalentDetailsViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion


                object[] param = new object[]
                {
                    hRTalentDetailsViewModel.filterFields.HRID,
                    hRTalentDetailsViewModel.pagenumber,
                    hRTalentDetailsViewModel.totalrecord
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                var result = commonInterface.ViewAllHR.sp_UTS_get_HRTalentDetails_UsingPagination_Result(paramasString);
                var TotalRecords = 0;
                if (result.Count > 0)
                {
                    TotalRecords = result.ToList().FirstOrDefault().TotalRecords.Value;
                }

                var FinalResult = CustomRendering.ListingResponses(result, TotalRecords, hRTalentDetailsViewModel.totalrecord, hRTalentDetailsViewModel.pagenumber);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = JsonConvert.SerializeObject(FinalResult) });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
