namespace UTSATSAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Repositories.Interfaces;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.ViewModel;
    using UTSATSAPI.Models.ViewModels;
    using AuthorizeAttribute = Helpers.Common.AuthorizeAttribute;
    using UTSATSAPI.Models.ComplexTypes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Microsoft.EntityFrameworkCore;
    using DocumentFormat.OpenXml.Office2010.Excel;
    using ElmahCore;
    using UTSATSAPI.Models.ViewModel;
    using System.Text.Json.Serialization;
    using System.Text.Json.Nodes;
    using DocumentFormat.OpenXml.Office2010.PowerPoint;
    using System.Data;
    using DocumentFormat.OpenXml.Drawing;
    using System.Linq;
    using Amazon.SimpleEmail.Model;

    [ApiController]
    [Route("UserOperationsAPI/", Name = "User Related Services")]
    public class UsrUserController : ControllerBase
    {
        #region Variables

        private readonly ILogger<UsrUserController> _logger;
        private readonly IConfiguration _iConfiguration;
        private readonly IAdminUserLogin _iAdminUserLogin;
        private readonly IClient _iClient;
        #endregion

        #region Constructors

        public UsrUserController(IAdminUserLogin iAdminUserLogin, ILogger<UsrUserController> logger, IConfiguration iConfiguration, IClient iClient)
        {
            _logger = logger;
            _iConfiguration = iConfiguration;
            _iAdminUserLogin = iAdminUserLogin;
            _iClient = iClient;
        }

        #endregion

        #region Public APIs

        [HttpPost("AdminLogin")]
        [AllowAnonymous]
        public async Task<ObjectResult> AdminLogin([FromBody] AdminLoginUser adminLogin)
        {
            try
            {
                List<Tuple<string, string, string>> employee = new List<Tuple<string, string, string>>();
                employee.Add(new Tuple<string, string, string>("notempty", adminLogin.username, "Username"));
                employee.Add(new Tuple<string, string, string>("notempty", adminLogin.password, "Password"));
                List<string> errors = new Validator(employee).Validate();

                if (errors.Count == 0)
                {
                    UsrUser usrUser = await _iAdminUserLogin.LoginUser(adminLogin.username, adminLogin.password).ConfigureAwait(false);
                    if (usrUser != null)
                    {
                        var result = CustomRendering.AdminLoginResponse(usrUser, _iConfiguration);

                        bool resultIsAddTokenInMemory = await _iAdminUserLogin.IsAddTokenInMemory(result.Token, result.LoggedInUserNameTC);

                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Authentication is Done", Details = result });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Username or password is invalid, please try again later.", Details = errors });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Failed", Details = errors });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ForgotPassword")]
        public async Task<ObjectResult> ForgotPassword(string email)
        {
            try
            {
                List<Tuple<string, string, string>> employee = new List<Tuple<string, string, string>>();
                employee.Add(new Tuple<string, string, string>("notempty", email, "Email"));
                employee.Add(new Tuple<string, string, string>("email", email, "Email"));

                List<string> errors = new Validator(employee).Validate();

                if (errors.Count == 0)
                {
                    UsrUser usrUser = await _iAdminUserLogin.IsValidEmail(email);
                    if (usrUser != null)
                    {
                        if (usrUser.IsActive.Value)
                        {
                            string resetToken = CommonLogic.Encrypt(string.Format("{0}_{1}", email, DateTime.Now.AddMinutes(30)));
                            string link = string.Format("http://{0}/resetpassword?token={1}", _iConfiguration["config:FrontURl"], resetToken);
                            //if (Helper.SendEmail(_iConfiguration["config:SMTPEmailName"], _iConfiguration["config:SMTPPasswordName"], _iConfiguration["config:SMTPSSLName"], _iConfiguration["config:SMTPPortName"], _iConfiguration["config:SMTPClientName"], email, "Forgot Password", CustomRendering.ForgotPasswordTemplate(link)))
                            //{
                            //    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = email + " : Email is valid, We have sent details on your email" });
                            //}
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Account is inActive, please contact admin" });
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = email + " : related account not found !" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Failed", Details = string.Join(Environment.NewLine, errors.ToArray()) });
                }
            }
            catch
            {
                throw;
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Something goes wrong, please contact Admin" });
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ObjectResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                var userId = SessionValues.LoginUserId;
                var LoggedInUser = await _iAdminUserLogin.LoginUser(userId);

                #region Pre-validaion
                if (changePasswordViewModel == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "Object an not be null"
                        });
                }
                else
                {
                    if (string.IsNullOrEmpty(changePasswordViewModel.CurrentPassword) ||
                        string.IsNullOrEmpty(changePasswordViewModel.NewPassword) ||
                        string.IsNullOrEmpty(changePasswordViewModel.ConfirmPassword))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                           new ResponseObject()
                           {
                               statusCode = StatusCodes.Status400BadRequest,
                               Message = "Password can not be null/empty"
                           });
                    }

                    if (string.IsNullOrEmpty(changePasswordViewModel.Token))
                    {
                        return StatusCode(StatusCodes.Status400BadRequest,
                           new ResponseObject()
                           {
                               statusCode = StatusCodes.Status400BadRequest,
                               Message = "Token can not be null/empty"
                           });
                    }

                }

                if (changePasswordViewModel.CurrentPassword != LoggedInUser.Password)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "Incorrect current Password"
                        });
                }

                if (changePasswordViewModel.CurrentPassword == changePasswordViewModel.NewPassword)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "Old and New Password can not be same"
                        });
                }

                if (changePasswordViewModel.NewPassword != changePasswordViewModel.ConfirmPassword)
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "New and Confirm Password must be same"
                        });
                }
                #endregion

                if (LoggedInUser != null)
                {
                    LoggedInUser.Password = changePasswordViewModel.NewPassword;
                    LoggedInUser.ModifyByDatetime = DateTime.Now;
                    LoggedInUser.ModifyById = Convert.ToInt32(LoggedInUser.Id);

                    bool result = await _iAdminUserLogin.ChangePassword(LoggedInUser);

                    if (result)
                    {
                        //Token needs to be expired
                        await _iAdminUserLogin.IsLogoutUser(changePasswordViewModel.Token);
                    }

                    return StatusCode(StatusCodes.Status200OK,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status200OK,
                            Message = "Password Updated Successfully"
                        });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseObject()
                        {
                            statusCode = StatusCodes.Status400BadRequest,
                            Message = "User is in valid / not active"
                        });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetTokenList")]
        public async Task<ObjectResult> GetTokenList()
        {
            var result = await _iAdminUserLogin.GetActiveTokenList();
            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Active Token Count " + result.Count().ToString(), Details = result });

        }

        [Authorize]
        [HttpGet("LogOut")]
        public async Task<ObjectResult> LogOut(string token)
        {
            bool logoutResult = await _iAdminUserLogin.IsLogoutUser(token);

            if (logoutResult)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Logout" });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Logout" });
            }
        }

        #endregion

        #region Fetch GSPace from Client Email
        [AllowAnonymous]
        [HttpGet("GetSpaceIdForClientEmail")]
        public async Task<ObjectResult> GetSpaceIdForClientEmail(string EmailID)
        {
            try
            {
                #region Validation

                if (string.IsNullOrEmpty(EmailID))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide EmailID." });
                }
                #endregion
                string SpaceID = "";
                UpdateSpaceIDForClient updateSpaceIDForClient = new UpdateSpaceIDForClient();
                var updateContact = await _iClient.GetGenContactByEmail(EmailID).ConfigureAwait(false);
                if (updateContact != null)
                {
                    long CompanyId = updateContact.CompanyId ?? 0;

                    var updateCompany = await _iClient.GetGenCompanysById(CompanyId).ConfigureAwait(false);

                    if (updateCompany != null)
                    {
                        updateSpaceIDForClient.SpaceID = updateCompany.GspaceId;
                        updateSpaceIDForClient.clientEmail = EmailID;
                        updateSpaceIDForClient.TokenObject = updateCompany.GspaceTokenObject;
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = updateSpaceIDForClient });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Update_SpaceID_For_Client
        [HttpPost("UpdateSpaceIDForClient")]
        public async Task<ObjectResult> UpdateSpaceIDForClient([FromBody] UpdateSpaceIDForClient updateSpaceIDForClient)
        {
            try
            {
                #region Validation

                if (updateSpaceIDForClient == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide data." });
                }
                #endregion

                object[] param = new object[]
                {
                    updateSpaceIDForClient.clientEmail,updateSpaceIDForClient.SpaceID,updateSpaceIDForClient.TokenObject
                };
                string paramasString = CommonLogic.ConvertToParamString(param);

                _iClient.Sproc_Update_SpaceID_For_Client(paramasString);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region ReceiveSnsNotification
        [HttpPost("ReceiveSnsNotificationOld")]
        public IActionResult ReceiveSnsNotificationOld([FromBody] JsonObject snsMessage)
        {
            _iClient.sproc_Add_AWS_Email_Payload_Result(snsMessage.ToString());

            //var AdminAPIProjectURL = _iConfiguration["AdminAPIProjectURL"].ToString();
            //if (System.IO.File.Exists(System.IO.Path.Combine("C:\\inetpub\\wwwroot\\QA_UTSAdmin\\Media\\JDParsing\\", "ReceiveSnsNotification.txt")))
            //    System.IO.File.Delete(System.IO.Path.Combine("C:\\inetpub\\wwwroot\\QA_UTSAdmin\\Media\\JDParsing\\", "ReceiveSnsNotification.txt"));

            //System.IO.File.WriteAllText(System.IO.Path.Combine("C:\\inetpub\\wwwroot\\QA_UTSAdmin\\Media\\JDParsing\\", "ReceiveSnsNotification.txt"), snsMessage.ToString());

            return Ok();
        }

        [HttpPost("ReceiveSnsNotification")]
        public async Task<JsonResult> ReceiveSnsNotification()
        {
            string payloads;
            using (var reader = new StreamReader(Request.Body))
            {
                payloads = await reader.ReadToEndAsync();
            }

            //_iClient.sproc_Add_AWS_Email_Payload_Result(payloads);

            if (payloads != null)
            {
                EmailEventPayload emailEventPayload = JsonConvert.DeserializeObject<EmailEventPayload>(payloads);
                InsertEmailEvent(emailEventPayload);
            }
            var response = new
            {
                status = 200,
                Data = "Success"
            };

            return new JsonResult(response);
        }

        private void InsertEmailEvent(EmailEventPayload emailEventPayload)
        {
            switch (emailEventPayload.EventType)
            {
                case "Send":
                    InsertSendEvent(emailEventPayload.Mail);
                    break;
                case "Delivery":
                    InsertDeliveryEvent(emailEventPayload.Mail, emailEventPayload.Delivery);
                    break;
                case "Open":
                    InsertOpenEvent(emailEventPayload.Mail, emailEventPayload.Open);
                    break;
                case "Click":
                    InsertClickEvent(emailEventPayload.Mail, emailEventPayload.Click);
                    break;
                case "Bounce":
                    InsertBounceEvent(emailEventPayload.Mail, emailEventPayload.Bounce);
                    break;
                default:
                    throw new ArgumentException("Unknown event type");
            }
        }


        private void InsertSendEvent(Mail mail)
        {

            try
            {
                string subject = "", UserAgent = "", IpAddress = "", EmailLinks = "", EmailLinkTags = "",
                    bouncedRecipients = "", bounceType = "", bounceSubType = "";
                foreach (var item in mail.Headers)
                {
                    if (item.Name == "Subject")
                        subject = item.Value;
                }

                object[] param = new object[]
               {
                    mail.MessageId,
                    "Send",
                    JsonConvert.SerializeObject(mail.Destination),
                    subject,
                    UserAgent,
                    IpAddress,
                    EmailLinks,
                    EmailLinkTags,
                    bounceType,
                    bounceSubType,
                    bouncedRecipients,
                    mail.Timestamp
               };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _iClient.Sproc_Add_AWS_SES_EmailTracking(paramString);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InsertDeliveryEvent(Mail mail, Delivery delivery)
        {

            try
            {
                string subject = "", UserAgent = "", IpAddress = "", EmailLinks = "", EmailLinkTags = "",
                     bouncedRecipients = "", bounceType = "", bounceSubType = "";
                foreach (var item in mail.Headers)
                {
                    if (item.Name == "Subject")
                        subject = item.Value;
                }

                object[] param = new object[]
                {
                    mail.MessageId,
                    "Delivery",
                    JsonConvert.SerializeObject(delivery.Recipients),
                    subject,
                    UserAgent,
                    IpAddress,
                    EmailLinks,
                    EmailLinkTags,
                    bounceType,
                    bounceSubType,
                    bouncedRecipients,
                    delivery.Timestamp
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _iClient.Sproc_Add_AWS_SES_EmailTracking(paramString);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void InsertOpenEvent(Mail mail, Open open)
        {
            try
            {
                string subject = "", UserAgent = open.UserAgent, IpAddress = open.IpAddress, EmailLinks = "", EmailLinkTags = "",
                     bouncedRecipients = "", bounceType = "", bounceSubType = "";
                foreach (var item in mail.Headers)
                {
                    if (item.Name == "Subject")
                        subject = item.Value;
                }

                object[] param = new object[]
                {
                    mail.MessageId,
                    "Open",
                    JsonConvert.SerializeObject(mail.Destination),
                    subject,
                    UserAgent,
                    IpAddress,
                    EmailLinks,
                    EmailLinkTags,
                    bounceType,
                    bounceSubType,
                    bouncedRecipients,
                    open.Timestamp
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _iClient.Sproc_Add_AWS_SES_EmailTracking(paramString);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void InsertClickEvent(Mail mail, Click click)
        {
            try
            {
                string subject = "", UserAgent = click.UserAgent, IpAddress = click.IpAddress, EmailLinks = click.Link, EmailLinkTags = click.LinkTags != null ? JsonConvert.SerializeObject(click.LinkTags) : "",
                       bouncedRecipients = "", bounceType = "", bounceSubType = "";
                foreach (var item in mail.Headers)
                {
                    if (item.Name == "Subject")
                        subject = item.Value;
                }


                object[] param = new object[]
                {
                    mail.MessageId,
                    "Click",
                    JsonConvert.SerializeObject(mail.Destination),
                    subject,
                    UserAgent,
                    IpAddress,
                    EmailLinks,
                    EmailLinkTags,
                    bounceType,
                    bounceSubType,
                    bouncedRecipients,
                    click.Timestamp
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _iClient.Sproc_Add_AWS_SES_EmailTracking(paramString);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void InsertBounceEvent(Mail mail, BounceDetails bounceDetails)
        {
            try
            {
                string subject = "", bounceType = bounceDetails.BounceType, bounceSubType = bounceDetails.BounceSubType,
                        UserAgent = "", IpAddress = "", EmailLinks = "", EmailLinkTags = "";
                string bouncedRecipients = "";

                foreach (var item in mail.Headers)
                {
                    if (item.Name == "Subject")
                        subject = item.Value;
                }

                foreach (var item in bounceDetails.BouncedRecipients)
                {
                    if (bouncedRecipients == "")
                        bouncedRecipients = item.EmailAddress;
                    else
                        bouncedRecipients += ", " + item.EmailAddress;
                }

                object[] param = new object[]
                {
                    mail.MessageId,
                    "Bounce",
                    JsonConvert.SerializeObject(mail.Destination),
                    subject,
                    UserAgent,
                    IpAddress,
                    EmailLinks,
                    EmailLinkTags,
                    bounceType,
                    bounceSubType,
                    bouncedRecipients,
                    bounceDetails.Timestamp
                };

                string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                _iClient.Sproc_Add_AWS_SES_EmailTracking(paramString);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}
