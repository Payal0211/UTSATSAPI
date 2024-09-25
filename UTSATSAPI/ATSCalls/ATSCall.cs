using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;
using UTSATSAPI.Helpers;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using static UTSATSAPI.Helpers.Enum;

namespace UTSATSAPI.ATSCalls
{
    public class ATSCall
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly TalentConnectAdminDBContext _talentConnectAdminDBContext;
        #endregion

        #region Constructors
        public ATSCall(IConfiguration configuration, TalentConnectAdminDBContext talentConnectAdminDBContext)
        {
            _configuration = configuration;
            _talentConnectAdminDBContext = talentConnectAdminDBContext;
        }
        #endregion

        #region Public Methods
        //public void SendInterviewDetailsWithSlot(string json, long loggedinuserid = 0, long hrId = 0)
        //{
        //    string apiType = "SendInterviewDetailsWithSlot";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendInterviewDetailsWithSlot";
        //    string payloads = json;

        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;

        //        loggedinuserid = SessionValues.LoginUserId;

        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord
        //        {
        //            FromApiUrl = _configuration["ProjectURL_API"] + "SendInterviewDetailsWithSlot",
        //            ToApiUrl = _configuration["ATS_API_URL"] + "hr/saveInterviewSlot",
        //            PayloadToSend = json,
        //            CreatedById = loggedinuserid,
        //            HrId = hrId
        //        };

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "hr/saveInterviewSlot";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];


        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;
        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;

        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }

        //}

        //public void SaveContactTalentPriority(string json, long loggedinuserid, long hrId = 0)
        //{
        //    // UTS-7364: If user is logged in demo account than do not send the talent status update to ATS.
        //    bool isDemoHR = false;
        //    try
        //    {
        //        Sproc_Identify_HRAssociated_WithDemoAccount_Result? data = _talentConnectAdminDBContext.Set<Sproc_Identify_HRAssociated_WithDemoAccount_Result>().FromSqlRaw(String.Format("{0} {1}",
        //            Constants.ProcConstant.Sproc_Identify_HRAssociated_WithDemoAccount, hrId.ToString())).AsEnumerable().FirstOrDefault();

        //        if (data != null)
        //        {
        //            if (Convert.ToString(data.CompanyID) == _configuration["DemoCompanyID"])
        //            {
        //                isDemoHR = true;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        isDemoHR = false;
        //    }

        //    if (!isDemoHR)
        //    {

        //        string apiType = "SaveContactTalentPriority";
        //        string endPoint = _configuration["ProjectURL_API"] + "SaveContactTalentPriority";
        //        string payloads = json;
        //        try
        //        {
        //            loggedinuserid = SessionValues.LoginUserId;
        //            dynamic returnObj = new ExpandoObject();
        //            dynamic responseJObject = null;
        //            GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();
        //            utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API"] + "SaveContactTalentPriority";
        //            utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "hr/updateStatusHRTalent";
        //            utsAtsApi_Records.PayloadToSend = json;
        //            utsAtsApi_Records.CreatedById = loggedinuserid;
        //            utsAtsApi_Records.HrId = hrId;

        //            var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //            var TMD_APIURL = _configuration["ATS_API_URL"] + "hr/updateStatusHRTalent";
        //            var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //            string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //            bool isSuccess = false;
        //            if (!string.IsNullOrEmpty(response))
        //            {
        //                response = response.TrimStart();

        //                responseJObject = JsonConvert.DeserializeObject<dynamic>(response);

        //                if (responseJObject != null)
        //                {
        //                    var responseValue = responseJObject.status;
        //                    if (responseValue == "200")
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }

        //                if (isSuccess)
        //                {
        //                    returnObj.Status = 201;
        //                    returnObj.Message = "Success";
        //                }
        //                else
        //                {
        //                    returnObj.Status = 401;
        //                    returnObj.Message = "Error in getting response";
        //                }
        //            }

        //            if (responseJObject != null)
        //            {
        //                GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //                if (records != null)
        //                {
        //                    records.ResponseReceived = Convert.ToString(responseJObject);
        //                    CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //                }
        //            }

        //            // Save Interview reject or screen reject
        //            try
        //            {
        //                var rejectionPayload = JsonConvert.DeserializeObject<ContactTalentPriorityResponseModel>(json);
        //                if (rejectionPayload != null)
        //                {
        //                    if (rejectionPayload.TalentDetails != null)
        //                    {
        //                        var talentDetails = rejectionPayload.TalentDetails;
        //                        if (talentDetails != null)
        //                        {
        //                            string status = talentDetails[0].TalentStatus;
        //                            if (status == "Rejected")
        //                            {
        //                                var atsResponse = responseJObject;
        //                                if (atsResponse != null)
        //                                {
        //                                    string rejectionStage = Convert.ToString(atsResponse.RejectionStage);
        //                                    string hiringRequestId = Convert.ToString(rejectionPayload.HRID);
        //                                    string talentId = Convert.ToString(talentDetails[0].UTS_TalentID);

        //                                    object[] param = new object[]
        //                                    {
        //                                                    hiringRequestId,
        //                                                    talentId,
        //                                                    rejectionStage
        //                                    };
        //                                    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
        //                                    _talentConnectAdminDBContext.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_UpdateRejectionStage, paramString));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            catch
        //            {

        //            }

        //            returnObj.Response = json;

        //            var returnJson = new
        //            {
        //                returnObj.Status,
        //                returnObj.Message,
        //                returnObj.Response
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //            emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //        }
        //    }
        //}

        //public string SendReplacementDataToInvoice(ArchiveInvoice archiveInvoice)
        //{
        //    string InvoiceData = JsonConvert.SerializeObject(archiveInvoice);

        //    string apiType = "SendReplacementDataToInvoice";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendReplacementDataToInvoice";
        //    string payloads = InvoiceData;
        //    try
        //    {
        //        var InvoiceAPIKey = _configuration["Invoice_APIKey"];
        //        var InvoiceDataPost_APIURL = _configuration["InvoiceDataPost_APIURL"];
        //        string url = InvoiceDataPost_APIURL + "UTSInvoiceAPI/ArchiveInvoiceForReplacementandContractEnd";
        //        string response = CommonLogic.CoreHttpCallsPost(url, InvoiceData, InvoiceAPIKey);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //        return "";
        //    }
        //}

        //public void SendMatchMakingRecords(string json, long loggedinuserid = 0, long hrId = 0)
        //{
        //    string apiType = "SendMatchMakingRecords";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendMatchMakingRecords";
        //    string payloads = json;

        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        loggedinuserid = SessionValues.LoginUserId;

        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord
        //        {
        //            FromApiUrl = _configuration["ProjectURL_API"] + "SendMatchMakingRecords",
        //            ToApiUrl = _configuration["ATS_API_URL"] + "hr/storeHrTalent",
        //            PayloadToSend = json,
        //            CreatedById = loggedinuserid,
        //            HrId = hrId
        //        };

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "hr/storeHrTalent";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;

        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();
        //            JObject responseJObject = null;
        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }

        //            if (responseJObject != null)
        //            {
        //                GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //                if (records != null)
        //                {
        //                    records.ResponseReceived = Convert.ToString(responseJObject);
        //                    CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }

        //}

        //public bool SendHRDataToPMS(string HRDetails_Json, long hrId = 0)
        //{
        //    string apiType = "SendHRDataToPMS";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendHRDataToPMS";
        //    var PMS_APIURL = _configuration["HRDetailsPassAPIURL"].ToString();
        //    string payloads = HRDetails_Json;
        //    try
        //    {
        //        long APIRecordInsertedID = 0;

        //        #region Add record in gen_UtsAts_Records

        //        GenUtsAtsApiRecord utsAtsApi_Records = new()
        //        {
        //            FromApiUrl = endPoint, //"UTS Add/Edit HR URL",
        //            //ToApiUrl = _configuration["ProjectURL_API"].ToString() + "SendHRDataToPMS",
        //            ToApiUrl = PMS_APIURL,
        //            PayloadToSend = HRDetails_Json,
        //            CreatedById = SessionValues.LoginUserId,
        //            CreatedByDateTime = DateTime.Now,
        //            HrId = hrId
        //        };

        //        APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        #endregion

        //        bool isSuccess = false;

        //        var PMS_APIKey = _configuration["TalentDetailsPassAPIKey"].ToString();

        //        string talentdetailsUrl = string.Format(PMS_APIURL);

        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //        ServicePointManager.DefaultConnectionLimit = 9999;

        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(PMS_APIURL);
        //        if (webRequest != null)
        //        {

        //            webRequest.Method = "POST";
        //            webRequest.Timeout = 500000;
        //            webRequest.ContentType = "application/json";
        //            webRequest.Headers.Add("authorization", PMS_APIKey);
        //            webRequest.Credentials = CredentialCache.DefaultCredentials;

        //            webRequest.MaximumAutomaticRedirections = 4;
        //            webRequest.MaximumResponseHeadersLength = 4;
        //            webRequest.UserAgent = "Mozilla/5.0 (Taco2) Gecko/20100101";

        //            using (var requestWriter = new StreamWriter(webRequest.GetRequestStream()))
        //            {
        //                requestWriter.Write(HRDetails_Json);
        //                requestWriter.Flush();
        //                requestWriter.Close();
        //            }
        //        }

        //        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        //        Thread.Sleep(1000);
        //        Stream resStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(resStream);
        //        string ResponseJson = reader.ReadToEnd();

        //        if (!string.IsNullOrEmpty(ResponseJson))
        //        {
        //            ResponseJson = ResponseJson.TrimStart();

        //            #region Update record in gen_UtsAts_Records

        //            utsAtsApi_Records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.Where(x => x.Id == APIRecordInsertedID).FirstOrDefault();
        //            if (utsAtsApi_Records != null)
        //            {
        //                utsAtsApi_Records.ResponseReceived = ResponseJson;
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, utsAtsApi_Records, EntityState.Modified);
        //            }

        //            #endregion

        //            JObject ResponseJObject = JObject.Parse(ResponseJson);
        //            if (ResponseJObject.Count > 0)
        //            {
        //                dynamic Response = ResponseJObject;
        //                if (ResponseJObject.HasValues)
        //                {
        //                    var responseValue = ResponseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //        }

        //        return isSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //        return false;
        //    }
        //}

        //public void SendHrStatusToATS(string json, long loggedinuserid, long hrId = 0)
        //{
        //    string apiType = "SendHrStatusToATS";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendHrStatusToATS";
        //    string payloads = json;
        //    try
        //    {
        //        loggedinuserid = SessionValues.LoginUserId;
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;
        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();
        //        utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API"] + "SendHrStatusToATS";
        //        utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "updateHrStatus";
        //        utsAtsApi_Records.PayloadToSend = json;
        //        utsAtsApi_Records.CreatedById = loggedinuserid;
        //        utsAtsApi_Records.HrId = hrId;

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "updateHrStatus";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;
        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;

        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }
        //}

        //public bool SendAddEditCompanyData(string jsonData, long LoggedInUserId = 0)
        //{
        //    string apiType = "SendAddEditCompanyData";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendAddEditCompanyData";
        //    string payloads = jsonData;
        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;
        //        LoggedInUserId = SessionValues.LoginUserId;
        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();

        //        utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API "] + "SendAddUpdateCompanyData";
        //        utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "client-company";
        //        utsAtsApi_Records.PayloadToSend = jsonData;
        //        utsAtsApi_Records.CreatedById = LoggedInUserId;
        //        utsAtsApi_Records.HrId = 0;

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var SAECD_APIURL = _configuration["ATS_API_URL"] + "client-company";
        //        var SAECD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(SAECD_APIURL, jsonData, SAECD_APIKey);

        //        bool isSuccess = false;

        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 200;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //        return false;
        //    }
        //}


        ///// <summary>
        ///// Sends the automatic cancelled talent hr status to ats.
        ///// </summary>
        ///// <param name="json">The json.</param>
        ///// <param name="LoggedInUserID">The logged in user identifier.</param>
        //public void SendAutoCancelledTalentHRStatusToATS(string json, long LoggedInUserID, long hrId = 0)
        //{
        //    string apiType = "SendAutoCancelledTalentHRStatusToATS";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendAutoCancelledTalentHRStatusToATS";
        //    string payloads = json;
        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;
        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();

        //        utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API "] + "SendAutoCancelledTalentHRStatusToATS";
        //        utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "hr/talentHrAutoCancel";
        //        utsAtsApi_Records.PayloadToSend = json;
        //        utsAtsApi_Records.CreatedById = LoggedInUserID;
        //        utsAtsApi_Records.HrId = hrId;

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "hr/talentHrAutoCancel";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(TMD_APIURL);
        //        if (webRequest != null)
        //        {
        //            webRequest.Method = "POST";
        //            webRequest.Timeout = 500000;
        //            webRequest.ContentType = "application/json";
        //            webRequest.Headers.Add("authorization", TMD_APIKey);
        //            webRequest.Credentials = CredentialCache.DefaultCredentials;

        //            using (var requestWriter = new StreamWriter(webRequest.GetRequestStream()))
        //            {
        //                requestWriter.Write(json);
        //                requestWriter.Flush();
        //                requestWriter.Close();
        //            }
        //        }

        //        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        //        Thread.Sleep(1000);
        //        Stream resStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(resStream);
        //        string ResponseJson = reader.ReadToEnd();

        //        bool isSuccess = false;

        //        if (!string.IsNullOrEmpty(ResponseJson))
        //        {
        //            ResponseJson = ResponseJson.TrimStart();

        //            responseJObject = JObject.Parse(ResponseJson);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;
        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }
        //}

        ////Update TR to ATS
        //public void SendUpdatedTRtoATS(string json, long loggedinuserid, long hrId = 0)
        //{
        //    string apiType = "SendUpdatedTRtoATS";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendUpdatedTRtoATS";
        //    string payloads = json;
        //    try
        //    {
        //        loggedinuserid = SessionValues.LoginUserId;
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;
        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();
        //        utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API"] + "SendUpdatedTRtoATS";
        //        utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "hr/trUpdation";
        //        utsAtsApi_Records.PayloadToSend = json;
        //        utsAtsApi_Records.CreatedById = loggedinuserid;
        //        utsAtsApi_Records.HrId = hrId;

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "hr/trUpdation";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;
        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;

        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }
        //}

        //public void SendPayrateBillratetoATS(string json, long LoggedInUserId, long hrId = 0)
        //{
        //    string apiType = "SendPayrateBillratetoATS";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendPayrateBillratetoATS";
        //    string payloads = json;
        //    try
        //    {
        //        LoggedInUserId = SessionValues.LoginUserId;
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;
        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();
        //        utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API"] + "SendPayrateBillratetoATS";
        //        utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "updateTalentHRPayrate";
        //        utsAtsApi_Records.PayloadToSend = json;
        //        utsAtsApi_Records.CreatedById = LoggedInUserId;
        //        utsAtsApi_Records.HrId = hrId;

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "updateTalentHRPayrate";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;
        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;

        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }
        //}

        //public void CallAtsAPIToSendTalentAndHRStatus(long UTSTalentID, long LoggedInUserID, long HiringRequestID, string RejectedBy = "")
        //{

        //    ContactTalentPriorityResponseModel contactTalentPriorityResponseModel = new ContactTalentPriorityResponseModel();
        //    string TalentStatus = "";
        //    GenTalent _Talent = _talentConnectAdminDBContext.GenTalents.Where(x => x.Id == UTSTalentID).FirstOrDefault();

        //    if (_Talent != null)
        //    {
        //        var HiringRequestData = _talentConnectAdminDBContext.GenSalesHiringRequests.Where(x => x.Id == HiringRequestID).FirstOrDefault();
        //        var ContactTalentPriorityDetails = _talentConnectAdminDBContext.GenContactTalentPriorities.Where(x => x.TalentId == _Talent.Id && x.HiringRequestId == HiringRequestData.Id).FirstOrDefault();
        //        _talentConnectAdminDBContext.Entry(ContactTalentPriorityDetails).Reload();
        //        if (HiringRequestData != null)
        //        {
        //            #region Save Data in model to send reponse to PHP team after serialize                        
        //            contactTalentPriorityResponseModel.HRID = HiringRequestID;
        //            contactTalentPriorityResponseModel.HRStatusID = HiringRequestData.StatusId ?? 0;

        //            var HRStatusData = _talentConnectAdminDBContext.PrgHiringRequestStatuses.Where(x => x.Id == contactTalentPriorityResponseModel.HRStatusID).FirstOrDefault();
        //            if (HRStatusData != null)
        //                contactTalentPriorityResponseModel.HRStatus = HRStatusData.HiringRequestStatus;

        //            outTalentDetail talentDetail = new outTalentDetail();
        //            talentDetail.ATS_TalentID = _Talent.AtsTalentId ?? 0;

        //            talentDetail.TalentStatus = GetTalentStatus(HiringRequestData, ContactTalentPriorityDetails);
        //            talentDetail.UTS_TalentID = _Talent.Id;
        //            talentDetail.Talent_USDCost = _Talent.FinalCost ?? 0;

        //            object[] objParam = new object[] { contactTalentPriorityResponseModel.HRID, _Talent.Id };
        //            string strParamas = CommonLogic.ConvertToParamString(objParam);

        //            //var varTalent_RejectReason = sproc_UTS_get_HRTalentProfileReason(strParamas).ActualReason;
        //            //talentDetail.Talent_RejectReason = varTalent_RejectReason;

        //            var varTalent_RejectReason = sproc_UTS_get_HRTalentProfileReason(strParamas);

        //            string? actualReason = string.Empty;
        //            string? rejectionComments = string.Empty;
        //            string? rejectionMessageToTalent = string.Empty;

        //            if (varTalent_RejectReason != null)
        //            {
        //                actualReason = Convert.ToString(varTalent_RejectReason.ActualReason);
        //                rejectionComments = Convert.ToString(varTalent_RejectReason.RejectionComments);
        //                rejectionMessageToTalent = Convert.ToString(varTalent_RejectReason.RejectionMessageToTalents);
        //            }

        //            talentDetail.Talent_RejectReason = actualReason;
        //            talentDetail.RejectionComments = rejectionComments;
        //            talentDetail.RejectionMessageForTalent = rejectionMessageToTalent;

        //            try
        //            {
        //                if (LoggedInUserID > 0)
        //                {
        //                    UsrUser? userDetails = _talentConnectAdminDBContext.UsrUsers.Where(x => x.Id == LoggedInUserID).FirstOrDefault();
        //                    if (userDetails != null)
        //                    {
        //                        talentDetail.ActionUserName = userDetails.FullName;
        //                        talentDetail.ActionUserEmail = userDetails.EmailId;
        //                        talentDetail.RejectedBy = RejectedBy;
        //                        talentDetail.ActionBy = Convert.ToString(StatusChangeAction.Sales);
        //                    }
        //                }

        //            }
        //            catch
        //            {

        //            }

        //            try
        //            {
        //                // UTS-7093: Fetch the round details and send to ATS.
        //                object[] param = new object[] { HiringRequestID, 0, UTSTalentID };
        //                string paramString = CommonLogic.ConvertToParamString(param);
        //                sproc_Get_InterviewRoundDetails_Result roundDetails = sproc_Get_InterviewRoundDetails(paramString);
        //                if (roundDetails != null)
        //                {
        //                    string? talentStatusRoundDetails = roundDetails.StrInterviewRound;
        //                    if (!string.IsNullOrEmpty(talentStatusRoundDetails))
        //                    {
        //                        talentDetail.TalentStatusRoundDetails = talentStatusRoundDetails;
        //                    }
        //                }
        //            }
        //            catch
        //            {

        //            }

        //            contactTalentPriorityResponseModel.TalentDetails.Add(talentDetail);
        //            #endregion

        //            try
        //            {
        //                var json = JsonConvert.SerializeObject(contactTalentPriorityResponseModel);
        //                SaveContactTalentPriority(json, LoggedInUserID, HiringRequestID);
        //            }
        //            catch (Exception)
        //            {

        //            }
        //        }

        //    }
        //}

        ////for get talent status
        //public string GetTalentStatus(GenSalesHiringRequest HRdata, GenContactTalentPriority CTPdata)
        //{
        //    string TalentStatus = "";
        //    bool IsPayPerHire = false;
        //    bool IsPayPerCredit = false;
        //    if (HRdata != null)
        //    {
        //        if (HRdata.HrtypeId == (short)PayPerHire.SalesHR)
        //        {
        //            IsPayPerHire = true;
        //        }
        //        if (HRdata.HrtypeId == (short)PayPerCredit.PostaJobViewBasedCredit || HRdata.HrtypeId == (short)PayPerCredit.PostaJobCreditBased || HRdata.HrtypeId == (short)PayPerCredit.PostajobWithViewCreditsButnoJobPostCredits)
        //        {
        //            IsPayPerCredit = true;
        //        }

        //        if (IsPayPerHire)
        //        {
        //            if (CTPdata != null && CTPdata.TalentStatusIdBasedOnHr > 0)
        //            {
        //                var TalentStatusMasterDetails = _talentConnectAdminDBContext.PrgTalentStatusAfterClientSelections.Where(x => x.Id == CTPdata.TalentStatusIdBasedOnHr).FirstOrDefault();
        //                if (TalentStatusMasterDetails != null)
        //                {
        //                    TalentStatus = TalentStatusMasterDetails.TalentStatus ?? "";
        //                }
        //            }
        //        }

        //        if (IsPayPerCredit)
        //        {
        //            if (CTPdata != null && CTPdata.TalentStatusIdClientPortal > 0)
        //            {
        //                var TalentStatusMasterDetails = _talentConnectAdminDBContext.PrgTalentStatusClientPortals.Where(x => x.Id == CTPdata.TalentStatusIdClientPortal).FirstOrDefault();
        //                if (TalentStatusMasterDetails != null)
        //                {
        //                    TalentStatus = TalentStatusMasterDetails.TalentStatus ?? "";
        //                }
        //            }
        //        }
        //    }
        //    return TalentStatus;
        //}

        //public void SendUserDetails(string json, long loggedinuserid = 0, long hrId = 0)
        //{
        //    string apiType = "SendUserDetails";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendUserDetails";
        //    string payloads = json;

        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;

        //        loggedinuserid = SessionValues.LoginUserId;

        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord
        //        {
        //            FromApiUrl = _configuration["ProjectURL_API"] + "SendUserDetails",
        //            ToApiUrl = _configuration["ATS_API_URL"] + "user/storeUserFromUTS",
        //            PayloadToSend = json,
        //            CreatedById = loggedinuserid,
        //            HrId = hrId
        //        };

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "user/storeUserFromUTS";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];


        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;
        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;

        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }

        //}
        //public void SendTalentEngagementDetails(string json, long loggedinuserid = 0, long? hrId = 0)
        //{
        //    string apiType = "SendTalentEngagementDetails";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendTalentEngagementDetails";
        //    string payloads = json;

        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        loggedinuserid = SessionValues.LoginUserId;

        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord
        //        {
        //            FromApiUrl = _configuration["ProjectURL_API"] + "SendTalentEngagementDetails",
        //            ToApiUrl = _configuration["ATS_API_URL"] + "storeTalentEngagement",
        //            PayloadToSend = json,
        //            CreatedById = loggedinuserid,
        //            HrId = hrId
        //        };

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "storeTalentEngagement";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;

        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();
        //            JObject responseJObject = null;
        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }

        //            if (responseJObject != null)
        //            {
        //                GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //                if (records != null)
        //                {
        //                    records.ResponseReceived = Convert.ToString(responseJObject);
        //                    CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }

        //}

        //public void SendResetAllDemoHRRequest(string json, long loggedinuserid = 0, long? hrId = 0)
        //{
        //    string apiType = "SendResetAllDemoHRRequest";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendResetAllDemoHRRequest";
        //    string payloads = json;

        //    try
        //    {
        //        dynamic returnObj = new ExpandoObject();
        //        loggedinuserid = SessionValues.LoginUserId;

        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord
        //        {
        //            FromApiUrl = _configuration["ProjectURL_API"] + "SendResetAllDemoHRRequest",
        //            ToApiUrl = _configuration["ATS_API_URL"] + "reset_demo_account",
        //            PayloadToSend = json,
        //            CreatedById = loggedinuserid,
        //            HrId = hrId
        //        };

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "reset_demo_account";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;

        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();
        //            JObject responseJObject = null;
        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }

        //            if (responseJObject != null)
        //            {
        //                GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //                if (records != null)
        //                {
        //                    records.ResponseReceived = Convert.ToString(responseJObject);
        //                    CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }

        //}

        //public void SendDeleteHRToATS(string json, long loggedinuserid, long hrId = 0)
        //{
        //    string apiType = "SendDeleteHRToATS";
        //    string endPoint = _configuration["ProjectURL_API"] + "SendDeleteHRToATS";
        //    string payloads = json;

        //    //https://ats.uplers.com/api/hr/delete-test-hr

        //    try
        //    {
        //        loggedinuserid = SessionValues.LoginUserId;
        //        dynamic returnObj = new ExpandoObject();
        //        JObject responseJObject = null;
        //        GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();
        //        utsAtsApi_Records.FromApiUrl = _configuration["ProjectURL_API"] + "SendDeleteHRToATS";
        //        utsAtsApi_Records.ToApiUrl = _configuration["ATS_API_URL"] + "hr/delete-test-hr";
        //        utsAtsApi_Records.PayloadToSend = json;
        //        utsAtsApi_Records.CreatedById = loggedinuserid;
        //        utsAtsApi_Records.HrId = hrId;

        //        var APIRecordInsertedID = InsertUtsAtsApiDetails(utsAtsApi_Records);

        //        var TMD_APIURL = _configuration["ATS_API_URL"] + "hr/delete-test-hr";
        //        var TMD_APIKey = _configuration["TalentDetailsPassAPIKey"];

        //        string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, TMD_APIKey);

        //        bool isSuccess = false;
        //        if (!string.IsNullOrEmpty(response))
        //        {
        //            response = response.TrimStart();

        //            responseJObject = JObject.Parse(response);
        //            if (responseJObject.Count > 0)
        //            {
        //                dynamic Response = responseJObject;
        //                if (responseJObject.HasValues)
        //                {
        //                    var responseValue = responseJObject.First.ToString();
        //                    if (responseValue.Contains("200"))
        //                    {
        //                        isSuccess = true;
        //                    }
        //                }
        //            }

        //            if (isSuccess)
        //            {
        //                returnObj.Status = 201;
        //                returnObj.Message = "Success";
        //            }
        //            else
        //            {
        //                returnObj.Status = 401;
        //                returnObj.Message = "Error in getting response";
        //            }
        //        }

        //        if (responseJObject != null)
        //        {
        //            GenUtsAtsApiRecord records = _talentConnectAdminDBContext.GenUtsAtsApiRecords.FirstOrDefault(x => x.Id == APIRecordInsertedID);
        //            if (records != null)
        //            {
        //                records.ResponseReceived = Convert.ToString(responseJObject);
        //                CommonLogic.DBOperator(_talentConnectAdminDBContext, records, EntityState.Modified);
        //            }
        //        }

        //        returnObj.Response = json;

        //        var returnJson = new
        //        {
        //            returnObj.Status,
        //            returnObj.Message,
        //            returnObj.Response
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailBinder emailBinder = new EmailBinder(_configuration, _talentConnectAdminDBContext);
        //        emailBinder.sendErrorEmail(apiType, endPoint, payloads, ex.Message.ToString());
        //    }
        //}

        #endregion

        #region Private methods 
        private long InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records)
        {
            GenUtsAtsApiRecord utsAtsApi_Records = new GenUtsAtsApiRecord();

            utsAtsApi_Records.FromApiUrl = gen_UtsAtsApi_Records.FromApiUrl;
            utsAtsApi_Records.ToApiUrl = gen_UtsAtsApi_Records.ToApiUrl;    //Here API URL of ATS will come.
            utsAtsApi_Records.PayloadToSend = gen_UtsAtsApi_Records.PayloadToSend;
            utsAtsApi_Records.CreatedById = gen_UtsAtsApi_Records.CreatedById;
            utsAtsApi_Records.CreatedByDateTime = DateTime.Now;
            utsAtsApi_Records.HrId = gen_UtsAtsApi_Records.HrId;
            _talentConnectAdminDBContext.GenUtsAtsApiRecords.Add(utsAtsApi_Records);
            _talentConnectAdminDBContext.SaveChanges();

            return utsAtsApi_Records.Id;
        }
        #endregion

        #region DBCalls
        //public sproc_Get_InterviewRoundDetails_Result sproc_Get_InterviewRoundDetails(string param)
        //{
        //    return _talentConnectAdminDBContext.Set<sproc_Get_InterviewRoundDetails_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_Get_InterviewRoundDetails, param)).AsEnumerable().FirstOrDefault();
        //}
        //public sp_UTS_get_HRTalentProfileReason_Result sproc_UTS_get_HRTalentProfileReason(string param)
        //{
        //    return _talentConnectAdminDBContext.Set<sp_UTS_get_HRTalentProfileReason_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sp_UTS_get_HRTalentProfileReason, param)).AsEnumerable().FirstOrDefault();
        //}
        #endregion
    }
}
