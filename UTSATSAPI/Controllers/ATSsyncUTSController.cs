using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{

    [ApiController]
    [Route("ATSsyncUTS/")]

    public class ATSsyncUTSController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _db;
        private readonly IUniversalProcRunner _uniProcRunner;
        private readonly IConfiguration _configuration;
        private readonly IATSsyncUTS _iATSsyncUTS;
        #endregion

        #region Constructors
        public ATSsyncUTSController(TalentConnectAdminDBContext adminDBContext, IUniversalProcRunner uniProcRunner, IConfiguration configuration, IATSsyncUTS iATSsyncUTS)
        {
            _db = adminDBContext;
            _uniProcRunner = uniProcRunner;
            _configuration = configuration;
            _iATSsyncUTS = iATSsyncUTS;
        }
        #endregion

        #region Public Methods

        #region Add/Edit HR

        [HttpPost("EditHRThroughATS")]
        public async Task<ObjectResult> EditHRThroughATS(ATSHiringReqeustModel1 model)
        {
            long APIRecordInsertedID = 0;
            string Message = "";
            try
            {
                #region Validation
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                if (model.HrId == 0 || model.HrId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass proper HrId" });
                }
                #endregion

                #region Add record in gen_UtsAts_Records
                string EditHRJsonData = JsonConvert.SerializeObject(model);
                GenUtsAtsApiRecord utsAtsApi_Records = new()
                {
                    FromApiUrl = "ATS Edit HR",
                    ToApiUrl = _configuration["ProjectURL"].ToString() + "EditHRThroughATS",
                    PayloadToSend = EditHRJsonData,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.Now,
                    HrId = model.HrId
                };

                APIRecordInsertedID = _iATSsyncUTS.InsertUtsAtsApiDetails(utsAtsApi_Records);
                #endregion

                #region Download file from AWS Server & upload to UTS server
                try
                {
                    if (!string.IsNullOrEmpty(model.JDFile_ATSURL))
                    {
                        string BucketName = _configuration["BucketName"].ToString();
                        string KeyName = _configuration["KeyName"].ToString();
                        string AccessKey = _configuration["AccessKey"].ToString();
                        string SecretKey = _configuration["SecretKey"].ToString();

                        string fileName = Path.GetFileName(model.JDFile_ATSURL);

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
                                // Create a FileStream to write the file to the UTS server
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    // Copy the file from S3 to the UTS server
                                    await response.ResponseStream.CopyToAsync(fileStream);
                                }
                            }
                        }
                    }
                }
                catch (AmazonS3Exception ex)
                {
                    Message += "[Error downloading file:" + ex.Message.ToString() + "]";
                    _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                }
                catch (Exception ex)
                {
                    Message += "[" + ex.Message.ToString() + "]";
                    _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                }
                #endregion

                #region SP call

                string strCompensationOption = null;
                string strCandidateIndustry = null;

                if (model.VitalInformation != null)
                {
                    if (model.VitalInformation.CompensationOption != null && model.VitalInformation.CompensationOption.Any())
                    {
                        strCompensationOption = string.Join('^', model.VitalInformation.CompensationOption);
                    }
                    if (model.VitalInformation.CandidateIndustry != null && model.VitalInformation.CandidateIndustry.Any())
                    {
                        strCandidateIndustry = string.Join('^', model.VitalInformation.CandidateIndustry);
                    }
                }

                object[] param = new object[]
                {
                       model?.HrId,
                       model?.ContactId,
                       model?.Availability,
                       model?.ContractDuration,
                       model?.Currency,
                       model?.AdhocBudgetCost,
                       model?.MinimumBudget,
                       model?.MaximumBudget,
                       model?.IsConfidentialBudget,
                       model?.ModeOfWorkingId,
                       model?.City,
                       model?.Country,
                       model?.JDFilename,
                       model?.JDURL,
                       model?.YearOfExp,
                       model?.NoofTalents,
                       model?.TimezoneId,
                       model?.TimeZoneFromTime,
                       model?.TimeZoneEndTime,
                       model?.HowSoon,
                       model?.PartialEngagementTypeID,
                       model?.NoofHoursworking,
                       model?.DurationType,
                       model?.HrTitle,
                       model?.RoleAndResponsibilites,
                       model?.Requirements,
                       model?.JobDescription,
                       model?.MustHaveSkills,
                       model?.GoodToHaveSkills,
                       model?.IsHrfocused,
                       model?.ATS_PayPerHire?.IsHRTypeDP,
                       model?.ATS_PayPerHire?.DpPercentage,
                       model?.ATS_PayPerHire?.NRMargin,
                       model?.ATS_PayPerHire?.IsTransparentPricing,
                       model?.ATS_PayPerHire?.HrTypePricingId,
                       model?.ATS_PayPerHire?.PayrollTypeId,
                       model?.ATS_PayPerHire?.PayrollPartnerName,
                       model?.ATS_PayPerCredit?.IsVettedProfile,
                       model?.ATS_PayPerCredit?.IsHiringLimited,
                       model?.LastModifiedById,
                       model?.ATS_PayPerCredit?.JobTypeId,
                       strCompensationOption,
                       strCandidateIndustry,
                       model?.VitalInformation?.HasPeopleManagementExp,
                       model?.VitalInformation?.Prerequisites,
                       model?.JobLocation,
                       model?.FrequencyOfficeVisitID,
                       model?.IsOpenToWorkNearByCities,
                       model?.NearByCities,
                       model?.ATS_JobLocationID,
                       model?.ATS_NearByCities,
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UTSAdmin_EditHrByATS, paramString));

                #endregion

                Message += "[Hiring request updated successfully by ATS]";
                _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message });
            }
            catch (Exception ex)
            {
                Message += "[" + ex.Message.ToString() + "]";
                _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message });
                throw;          
            }
        }

        [HttpPost("AddEditHRThroughATS")]
        public async Task<ObjectResult> AddEditHRThroughATS(ATSHiringReqeustModel model)
        {
            long APIRecordInsertedID = 0;
            string Message = "";
            try
            {
                #region Validation
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                #endregion

                #region Add record in gen_UtsAts_Records
                string EditHRJsonData = JsonConvert.SerializeObject(model);
                GenUtsAtsApiRecord utsAtsApi_Records = new()
                {
                    FromApiUrl = "ATS Edit HR",
                    ToApiUrl = _configuration["ProjectURL"].ToString() + "EditHRThroughATS",
                    PayloadToSend = EditHRJsonData,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.Now,
                    HrId = model.hiring_request_id
                };

                APIRecordInsertedID = _iATSsyncUTS.InsertUtsAtsApiDetails(utsAtsApi_Records);
                #endregion

                #region Download file from AWS Server & upload to UTS server
                try
                {
                    if (!string.IsNullOrEmpty(model.jd_file_ats_url))
                    {
                        string BucketName = _configuration["BucketName"].ToString();
                        string KeyName = _configuration["KeyName"].ToString();
                        string AccessKey = _configuration["AccessKey"].ToString();
                        string SecretKey = _configuration["SecretKey"].ToString();

                        string fileName = Path.GetFileName(model.jd_file_ats_url);

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
                                // Create a FileStream to write the file to the UTS server
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    // Copy the file from S3 to the UTS server
                                    await response.ResponseStream.CopyToAsync(fileStream);
                                }
                            }
                        }
                    }
                }
                catch (AmazonS3Exception ex)
                {
                    Message += "[Error downloading file:" + ex.Message.ToString() + "]";
                    _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                }
                catch (Exception ex)
                {
                    Message += "[" + ex.Message.ToString() + "]";
                    _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                }
                #endregion

                #region SP call --- Sproc_UTS_AddEdit_ATSHR

                bool? is_confidential_budget = null;
                bool? is_fresher_allowed = null;
                bool? is_open_to_work_near_by = null;
                bool? has_people_management_exp = null;

                if (model?.is_confidential_budget != null)
                {
                    is_confidential_budget = model?.is_confidential_budget == 1 ? true : false;
                }
                if (model?.is_fresher_allowed != null)
                {
                    is_fresher_allowed = model?.is_fresher_allowed == 1 ? true : false;
                }
                if (model?.is_open_to_work_near_by != null)
                {
                    is_open_to_work_near_by = model?.is_open_to_work_near_by == 1 ? true : false;
                }
                if (model?.vital_information?.has_people_management_exp != null)
                {
                    has_people_management_exp = model?.vital_information.has_people_management_exp == 1 ? true : false;
                }

                string? strCompensationOption = null;
                string? strCandidateIndustry = null;

                if (model?.vital_information != null)
                {
                    if (model.vital_information.compensation_option != null && model.vital_information.compensation_option.Any())
                    {
                        strCompensationOption = string.Join('^', model.vital_information.compensation_option);
                    }
                    if (model.vital_information.industry != null && model.vital_information.industry.Any())
                    {
                        strCandidateIndustry = string.Join('^', model.vital_information.industry);
                    }
                }

                object[] param = new object[]
                {
                       model?.hiring_request_id,
                       model?.hr_number,
                       model?.contact_id,
                       model?.availability,
                       model?.month_duration,
                       model?.currency,
                       model?.cost,
                       model?.cost_start,
                       model?.cost_end,
                       is_confidential_budget,
                       model?.mode_of_working,
                       model?.jd_filename,
                       model?.jd_url,
                       model?.years_of_exp,
                       is_fresher_allowed,
                       model?.no_of_talents,
                       model?.timezone_id,
                       model?.shift_start_time,
                       model?.shift_end_time,
                       model?.notice_period,
                       model?.partial_engagement_type,
                       model?.no_of_hours_working,
                       model?.durationType,
                       model?.job_title,
                       model?.RoleAndResponsibilites,
                       model?.Requirements,
                       model?.job_desciption,
                       model?.must_have_skills,
                       model?.good_to_have_skills,
                       model?.is_hr_focused,
                       model?.pay_per_hire?.is_dp,
                       model?.pay_per_hire?.dp_margin,
                       model?.pay_per_hire?.nr_margin,
                       model?.pay_per_hire?.is_transparent,
                       model?.pay_per_hire?.pricing_id,
                       model?.pay_per_hire?.payroll_type_id,
                       model?.pay_per_hire?.payroll_partner_name,
                       model?.pay_per_credit?.is_vetted_profile,
                       model?.pay_per_credit?.is_hiring_limited,
                       model?.LastModifiedById,
                       model?.pay_per_credit?.job_type_id,
                       strCompensationOption,
                       strCandidateIndustry,
                       has_people_management_exp,
                       model?.vital_information?.prerequisites,
                       model?.location,
                       model?.frequency_office_visit,
                       is_open_to_work_near_by,
                       model?.near_by_cities,
                       model?.location_id,
                       model?.ats_near_by_cities,
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                Sproc_UTS_AddEdit_ATSHR_Result result = _iATSsyncUTS.Sproc_UTS_AddEdit_ATSHR(paramString);

                if (result != null && result.HiringRequestID > 0)
                {
                    model.hiring_request_id = result.HiringRequestID;
                    if (!string.IsNullOrEmpty(result.ResponseMsg))
                    {
                        Message += string.Format("[{0}]", result.ResponseMsg);
                    }
                }
                #endregion

                #region Save HR POC users
                try
                {
                    StringBuilder POcDetails = new();
                    string pocDetailsString = string.Empty;
                    if (model?.job_poc != null && model.job_poc.Any())
                    {
                        param = null;
                        foreach (var item in model.job_poc)
                        {
                            //Update contact Number in gen_contact
                            if (!string.IsNullOrEmpty(item.contact_number) && item.contact_id > 0)
                            {
                                param = new object[] { item.contact_id, item.contact_number };
                                _uniProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_HR_EditPOC, param);
                            }

                            POcDetails.Append(item.contact_id + "&");
                            POcDetails.Append(item.show_email + "&");
                            POcDetails.Append(item.show_contact_number);
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
                            model?.hiring_request_id,
                            //string.Join(",", hrModel?.HRPOCUserID),
                            pocDetailsString,
                            0,
                            true
                        };
                        string pocParamString = CommonLogic.ConvertToParamStringWithNull(pocParam);
                        _iATSsyncUTS.SaveHRPOCDetails(pocParamString);
                    }
                }
                catch
                {

                }
                #endregion

                _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);

                HRUpdateReponse hRUpdateReponse = new HRUpdateReponse();
                hRUpdateReponse.hiring_request_id = model?.hiring_request_id;
                hRUpdateReponse.response_messages = Message;

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message, Details = hRUpdateReponse });
            }
            catch (Exception ex)
            {
                Message += string.Format("[{0}]", ex.Message.ToString());
                _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, Message);
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = Message });
                throw;
            }
        }

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

        #region Add/Edit Company

        #endregion

        #region CreditTransaction

        #endregion

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
                    var data = _iATSsyncUTS.Sproc_Add_Company_Transactions_With_ATS(paramString);

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
        [HttpPost("InsertUtsAtsUnlockTalentApiDetails")]
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
        [HttpPost("UpdateUtsAtsApiUnlockTalentDetails")]
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
        

    }
}
