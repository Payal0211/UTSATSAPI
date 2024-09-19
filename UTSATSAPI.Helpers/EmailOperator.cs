using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using UTSATSAPI.Helpers.Common;
using Amazon;

namespace UTSATSAPI.Helpers
{
    public class EmailOperator
    {
        #region Variables
        private IConfiguration _configuration;
        private bool ccemailhascomma = false;
        private bool ccnamehascomma = false;
        private bool exceptionEmailHasComma = false;
        private bool exceptionEmailNameHasComma = false;
        private static MailMessage mailMessage;
        private static SmtpClient smtpClient;
        private string awsAccessKeyId { get; set; }
        private string awsSecretAccessKey { get; set; }
        private string environment { get; set; }
        private string configurationSetName { get; set; }
        private string AWSEmailFromName { get; set; }
        #endregion

        #region Properties
        private List<string> toEmail { get; set; }
        private List<string> toCCEmail { get; set; }
        private List<string> toCCEmailName { get; set; }
        private List<string> toBCCEmail { get; set; }
        private List<string> toBCCEmailName { get; set; }
        private List<string> toEmailName { get; set; }
        private string subject { get; set; }
        private List<Attachment> attachment { get; set; }
        private string body { get; set; }
        private string ccEmails { get; set; }
        private string ccNames { get; set; }
        private string bccEmails { get; set; }
        private string bccNames { get; set; }
        private string fromname { get; set; }
        private string fromemail { get; set; }
        private string smtpEmailName { get; set; }
        private string smtpPasswordName { get; set; }
        private string smtpClientName { get; set; }
        private bool smtpSSLName { get; set; }
        private int smtpPortName { get; set; }
        private string exceptionEmails { get; set; }
        private string exceptionEmailNames { get; set; }
        private string smtpPasswordNameClientPortal { get; set; }
        private string smtpEmailNameClientPortal { get; set; }
        #endregion

        #region Constructor
        public EmailOperator(IConfiguration configuration)
        {
            _configuration = configuration;
            ccEmails = _configuration["app_settings:CCEmailId"].ToString();
            ccNames = _configuration["app_settings:CCEmailName"].ToString();

            smtpEmailName = _configuration["app_settings:SMTPEmailName"].ToString();
            smtpPasswordName = _configuration["app_settings:SMTPPasswordName"].ToString();
            smtpClientName = _configuration["app_settings:SMTPClientName"].ToString();
            smtpSSLName = Convert.ToBoolean(_configuration["app_settings:SMTPSSLName"].ToString());
            smtpPortName = Convert.ToInt16(_configuration["app_settings:SMTPPortName"].ToString());
            smtpPasswordNameClientPortal = _configuration["app_settings:SMTPPasswordNameClientPortal"].ToString();
            smtpEmailNameClientPortal = _configuration["app_settings:SMTPEmailNameClientPortal"].ToString();
            exceptionEmails = _configuration["app_settings:ExceptionEmailId"].ToString();
            exceptionEmailNames = _configuration["app_settings:ExceptionEmailName"].ToString();

            AWSEmailFromName = Convert.ToString(_configuration["app_settings:AWSEmailFromName"]);
            awsAccessKeyId = Convert.ToString(_configuration["app_settings:awsAccessKeyId"]);
            awsSecretAccessKey = Convert.ToString(_configuration["app_settings:awsSecretAccessKey"]);
            environment = _configuration["environment"].ToString();
            configurationSetName = _configuration["configurationSetName"].ToString();

            if (ccEmails.Contains(','))
                ccemailhascomma = true;

            if (ccNames.Contains(','))
                ccnamehascomma = true;

            if (exceptionEmails.Contains(','))
                exceptionEmailHasComma = true;

            if (exceptionEmailNames.Contains(','))
                exceptionEmailNameHasComma = true;
        }
        #endregion

        #region KeyMethods
        private void SetMailMessage(bool isException = false, bool isBcc = false, bool isCustomCC = false)
        {
            try
            {
                mailMessage = new MailMessage();

                //Added this case for sending Exception Emails to specific audience mentioned in appsettings.json file.
                if (isException)
                {
                    if (exceptionEmailNameHasComma && exceptionEmailHasComma)
                    {
                        string[] strEmailCC = exceptionEmails.Split(',');
                        string[] strEmailName = exceptionEmailNames.Split(',');
                        for (int intCnt = 0; intCnt <= strEmailCC.Length - 1; intCnt++)
                        {
                            if (CommonLogic.IsValidEmail(strEmailCC[intCnt].Trim()))
                            {
                                if (isBcc)
                                    mailMessage.Bcc.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                                else
                                    mailMessage.CC.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                            }
                        }
                    }
                    else
                    {
                        if (CommonLogic.IsValidEmail(exceptionEmails.Trim()))
                        {
                            if (isBcc)
                                mailMessage.Bcc.Add(new MailAddress(exceptionEmails, exceptionEmailNames));
                            else
                                mailMessage.CC.Add(new MailAddress(exceptionEmails, exceptionEmailNames));
                        }
                    }
                }
                else
                {
                    if (!isCustomCC)
                    {
                        if (ccemailhascomma && ccnamehascomma)
                        {
                            string[] strEmailCC = ccEmails.Split(',');
                            string[] strEmailName = ccNames.Split(',');
                            for (int intCnt = 0; intCnt <= strEmailCC.Length - 1; intCnt++)
                            {
                                if (CommonLogic.IsValidEmail(strEmailCC[intCnt].Trim()))
                                {
                                    if (isBcc)
                                        mailMessage.Bcc.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                                    else
                                        mailMessage.CC.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                                }
                            }
                        }
                        else if (ccemailhascomma && !ccnamehascomma)
                        {
                            string[] strEmailCC = ccEmails.Split(',');
                            for (int intCnt = 0; intCnt <= strEmailCC.Length - 1; intCnt++)
                            {
                                if (CommonLogic.IsValidEmail(strEmailCC[intCnt].Trim()))
                                {
                                    if (isBcc)
                                        mailMessage.Bcc.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.Empty));
                                    else
                                        mailMessage.CC.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.Empty));
                                }
                            }
                        }
                        else if (!ccemailhascomma && ccnamehascomma)
                        {
                            if (CommonLogic.IsValidEmail(ccEmails.Trim()))
                            {
                                if (isBcc)
                                    mailMessage.Bcc.Add(new MailAddress(ccEmails, string.Empty));
                                else
                                    mailMessage.CC.Add(new MailAddress(ccEmails, string.Empty));
                            }
                        }
                        else
                        {
                            if (CommonLogic.IsValidEmail(ccEmails.Trim()))
                            {
                                if (isBcc)
                                    mailMessage.Bcc.Add(new MailAddress(ccEmails, bccNames));
                                else
                                    mailMessage.CC.Add(new MailAddress(ccEmails, ccNames));
                            }
                        }
                    }
                    if (toEmail != null && toEmail.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < toEmail.Count; intCnt++)
                        {
                            if (!string.IsNullOrEmpty(toEmail[intCnt]))
                            {
                                if (CommonLogic.IsValidEmail(toEmail[intCnt].Trim()))
                                {
                                    mailMessage.To.Add(new MailAddress(toEmail[intCnt].Trim(), toEmailName[intCnt]));
                                }
                            }
                        }
                    }
                    if (toCCEmail != null && toCCEmail.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < toCCEmail.Count; intCnt++)
                        {
                            if (!string.IsNullOrEmpty(toCCEmail[intCnt]))
                            {
                                if (CommonLogic.IsValidEmail(toCCEmail[intCnt].Trim()))
                                {
                                    if (isBcc)
                                        mailMessage.Bcc.Add(new MailAddress(toCCEmail[intCnt].Trim(), toCCEmailName[intCnt]));
                                    else
                                        mailMessage.CC.Add(new MailAddress(toCCEmail[intCnt].Trim(), toCCEmailName[intCnt]));
                                }
                            }
                        }
                    }

                    if (toBCCEmail != null && toBCCEmail.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < toBCCEmail.Count; intCnt++)
                        {
                            if (!string.IsNullOrEmpty(toBCCEmail[intCnt]))
                            {
                                if (CommonLogic.IsValidEmail(toBCCEmail[intCnt].Trim()))
                                {
                                    mailMessage.Bcc.Add(new MailAddress(toBCCEmail[intCnt].Trim(), toBCCEmailName[intCnt]));
                                }
                            }
                        }
                    }

                    if (attachment != null && attachment.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < attachment.Count; intCnt++)
                        {
                            if (attachment[intCnt] != null)
                            {
                                mailMessage.Attachments.Add(attachment[intCnt]);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(fromname))
                    fromname = _configuration["app_settings:SMTPSenderName"];

                if (string.IsNullOrEmpty(fromemail))
                    fromemail = _configuration["app_settings:SMTPEmailName"];

                mailMessage.From = new MailAddress(fromemail, fromname);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void SetMailMessageClientPortal(bool isException = false, bool isBcc = false)
        {
            try
            {
                mailMessage = new MailMessage();

                //Added this case for sending Exception Emails to specific audience mentioned in appsettings.json file.
                if (isException)
                {
                    if (exceptionEmailNameHasComma && exceptionEmailHasComma)
                    {
                        string[] strEmailCC = exceptionEmails.Split(',');
                        string[] strEmailName = exceptionEmailNames.Split(',');
                        for (int intCnt = 0; intCnt <= strEmailCC.Length - 1; intCnt++)
                        {
                            if (CommonLogic.IsValidEmail(strEmailCC[intCnt].Trim()))
                            {
                                if (isBcc)
                                    mailMessage.Bcc.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                                else
                                    mailMessage.CC.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                            }
                        }
                    }
                    else
                    {
                        if (CommonLogic.IsValidEmail(exceptionEmails.Trim()))
                        {
                            if (isBcc)
                                mailMessage.Bcc.Add(new MailAddress(exceptionEmails, exceptionEmailNames));
                            else
                                mailMessage.CC.Add(new MailAddress(exceptionEmails, exceptionEmailNames));
                        }
                    }
                }
                else
                {
                    if (ccemailhascomma && ccnamehascomma)
                    {
                        string[] strEmailCC = ccEmails.Split(',');
                        string[] strEmailName = ccNames.Split(',');
                        for (int intCnt = 0; intCnt <= strEmailCC.Length - 1; intCnt++)
                        {
                            if (CommonLogic.IsValidEmail(strEmailCC[intCnt].Trim()))
                            {
                                if (isBcc)
                                    mailMessage.Bcc.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                                else
                                    mailMessage.CC.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.IsNullOrEmpty(strEmailName[intCnt]) ? "" : strEmailName[intCnt]));
                            }
                        }
                    }
                    else if (ccemailhascomma && !ccnamehascomma)
                    {
                        string[] strEmailCC = ccEmails.Split(',');
                        for (int intCnt = 0; intCnt <= strEmailCC.Length - 1; intCnt++)
                        {
                            if (CommonLogic.IsValidEmail(strEmailCC[intCnt].Trim()))
                            {
                                if (isBcc)
                                    mailMessage.Bcc.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.Empty));
                                else
                                    mailMessage.CC.Add(new MailAddress(strEmailCC[intCnt].Trim(), string.Empty));
                            }
                        }
                    }
                    else if (!ccemailhascomma && ccnamehascomma)
                    {
                        if (CommonLogic.IsValidEmail(ccEmails.Trim()))
                        {
                            if (isBcc)
                                mailMessage.Bcc.Add(new MailAddress(ccEmails, string.Empty));
                            else
                                mailMessage.CC.Add(new MailAddress(ccEmails, string.Empty));
                        }
                    }
                    else
                    {
                        if (CommonLogic.IsValidEmail(ccEmails.Trim()))
                        {
                            if (isBcc)
                                mailMessage.Bcc.Add(new MailAddress(ccEmails, bccNames));
                            else
                                mailMessage.CC.Add(new MailAddress(ccEmails, ccNames));
                        }
                    }
                    if (toEmail != null && toEmail.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < toEmail.Count; intCnt++)
                        {
                            if (!string.IsNullOrEmpty(toEmail[intCnt]))
                            {
                                if (CommonLogic.IsValidEmail(toEmail[intCnt].Trim()))
                                {
                                    mailMessage.To.Add(new MailAddress(toEmail[intCnt].Trim(), toEmailName[intCnt]));
                                }
                            }
                        }
                    }
                    if (toCCEmail != null && toCCEmail.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < toCCEmail.Count; intCnt++)
                        {
                            if (!string.IsNullOrEmpty(toCCEmail[intCnt]))
                            {
                                if (CommonLogic.IsValidEmail(toCCEmail[intCnt].Trim()))
                                {
                                    if (isBcc)
                                        mailMessage.Bcc.Add(new MailAddress(toCCEmail[intCnt].Trim(), toCCEmailName[intCnt]));
                                    else
                                        mailMessage.CC.Add(new MailAddress(toCCEmail[intCnt].Trim(), toCCEmailName[intCnt]));
                                }
                            }
                        }
                    }

                    if (toBCCEmail != null && toBCCEmail.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < toBCCEmail.Count; intCnt++)
                        {
                            if (!string.IsNullOrEmpty(toBCCEmail[intCnt]))
                            {
                                if (CommonLogic.IsValidEmail(toBCCEmail[intCnt].Trim()))
                                {
                                    mailMessage.Bcc.Add(new MailAddress(toBCCEmail[intCnt].Trim(), toBCCEmailName[intCnt]));
                                }
                            }
                        }
                    }

                    if (attachment != null && attachment.Count > 0)
                    {
                        for (int intCnt = 0; intCnt < attachment.Count; intCnt++)
                        {
                            if (attachment[intCnt] != null)
                            {
                                mailMessage.Attachments.Add(attachment[intCnt]);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(fromname))
                    fromname = _configuration["app_settings:SMTPSenderNameClientPortal"];

                if (string.IsNullOrEmpty(fromemail))
                    fromemail = _configuration["app_settings:SMTPEmailNameClientPortal"];

                mailMessage.From = new MailAddress(fromemail, fromname);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void SetMailMessageForUpchat()
        {
            try
            {
                mailMessage = new MailMessage();

                if (toEmail != null && toEmail.Count > 0)
                {
                    for (int intCnt = 0; intCnt < toEmail.Count; intCnt++)
                    {
                        if (!string.IsNullOrEmpty(toEmail[intCnt]))
                        {
                            if (CommonLogic.IsValidEmail(toEmail[intCnt].Trim()))
                            {
                                mailMessage.To.Add(new MailAddress(toEmail[intCnt].Trim(), toEmailName[intCnt]));
                            }
                        }
                    }
                }
                if (toCCEmail != null && toCCEmail.Count > 0)
                {
                    for (int intCnt = 0; intCnt < toCCEmail.Count; intCnt++)
                    {
                        if (!string.IsNullOrEmpty(toCCEmail[intCnt]))
                        {
                            if (CommonLogic.IsValidEmail(toCCEmail[intCnt].Trim()))
                            {
                                mailMessage.CC.Add(new MailAddress(toCCEmail[intCnt].Trim(), toCCEmailName[intCnt]));
                            }
                        }
                    }
                }

                if (attachment != null && attachment.Count > 0)
                {
                    for (int intCnt = 0; intCnt < attachment.Count; intCnt++)
                    {
                        if (attachment[intCnt] != null)
                        {
                            mailMessage.Attachments.Add(attachment[intCnt]);
                        }
                    }
                }


                if (string.IsNullOrEmpty(fromname))
                    fromname = _configuration["app_settings:SMTPSenderName"];

                if (string.IsNullOrEmpty(fromemail))
                    fromemail = _configuration["app_settings:SMTPEmailName"];

                mailMessage.From = new MailAddress(fromemail, fromname);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void SetSmtpClient()
        {
            smtpClient = new SmtpClient();

            smtpClient.Credentials = new System.Net.NetworkCredential(smtpEmailName, smtpPasswordName);
            smtpClient.Host = smtpClientName;
            smtpClient.EnableSsl = smtpSSLName;
            smtpClient.Port = smtpPortName;
        }

        private void SetSmtpClientPortal()
        {
            smtpClient = new SmtpClient();

            smtpClient.Credentials = new System.Net.NetworkCredential(smtpEmailNameClientPortal, smtpPasswordNameClientPortal);
            smtpClient.Host = smtpClientName;
            smtpClient.EnableSsl = smtpSSLName;
            smtpClient.Port = smtpPortName;
        }
        #endregion

        #region GetterSetter
        public void SetCCEmail(string CCEmail, string CCEmailName)
        {
            if (!string.IsNullOrEmpty(CCEmail) && !string.IsNullOrEmpty(CCEmailName))
            {
                if (CCEmail.Contains(",") && CCEmailName.Contains(","))
                {
                    toCCEmail = CCEmail.Split(",").ToList();
                    toCCEmailName = CCEmailName.Split(",").ToList();

                    // Remove to emails from CC else it will send duplicate emails
                    //toCCEmail = toCCEmail.Except(toEmail).ToList();
                    //toCCEmailName = toCCEmailName.Except(toEmailName).ToList();
                }
                else
                {
                    if (toCCEmail == null)
                    {
                        toCCEmail = new List<string>();
                        toCCEmailName = new List<string>();
                    }

                    if (toCCEmail != null)
                    {
                        toCCEmail.Add(CCEmail);
                        toCCEmailName.Add(CCEmailName);
                    }
                }
            }
        }
        public void SetBCCEmail(string BCCEmail, string BCCEmailName)
        {
            if (!string.IsNullOrEmpty(BCCEmail) && !string.IsNullOrEmpty(BCCEmailName))
            {
                if (BCCEmail.Contains(",") && BCCEmailName.Contains(","))
                {
                    toBCCEmail = BCCEmail.Split(",").ToList();
                    toBCCEmailName = BCCEmailName.Split(",").ToList();
                }
                else
                {
                    toBCCEmail.Add(BCCEmail);
                    toBCCEmailName.Add(BCCEmailName);
                }
            }
        }
        public void SetToEmail(List<string> toemail)
        {
            toEmail = toemail;
        }
        public void SetToEmailName(List<string> _toEmailName)
        {
            toEmailName = _toEmailName;
        }
        public void SetBccEmail(List<string> bccemail)
        {
            toBCCEmail = bccemail;
        }
        public void SetBccEmailName(List<string> _bccEmailName)
        {
            toBCCEmailName = _bccEmailName;
        }
        public void SetBody(string _body)
        {
            body = _body;
        }
        public void SetAttachment(List<Attachment> _attachment)
        {
            attachment = _attachment;
        }
        public void SetSubject(string _subject)
        {
            subject = _subject;
        }
        public void SetFromName(string _fromname = "UTS")
        {
            fromname = _fromname;
        }

        public void SetFromEmail(string _fromemail = "UTS")
        {
            fromemail = _fromemail;
        }
        #endregion

        #region Sending
        public async void SendEmail(bool isException = false, bool isBcc = false, bool isCustomCC = false)
        {
            try
            {
                SetMailMessage(isException, isBcc, isCustomCC);
                SetSmtpClient();
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {

            }
        }

        public async void SendEmailClientPortal(bool isException = false, bool isBcc = false)
        {
            try
            {
                SetMailMessageClientPortal(isException, isBcc);
                SetSmtpClientPortal();
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {

            }
        }
        public async void SendEmailForUpchat()
        {
            try
            {
                SetMailMessageForUpchat();
                SetSmtpClient();
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {

            }
        }
        #endregion

        #region AWS SES Email Sending
        public async void SendEmailAWSSES(List<string> toEmailID, string Emailsubject, string Emailbody)
        {
            try
            {
                if (environment == "Live")
                {
                    var AWSAccessKeyId = awsAccessKeyId;
                    var AWSSecretAccessKey = awsSecretAccessKey;
                    var region = RegionEndpoint.EUNorth1; // Specify your AWS region
                                                          //var CCEmailAppSettings = _configuration["app_settings:CCEmailId"];

                    //if (!string.IsNullOrEmpty(CCEmailAppSettings))
                    //    toCCEmail.AddRange(CCEmailAppSettings.Split(','));

                    //bool addTobCc = false;
                    //if (isBcc)
                    //{
                    //    addTobCc = true;
                    //}

                    //if (toCCEmail != null)
                    //    toCCEmail.RemoveAll(string.IsNullOrWhiteSpace);

                    using (var client = new AmazonSimpleEmailServiceClient(AWSAccessKeyId, AWSSecretAccessKey, region))
                    {
                        var sendRequest = new SendEmailRequest
                        {
                            Source = AWSEmailFromName.ToString(),
                            Destination = new Destination
                            {
                                ToAddresses = toEmailID,
                                CcAddresses = null, //addTobCc ? null : toCCEmail, // Add Cc addresses
                                BccAddresses = null, //addTobCc ? toCCEmail : null// Add Bcc addresses
                            },
                            Message = new Message
                            {
                                Subject = new Content(Emailsubject),
                                Body = new Body
                                {
                                    Html = new Content(Emailbody)
                                }
                            },
                            ConfigurationSetName = configurationSetName // Add the ConfigurationSetName here
                        };

                        try
                        {
                            var response = await client.SendEmailAsync(sendRequest);
                            //if (response != null)
                            //{
                            //    object[] param = new object[]
                            //    {
                            //    response.MessageId,
                            //    "Send",
                            //    toEmail[0].ToString(),
                            //    subject,
                            //    System.DateTime.Now
                            //    };

                            //    string paramString = CommonLogic.ConvertToParamStringWithNull(param);
                            //    //_iEmail.Sproc_Add_AWS_SES_EmailSending_Details(paramString);                            
                            //    Console.WriteLine("Email sent! Message ID: " + response.MessageId);
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error sending email: " + ex.Message);
                        }
                    }
                }



            }
            catch
            {

            }
        }
        #endregion
    }
}
