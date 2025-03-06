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

namespace UTSATSAPI.Controllers
{
    [ApiController]
    [Route("CommonAPI/")]
    public class ReverseMatchmakingController : ControllerBase
    {
        private readonly UTSATSAPIDBConnection _dbContext;
        private readonly IConfiguration _iConfiguration;

        public ReverseMatchmakingController(UTSATSAPIDBConnection dbContext, IConfiguration iConfiguration)
        {
            _dbContext = dbContext;
            _iConfiguration = iConfiguration;
        }

        #region Reverse Matchmaking

        [HttpPost("ReverseMatchmaking")]
        public async Task<IActionResult> ReverseMatchmaking()
        {
            string projectUrlApi = _iConfiguration["ProjectURL_API"];
            string endPoint = projectUrlApi + "ReverseMatchmaking";
            string result = string.Empty;

            try
            {
                // Read request body
                using var reader = new StreamReader(Request.Body);
                string body = await reader.ReadToEndAsync();

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

                if (varSalesHiringRequest != null && !Convert.ToBoolean(varSalesHiringRequest.IsAccepted))
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

            InsertHistoryForTalent(Action_Of_TalentHistory.Talent_Status_Selected, talent.Id, false, userRequest?.Id ?? atsUserId, reverseMatchmakingViewModel.HRID);

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

            InsertHistoryForTalent(Action_Of_TalentHistory.ATS_ReverseMatchMaking, talent.Id, false, userRequest?.Id ?? atsUserId, reverseMatchmakingViewModel.HRID);
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

                InsertHistoryForHiringRequest(history, Action_Of_History.Talent_Fees_Update, AppActionDoneBy.ATS);

                return "Success";
            }
            catch (Exception ex)
            {                
                return $"Error: {ex.Message}";
            }
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
        private void InsertHistoryForTalent(Action_Of_TalentHistory action_Of_TalentHistory, long TalentID, bool IsTalent, long LoggedInUserId, long HiringRequestId)
        {
            string sql = string.Format(Constants.ProcConstant.sproc_Talent_History_Insert, action_Of_TalentHistory, TalentID, IsTalent, LoggedInUserId, HiringRequestId);

            _dbContext.Database.ExecuteSqlRawAsync(sql);
        }
        private void InsertHistoryForHiringRequest(HiringRequestInsertHistoryModel model, Action_Of_History action_Of_History, AppActionDoneBy appActionDoneBy)
        {            
            string sql = string.Format(Constants.ProcConstant.sproc_HiringRequest_History_Insert, action_Of_History, model.HiringRequestId, model.TalentId, model.CreatedFrom, model.LoggedInUserId, model.ContactTalentPriorityId, model.InterviewMasterId, model.HRAcceptedDateTime, model.OnBoardId, model.IsManagedByClient, model.IsManagedByTalent, model.SalesUserId, model.OldSalesUserId, (int)appActionDoneBy);
            _dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
