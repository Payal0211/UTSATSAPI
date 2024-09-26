using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.ChatGPTCalls
{
    public class ChatGPTCommonAPI
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly UTSATSAPIDBConnection _UTSATSAPIDBConnection;
        #endregion

        #region Constructors
        public ChatGPTCommonAPI(IConfiguration configuration, UTSATSAPIDBConnection UTSATSAPIDBConnection)
        {
            _configuration = configuration;
            _UTSATSAPIDBConnection = UTSATSAPIDBConnection;
        }
        #endregion

        /// <summary>
        /// Login from the Admin portal into the client portal and fetch the URL parsing data from chatgpt.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="url"></param>
        /// <param name="isFromUrl"></param>
        /// <returns></returns>
        public ClientAPIResult LoginfromAdminToClient(string json, string content, bool isFromUrl, bool isForSkills = false, bool isFileUploadFromAdmin = false)
        {
            try
            {
                ClientAPIResult clientAPIResult = new ClientAPIResult();

                var TMD_APIURL = _configuration["ClientProjectAPIURL"] + "account/LoginWithEmail";

                string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, json, "");

                bool isSuccess = false;
                string loginToken = string.Empty;

                if (!string.IsNullOrEmpty(response))
                {
                    response = response.TrimStart();
                    var responseJObject = JsonConvert.DeserializeObject<Root>(response);

                    if (responseJObject != null)
                    {
                        if (responseJObject.statusCode == 200)
                        {
                            isSuccess = true;
                            loginToken = responseJObject.details?.Token ?? "";
                        }
                    }

                    clientAPIResult.StatusCode = responseJObject?.statusCode;

                    if (isSuccess)
                    {
                        if (isFromUrl)
                        {
                            var urlResponse = ExtractDataFromURL(content, loginToken);
                            if (urlResponse != null && urlResponse.statusCode == 200)
                            {
                                clientAPIResult.UrlParsingData = urlResponse;
                                LogoutfromAdminToClient(loginToken);
                            }
                        }
                        else if (isForSkills)
                        {
                            var skillResponse = ExtractSkillsFromRoleAndExp(content, loginToken);
                            clientAPIResult.SkillsResponseData = skillResponse;
                            LogoutfromAdminToClient(loginToken);
                        }
                        else
                        {
                            if (isFileUploadFromAdmin)
                            {
                                var jdResponse = ExtractAllDetailsBasedOnText(content, loginToken);
                                clientAPIResult.RRRWithSkillsData = jdResponse;
                            }
                            else
                            {
                                var jdResponse = ExtraCtJDBasedOnText(content, loginToken);
                                clientAPIResult.JDResponeData = jdResponse;
                            }

                            LogoutfromAdminToClient(loginToken);
                        }
                    }
                }

                return clientAPIResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logout from the client portal after successfull data fetch.
        /// </summary>
        /// <param name="token"></param>
        private void LogoutfromAdminToClient(string token)
        {
            try
            {
                var TMD_APIURL = $"{_configuration["ClientProjectAPIURL"]}account/logout?token={token}";
                string response = CommonLogic.CoreHttpCallsGet(TMD_APIURL, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetch the data of the URL from the chatgpt from client portal API call.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private ExtractTextFromURl ExtractDataFromURL(string url, string token)
        {
            try
            {
                ExtractTextFromURl returnObj = new ExtractTextFromURl();
                string TMD_APIURL = "";
                //if (_configuration["HRDataSendSwitchForPHP"].ToString() == "Live")
                //    TMD_APIURL = $"{_configuration["ClientProjectAPIURL"]}TextExtraction/ExtractTextUsingPython?psUrl={url}";
                //else
                TMD_APIURL = $"{_configuration["ClientProjectAPIURL"]}TextExtraction/ExtractTextUsingclaudAI?psUrl={url}";


                string response = CommonLogic.CoreHttpCallsGet(TMD_APIURL, token);

                if (!string.IsNullOrEmpty(response))
                {
                    response = response.TrimStart();
                    var responseJObject = JsonConvert.DeserializeObject<ExtractTextFromURl>(response);

                    if (responseJObject != null)
                    {
                        returnObj = responseJObject;
                    }
                }

                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetch the data from file upload 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private ExtractJDFileText ExtraCtJDBasedOnText(string content, string token)
        {
            ExtractJDFileText extractJDFileText = new ExtractJDFileText();
            var TMD_APIURL = $"{_configuration["ClientProjectAPIURL"]}JobDescription/ExtraCtJDBasedOnTextClaudAI";

            string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, content, token);

            if (!string.IsNullOrEmpty(response))
            {
                response = response.TrimStart();
                var responseJObject = JsonConvert.DeserializeObject<ExtractJDFileText>(response);

                if (responseJObject != null)
                {
                    extractJDFileText = responseJObject;
                }
            }

            return extractJDFileText;
        }

        /// <summary>
        /// Fetch the skills from chatgpt based on role and years and exp.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private ExtractSkills ExtractSkillsFromRoleAndExp(string content, string token)
        {
            try
            {
                ExtractSkills returnObj = new ExtractSkills();

                dynamic? payload = new ExpandoObject();
                payload = JsonConvert.DeserializeObject<ExpandoObject>(content);

                var TMD_APIURL = $"{_configuration["ClientProjectAPIURL"]}TextExtraction/ExtractSkillsUsingPython";

                //string response = CommonLogic.CoreHttpCallsGet(TMD_APIURL, token);
                string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, content, token);

                if (!string.IsNullOrEmpty(response))
                {
                    response = response.TrimStart();
                    var responseJObject = JsonConvert.DeserializeObject<ExtractTextFromURl>(response);

                    if (responseJObject != null && responseJObject.details != null)
                    {
                        returnObj.ChatGptAllSkills = responseJObject.details.allSkills;
                        returnObj.ChatGptSkills = responseJObject.details.skills;
                    }
                }

                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ExtractRRRSkillsJDFileText ExtractAllDetailsBasedOnText(string content, string token)
        {
            ExtractRRRSkillsJDFileText extractJDFileText = new();
            var TMD_APIURL = $"{_configuration["ClientProjectAPIURL"]}JobDescription/ExtractJDRRRSkillsBasedOnText";

            string response = CommonLogic.CoreHttpCallsPost(TMD_APIURL, content, token);

            if (!string.IsNullOrEmpty(response))
            {
                response = response.TrimStart();
                var responseJObject = JsonConvert.DeserializeObject<ExtractRRRSkillsJDFileText>(response);

                if (responseJObject != null)
                {
                    extractJDFileText = responseJObject;
                }
            }

            return extractJDFileText;
        }


        
    }

    public class Details
    {
        public string? Token { get; set; }
        public string? LoggedInUserNameTC { get; set; }
        public object? LoggedInUserProfilePic { get; set; }
        public int LoggedInUserCompanyID { get; set; }
        public string? FullName { get; set; }
        public string? Designation { get; set; }
        public bool? IsEmailVerified { get; set; }
        public int LoggedInUserID { get; set; }
    }

    public class Root
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public Details? details { get; set; }
    }

    public class ExtractTextFromURlDetails
    {
        public int? gptjdid { get; set; }
        public string? guid { get; set; }
        public object? contactId { get; set; }
        public object? clientIPAddress { get; set; }
        public object? ipAddress { get; set; }
        public string? processType { get; set; }
        public int? currentStepId { get; set; }
        public int? nextStepId { get; set; }
        public string? roleName { get; set; }
        public int? experienceYears { get; set; }
        public int? noOfTalents { get; set; }
        public string? isHiringLimited { get; set; }
        public object? availability { get; set; }
        public object? contractDuration { get; set; }
        public string? skills { get; set; }
        public string? allSkills { get; set; }
        public string? currency { get; set; }
        public decimal? budgetFrom { get; set; }
        public decimal? budgetTo { get; set; }
        public string? employmentType { get; set; }
        public object? howSoon { get; set; }
        public int? workingModeId { get; set; }
        public object? companyLocation { get; set; }
        public int? achievementId { get; set; }
        public object? timeZoneId { get; set; }
        public int? timezonePreferenceId { get; set; }
        public string? timeZoneFromTime { get; set; }
        public string? timeZoneEndTime { get; set; }
        public object? reason { get; set; }
        public object? timeZone { get; set; }
        public object? region { get; set; }
        public string? rolesResponsibilities { get; set; }
        public string? requirements { get; set; }
        public object? jdFileName { get; set; }
        public int? achievedCount { get; set; }
        public string? JobDescription { get; set; }
    }

    public class ExtractTextFromURl
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public ExtractTextFromURlDetails? details { get; set; }
    }

    public class ClientAPIResult
    {
        public int? StatusCode { get; set; }
        public ExtractTextFromURl? UrlParsingData { get; set; }
        public ExtractJDFileText? JDResponeData { get; set; }
        public ExtractSkills? SkillsResponseData { get; set; }
        public ExtractRRRSkillsJDFileText? RRRWithSkillsData { get; set; }
    }

    public class ExtractJDFileText
    {
        public List<string>? Requirements { get; set; }
        public List<string>? RolesResponsibilities { get; set; }
        public List<string>? JobDescription { get; set; }
    }

    public class ExtractSkills
    {
        public string? ChatGptSkills { get; set; }
        public string? ChatGptAllSkills { get; set; }
    }

    public class ExtractRRRSkillsJDFileText
    {
        public List<string>? Requirements { get; set; }
        public List<string>? RolesResponsibilities { get; set; }
        public string? ChatGptSkills { get; set; }
        public List<string>? JobDescription { get; set; }

    }
}
