using System.Net.Mail;
using System.Text.RegularExpressions;
using UTSATSAPI.Helpers.Common;

namespace UTSATSAPI.Helpers
{
    /// <summary>
    /// SendEmails
    /// </summary>
    public class SendEmails
    {
        /// <summary>
        /// Sends the emails with cc with out BCC.
        /// </summary>
        /// <param name="webRootPath">The web root path.</param>
        /// <param name="emailFrom">The email from.</param>
        /// <param name="emailFromName">Name of the email from.</param>
        /// <param name="emailTo">The email to.</param>
        /// <param name="emailToName">Name of the email to.</param>
        /// <param name="ccEmailName">Name of the cc email.</param>
        /// <param name="ccEmail">The cc email.</param>
        /// <param name="emailSubject">The email subject.</param>
        /// <param name="emailBody">The email body.</param>
        /// <param name="attachmentName">Name of the attachment.</param>
        /// <param name="attachmentName1">The attachment name1.</param>
        /// <param name="smtpEmailName">Name of the SMTP email.</param>
        /// <param name="smtpPasswordName">Name of the SMTP password.</param>
        /// <param name="smtpClientName">Name of the SMTP client.</param>
        /// <param name="smtpSSLName">if set to <c>true</c> [SMTP SSL name].</param>
        /// <param name="smtpPortName">Name of the SMTP port.</param>
        /// <param name="attachment">The attachment.</param>
        /// 

        public static void SendEmailsWithCC_WithOutBcc(string webRootPath, string emailFrom, string emailFromName, string emailTo, string emailToName, string ccEmailName, string ccEmail, string emailSubject, string emailBody, string attachmentName, string attachmentName1, string smtpEmailName, string smtpPasswordName, string smtpClientName, bool smtpSSLName, int smtpPortName, Attachment attachment = null)
        {
            MailMessage mailm = new MailMessage();
            SmtpClient smtpc = default(SmtpClient);

            emailFromName = string.IsNullOrEmpty(emailFromName) ? "TalentConnect" : emailFromName;
            emailTo = string.IsNullOrEmpty(emailTo) ? "" : emailTo;
            emailToName = string.IsNullOrEmpty(emailToName) ? "" : emailToName;

            List<string> strEmailTolist = new List<string>();                        
            foreach (var item in emailTo.Split(','))
            {
                if (item != null && item != "")
                    strEmailTolist.Add(item);
            }

            List<string> strEmailToNamelist = new List<string>();
            foreach (var item in emailToName.Split(','))
            {
                if (item != null && item != "")
                    strEmailToNamelist.Add(item);
            }

            List<string> strEmailCClist = new List<string>();
            foreach (var item in ccEmail.Split(','))
            {
                if (item != null && item !="")
                    strEmailCClist.Add(item);
            }

            List<string> strEmailNamelist = new List<string>();
            foreach (var item in ccEmailName.Split(','))
            {
                if (item != null && item != "")
                    strEmailNamelist.Add(item);
            }

            try
            {
                for (int intCnt = 0; intCnt <= strEmailTolist.Count - 1; intCnt++)
                {
                    if (CommonLogic.IsValidEmail(strEmailTolist[intCnt].Trim()))
                    {
                        mailm.To.Add(new MailAddress(strEmailTolist[intCnt].Trim(), string.IsNullOrEmpty(emailToName) ? "" : strEmailToNamelist[intCnt]));
                    }
                }

                for (int intCnt = 0; intCnt <= strEmailCClist.Count - 1; intCnt++)
                {
                    if (CommonLogic.IsValidEmail(strEmailCClist[intCnt].Trim()))
                    {
                        mailm.CC.Add(new MailAddress(strEmailCClist[intCnt].Trim(), string.IsNullOrEmpty(strEmailNamelist[intCnt]) ? "" : strEmailNamelist[intCnt]));
                    }
                }

                mailm.From = new MailAddress(emailFrom, emailFromName);
                mailm.Subject = emailSubject;
                mailm.Body = emailBody;
                mailm.IsBodyHtml = true;

                string path = "";

                if (!string.IsNullOrEmpty(attachmentName))
                {
                    path = Path.Combine(webRootPath, attachmentName);
                    mailm.Attachments.Add(new Attachment(path));
                }

                if (!string.IsNullOrEmpty(attachmentName1))
                {
                    path = Path.Combine(webRootPath, attachmentName1);
                    mailm.Attachments.Add(new Attachment(path));
                }

                if (attachment != null)
                {
                    mailm.Attachments.Add(attachment);
                }

                smtpc = new SmtpClient();
                smtpc.Credentials = new System.Net.NetworkCredential(smtpEmailName, smtpPasswordName);
                smtpc.Host = smtpClientName;
                smtpc.EnableSsl = smtpSSLName;
                smtpc.Port = smtpPortName;

                for (int intCounter = 0; intCounter <= 5; intCounter++)
                {
                    try
                    {
                        smtpc.Send(mailm);
                        intCounter = 6;
                    }
                    catch (Exception ex)
                    {
                        intCounter = intCounter + 1;
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mailm = null;
                smtpc = null;
                strEmailTolist = null;
                strEmailToNamelist = null;
            }
        }

        public static void SendEmail(string emailFrom, string emailFromName, string emailTo, string emailToName, string emailSubject, string emailBody, string smtpEmailName, string smtpPasswordName, string smtpClientName, bool smtpSSLName, int smtpPortName, Attachment attachment = null, string ccEmailName = null, string ccEmail = null, string attachmentName = null, string attachmentName1 = null, string webRootPath = "")
        {
            MailMessage mailm = new MailMessage();
            SmtpClient smtpc = default(SmtpClient);

            emailFromName = string.IsNullOrEmpty(emailFromName) ? "TalentConnect" : emailFromName;
            emailTo = string.IsNullOrEmpty(emailTo) ? "" : emailTo;
            emailToName = string.IsNullOrEmpty(emailToName) ? "" : emailToName;

            try
            {
                mailm.To.Add(emailTo);
                mailm.CC.Add("siddharth.j@uplers.in");
                mailm.From = new MailAddress(emailFrom, emailFromName);
                mailm.Subject = emailSubject;
                mailm.Body = emailBody;
                mailm.IsBodyHtml = true;

                smtpc = new SmtpClient();
                smtpc.Credentials = new System.Net.NetworkCredential(smtpEmailName, smtpPasswordName);
                smtpc.Host = smtpClientName;
                smtpc.EnableSsl = smtpSSLName;
                smtpc.Port = smtpPortName;
                smtpc.Send(mailm);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mailm = null;
                smtpc = null;
            }
        }


       


    }
}