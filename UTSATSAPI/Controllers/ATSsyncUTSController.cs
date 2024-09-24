using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using UTSATSAPI.Attributes;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;
using static UTSATSAPI.Config.HubSpotResponseUTSAdmin;

namespace UTSATSAPI.Controllers
{

    [ApiController]
    [Route("ATSsyncUTS/")]

    public class ATSsyncUTSController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _db;
        private readonly IUniversalProcRunner _uniProcRunner;
        private IConfiguration _configuration;
        #endregion

        #region Constructors
        public ATSsyncUTSController(TalentConnectAdminDBContext adminDBContext, IUniversalProcRunner uniProcRunner, IConfiguration configuration)
        {
            _db = adminDBContext;
            _uniProcRunner = uniProcRunner;
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        //[ApiKey]
        //[HttpPost("EditHRThroughATS")]
        //public async Task<ObjectResult> EditHRThroughATS(ATSHiringReqeustModel model)
        //{
        //    long APIRecordInsertedID = 0;
        //    string Message = "";
        //    try
        //    {
        //        #region Validation
        //        if (model == null)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
        //        }
        //        if (model.HrId == 0 || model.HrId == null)
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass proper HrId" });
        //        }
        //        #endregion

        //        #region Add record in gen_UtsAts_Records
        //        string EditHRJsonData = JsonConvert.SerializeObject(model);
        //        GenUtsAtsApiRecord utsAtsApi_Records = new()
        //        {
        //            FromApiUrl = "ATS Edit HR",
        //            ToApiUrl = _configuration["ProjectURL"].ToString() + "EditHRThroughATS",
        //            PayloadToSend = EditHRJsonData,
        //            CreatedById = 0,
        //            CreatedByDateTime = DateTime.Now,
        //            HrId = model.HrId
        //        };

        //        APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);
        //        #endregion

        //        #region Download file from AWS Server & upload to UTS server
        //        try
        //        {
        //            if (!string.IsNullOrEmpty(model.JDFile_ATSURL))
        //            {
        //                string BucketName = _configuration["BucketName"].ToString();
        //                string KeyName = _configuration["KeyName"].ToString();
        //                string AccessKey = _configuration["AccessKey"].ToString();
        //                string SecretKey = _configuration["SecretKey"].ToString();

        //                string fileName = Path.GetFileName(model.JDFile_ATSURL);

        //                var credentials = new Amazon.Runtime.BasicAWSCredentials(AccessKey, SecretKey);

        //                using (var client = new AmazonS3Client(credentials, RegionEndpoint.USEast1)) // Replace YOUR_REGION with the appropriate AWS region
        //                {
        //                    var getObjectRequest = new GetObjectRequest
        //                    {
        //                        BucketName = BucketName,
        //                        Key = KeyName + fileName
        //                    };

        //                    using (GetObjectResponse response = await client.GetObjectAsync(getObjectRequest))
        //                    {
        //                        string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", fileName);
        //                        // Create a FileStream to write the file to the UTS server
        //                        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                        {
        //                            // Copy the file from S3 to the UTS server
        //                            await response.ResponseStream.CopyToAsync(fileStream);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (AmazonS3Exception ex)
        //        {
        //            Message += "[Error downloading file:" + ex.Message.ToString() + "]";
        //            UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Message += "[" + ex.Message.ToString() + "]";
        //            UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
        //        }
        //        #endregion

        //        #region SP call

        //        string strCompensationOption = null;
        //        string strCandidateIndustry = null;

        //        if (model.VitalInformation != null)
        //        {
        //            if (model.VitalInformation.CompensationOption != null && model.VitalInformation.CompensationOption.Any())
        //            {
        //                strCompensationOption = string.Join('^', model.VitalInformation.CompensationOption);
        //            }
        //            if (model.VitalInformation.CandidateIndustry != null && model.VitalInformation.CandidateIndustry.Any())
        //            {
        //                strCandidateIndustry = string.Join('^', model.VitalInformation.CandidateIndustry);
        //            }
        //        }

        //        object[] param = new object[]
        //        {
        //               model?.HrId,
        //               model?.ContactId,
        //               model?.Availability,
        //               model?.ContractDuration,
        //               model?.Currency,
        //               model?.AdhocBudgetCost,
        //               model?.MinimumBudget,
        //               model?.MaximumBudget,
        //               model?.IsConfidentialBudget,
        //               model?.ModeOfWorkingId,
        //               model?.City,
        //               model?.Country,
        //               model?.JDFilename,
        //               model?.JDURL,
        //               model?.YearOfExp,
        //               model?.NoofTalents,
        //               model?.TimezoneId,
        //               model?.TimeZoneFromTime,
        //               model?.TimeZoneEndTime,
        //               model?.HowSoon,
        //               model?.PartialEngagementTypeID,
        //               model?.NoofHoursworking,
        //               model?.DurationType,
        //               model?.HrTitle,
        //               model?.RoleAndResponsibilites,
        //               model?.Requirements,
        //               model?.JobDescription,
        //               model?.MustHaveSkills,
        //               model?.GoodToHaveSkills,
        //               model?.IsHrfocused,
        //               model?.ATS_PayPerHire?.IsHRTypeDP,
        //               model?.ATS_PayPerHire?.DpPercentage,
        //               model?.ATS_PayPerHire?.NRMargin,
        //               model?.ATS_PayPerHire?.IsTransparentPricing,
        //               model?.ATS_PayPerHire?.HrTypePricingId,
        //               model?.ATS_PayPerHire?.PayrollTypeId,
        //               model?.ATS_PayPerHire?.PayrollPartnerName,
        //               model?.ATS_PayPerCredit?.IsVettedProfile,
        //               model?.ATS_PayPerCredit?.IsHiringLimited,
        //               model?.LastModifiedById,
        //               model?.ATS_PayPerCredit?.JobTypeId,
        //               strCompensationOption,
        //               strCandidateIndustry,
        //               model?.VitalInformation?.HasPeopleManagementExp,
        //               model?.VitalInformation?.Prerequisites,
        //               model?.JobLocation,
        //               model?.FrequencyOfficeVisitID,
        //               model?.IsOpenToWorkNearByCities,
        //               model?.NearByCities,
        //               model?.ATS_JobLocationID,
        //               model?.ATS_NearByCities,
        //        };

        //        string paramString = CommonLogic.ConvertToParamStringWithNull(param);
        //        _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTSAdmin_EditHrByATS, paramString));

        //        #endregion

        //        Message += "[Hiring request updated successfully by ATS]";
        //        UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
        //        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        Message += "[" + ex.Message.ToString() + "]";
        //        UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
        //        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message });
        //        throw;
        //    }
        //}

        [HttpPost("TransferFileFromAWSToUTSServer")]
        public async Task<ObjectResult> TransferFileFromAWSToUTSServer([FromBody] FilePath file)
        {
            try
            {
                if (file == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                string BucketName = _configuration["BucketName"].ToString();
                string KeyName = _configuration["KeyName"].ToString();
                string AccessKey = _configuration["AccessKey"].ToString();
                string SecretKey = _configuration["SecretKey"].ToString();

                string fileName = Path.GetFileName(file.FileName);

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
                        string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", fileName);
                        // Create a FileStream to write the file to the local server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            // Copy the file from S3 to the local server
                            await response.ResponseStream.CopyToAsync(fileStream);
                        }
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
            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "successfully trasfer file" });
        }

        #endregion

        #region Private
        private long InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records)
        {
            GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();

            utsAtsApi_Records.FromApiUrl = gen_UtsAtsApi_Records.FromApiUrl;
            utsAtsApi_Records.ToApiUrl = gen_UtsAtsApi_Records.ToApiUrl;    //Here API URL of ATS will come.
            utsAtsApi_Records.PayloadToSend = gen_UtsAtsApi_Records.PayloadToSend;
            utsAtsApi_Records.CreatedById = gen_UtsAtsApi_Records.CreatedById;
            utsAtsApi_Records.CreatedByDateTime = DateTime.Now;
            utsAtsApi_Records.HrId = gen_UtsAtsApi_Records.HrId;
            _db.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
            _db.SaveChanges();

            return utsAtsApi_Records.Id;
        }
        private void UpdateUtsAtsApiDetails(long APIRecordInsertedID, string Message)
        {
            #region Update record in gen_UtsAts_Records
            GenUtsAtsApiRecord utsAtsApi_Records = _db.GenUtsAtsApiRecords.Where(x => x.Id == APIRecordInsertedID).FirstOrDefault();
            if (utsAtsApi_Records != null)
            {
                utsAtsApi_Records.ResponseReceived = Message;
                CommonLogic.DBOperator(_db, utsAtsApi_Records, EntityState.Modified);
            }
            #endregion
        }
        #endregion


        [HttpPost("GetCreditTransaction")]
        public async Task<IActionResult> GetCreditTransaction()
        {

            var headers = Request.Headers;
            string? token = "";
            bool isUpScreen = false;

            var dict = headers.ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value);
            Hashtable htable = new Hashtable(dict);
            if (!htable.ContainsKey("authorization"))
            {
                var JsonString = new
                {
                    status = 401,
                    ErrorMessage = "No Authorization Key found."
                };

                return Ok(JsonString);
            }

            token = Convert.ToString(htable["authorization"]);

            if (token != _configuration["ATSAPIKeyForUnlock"].ToString())
            {
                var JsonString = new
                {
                    status = 401,
                    ErrorMessage = "Invalid Token."
                };

                return Ok(JsonString);
            }

            string responseMessage = "";

            using (StreamReader reader = new StreamReader(Request.Body))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                string xxjson = await reader.ReadToEndAsync();

                // Process xxjson if needed
                long id = await SaveAPILogs(xxjson, isUpScreen, Request.GetDisplayUrl());

                CreditTransactionPayloadFromATS json = JsonConvert.DeserializeObject<CreditTransactionPayloadFromATS>(xxjson);
                if (json != null)
                {
                    object[] param = new object[]
                    {
                       json.company_id,
                       json.contact_id,
                       json.hr_id,
                       json.atstalent_id,
                       json.transaction_type,
                       json.credit_type,
                       json.credit_used,
                       json.credit_amount,
                       json.credit_currency,
                       json.action_type,
                       json.balance_credit,
                       json.user_id,
                       json.transactiondoneby,
                       json.transaction_date,
                       "set",
                       0,
                       "",
                       json.order_amount,
                       json.payment_provider,
                       json.payment_status,
                       json.payer_name,
                       json.payer_email,
                       json.payer_id,
                       json.order_comments
                    };

                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    var data = _uniProcRunner.Sproc_Add_Company_Transactions_With_ATS(paramString);

                    if (data != null)
                    {
                        int statusCode = 200; // responseMessage == "success" ? 200 : 400;
                        responseMessage = "success";
                        var response = new
                        {
                            status = statusCode,
                            detail = data
                        };
                        await UpdateAPILogs(responseMessage, id);
                        return Ok(response);
                    }
                }                
            }

            Sproc_Add_Company_Transactions_With_ATS_Result sproc_Add_Company_Transactions_With_ATS = new Sproc_Add_Company_Transactions_With_ATS_Result();
            var response1 = new
            {
                status = 400,
                detail = sproc_Add_Company_Transactions_With_ATS
            };
            // Your other code logic here

            return Ok(response1); // Or return appropriate IActionResult
        }

        private async Task<long> SaveAPILogs(string json, bool isUpScreen, string toUrl)
        {
            try
            {
                string url = isUpScreen ? "UpScreen" : "ATS";
                GenAtsupscreenApiRecordsClientPortal utsAtsApi_Records = new GenAtsupscreenApiRecordsClientPortal();
                utsAtsApi_Records.FromApiUrl = url;
                utsAtsApi_Records.ToApiUrl = toUrl;
                utsAtsApi_Records.PayloadToSend = json;
                utsAtsApi_Records.CreatedById = 1;

                long APIRecordInsertedID = await InsertUtsAtsUnlockTalentApiDetails(utsAtsApi_Records);

                return APIRecordInsertedID;
            }
            catch
            {
                return 0;
            }
        }
        private async Task UpdateAPILogs(string response, long id)
        {
            try
            {
                await UpdateUtsAtsApiUnlockTalentDetails(response, id);
            }
            catch
            {

            }
        }
        public async Task<long> InsertUtsAtsUnlockTalentApiDetails(GenAtsupscreenApiRecordsClientPortal gen_UtsAtsApi_Records)
        {
            GenAtsupscreenApiRecordsClientPortal utsAtsApi_Records = new GenAtsupscreenApiRecordsClientPortal();

            utsAtsApi_Records.FromApiUrl = gen_UtsAtsApi_Records.FromApiUrl;
            utsAtsApi_Records.ToApiUrl = gen_UtsAtsApi_Records.ToApiUrl;    //Here API URL of ATS will come.
            utsAtsApi_Records.PayloadToSend = gen_UtsAtsApi_Records.PayloadToSend;
            utsAtsApi_Records.CreatedById = gen_UtsAtsApi_Records.CreatedById;
            utsAtsApi_Records.CreatedByDateTime = DateTime.Now;
            await _db.GenAtsupscreenApiRecordsClientPortals.AddAsync(utsAtsApi_Records);
            _db.SaveChangesAsync();

            return utsAtsApi_Records.Id;
        }
        public async Task<bool> UpdateUtsAtsApiUnlockTalentDetails(string responseJson, long APIRecordInsertedID)
        {
            GenAtsupscreenApiRecordsClientPortal utsAtsApi_Records = await _db.GenAtsupscreenApiRecordsClientPortals.Where(x => x.Id == APIRecordInsertedID).FirstOrDefaultAsync();
            if (utsAtsApi_Records != null)
            {
                utsAtsApi_Records.ResponseReceived = responseJson;
                _db.GenAtsupscreenApiRecordsClientPortals.Update(utsAtsApi_Records);
                _db.SaveChangesAsync();
            }

            return true;
        }
        public class FilePath
        {
            public string FileName { get; set; }
        }

    }
}
