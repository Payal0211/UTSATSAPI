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
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.Controllers
{

    [ApiController]
    [Route("ATSsyncUTS/")]

    public class ATSsyncUTSController : ControllerBase
    {
        #region Variables
        private readonly UTSATSAPIDBConnection _db;
        private readonly IUniversalProcRunner _uniProcRunner;
        private readonly IConfiguration _configuration;
        private readonly IATSsyncUTS _iATSsyncUTS;
        #endregion

        #region Constructors
        public ATSsyncUTSController(UTSATSAPIDBConnection adminDBContext, IUniversalProcRunner uniProcRunner, IConfiguration configuration, IATSsyncUTS iATSsyncUTS)
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
                                //string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", fileName);
                                string filePath = _configuration["AdminProjectURLForUploadPath"].ToString() + "Media/JDParsing/JDFiles/" + fileName;
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
                    ToApiUrl = _configuration["ProjectURL"].ToString() + "AddEditHRThroughATS",
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
                    if (!string.IsNullOrEmpty(model.jd_file_aws_url))
                    {
                        string BucketName = _configuration["BucketName"].ToString();
                        string KeyName = _configuration["KeyName"].ToString();
                        string AccessKey = _configuration["AccessKey"].ToString();
                        string SecretKey = _configuration["SecretKey"].ToString();

                        string fileName = Path.GetFileName(model.jd_file_aws_url);

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
                                //string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", fileName);
                                string filePath = _configuration["AdminProjectURLForUploadPath"].ToString() + "Media/JDParsing/JDFiles/" + fileName;
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

                #region convert int to bool
                bool? is_confidential_budget = null;
                bool? is_fresher_allowed = null;
                bool? is_open_to_work_near_by = null;
                bool? has_people_management_exp = null;
                bool? is_vetted_profile = null;
                bool? is_hiring_limited = null;
                bool? is_transparent = null;
                bool? is_dp = null;
                bool? is_hr_focused = null;

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
                if (model?.pay_per_credit?.is_vetted_profile != null)
                {
                    is_vetted_profile = model?.pay_per_credit?.is_vetted_profile == 1 ? true : false;
                }
                if (model?.pay_per_credit?.is_hiring_limited != null)
                {
                    is_hiring_limited = model?.pay_per_credit?.is_hiring_limited == 1 ? true : false;
                }
                if (model?.pay_per_hire?.is_transparent != null)
                {
                    is_transparent = model?.pay_per_hire?.is_transparent == 1 ? true : false;
                }
                if (model?.pay_per_hire?.is_dp != null)
                {
                    is_dp = model?.pay_per_hire?.is_dp == 1 ? true : false;
                }
                if (model?.is_hr_focused != null)
                {
                    is_hr_focused = model?.is_hr_focused == 1 ? true : false;
                }
                #endregion

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
                       model?.jd_file_aws_url,
                       model?.years_of_exp,
                       is_fresher_allowed,
                       model?.no_of_talents,
                       model?.timezone_id,
                       model?.shift_start_time,
                       model?.shift_end_time,
                       model?.notice_period,
                       model?.partial_engagement_type,
                       model?.no_of_hours_working,
                       model?.duration_type,
                       model?.job_title,
                       null,
                       null,
                       null,
                       model?.must_have_skills,
                       model?.good_to_have_skills,
                       is_hr_focused,
                       is_dp,
                       model?.pay_per_hire?.dp_margin,
                       model?.pay_per_hire?.nr_margin,
                       is_transparent,
                       model?.pay_per_hire?.pricing_id,
                       model?.pay_per_hire?.payroll_type_id,
                       model?.pay_per_hire?.payroll_partner_name,
                       is_vetted_profile,
                       is_hiring_limited,
                       null,
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
                       model?.ats_logged_in_employee_id,
                       model?.created_at,
                       model?.updated_at
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                Sproc_UTS_AddEdit_ATSHR_Result result = _iATSsyncUTS.Sproc_UTS_AddEdit_ATSHR(paramString);

                if (result != null && result.HiringRequestID > 0)
                {
                    model.hiring_request_id = result.HiringRequestID;
                    model.hr_number = result.HR_Number;
                    if (!string.IsNullOrEmpty(result.ResponseMsg))
                    {
                        Message += string.Format("[{0}]", result.ResponseMsg);
                    }
                }

                #region Update Job Description With Unicode Characters
                if (!string.IsNullOrEmpty(model?.job_desciption))
                    _iATSsyncUTS.SaveStepInfoWithUnicode(model.hiring_request_id.ToString(), model?.job_desciption);
                #endregion

                #region Update Prerequisites With Unicode Characters
                if (!string.IsNullOrEmpty(model?.vital_information?.prerequisites))
                    _iATSsyncUTS.SaveperquisitesWithUnicode(model.hiring_request_id.ToString(), model?.vital_information?.prerequisites);
                #endregion

                #endregion

                #region Save HR POC users
                try
                {
                    StringBuilder POcDetails = new();
                    string pocDetailsString = string.Empty;

                    bool? show_email = null;
                    bool? show_contact_number = null;
                    if (model?.job_poc != null && model.job_poc.Any())
                    {
                        param = null;
                        foreach (var item in model.job_poc)
                        {
                            show_email = null;
                            show_contact_number = null;

                            if (item?.show_email != null)
                            {
                                show_email = item?.show_email == 1 ? true : false;
                            }
                            if (item?.show_contact_number != null)
                            {
                                show_contact_number = item?.show_contact_number == 1 ? true : false;
                            }

                            //Update contact Number in gen_contact
                            if (!string.IsNullOrEmpty(item?.contact_number) && item.contact_id > 0)
                            {
                                param = new object[] { item.contact_id, item.contact_number };
                                _uniProcRunner.ManipulationWithNULL(Constants.ProcConstant.Sproc_HR_EditPOC, param);
                            }

                            POcDetails.Append(item?.contact_id + "&");
                            POcDetails.Append(show_email + "&");
                            POcDetails.Append(show_contact_number);
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
                hRUpdateReponse.hr_number = model?.hr_number;
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
                        //string filePath = System.IO.Path.Combine("Media/JDParsing/JDFiles", fileName);
                        string filePath = _configuration["AdminProjectURLForUploadPath"].ToString() + "Media/JDParsing/JDFiles/" + fileName;
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

        [HttpPost("AddEditCompanyThroughATS")]
        public async Task<ObjectResult> AddEditCompanyThroughATS([FromBody] ATSCompanyProfileDetail updateDetails)
        {
            try
            {
                #region Variable
                long LoggedInUserId = SessionValues.LoginUserId;
                long? CompanyID = 0;
                string? CompanyNumber = string.Empty;
                bool IsNewCompany = false;
                bool? is_self_funded = null;
                short? Portal = (short)AppActionDoneBy.ATS;
                int? CompanyTypeID = null;
                int? AnotherCompanyTypeID = null;

                bool? is_post_job = null;
                bool? is_profile_view = null;
                bool? is_transparent_pricing = null;
                bool? is_vetted_profile = null;
                #endregion

                #region Validation
                if (updateDetails != null)
                {
                    if (updateDetails.basic_details == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Company Basic Details must not be empty.", Details = null });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Object must not be empty.", Details = null });
                }
                #endregion

                #region Add record in gen_UtsAts_Records
                long APIRecordInsertedID = 0;

                string EditHRJsonData = JsonConvert.SerializeObject(updateDetails);
                GenUtsAtsApiRecord utsAtsApi_Records = new()
                {
                    FromApiUrl = "ATS Edit Company",
                    ToApiUrl = _configuration["ProjectURL"].ToString() + "AddEditCompanyThroughATS",
                    PayloadToSend = EditHRJsonData,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.Now,
                    HrId = 0
                };

                APIRecordInsertedID = _iATSsyncUTS.InsertUtsAtsApiDetails(utsAtsApi_Records);
                #endregion

                #region Variable Assignment

                if (updateDetails?.engagement_details != null)
                {                  

                    if (updateDetails?.engagement_details?.is_pay_per_hire == 1)
                    {
                        CompanyTypeID = 1;
                    }
                    if (updateDetails?.engagement_details?.is_pay_per_credit == 1)
                    {
                        AnotherCompanyTypeID = 2;
                    }
                    if (updateDetails?.engagement_details?.is_post_job != null)
                    {
                        is_post_job = updateDetails?.engagement_details?.is_post_job == 1 ? true : false;
                    }
                    if (updateDetails?.engagement_details?.is_profile_view != null)
                    {
                        is_profile_view = updateDetails?.engagement_details?.is_profile_view == 1 ? true : false;
                    }
                    if (updateDetails?.engagement_details?.is_transparent_pricing != null)
                    {
                        is_transparent_pricing = updateDetails?.engagement_details?.is_transparent_pricing == 1 ? true : false;
                    }
                    if (updateDetails?.engagement_details?.is_vetted_profile != null)
                    {
                        is_vetted_profile = updateDetails?.engagement_details?.is_vetted_profile == 1 ? true : false;
                    }
                }
                #endregion

                #region 1) ADD/Update Company & Company Basic Details -- Sproc_Update_Basic_CompanyDetails
                if (updateDetails.basic_details != null)
                {
                    string CompanyLogo = null;
                    CompanyID = updateDetails.basic_details.company_id;

                    if (updateDetails?.basic_details?.is_self_funded != null)
                    {
                        is_self_funded = updateDetails?.basic_details?.is_self_funded == 1 ? true : false;
                    }

                    object[] param = new object[]
                    {
                            CompanyID,
                            updateDetails?.basic_details?.company_name,
                            updateDetails?.basic_details?.founded_year,
                            //updateDetails?.basic_details?.company_size,
                            null,
                            updateDetails?.basic_details?.website_url,
                            //updateDetails?.basic_details?.CompanyType,
                            string.Empty,
                            updateDetails?.basic_details?.industry,
                            updateDetails?.basic_details?.headquaters,
                            updateDetails?.basic_details?.about_company_desc != null ? null : updateDetails?.basic_details?.about_company_desc,
                            updateDetails?.basic_details?.culture != null ? null : updateDetails?.basic_details?.culture,
                            LoggedInUserId,
                            CompanyLogo,
                            is_self_funded,
                            Portal,
                            updateDetails?.basic_details?.linkedin_profile,
                            updateDetails?.basic_details?.teamsize,
                            updateDetails?.basic_details?.company_number,
                            updateDetails.basic_details?.company_logo  // company_logo_aws_url , always come aws url from ats
                    };

                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    Sproc_Update_Basic_CompanyDetails_Result result =
                    _iATSsyncUTS.UpdateCompanyBasicDetails(paramString);

                    if (result != null && result.CompanyID > 0)
                    {
                        CompanyID = result.CompanyID;
                        CompanyNumber = result.CompanyNumber;

                        #region Update Company Details About desc With Unicode Characters
                        if (!string.IsNullOrEmpty(updateDetails?.basic_details?.about_company_desc))
                            _iATSsyncUTS.SaveCompanyDescUnicode(CompanyID ?? 0, updateDetails?.basic_details?.about_company_desc, LoggedInUserId);
                        #endregion

                        #region Update  Culture With Unicode Characters
                        if (!string.IsNullOrEmpty(updateDetails?.basic_details?.culture))
                            _iATSsyncUTS.SaveCultureDetailUnicode(CompanyID ?? 0, updateDetails?.basic_details?.culture, LoggedInUserId);
                        #endregion

                    }

                }
                #endregion

                #region 2) Update Funding Details -- Sproc_Add_Company_Funding_Details

                if (!Convert.ToBoolean(is_self_funded) && updateDetails?.funding_details != null)
                {
                    //delete existing then insert new entry
                    object[] param = new object[]
                    {
                            CompanyID,
                            0,
                            LoggedInUserId,
                            Portal,
                        };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iATSsyncUTS.Delete_Company_Funding_Details(paramString);

                    foreach (var item in updateDetails.funding_details)
                    {
                        param = new object[]
                        {
                                CompanyID,
                                item?.funding_amount,
                                item?.funding_round,
                                item?.series,
                                item?.month,
                                item?.year,
                                item?.investors,
                                LoggedInUserId,
                                Portal,
                                0,
                                item?.additional_information != null ? null : item?.additional_information
                        };
                        paramString = CommonLogic.ConvertToParamStringWithNull(param);
                        _iATSsyncUTS.Sproc_Add_Company_Funding_Details_Result(paramString);

                        #region Update AddiInfo With Unicode Characters
                        if (!string.IsNullOrEmpty(item?.additional_information))
                            _iATSsyncUTS.SaveAdditionalInfoUnicode(CompanyID ?? 0, item?.additional_information, LoggedInUserId);
                        #endregion
                    }
                }
                #endregion

                #region 3) Add Culture Image -- Sproc_Add_Company_CultureandPerksDetails
                if (updateDetails?.culture_details != null)
                {
                    //delete existing then insert new entry
                    object[] param = new object[]
                            {
                                CompanyID,
                                0,
                                LoggedInUserId,
                                Portal
                            };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iATSsyncUTS.Delete_Company_CultureandPerksDetails(paramString);

                    foreach (var item in updateDetails.culture_details)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            param = new object[]
                            {
                                CompanyID,
                                null,
                                LoggedInUserId,
                                Portal,
                                0,
                                item
                            };
                            paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iATSsyncUTS.Sproc_Add_Company_CultureandPerksDetails_Result(paramString);
                        }
                    }
                }
                #endregion

                #region 4) Update Perk Details -- Sproc_Add_Company_PerksDetails
                if (updateDetails?.perk_details != null && updateDetails.perk_details.Any())
                {
                    string PerksString = string.Join(",", updateDetails.perk_details);
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
                        _iATSsyncUTS.Sproc_Add_Company_PerksDetails_Result(paramString);
                    }
                }
                #endregion

                #region 5) Add YouTube Details -- Sproc_Add_YoutubeLink
                if (updateDetails?.youtube_links != null)
                {
                    //delete existing then insert new entry
                    object[] param = new object[]
                    {
                            CompanyID,
                            0,
                            LoggedInUserId,
                            Portal,
                        };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iATSsyncUTS.Delete_Company_YoutubeDetails(paramString);

                    foreach (var item in updateDetails.youtube_links)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            param = new object[]
                            {
                                CompanyID,
                                item,
                                LoggedInUserId,
                                Portal,
                                0
                            };
                            paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            _iATSsyncUTS.Sproc_Add_YoutubeLink(paramString);
                        }
                    }
                }
                #endregion

                #region 6) Add/Update Contact(Client) Details  -- sproc_UTS_UpdateContactDetails
                List<SummaryClientDetails> summaryClients = new List<SummaryClientDetails>();
                if (updateDetails?.client_details != null)
                {
                    foreach (var item in updateDetails.client_details)
                    {
                        #region need to identify it is new client or existing client

                        long? ClientID = 0;
                        bool IsNewClient = true;

                        if (item.client_id > 0)
                        {
                            ClientID = item.client_id;
                            IsNewClient = false;
                        }

                        #endregion

                        #region SP call : sproc_UTS_UpdateContactDetails
                        object[] param = new object[]
                        {
                                 CompanyID,
                                 ClientID,
                                 item?.full_Name,
                                 item?.email_Id,
                                 item?.designation,
                                 item?.access_role_id,
                                 null,
                                 item?.phone_number,
                                 LoggedInUserId,
                                 Portal,
                                 null,
                                 null,
                                 CompanyTypeID,
                                 AnotherCompanyTypeID
                        };
                        string paramString = CommonLogic.ConvertToParamStringWithNull(param);

                        sproc_UTS_UpdateContactDetails_Result result = _iATSsyncUTS.UpdateClientDetails(paramString);

                        #endregion

                        #region Summary details
                        SummaryClientDetails summaryClient = new SummaryClientDetails();
                        if (IsNewClient && result != null && result.ContactID > 0)
                            summaryClient.client_id = result.ContactID;
                        else
                            summaryClient.client_id = ClientID;
                        summaryClient.client_email = item.email_Id;
                        summaryClient.is_newly_added = IsNewClient;
                        summaryClients.Add(summaryClient);
                        #endregion
                    }
                }
                #endregion

                #region 7) Update Company Engengement Details  -- Sproc_Update_Company_EngagementDetails
                if (updateDetails?.engagement_details != null)
                {
                    object[] param = new object[]
                    {
                            CompanyID,
                            CompanyTypeID,
                            AnotherCompanyTypeID,
                            is_post_job,
                            is_profile_view,
                            updateDetails?.engagement_details?.free_credit,
                            is_transparent_pricing,
                            is_vetted_profile,
                            updateDetails?.engagement_details?.credit_amount,
                            updateDetails?.engagement_details?.credit_currency,
                            updateDetails?.engagement_details?.job_post_credit,
                            null,
                            null,
                            updateDetails?.engagement_details?.hiring_type_pricingid,
                            LoggedInUserId,
                            Portal
                    };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iATSsyncUTS.UpdateCompanyEngagementDetails(paramString);
                }
                #endregion

                #region 8) Update Company POC Details  -- sproc_UTS_UpdatePOCUserIDsByCompanyID
                if (!string.IsNullOrEmpty(updateDetails?.poc_id))
                {
                    object[] param = new object[]
                            {
                                CompanyID,                              
                                0,
                                LoggedInUserId,
                                0,
                                "",
                                "Add",
                                updateDetails?.poc_id
                             };
                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                    _iATSsyncUTS.DeleteInsertPOCDetails(paramString);
                }
                #endregion

                #region return summary object
                SummaryDetails summary_details = new SummaryDetails();
                summary_details.company_id = CompanyID;
                summary_details.company_number = CompanyNumber;
                summary_details.company_name = updateDetails?.basic_details?.company_name;
                summary_details.summary_clients = summaryClients;
                #endregion

                try
                {
                    string ResponseJsonData = JsonConvert.SerializeObject(summary_details);
                    _iATSsyncUTS.UpdateUtsAtsApiDetails(APIRecordInsertedID, ResponseJsonData);
                }
                catch (Exception)
                {
                    throw;
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Successfully Add/Updated Company profile details", Details = summary_details });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region CreditTransaction

        #endregion

        #region DeleteTalents

        [HttpPost("RemoveTalentsFromUTSViaATS")]
        public async Task<ObjectResult> RemoveTalentsFromUTSViaATS(RemoveTalentFromATSViaATS model)
        {
            long APIRecordInsertedID = 0;
            string Message = "";
            long hrId = 0;
            try
            {
                #region Validation
                if (model == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }
                if (string.IsNullOrEmpty(model.HR_Number))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please pass proper HrId" });
                }
                #endregion

                #region GetHiringDetails
                var genSalesHiringRequest = await _iATSsyncUTS.GetHiringRequestbyNumber(model.HR_Number);
                if (genSalesHiringRequest != null)
                {
                    hrId = Convert.ToInt64(genSalesHiringRequest.Id);
                }
                #endregion

                #region Add record in gen_UtsAts_Records
                string EditHRJsonData = JsonConvert.SerializeObject(model);
                GenUtsAtsApiRecord utsAtsApi_Records = new()
                {
                    FromApiUrl = "ATS Delete Talent",
                    ToApiUrl = Convert.ToString(_configuration["ProjectURL"]) + "RemoveTalentsFromUTSViaATS",
                    PayloadToSend = EditHRJsonData,
                    CreatedById = 0,
                    CreatedByDateTime = DateTime.Now,
                    HrId = hrId
                };

                APIRecordInsertedID = _iATSsyncUTS.InsertUtsAtsApiDetails(utsAtsApi_Records);
                #endregion               

                #region SP call
                

                object[] param = new object[]
                {
                       model?.HR_Number,
                       model?.TalentEmail                       
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_RemoveMatchmakeTalentFromUTSViaATS, paramString));

                #endregion

                #region Emails

                EmailBinder emailBinder = new EmailBinder(_configuration, _db);
                emailBinder.SendEmailForHRDeleteToInternalTeam(genSalesHiringRequest,model?.ActionDoneBy, model?.TalentEmail);

                #endregion

                Message += "[Talent removed successfully by ATS]";
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
                       //json.credit_type,
                       json.freecredit_used,
                       json.paidcredit_used,
                       json.credit_amount,
                       json.credit_currency,
                       json.action_type,
                       json.balance_credit,
                       json.employee_id,
                       json.transactiondoneby,
                       json.transaction_date,
                       "",
                       json.order_amount,
                       json.payment_provider,
                       json.payment_status,
                       json.payer_name,
                       json.payer_email,
                       json.payer_id,
                       json.order_comments,
                       json.invoice_url,
                       json.invoice_name
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
