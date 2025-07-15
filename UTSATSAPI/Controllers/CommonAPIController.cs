using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Helpers;
using static UTSATSAPI.Helpers.Enum;
using System.Transactions;
using DocumentFormat.OpenXml.InkML;
using UTSATSAPI.Middlewares;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Repositories.Interfaces;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace UTSATSAPI.Controllers
{
    [ApiController]
    [Route("CommonAPI/")]
    [CustomAuthorize("Authorization", "cf5982fd8d34e51113d0ec762a42c190be2bf15084")]
    public class CommonAPIController : ControllerBase
    {
        private readonly UTSATSAPIDBConnection _dbContext;
        private readonly IConfiguration _iConfiguration;
        private readonly IATSsyncUTS _iATSsyncUTS;

        public CommonAPIController(UTSATSAPIDBConnection dbContext, IConfiguration iConfiguration, IATSsyncUTS iATSsyncUTS)
        {
            _dbContext = dbContext;
            _iConfiguration = iConfiguration;
            _iATSsyncUTS = iATSsyncUTS;
        }

        #region Reverse Matchmaking

        [HttpPost("ReverseMatchmaking")]
        public async Task<IActionResult> ReverseMatchmaking()
        {
            string projectUrlApi = _iConfiguration["ProjectURL_API"];
            string endPoint = projectUrlApi + "ReverseMatchmaking";
            string result = string.Empty;
            string GspacePayload = string.Empty;

            try
            {
                // Read request body
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();
                GspacePayload = body;

                var reverseMatchmakingViewModel = JsonConvert.DeserializeObject<ReverseMatchmakingViewModel>(body);
                if (reverseMatchmakingViewModel == null)
                {
                    return BadRequest(new { status = 400, ErrorMessage = "Invalid request payload." });
                }

                // Add record in gen_UtsAts_Records
                var utsAtsApi_Records = new GenUtsAtsApiRecord
                {
                    FromApiUrl = "ATS Reserve Match Making URL",
                    ToApiUrl = endPoint,
                    PayloadToSend = body,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.UtcNow,
                    HrId = reverseMatchmakingViewModel.HRID
                };

                _dbContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
                await _dbContext.SaveChangesAsync();
                long APIRecordInsertedID = utsAtsApi_Records.Id;

                // Check if talent exists
                var varIfTalentExist = await _dbContext.GenTalents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AtsTalentId == reverseMatchmakingViewModel.ATS_TalentID
                        && x.TalentAccountStatusId != 2
                        && x.TalentAccountStatusId != 6);

                if (varIfTalentExist == null)
                {
                    result = "Talent Inactive or Deleted";
                    await UpdateApiRecord(APIRecordInsertedID, result);
                    return Unauthorized(new { status = 401, Message = result });
                }

                // Check if HR request exists
                var varSalesHiringRequest = await _dbContext.GenSalesHiringRequests
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reverseMatchmakingViewModel.HRID && x.IsActive == true);

                int LoggedInUserID = 0;
                var varUsrUser = await _dbContext.UsrUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EmployeeId == reverseMatchmakingViewModel.CreatedByID);

                if (varUsrUser == null)
                {
                    var varUsrUserDarshanID = await _dbContext.UsrUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.EmployeeId == "UP0012");

                    if (varUsrUserDarshanID != null)
                    {
                        LoggedInUserID = (int)varUsrUserDarshanID.Id;
                        reverseMatchmakingViewModel.CreatedByID = LoggedInUserID.ToString();
                    }
                }

                if (varSalesHiringRequest != null)
                {
                    if (!Convert.ToBoolean(varSalesHiringRequest.IsAccepted))
                    {
                        object[] param = new object[]
                        {
                           reverseMatchmakingViewModel.HRID,
                           1,
                           LoggedInUserID,
                           "HR Auto Accept"
                        };

                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                        await _dbContext.Database.ExecuteSqlRawAsync(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Accept_HR, paramString));
                    }

                    result = await InsertReverseMatchmakingAsync(reverseMatchmakingViewModel);
                }
                else
                {
                    result = "HR not Accepted/Active in UTS.";
                }

                await UpdateApiRecord(APIRecordInsertedID, result);

                if (result == "Success")
                {
                    return Ok(new { status = 200, Message = result });
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            if(result != "")
            {
                GspacePayload = GspacePayload + result;
                var uri = _iConfiguration["chatgoogleapis"].ToString();
                var varProjectURL_API = _iConfiguration["AdminUTSFeedbackURL"].ToString();
                               StringBuilder sb = new();
                sb.Append("ATS to UTS ReverseMatchmaking,\\n");
                sb.Append("*To URL:* " + endPoint + "\\n");
                sb.Append("*Payload:* " + GspacePayload + "\\n");           
                

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 500000;
                    webRequest.ContentType = "application/json";
                    webRequest.Credentials = CredentialCache.DefaultCredentials;

                    using (var requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string text = "{text:\"" + sb.ToString() + "\"}";

                        requestWriter.Write(text);
                        requestWriter.Flush();
                        requestWriter.Close();
                    }
                }

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                string ResponseJson = reader.ReadToEnd();
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { status = 500, ErrorMessage = result });
        }
        private async Task<string> InsertReverseMatchmakingAsync(ReverseMatchmakingViewModel reverseMatchmakingViewModel)
        {
            var salesHiringRequestDetails = await _dbContext.GenSalesHiringRequestDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.HiringRequestId == reverseMatchmakingViewModel.HRID);

            var salesHiringRequest = await _dbContext.GenSalesHiringRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reverseMatchmakingViewModel.HRID);

            if (salesHiringRequest == null || salesHiringRequestDetails == null)
                return "Hiring Request Not Found";

            if (salesHiringRequest.StatusId == 3)
                return "HR is Completed. Cannot perform Reverse Match-Making.";

            var atsUser = await _dbContext.UsrUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeId == "ATS_1");

            long atsUserId = atsUser?.Id ?? 0;

            var userRequest = await _dbContext.UsrUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeId == reverseMatchmakingViewModel.CreatedByID && (x.UserTypeId == 5 || x.UserTypeId == 10));

            if (userRequest == null)
                reverseMatchmakingViewModel.CreatedByID = "ATS_1";

            var usertypeRequest = await _dbContext.UsrUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmployeeId == reverseMatchmakingViewModel.CreatedByID && (x.UserTypeId == 5 || x.UserTypeId == 10));

            if (usertypeRequest == null)
                return "Action not performed by Ops User or Ops Manager";

            decimal dpAmount = 0, dpPercentage = 0;
            if (salesHiringRequest.IsHrtypeDp)
            {
                dpPercentage = salesHiringRequest.Dppercentage ?? 0;
                dpAmount = ((reverseMatchmakingViewModel.monthly_salary_in_usd ?? 0) * 12 * dpPercentage) / 100;
            }

            var talent = await _dbContext.GenTalents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AtsTalentId == reverseMatchmakingViewModel.ATS_TalentID);

            if (talent == null)
                return "Talent Not Found";

            if (talent.RoleId == null || talent.RoleId == 0)
            {
                talent.RoleId = salesHiringRequestDetails.RoleId ?? 0;
                _dbContext.GenTalents.Update(talent);
                await _dbContext.SaveChangesAsync();
            }

            talent.TalentStatusIdAfterClientSelection = 1;
            _dbContext.GenTalents.Update(talent);
            await _dbContext.SaveChangesAsync();

            await InsertHistoryForTalent(Action_Of_TalentHistory.Talent_Status_Selected, talent.Id, false, userRequest?.Id ?? atsUserId, reverseMatchmakingViewModel.HRID);

            var contactTalentPriority = await _dbContext.GenContactTalentPriorities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TalentId == talent.Id && x.HiringRequestId == reverseMatchmakingViewModel.HRID);

            if (contactTalentPriority == null)
            {
                var newContactTalentPriority = new GenContactTalentPriority
                {
                    ContactId = salesHiringRequest.ContactId,
                    HiringRequestDetailId = salesHiringRequestDetails.Id,
                    HiringRequestId = reverseMatchmakingViewModel.HRID,
                    RoleId = salesHiringRequestDetails.RoleId,
                    TalentId = talent.Id,
                    HrCost = reverseMatchmakingViewModel.monthly_salary_in_usd?.ToString(),
                    AllowedtoSelectforHr = true,
                    TalentStatusIdBasedOnHr = 1,
                    CreatedByDatetime = reverseMatchmakingViewModel.CreatedByDateTime,
                    CreatedById = (int?)(userRequest?.Id ?? atsUserId),
                    IsHrtypeDp = salesHiringRequest.IsHrtypeDp,
                    DpPercentage = dpPercentage,
                    TalentCurrencyCode = reverseMatchmakingViewModel.Talent_CurrencyCode,
                    AtsTalentLiveUrl = reverseMatchmakingViewModel.ATS_Talent_LiveURL,
                    AtsNonNdaurl = reverseMatchmakingViewModel.ATS_Non_NDAURL,
                    AtsTalentResume = reverseMatchmakingViewModel.resume
                };
                _dbContext.GenContactTalentPriorities.Add(newContactTalentPriority);
                await _dbContext.SaveChangesAsync();
            }

            var shortlistedTalent = await _dbContext.GenShortlistedTalents
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TalentId == talent.Id && x.HiringRequestId == reverseMatchmakingViewModel.HRID);

            if (shortlistedTalent == null)
            {
                var newShortlistedTalent = new GenShortlistedTalent
                {
                    HiringRequestId = reverseMatchmakingViewModel.HRID,
                    ContactId = salesHiringRequest.ContactId,
                    TalentId = talent.Id,
                    HiringRequestDetailId = salesHiringRequestDetails.Id,
                    CreatedById = (int?)(userRequest?.Id ?? atsUserId),
                    CreatedByDatetime = reverseMatchmakingViewModel.CreatedByDateTime
                };
                _dbContext.GenShortlistedTalents.Add(newShortlistedTalent);
                await _dbContext.SaveChangesAsync();
            }

            await InsertHistoryForTalent(Action_Of_TalentHistory.ATS_ReverseMatchMaking, talent.Id, false, userRequest?.Id ?? atsUserId, reverseMatchmakingViewModel.HRID);

            var RM_TalentData = await _dbContext.GenContactTalentPriorities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TalentId == talent.Id && x.HiringRequestId == reverseMatchmakingViewModel.HRID);

            if (RM_TalentData == null)
            {
                return "Error";
            }
            else
            {
                long varEmpID = 0, utsTalentId = 0;
                var talentObj = await _dbContext.GenTalents
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AtsTalentId == reverseMatchmakingViewModel.ATS_TalentID);

                var hiringRequestObj = await _dbContext.GenSalesHiringRequests
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reverseMatchmakingViewModel.HRID);

                var hiringRequestDetailObj = await _dbContext.GenSalesHiringRequestDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.HiringRequestId == hiringRequestObj.Id);

                if (!string.IsNullOrEmpty(reverseMatchmakingViewModel.CreatedByID))
                {
                    var varUsrUser = await _dbContext.UsrUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.EmployeeId == reverseMatchmakingViewModel.CreatedByID);

                    if (varUsrUser == null)
                    {
                        var varUsrUserDarshanID = await _dbContext.UsrUsers
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.EmployeeId == "UP0012");

                        if (varUsrUserDarshanID != null)
                        {
                            varEmpID = varUsrUserDarshanID.Id;
                        }
                    }
                    else
                    {
                        varEmpID = varUsrUser.Id;
                    }
                }
                else
                {
                    varEmpID = talentObj.Id;
                    utsTalentId = talentObj.Id;
                }

                var contactTalentPriorityObj = await _dbContext.GenContactTalentPriorities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.HiringRequestId == hiringRequestObj.Id && x.HiringRequestDetailId == hiringRequestDetailObj.Id && x.TalentId == talentObj.Id);

                if (hiringRequestDetailObj != null && contactTalentPriorityObj != null)
                {
                    var shortListedTalentObj = await _dbContext.GenShortlistedTalents
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.TalentId == talentObj.Id && x.HiringRequestId == hiringRequestObj.Id && x.HiringRequestDetailId == hiringRequestDetailObj.Id);

                    if (shortListedTalentObj != null)
                    {
                        object[] param = new object[]
                        {
                            shortListedTalentObj.Id,
                            talentObj.Id,
                            contactTalentPriorityObj.Id,
                            hiringRequestDetailObj.Id,
                            reverseMatchmakingViewModel.shift,
                            reverseMatchmakingViewModel.availibility,
                            reverseMatchmakingViewModel.notice
                        };

                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                        await _dbContext.Database.ExecuteSqlRawAsync(String.Format("{0} {1}", Constants.ProcConstant.sproc_UpdateTalentAcceptanceDetailsFromATS, paramString));

                        // Insert history for hiring request
                        var history = new HiringRequestInsertHistoryModel
                        {
                            HiringRequestId = shortListedTalentObj.HiringRequestId.Value,
                            TalentId = shortListedTalentObj.TalentId.Value,
                            LoggedInUserId = varEmpID,
                            HRAcceptedDateTime = DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss"),
                            IsManagedByTalent = utsTalentId > 0
                        };

                        await InsertHistoryForHiringRequest(history, Action_Of_History.HRPost_Acceptance, AppActionDoneBy.ATS);

                        var historyEntry2 = new HiringRequestInsertHistoryModel
                        {
                            HiringRequestId = hiringRequestObj.Id,
                            TalentId = talentObj.Id,
                            LoggedInUserId = varEmpID,
                            ContactTalentPriorityId = contactTalentPriorityObj.Id
                        };

                        await InsertHistoryForHiringRequest(historyEntry2, Action_Of_History.Profile_Waiting_For_Feedback, AppActionDoneBy.ATS);

                        return "Success";
                    }
                }
            }

            return "Success";
        }

        #endregion

        #region PayRate API

        [HttpPost("GetPayRate")]
        public async Task<IActionResult> GetPayRate()
        {
            string apiType = "GetPayRate";
            string endPoint = $"{_iConfiguration["ProjectURL_API"]}{apiType}";
            string result = string.Empty;

            try
            {
                // Read request body
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();

                var contractualDpViewModel = JsonConvert.DeserializeObject<ContractualDpViewModel>(body);
                if (contractualDpViewModel == null)
                {
                    return BadRequest(new { status = 400, ErrorMessage = "Invalid request payload." });
                }

                // Add record in gen_UtsAts_Records
                var utsAtsApi_Records = new GenUtsAtsApiRecord
                {
                    FromApiUrl = "ATS Pay Rate URL",
                    ToApiUrl = endPoint,
                    PayloadToSend = body,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.UtcNow,
                    HrId = contractualDpViewModel.HRID
                };

                _dbContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
                await _dbContext.SaveChangesAsync();
                long APIRecordInsertedID = utsAtsApi_Records.Id;

                // Call external service
                result = await InsertPayRateAsync(contractualDpViewModel);

                // Update record in gen_UtsAts_Records
                await UpdateApiRecord(APIRecordInsertedID, result);

                if (result == "Success")
                {
                    return Ok(new { status = 200, Message = result });
                }
            }
            catch (DbUpdateException dbEx)
            {
                result = "Database error: " + dbEx.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Unauthorized(new { status = 401, ErrorMessage = result });
        }
        private async Task<string> InsertPayRateAsync(ContractualDpViewModel contractualDpView)
        {
            try
            {
                if (contractualDpView?.TalentDetails == null || !contractualDpView.TalentDetails.Any())
                    return "Error: No Talent Details provided.";

                long CTP_ID = 0;
                long TalentID = 0, Uts_TalentID = 0, ATS_TalentID = 0;
                string Talent_Currency = "", HR_type = "", HRPricingType = "", YearsOfExperience = "", NoticePeriod = "", AgreedShift = "", PreferredAvailability = "";
                decimal? HrCost = 0, DporNRPercentage = 0, TalentExpected_fee = 0, ExchangeRate_UTS = 0, TalentCurrenct_fee = 0, DPAmount = 0, UplersFee = 0, UplersFeeAmount = 0;
                string CreatedByDateTime = "", HRTypeText = "", Talent_Expected_fee_yearly = "", Talent_current_fee_yearly = "";
                string AISummary = "", IsVideoResume = "", VideoVetting = "", TalentResume = "";


                GenContactTalentPriority? gen_ContactTalent = null;
                foreach (var item in contractualDpView.TalentDetails)
                {
                    CTP_ID = item.CTPID;
                    TalentID = item.ATS_TalentID;
                    Uts_TalentID = item.UTS_TalentID;
                    Talent_Currency = item.TalentCurrency;
                    HR_type = item.HR_Type;
                    DporNRPercentage = item.DPorNR_Percent;
                    TalentExpected_fee = item.Talent_Expected_fee;
                    DPAmount = item.DPAmount;
                    TalentCurrenct_fee = item.Talent_current_fee;
                    ExchangeRate_UTS = item.ExchangeRateUTS;
                    HRPricingType = item.HRPricingType;
                    UplersFee = item.UplersFee;
                    UplersFeeAmount = item.UplersFeeAmount;
                    YearsOfExperience = item.YearsOfExperience;
                    NoticePeriod = item.NoticePeriod;
                    AgreedShift = item.AgreedShift;
                    PreferredAvailability = item.PreferredAvailability;
                    ATS_TalentID = item.ATS_TalentID;
                    HRTypeText = item.HRTypeText;
                    Talent_Expected_fee_yearly = item.Talent_Expected_fee_yearly;
                    Talent_current_fee_yearly = item.Talent_current_fee_yearly;
                    HrCost = item.HR_Cost;
                    AISummary = item.AISummary;
                    IsVideoResume = item.IsVideoResume;
                    VideoVetting = item.VideoVetting;
                    TalentResume = item.TalentResume;
                }

                // Find Talent Data
                var talentData = await _dbContext.GenTalents.FirstOrDefaultAsync(x => x.AtsTalentId == TalentID);

                // Determine the appropriate gen_ContactTalentPriority record
                if (CTP_ID > 0)
                {
                    gen_ContactTalent = await _dbContext.GenContactTalentPriorities.FindAsync(CTP_ID);
                }
                else if (contractualDpView.HRID > 0 && talentData != null)
                {
                    gen_ContactTalent = await _dbContext.GenContactTalentPriorities
                        .FirstOrDefaultAsync(x => x.HiringRequestId == contractualDpView.HRID && x.TalentId == talentData.Id);
                }
                else if (contractualDpView.HRID > 0 && Uts_TalentID > 0)
                {
                    gen_ContactTalent = await _dbContext.GenContactTalentPriorities
                        .FirstOrDefaultAsync(x => x.HiringRequestId == contractualDpView.HRID && x.TalentId == Uts_TalentID);
                }

                if (gen_ContactTalent == null)
                    return "Error: Talent Record Not Found";

                // Get user type
                var userType = await _dbContext.UsrUsers.FirstOrDefaultAsync(x => x.EmployeeId == contractualDpView.CreatedByID)
                              ?? await _dbContext.UsrUsers.FirstOrDefaultAsync(x => x.EmployeeId == "ATS_1");

                if (userType == null)
                    return "Error: User Not Found";

                // Call stored procedure for updating ContactTalentPriority

                object[] param = new object[]
                   {
                       gen_ContactTalent.Id, gen_ContactTalent.TalentId,
                        TalentCurrenct_fee ?? 0, TalentExpected_fee ?? 0, HrCost,
                        Talent_Currency, ExchangeRate_UTS, UplersFee, UplersFeeAmount,
                        HR_type, HRPricingType, HRTypeText,
                        Talent_Expected_fee_yearly, Talent_current_fee_yearly,
                        YearsOfExperience, NoticePeriod, AgreedShift, PreferredAvailability,
                        ATS_TalentID, Convert.ToInt32(userType.Id), contractualDpView.CreatedByDateTime,
                        AISummary, IsVideoResume, VideoVetting, TalentResume
                   };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                await _dbContext.Database.ExecuteSqlRawAsync(String.Format("{0} {1}", Constants.ProcConstant.sproc_gen_ContactTalentPriorityupdate, paramString));

                // Insert history for hiring request
                var history = new HiringRequestInsertHistoryModel
                {
                    HiringRequestId = contractualDpView.HRID,
                    TalentId = gen_ContactTalent.TalentId.Value,
                    LoggedInUserId = userType.Id,
                    ContactTalentPriorityId = gen_ContactTalent.Id
                };

                await InsertHistoryForHiringRequest(history, Action_Of_History.Talent_Fees_Update, AppActionDoneBy.ATS);

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        #endregion

        #region GetAllTalentDetails API

        [HttpPost("GetAllTalentDetails")]
        public async Task<IActionResult> GetAllTalentDetails()
        {
            string apiType = "GetAllTalentDetails";
            string endPoint = $"{_iConfiguration["ProjectURL_API"]}{apiType}";
            string result = string.Empty;

            try
            {
                // Read request body
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();

                PMSTalentProperties? pMSTalent = JsonConvert.DeserializeObject<PMSTalentProperties>(body);
                if (pMSTalent == null)
                {
                    return BadRequest(new { status = 400, ErrorMessage = "Invalid request payload." });
                }

                // Add record in gen_UtsAts_Records
                var utsAtsApi_Records = new GenUtsAtsApiRecord
                {
                    FromApiUrl = "ATS OnBoard Active URL",
                    ToApiUrl = endPoint,
                    PayloadToSend = body,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.UtcNow,
                    HrId = 0
                };

                _dbContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
                await _dbContext.SaveChangesAsync();
                long APIRecordInsertedID = utsAtsApi_Records.Id;

                long TalentID = 0;

                // Call external service
                PMSTalentOutput talentResult = await GetNodeValues(pMSTalent);

                // Update record in gen_UtsAts_Records
                await UpdateApiRecord(APIRecordInsertedID, talentResult?.Message);

                if (talentResult.Message == "Success")
                {
                    return Ok(new { status = 200, Message = TalentID });
                }
            }
            catch (DbUpdateException dbEx)
            {
                result = "Database error: " + dbEx.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Unauthorized(new { status = 401, ErrorMessage = result });
        }

        private async Task<PMSTalentOutput> GetNodeValues(PMSTalentProperties talentProperties)
        {
            long TalentID = 0;
            if (talentProperties != null)
            {
                long PMS_TalentInsertedID = 0;

                var TalentProfileData = talentProperties.data.profileData;
                var TalentLegalData = talentProperties.data.legals;
                var TalentAssesmentData = talentProperties.data.assesments;

                #region Insert PMS Json Data in gen_Talent Table.

                GenTalent? _PMSTalent = await _dbContext.GenTalents.AsNoTracking().Where(x => x.AtsTalentId == TalentProfileData.basicDetails.ATS_TalentID).FirstOrDefaultAsync();
                if (_PMSTalent == null)
                {
                    //Insert
                    _PMSTalent = new GenTalent();

                    if (!string.IsNullOrEmpty(TalentProfileData.basicDetails.short_created_at))
                        _PMSTalent.CreatedByDatetime = Convert.ToDateTime(TalentProfileData.basicDetails.short_created_at);
                    else
                        _PMSTalent.CreatedByDatetime = DateTime.Now;

                    string[] nameParts = TalentProfileData.basicDetails.name.Split(' ');
                    string firstName = nameParts[0];
                    string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                    _PMSTalent.FirstName = firstName;
                    _PMSTalent.LastName = lastName;
                    _PMSTalent.Name = TalentProfileData.basicDetails.name;
                    _PMSTalent.Password = TalentProfileData.basicDetails.talent_password;
                    _PMSTalent.Designation = string.IsNullOrEmpty(TalentProfileData.basicDetails.designation) ? null : TalentProfileData.basicDetails.designation;
                    _PMSTalent.ContactNumber = TalentProfileData.basicDetails.contact_number;
                    _PMSTalent.EmailId = TalentProfileData.basicDetails.email;
                    _PMSTalent.Username = TalentProfileData.basicDetails.email;
                    _PMSTalent.Description = TalentProfileData.basicDetails.objective;
                    _PMSTalent.TypeofDeveloper = string.IsNullOrEmpty(TalentProfileData.basicDetails.job_title) ? null : TalentProfileData.basicDetails.job_title;
                    _PMSTalent.TotalExpYears = Convert.ToDecimal(TalentProfileData.basicDetails.total_experience);
                    if (!string.IsNullOrEmpty(TalentProfileData.basicDetails.worked_with_international_client))
                    {
                        _PMSTalent.DirectlyWorkedInternationalClients = TalentProfileData.basicDetails.worked_with_international_client.ToLower().Trim() == "yes" ? true : false;
                    }
                    //var TalentAssociatedWithUplersDetails = await _dbContext.PrgTalentAssociatedWithUplers.Where(x => x.AssociatedWithUplers.ToLower().Trim() == TalentProfileData.basicDetails.availability.ToLower().Trim()).FirstOrDefaultAsync();
                    //if (TalentAssociatedWithUplersDetails != null)
                    //{
                    //    _PMSTalent.AssociatedwithUplersId = TalentAssociatedWithUplersDetails.Id;
                    //}

                    var TalentJoiningDetails = await _dbContext.PrgTalentJoinnings.Where(x => x.Joinning == TalentProfileData.basicDetails.joining_period).FirstOrDefaultAsync();
                    if (TalentJoiningDetails != null)
                        _PMSTalent.JoiningId = TalentJoiningDetails.Id;

                    var TalentEmploymentStatusDetails = await _dbContext.PrgEmploymentStatuses.Where(x => x.EmploymentStatus == TalentProfileData.basicDetails.current_employment_status).FirstOrDefaultAsync();
                    if (TalentEmploymentStatusDetails != null)
                        _PMSTalent.CurrentEmploymentStatusId = TalentEmploymentStatusDetails.Id;

                    _PMSTalent.ExpectedSalary = Convert.ToDecimal(TalentProfileData.basicDetails.expected_ctc);
                    _PMSTalent.CodeRepositoryUrl = string.IsNullOrEmpty(TalentProfileData.basicDetails.repository_url) ? null : TalentProfileData.basicDetails.repository_url;
                    _PMSTalent.LinkedInProfile = TalentProfileData.basicDetails.linkedin_id;

                    //If payload Role matches our defined Roles, then consider that Role, else assign 1st Role ID in UTS DB.
                    //Changes by Siddharth Jain Dtd: 08/Dec/2022
                    var TalentRoleDetails = await _dbContext.PrgTalentRoles.Where(x => x.TalentRole == TalentProfileData.basicDetails.role).FirstOrDefaultAsync();
                    if (TalentRoleDetails != null)
                        _PMSTalent.RoleId = TalentRoleDetails.Id;

                    _PMSTalent.Cvfilename = string.IsNullOrEmpty(TalentProfileData.basicDetails.resume) ? null : TalentProfileData.basicDetails.resume;
                    //enc_id ????
                    _PMSTalent.PhotoImage = string.IsNullOrEmpty(TalentProfileData.basicDetails.profile_pic) ? null : TalentProfileData.basicDetails.profile_pic;
                    _PMSTalent.ProfileUrl = TalentProfileData.basicDetails.profile_pic_url;
                    _PMSTalent.Status = TalentProfileData.basicDetails.status;

                    //Achivement Update
                    if (TalentProfileData.achievements.Count > 0)
                    {
                        var AchievemnentList = TalentProfileData.achievements.Where(x => x.title != "").Select(x => x.title).ToList();
                        var Achivements = string.Join("`", AchievemnentList);
                        _PMSTalent.Achievements = Achivements;
                    }


                    //Update Talent Status to 1 for passed Talent When Status = 200.
                    // pending talent active status Talent Active if type is Adhoc / Bench 
                    if (talentProperties.status == 200)
                    {
                        _PMSTalent.PmsStatus = true;
                        _PMSTalent.TalentAccountStatusId = 4; // Active 
                        _PMSTalent.IsResetPassword = true;
                        _PMSTalent.AtsTalentId = TalentProfileData.basicDetails.ATS_TalentID;
                        _PMSTalent.AtsTalentLiveUrl = TalentProfileData.basicDetails.ATS_Talent_LiveURL == null ? "" : TalentProfileData.basicDetails.ATS_Talent_LiveURL;
                        _PMSTalent.AtsNonNdaurl = TalentProfileData.basicDetails.ATS_Non_NDAURL == null ? "" : TalentProfileData.basicDetails.ATS_Non_NDAURL;
                        _PMSTalent.AtsTalentInramount = Convert.ToDecimal(TalentProfileData.basicDetails.talent_monthly_cost_in_inr);
                        _PMSTalent.FinalCost = Convert.ToDecimal(TalentProfileData.basicDetails.talent_monthly_cost_in_usd);
                        if (TalentProfileData.basicDetails.is_odr_active == true)
                            _PMSTalent.TalentTypeId = 3;  //ODR

                        _PMSTalent.IsOdrActive = TalentProfileData.basicDetails.is_odr_active;
                    }

                    if (_PMSTalent.PmsStatus == true)
                    {
                        _PMSTalent.IsAccountCreated = true;
                        _PMSTalent.IsApproved = true;
                        _PMSTalent.IsLegalSigned = true;
                    }
                    _PMSTalent.IsTalentNotificationSend = false;
                    if (!string.IsNullOrEmpty(TalentProfileData.basicDetails.talent_source))
                    {
                        if (TalentProfileData.basicDetails.talent_source.ToLower().Trim() == "skill based" && TalentProfileData.basicDetails.talent_source.ToLower().Trim() == "pool based")
                            _PMSTalent.TalentTypeId = 2; // Pool
                        else if (TalentProfileData.basicDetails.talent_source.ToLower().Trim() == "odp")
                            _PMSTalent.TalentTypeId = 5; // ODP
                    }

                    _dbContext.GenTalents.Add(_PMSTalent);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    if (_PMSTalent.TalentAccountStatusId != 2 || _PMSTalent.TalentAccountStatusId != 6)
                    {
                        #region Update
                        //Update
                        if (!string.IsNullOrEmpty(TalentProfileData.basicDetails.short_created_at))
                            _PMSTalent.CreatedByDatetime = Convert.ToDateTime(TalentProfileData.basicDetails.short_created_at);
                        else
                            _PMSTalent.CreatedByDatetime = DateTime.Now;

                        string[] nameParts = TalentProfileData.basicDetails.name.Split(' ');
                        string firstName = nameParts[0];
                        string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                        _PMSTalent.FirstName = firstName;
                        _PMSTalent.LastName = lastName;
                        _PMSTalent.Name = TalentProfileData.basicDetails.name;
                        _PMSTalent.Password = TalentProfileData.basicDetails.talent_password;
                        _PMSTalent.Designation = string.IsNullOrEmpty(TalentProfileData.basicDetails.designation) ? null : TalentProfileData.basicDetails.designation; ;
                        _PMSTalent.ContactNumber = TalentProfileData.basicDetails.contact_number;
                        _PMSTalent.EmailId = TalentProfileData.basicDetails.email;
                        _PMSTalent.Username = TalentProfileData.basicDetails.email;
                        _PMSTalent.Description = TalentProfileData.basicDetails.objective;
                        _PMSTalent.TypeofDeveloper = string.IsNullOrEmpty(TalentProfileData.basicDetails.job_title) ? null : TalentProfileData.basicDetails.job_title; ;
                        _PMSTalent.TotalExpYears = Convert.ToDecimal(TalentProfileData.basicDetails.total_experience);
                        if (!string.IsNullOrEmpty(TalentProfileData.basicDetails.worked_with_international_client))
                        {
                            _PMSTalent.DirectlyWorkedInternationalClients = TalentProfileData.basicDetails.worked_with_international_client.ToLower().Trim() == "yes" ? true : false;
                        }

                        var TalentAssociatedWithUplersDetails = await _dbContext.PrgTalentAssociatedWithUplers.Where(x => x.AssociatedWithUplers.ToLower().Trim() == TalentProfileData.basicDetails.availability.ToLower().Trim()).FirstOrDefaultAsync();
                        if (TalentAssociatedWithUplersDetails != null)
                        {
                            _PMSTalent.AssociatedwithUplersId = TalentAssociatedWithUplersDetails.Id;
                        }

                        var TalentJoiningDetails = await _dbContext.PrgTalentJoinnings.Where(x => x.Joinning.ToLower().Trim() == TalentProfileData.basicDetails.joining_period.ToLower().Trim()).FirstOrDefaultAsync();
                        if (TalentJoiningDetails != null)
                            _PMSTalent.JoiningId = TalentJoiningDetails.Id;

                        var TalentEmploymentStatusDetails = await _dbContext.PrgEmploymentStatuses.Where(x => x.EmploymentStatus.ToLower().Trim() == TalentProfileData.basicDetails.current_employment_status.ToLower().Trim()).FirstOrDefaultAsync();
                        if (TalentEmploymentStatusDetails != null)
                            _PMSTalent.CurrentEmploymentStatusId = TalentEmploymentStatusDetails.Id;

                        _PMSTalent.ExpectedSalary = Convert.ToDecimal(TalentProfileData.basicDetails.expected_ctc);
                        _PMSTalent.CodeRepositoryUrl = string.IsNullOrEmpty(TalentProfileData.basicDetails.repository_url) ? null : TalentProfileData.basicDetails.repository_url;
                        _PMSTalent.LinkedInProfile = TalentProfileData.basicDetails.linkedin_id;

                        //If payload Role matches our defined Roles, then consider that Role, else assign 1st Role ID in UTS DB.
                        //Changes by Siddharth Jain Dtd: 08/Dec/2022
                        var TalentRoleDetails = await _dbContext.PrgTalentRoles.Where(x => x.TalentRole.ToLower().Trim() == TalentProfileData.basicDetails.role.ToLower().Trim()).FirstOrDefaultAsync();
                        if (TalentRoleDetails != null)
                            _PMSTalent.RoleId = TalentRoleDetails.Id;

                        _PMSTalent.Cvfilename = string.IsNullOrEmpty(TalentProfileData.basicDetails.resume) ? null : TalentProfileData.basicDetails.resume;
                        //enc_id ????
                        _PMSTalent.PhotoImage = string.IsNullOrEmpty(TalentProfileData.basicDetails.profile_pic) ? null : TalentProfileData.basicDetails.profile_pic;
                        _PMSTalent.ProfileUrl = TalentProfileData.basicDetails.profile_pic_url;
                        _PMSTalent.Status = TalentProfileData.basicDetails.status;

                        //Achivement Update
                        if (TalentProfileData.achievements.Count > 0)
                        {
                            var AchievemnentList = TalentProfileData.achievements.Where(x => x.title != "").Select(x => x.title).ToList();
                            var Achivements = string.Join("`", AchievemnentList);
                            _PMSTalent.Achievements = Achivements;
                        }


                        //Update Talent Status to 1 for passed Talent When Status = 200.
                        // pending talent active status Talent Active if type is Adhoc / Bench 
                        if (talentProperties.status == 200)
                        {
                            _PMSTalent.PmsStatus = true;
                            _PMSTalent.TalentAccountStatusId = 4; // Active 
                            _PMSTalent.IsResetPassword = true;
                            _PMSTalent.AtsTalentId = TalentProfileData.basicDetails.ATS_TalentID;
                            _PMSTalent.AtsTalentLiveUrl = TalentProfileData.basicDetails.ATS_Talent_LiveURL == null ? "" : TalentProfileData.basicDetails.ATS_Talent_LiveURL;
                            _PMSTalent.AtsNonNdaurl = TalentProfileData.basicDetails.ATS_Non_NDAURL == null ? "" : TalentProfileData.basicDetails.ATS_Non_NDAURL;
                            _PMSTalent.AtsTalentInramount = Convert.ToDecimal(TalentProfileData.basicDetails.talent_monthly_cost_in_inr);
                            _PMSTalent.FinalCost = Convert.ToDecimal(TalentProfileData.basicDetails.talent_monthly_cost_in_usd);
                            if (TalentProfileData.basicDetails.is_odr_active == true)
                                _PMSTalent.TalentTypeId = 3;  //ODR

                            _PMSTalent.IsOdrActive = TalentProfileData.basicDetails.is_odr_active;
                        }

                        //if (!string.IsNullOrEmpty(ProfilePicURL))
                        //{
                        //    IFileDownloadAndSave PMS_fileDownloadAndSave = new PMSDownloadAndSaveFile();
                        //    PMS_fileDownloadAndSave.PMS_FileDownloadAndSave(ProfilePicURL, _PMSTalent.PhotoImage, "TalentProfilePic");
                        //}
                        if (_PMSTalent.PmsStatus == true)
                        {
                            _PMSTalent.IsAccountCreated = true;
                            _PMSTalent.IsApproved = true;
                            _PMSTalent.IsLegalSigned = true;
                        }
                        _PMSTalent.IsTalentNotificationSend = false;
                        if (!string.IsNullOrEmpty(TalentProfileData.basicDetails.talent_source))
                        {
                            if (TalentProfileData.basicDetails.talent_source.ToLower().Trim() == "skill based" && TalentProfileData.basicDetails.talent_source.ToLower().Trim() == "pool based")
                                _PMSTalent.TalentTypeId = 2; // Pool
                            else if (TalentProfileData.basicDetails.talent_source.ToLower().Trim() == "odp")
                                _PMSTalent.TalentTypeId = 5; // ODP
                        }

                        _dbContext.Entry(_PMSTalent).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                        #endregion
                    }
                    else
                    {
                        return new PMSTalentOutput { TalentId = TalentID, Message = "Talent InActive or Deleted" };
                    }
                }
                #endregion

                #region HR wise Talent Package

                var varTalentID = _PMSTalent.Id;
                if (varTalentID > 0 && talentProperties.data.HRID.HasValue != false)
                {
                    long HRID = talentProperties.data.HRID.HasValue ? talentProperties.data.HRID.Value : 0;

                    foreach (var AssesmentData in TalentAssesmentData)
                    {
                        var varIfExists = await _dbContext.GenSalesHiringRequestTalentPacketDetails.AsNoTracking().Where(x => x.HiringRequestId == HRID && x.TalentId == varTalentID).FirstOrDefaultAsync();
                        long ContactID = 0;
                        if (varIfExists == null)
                        {
                            GenSalesHiringRequest? salesHiringRequest = await _dbContext.GenSalesHiringRequests.Where(x => x.Id == HRID).FirstOrDefaultAsync();
                            if (salesHiringRequest != null)
                            {
                                ContactID = salesHiringRequest.ContactId ?? 0;
                            }

                            #region Insert gen_SalesHiringRequest_TalentPacketDetails
                            GenSalesHiringRequestTalentPacketDetail obj = new GenSalesHiringRequestTalentPacketDetail();
                            obj.ContactId = ContactID;
                            obj.HiringRequestId = HRID;
                            obj.TalentId = varTalentID;
                            obj.AssessmentTool = AssesmentData.assessment_tool;
                            obj.ClientAssesmentReport = AssesmentData.client_assesment_report;
                            obj.Score = AssesmentData.score.ToString();
                            //obj.assessment_date = AssesmentData.assessment_date != null ? DateTime.Parse(AssesmentData.assessment_date) : DateTime.MinValue;
                            obj.AssessmentDate = string.IsNullOrEmpty(AssesmentData.assessment_date) ? (DateTime?)null : Convert.ToDateTime(AssesmentData.assessment_date);
                            obj.TalentResumeLink = TalentProfileData.basicDetails.resume_url;
                            _dbContext.GenSalesHiringRequestTalentPacketDetails.Add(obj);
                            await _dbContext.SaveChangesAsync();
                            #endregion
                        }
                        else
                        {
                            #region update gen_SalesHiringRequest_TalentPacketDetails
                            GenSalesHiringRequestTalentPacketDetail? obj = new GenSalesHiringRequestTalentPacketDetail();
                            obj = _dbContext.GenSalesHiringRequestTalentPacketDetails.AsNoTracking().Where(x => x.Id == varIfExists.Id).FirstOrDefault();
                            obj.AssessmentTool = AssesmentData.assessment_tool;
                            obj.ClientAssesmentReport = AssesmentData.client_assesment_report;
                            obj.Score = AssesmentData.score.ToString();
                            obj.AssessmentDate = string.IsNullOrEmpty(AssesmentData.assessment_date) ? (DateTime?)null : Convert.ToDateTime(AssesmentData.assessment_date);
                            //obj.assessment_date = AssesmentData.assessment_date != null ? DateTime.Parse(AssesmentData.assessment_date) : DateTime.MinValue;
                            obj.TalentResumeLink = TalentProfileData.basicDetails.resume_url;
                            _dbContext.Entry(obj).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            #endregion
                        }
                    }
                }

                #endregion

                if (_PMSTalent == null)
                {
                    return new PMSTalentOutput { TalentId = TalentID, Message = "Error - No Talent Found" };
                }

                PMS_TalentInsertedID = _PMSTalent.Id;

                if (PMS_TalentInsertedID == 0)
                {
                    return new PMSTalentOutput { TalentId = TalentID, Message = "Error - Talent Found with ID = 0" };
                }

                TalentID = PMS_TalentInsertedID;

                //Insert History for Talent : Added By Rinku
                await InsertHistoryForTalent(Action_Of_TalentHistory.ATS_OnBoardingActive, PMS_TalentInsertedID, false, Convert.ToInt64(_PMSTalent.CreatedById), 0);

                #region Insert POC Data in gen_TalentPointofContact Table
                GenTalentPointofContact _TalentPointofContact = new GenTalentPointofContact();
                _TalentPointofContact.TalentId = PMS_TalentInsertedID;

                var _ExistingTalentPointofContact = await _dbContext.GenTalentPointofContacts.Where(x => x.TalentId == PMS_TalentInsertedID).ToListAsync();

                if (_ExistingTalentPointofContact.Any())
                {
                    _dbContext.GenTalentPointofContacts.RemoveRange(_ExistingTalentPointofContact);
                    await _dbContext.SaveChangesAsync();
                }

                #endregion

                #region Insert Preferable Working of PMS Json in gen_TalentPreferableWorking_Details Table
                var PMS_PrefWorkingDetails = TalentProfileData.shifts.ToList();

                var PMS_ExistingPrefWorking = await _dbContext.GenTalentPreferableWorkingDetails.Where(x => x.TalentId == PMS_TalentInsertedID).ToListAsync();
                if (PMS_ExistingPrefWorking.Any())
                {
                    _dbContext.GenTalentPreferableWorkingDetails.RemoveRange(PMS_ExistingPrefWorking);
                    await _dbContext.SaveChangesAsync();
                }

                foreach (var pref in PMS_PrefWorkingDetails)
                {
                    PrgTalentPreferableWorking? _TalentPreferableWorking = new PrgTalentPreferableWorking();
                    var _PrefWorkingPMSData = await _dbContext.PrgTalentPreferableWorkings.Where(x => x.TalentWorkingHrs.ToLower().Trim() == pref.shift.ToLower().Trim()).FirstOrDefaultAsync();

                    if (_PrefWorkingPMSData == null)
                    {
                        _PrefWorkingPMSData = new PrgTalentPreferableWorking();
                        _PrefWorkingPMSData.TalentWorkingHrs = pref.shift;
                        _PrefWorkingPMSData.WorkingZone = pref.shift.Split(' ')[0];
                        _dbContext.PrgTalentPreferableWorkings.Add(_PrefWorkingPMSData);
                        await _dbContext.SaveChangesAsync();

                        GenTalentPreferableWorkingDetail _TalentPreferableWorking_Details = new GenTalentPreferableWorkingDetail();
                        _TalentPreferableWorking_Details.TalentId = PMS_TalentInsertedID;
                        _TalentPreferableWorking_Details.IsAddedAfterPostAcceptance = false;
                        _TalentPreferableWorking_Details.PreferableWorkingId = _PrefWorkingPMSData.Id;
                        _dbContext.GenTalentPreferableWorkingDetails.Add(_TalentPreferableWorking_Details);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        GenTalentPreferableWorkingDetail? _TalentPreferableWorking_Details = await _dbContext.GenTalentPreferableWorkingDetails.Where(x => x.TalentId == PMS_TalentInsertedID && x.PreferableWorkingId == _PrefWorkingPMSData.Id).FirstOrDefaultAsync();
                        if (_TalentPreferableWorking_Details == null)
                        {
                            _TalentPreferableWorking_Details = new GenTalentPreferableWorkingDetail();
                            _TalentPreferableWorking_Details.TalentId = Convert.ToInt64(PMS_TalentInsertedID);
                            _TalentPreferableWorking_Details.IsAddedAfterPostAcceptance = false;
                            _TalentPreferableWorking_Details.PreferableWorkingId = _PrefWorkingPMSData.Id;
                            _dbContext.GenTalentPreferableWorkingDetails.Add(_TalentPreferableWorking_Details);
                            await _dbContext.SaveChangesAsync();
                        }

                    }
                }
                #endregion

                #region Insert Legals of PMS Json in gen_TalentLegalInfo Table
                var PMS_LegalDetails = TalentLegalData.ToList();

                var PMS_ExistingLegal = await _dbContext.GenTalentLegalInfos.Where(x => x.TalentId == PMS_TalentInsertedID).ToListAsync();
                if (PMS_ExistingLegal.Any())
                {
                    _dbContext.GenTalentLegalInfos.RemoveRange(PMS_ExistingLegal);
                    await _dbContext.SaveChangesAsync();
                }

                bool isAllLegalSigned = true;
                foreach (var pLegal in PMS_LegalDetails)
                {

                    GenTalentLegalInfo _TalentClientTele = new GenTalentLegalInfo();
                    _TalentClientTele.TalentId = PMS_TalentInsertedID;
                    _TalentClientTele.DocumentType = pLegal.document_type;
                    _TalentClientTele.DocumentName = pLegal.document_name;
                    _TalentClientTele.LegalDescription = pLegal.description;
                    _TalentClientTele.DocumentUrl = pLegal.file;
                    _TalentClientTele.AgreementStatus = pLegal.document_status;
                    if (pLegal.document_status.ToLower().Trim() == "signed")
                        _TalentClientTele.SignedDate = Convert.ToDateTime(pLegal.signed_date);
                    else
                        isAllLegalSigned = false;
                    if (!string.IsNullOrEmpty(pLegal.valid_start_date))
                        _TalentClientTele.ValidityStartDate = Convert.ToDateTime(pLegal.valid_start_date);
                    if (!string.IsNullOrEmpty(pLegal.valid_end_date))
                        _TalentClientTele.ValidityEndDate = Convert.ToDateTime(pLegal.valid_end_date);

                    _dbContext.GenTalentLegalInfos.Add(_TalentClientTele);
                    await _dbContext.SaveChangesAsync();
                }

                if (isAllLegalSigned)
                {
                    GenTalent? PMS_Talent = await _dbContext.GenTalents.Where(x => x.Id == PMS_TalentInsertedID).FirstOrDefaultAsync();
                    if (PMS_Talent != null)
                    {
                        PMS_Talent.IsLegalSigned = true;
                        _dbContext.Entry(PMS_Talent).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                }
                #endregion

                #region Insert into Primary Skills
                var PMS_PrimarySkills = TalentProfileData.primaryskills.ToList();
                var TalentRoleID1 = await _dbContext.GenTalents.Where(x => x.Id == PMS_TalentInsertedID).FirstOrDefaultAsync();
                var TalentRoleName1 = TalentProfileData.basicDetails.role;
                foreach (var pskills in PMS_PrimarySkills)
                {
                    PrgSkill? _Skills = await _dbContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == pskills.name.ToLower().Trim() && x.IsActive == true).FirstOrDefaultAsync();
                    if (_Skills == null)
                    {
                        PrgTempSkill _TempSkills = new PrgTempSkill();
                        _TempSkills.TempSkill = pskills.name;
                        _TempSkills.Source = "PMS_PrimarySkill";
                        _TempSkills.RoleId = TalentRoleID1?.RoleId;
                        _TempSkills.Role = TalentRoleName1;
                        _TempSkills.CreatedById = Convert.ToInt32(PMS_TalentInsertedID);
                        _TempSkills.CreatedByDatetime = DateTime.Now;
                        _TempSkills.Guid = "PMS_" + System.Guid.NewGuid().ToString();
                        _dbContext.PrgTempSkills.Add(_TempSkills);
                        await _dbContext.SaveChangesAsync();

                        var Curr_TempSkillID = await _dbContext.PrgTempSkills.Where(x => x.Id == _TempSkills.Id).FirstOrDefaultAsync();
                        if (Curr_TempSkillID != null)
                        {
                            Curr_TempSkillID.TempSkillId = "O_" + Curr_TempSkillID.Id;
                            Curr_TempSkillID.LastModifiedDatetime = DateTime.Now;
                            _dbContext.Entry(Curr_TempSkillID).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                        }


                        GenTalentPrimarySkillDetail? _TalentPrimarySkill_Details = await _dbContext.GenTalentPrimarySkillDetails.Where(x => x.TalentId == PMS_TalentInsertedID && x.TempSkillId == "O_" + Curr_TempSkillID.Id).FirstOrDefaultAsync();
                        if (_TalentPrimarySkill_Details == null)
                        {
                            _TalentPrimarySkill_Details = new GenTalentPrimarySkillDetail();
                            _TalentPrimarySkill_Details.TalentId = PMS_TalentInsertedID;
                            _TalentPrimarySkill_Details.TempSkillId = "O_" + Curr_TempSkillID.Id;
                            _TalentPrimarySkill_Details.YearsOfExp = string.IsNullOrEmpty(pskills.years_of_experience) ? 0 : Convert.ToDecimal(pskills.years_of_experience);

                            _dbContext.GenTalentPrimarySkillDetails.Add(_TalentPrimarySkill_Details);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        GenTalentPrimarySkillDetail? _TalentPrimarySkill_Details = await _dbContext.GenTalentPrimarySkillDetails.Where(x => x.TalentId == PMS_TalentInsertedID && x.PrimarySkillId == _Skills.Id).FirstOrDefaultAsync();
                        if (_TalentPrimarySkill_Details == null)
                        {
                            _TalentPrimarySkill_Details = new GenTalentPrimarySkillDetail();
                            _TalentPrimarySkill_Details.TalentId = PMS_TalentInsertedID;
                            _TalentPrimarySkill_Details.PrimarySkillId = Convert.ToInt32(_Skills.Id);
                            _TalentPrimarySkill_Details.YearsOfExp = string.IsNullOrEmpty(pskills.years_of_experience) ? 0 : Convert.ToDecimal(pskills.years_of_experience);

                            _dbContext.GenTalentPrimarySkillDetails.Add(_TalentPrimarySkill_Details);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }

                #endregion

                #region Insert into Secondary Skills
                if (TalentProfileData.secondaryskills != null)
                {
                    var PMS_SecondarySkills = TalentProfileData.secondaryskills.ToList();
                    var TalentRoleID2 = await _dbContext.GenTalents.Where(x => x.Id == PMS_TalentInsertedID).FirstOrDefaultAsync();
                    var TalentRoleName2 = TalentProfileData.basicDetails.role;
                    foreach (var pskills in PMS_SecondarySkills)
                    {
                        PrgSkill? _Skills = _dbContext.PrgSkills.Where(x => x.Skill.ToLower().Trim() == pskills.name.ToLower().Trim() && x.IsActive == true).FirstOrDefault();
                        if (_Skills == null)
                        {
                            PrgTempSkill _TempSkills = new PrgTempSkill();
                            _TempSkills.TempSkill = pskills.name;
                            _TempSkills.Source = "PMS_PrimarySkill";
                            _TempSkills.RoleId = TalentRoleID2?.RoleId;
                            _TempSkills.Role = TalentRoleName1;
                            _TempSkills.CreatedById = Convert.ToInt32(PMS_TalentInsertedID);
                            _TempSkills.CreatedByDatetime = DateTime.Now;
                            _TempSkills.Guid = "PMS_" + System.Guid.NewGuid().ToString();
                            _dbContext.PrgTempSkills.Add(_TempSkills);
                            await _dbContext.SaveChangesAsync();

                            var Curr_TempSkillID = await _dbContext.PrgTempSkills.Where(x => x.Id == _TempSkills.Id).FirstOrDefaultAsync();
                            if (Curr_TempSkillID != null)
                            {
                                Curr_TempSkillID.TempSkillId = "O_" + Curr_TempSkillID.Id;
                                Curr_TempSkillID.LastModifiedDatetime = DateTime.Now;
                                _dbContext.Entry(Curr_TempSkillID).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }


                            GenTalentSecondarySkillDetail? _TalentSecondarySkill_Details = await _dbContext.GenTalentSecondarySkillDetails.Where(x => x.TalentId == PMS_TalentInsertedID && x.TempSkillId == "O_" + Curr_TempSkillID.Id).FirstOrDefaultAsync();
                            if (_TalentSecondarySkill_Details == null)
                            {
                                _TalentSecondarySkill_Details = new GenTalentSecondarySkillDetail();
                                _TalentSecondarySkill_Details.TalentId = PMS_TalentInsertedID;
                                _TalentSecondarySkill_Details.TempSkillId = "O_" + Curr_TempSkillID.Id;
                                _dbContext.GenTalentSecondarySkillDetails.Add(_TalentSecondarySkill_Details);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            GenTalentSecondarySkillDetail? _TalentSecondarySkill_Details = await _dbContext.GenTalentSecondarySkillDetails.Where(x => x.TalentId == PMS_TalentInsertedID && x.SecondarySkillId == _Skills.Id).FirstOrDefaultAsync();
                            if (_TalentSecondarySkill_Details == null)
                            {
                                _TalentSecondarySkill_Details = new GenTalentSecondarySkillDetail();
                                _TalentSecondarySkill_Details.TalentId = PMS_TalentInsertedID;
                                _TalentSecondarySkill_Details.SecondarySkillId = Convert.ToInt32(_Skills.Id);

                                _dbContext.GenTalentSecondarySkillDetails.Add(_TalentSecondarySkill_Details);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
                #endregion



            }
            return new PMSTalentOutput { TalentId = TalentID, Message = "Success" };
        }

        #endregion

        #region CurrencyTransaction

        [HttpGet("GetCurrency")]
        public async Task<ObjectResult> GetCurrency()
        {
            List<CurrencyExchangeRateViewModel> currencyExchangeRateViewModelsList = new List<CurrencyExchangeRateViewModel>();

            object[] param = new object[] { 1, 1000, "ID", "asc" };
            string paramasString = CommonLogic.ConvertToParamString(param);

            List<Sproc_CurrencyExchangeRate_Result> varCurrencyExchangeRateList = await _iATSsyncUTS.GetCurrencyExchangeRate_Results(paramasString);

            foreach (var x in varCurrencyExchangeRateList)
            {
                CurrencyExchangeRateViewModel currencyExchangeRateViewModels = new CurrencyExchangeRateViewModel();
                currencyExchangeRateViewModels.ID = x.ID;
                currencyExchangeRateViewModels.CurrencyCode = x.CurrencyCode;
                currencyExchangeRateViewModels.CurrencySign = x.CurrencySign;
                currencyExchangeRateViewModels.ExchangeRate = x.USD_ExchangeRate;
                currencyExchangeRateViewModels.LastUpdatedDate = Convert.ToDateTime(x.LastUpdatedDate).ToString("dd-MM-yyyy HH:mm:ss");
                currencyExchangeRateViewModelsList.Add(currencyExchangeRateViewModels);
            }

            if (currencyExchangeRateViewModelsList.Any())
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = currencyExchangeRateViewModelsList });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No Currencies Found" });
            }
        }

        [HttpGet("GetCurrencyINR")]
        public async Task<ObjectResult> GetCurrencyINR()
        {
            List<CurrencyExchangeRateViewModel> currencyExchangeRateViewModelsList = new List<CurrencyExchangeRateViewModel>();

            object[] param = new object[] { 1, 1000, "ID", "asc" };
            string paramasString = CommonLogic.ConvertToParamString(param);

            List<Sproc_CurrencyExchangeRate_Result> varCurrencyExchangeRateList = await _iATSsyncUTS.GetCurrencyExchangeRate_Results(paramasString);

            foreach (var x in varCurrencyExchangeRateList)
            {
                CurrencyExchangeRateViewModel currencyExchangeRateViewModels = new CurrencyExchangeRateViewModel();
                currencyExchangeRateViewModels.ID = x.ID;
                currencyExchangeRateViewModels.CurrencyCode = x.CurrencyCode;
                currencyExchangeRateViewModels.CurrencySign = x.CurrencySign;
                currencyExchangeRateViewModels.ExchangeRate = x.ExchangeRate;
                currencyExchangeRateViewModels.LastUpdatedDate = Convert.ToDateTime(x.LastUpdatedDate).ToString("dd-MM-yyyy HH:mm:ss");
                currencyExchangeRateViewModelsList.Add(currencyExchangeRateViewModels);
            }

            if (currencyExchangeRateViewModelsList.Any())
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = currencyExchangeRateViewModelsList });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "No Currencies Found" });
            }
        }


        #endregion

        #region GetHrStatusSubStatusDetails

        [HttpPost("GetHrStatusSubStatusDetails")]
        public async Task<IActionResult> GetHrStatusSubStatusDetails()
        {
            string apiType = "GetHrStatusSubStatusDetails";
            string endPoint = $"{_iConfiguration["ProjectURL_API"]}{apiType}";
            string result = string.Empty;

            try
            {
                // Read request body
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();

                GetHRStatus? hRStatus = JsonConvert.DeserializeObject<GetHRStatus>(body);
                if (hRStatus == null)
                {
                    return BadRequest(new { status = 400, ErrorMessage = "Invalid request payload." });
                }

                // Add record in gen_UtsAts_Records
                var utsAtsApi_Records = new GenUtsAtsApiRecord
                {
                    FromApiUrl = "ATS HR Status Update",
                    ToApiUrl = endPoint,
                    PayloadToSend = body,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.UtcNow,
                    HrId = hRStatus.HR_ID
                };

                _dbContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
                await _dbContext.SaveChangesAsync();
                long APIRecordInsertedID = utsAtsApi_Records.Id;

                object[] param = new object[]
                {
                  hRStatus.HR_ID, hRStatus.HR_Status, hRStatus.HR_Sub_Status, hRStatus.ActionDoneBy, hRStatus.ActionDate
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                await _dbContext.Database.ExecuteSqlRawAsync(String.Format("{0} {1}", Constants.ProcConstant.Sproc_Update_HrStatus_SubStatus_ClientPortal, paramString));

                result = "Success";

                // Update record in gen_UtsAts_Records
                await UpdateApiRecord(APIRecordInsertedID, result);

                if (result == "Success")
                {
                    return Ok(new { status = 200, Message = result });
                }
            }
            catch (DbUpdateException dbEx)
            {
                result = "Database error: " + dbEx.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Unauthorized(new { status = 401, ErrorMessage = result });
        }
        #endregion

        #region AtsHrIsPublished

        [HttpPost("AtsHrIsPublished")]
        public async Task<IActionResult> AtsHrIsPublished()
        {
            string apiType = "AtsHrIsPublished";
            string endPoint = $"{_iConfiguration["ProjectURL_API"]}{apiType}";
            string result = string.Empty;

            try
            {
                // Read request body
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();

                ATS_HRStatusTalentDetailsViewModel? objHRStatusTalentDetail = JsonConvert.DeserializeObject<ATS_HRStatusTalentDetailsViewModel>(body);
                if (objHRStatusTalentDetail == null)
                {
                    return BadRequest(new { status = 400, ErrorMessage = "Invalid request payload." });
                }

                // Add record in gen_UtsAts_Records
                var utsAtsApi_Records = new GenUtsAtsApiRecord
                {
                    FromApiUrl = "Ats Hr Is Published",
                    ToApiUrl = endPoint,
                    PayloadToSend = body,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.UtcNow,
                    HrId = objHRStatusTalentDetail.HRID
                };

                _dbContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
                await _dbContext.SaveChangesAsync();
                long APIRecordInsertedID = utsAtsApi_Records.Id;

                var varStageWiseTextRes = await _dbContext.PrgAtshrstatuses.AsNoTracking().Where(x => x.Atshrstatus.ToLower() == objHRStatusTalentDetail.ATS_HR_Status.ToLower()).FirstOrDefaultAsync();
                var varIfHRExists = await _dbContext.GenAtsHrstatusDetails.AsNoTracking().Where(x => x.Hrid == objHRStatusTalentDetail.HRID && x.AtshrstatusId == varStageWiseTextRes.Id).FirstOrDefaultAsync();

                if (varIfHRExists == null)
                {
                    if (varStageWiseTextRes != null)
                    {
                        var varContactID = await _dbContext.GenSalesHiringRequests.AsNoTracking().Where(x => x.Id == objHRStatusTalentDetail.HRID).FirstOrDefaultAsync();

                        var myInClause = new long[] { 2 };//, 3, 4, 5, 6 
                        var varPrg_ATSHRStatus = await _dbContext.PrgAtshrstatuses.AsNoTracking().Where(x => myInClause.Contains(x.Id)).ToListAsync();
                        foreach (var item in varPrg_ATSHRStatus)
                        {
                            GenAtsHrstatusDetail obj = new GenAtsHrstatusDetail();
                            obj.Hrid = objHRStatusTalentDetail.HRID;
                            obj.AtshrstatusId = item.Id;
                            obj.TotalTalent = 0;
                            obj.PublishedDatetime = objHRStatusTalentDetail.Published_Datetime;
                            obj.CreatedById = varContactID != null ? varContactID.ContactId : 0;
                            obj.CreatedByDateTime = DateTime.Now;
                            obj.ModifiedById = null;
                            obj.ModifiedByDateTime = null;
                            _dbContext.GenAtsHrstatusDetails.Add(obj);
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    #region update ModifiedByDateTime in gen_ATS_HRStatus_Details
                    GenAtsHrstatusDetail? obj = new GenAtsHrstatusDetail();
                    obj = await _dbContext.GenAtsHrstatusDetails.AsNoTracking().Where(x => x.Id == varIfHRExists.Id).FirstOrDefaultAsync();
                    if (obj != null)
                    {
                        obj.ModifiedById = 0;
                        obj.ModifiedByDateTime = DateTime.Now;
                        _dbContext.Entry(obj).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                    #endregion
                }

                result = "Success";

                // Update record in gen_UtsAts_Records
                await UpdateApiRecord(APIRecordInsertedID, result);

                if (result == "Success")
                {
                    return Ok(new { status = 200, Message = result });
                }
            }
            catch (DbUpdateException dbEx)
            {
                result = "Database error: " + dbEx.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Unauthorized(new { status = 401, ErrorMessage = result });
        }
        #endregion

        private async Task UpdateApiRecord(long recordId, string response)
        {
            await Task.Delay(1000);
            var record = await _dbContext.GenUtsAtsApiRecords.FindAsync(recordId);
            if (record != null)
            {
                record.ResponseReceived = response;
                _dbContext.Entry(record).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
        private async Task InsertHistoryForTalent(Action_Of_TalentHistory action_Of_TalentHistory, long TalentID, bool IsTalent, long LoggedInUserId, long HiringRequestId)
        {
            object[] param = new object[]
            {
                action_Of_TalentHistory, TalentID, IsTalent, LoggedInUserId, HiringRequestId
            };

            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
            await _dbContext.Database.ExecuteSqlRawAsync(String.Format("{0} {1}", Constants.ProcConstant.sproc_Talent_History_Insert, paramString));
        }
        private async Task InsertHistoryForHiringRequest(HiringRequestInsertHistoryModel model, Action_Of_History action_Of_History, AppActionDoneBy appActionDoneBy)
        {
            object[] param = new object[]
            {
                action_Of_History,
                model.HiringRequestId,
                model.TalentId,
                model.CreatedFrom,
                model.LoggedInUserId,
                model.ContactTalentPriorityId,
                model.InterviewMasterId,
                model.HRAcceptedDateTime,
                model.OnBoardId,
                model.IsManagedByClient,
                model.IsManagedByTalent,
                model.SalesUserId,
                model.OldSalesUserId,
                (int)appActionDoneBy
            };

            string paramString = CommonLogic.ConvertToParamStringWithNull(param);
            await _dbContext.Database.ExecuteSqlRawAsync(String.Format("{0} {1}", Constants.ProcConstant.sproc_HiringRequest_History_Insert, paramString));
        }
    }
}
