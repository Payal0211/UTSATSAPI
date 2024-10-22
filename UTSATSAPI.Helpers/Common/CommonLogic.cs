using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;


namespace UTSATSAPI.Helpers.Common
{
    /// <summary>
    /// CommonLogic
    /// </summary>
    public static class CommonLogic
    {
        /// <summary>
        /// The working directory
        /// </summary>
        private static string workingDirectory = Environment.CurrentDirectory;
        static string Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string zoomSettings = "https://api.zoom.us/v2/users/hr@uplers.in/meetings";
        #region Public Methods

        /// <summary>
        /// Encrypts the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "uplers123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string ConvertToParamStringWithNull(object[] args)
        {
            string _arguments = string.Empty;
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                    {
                        if (i == args.Length - 1)
                        {
                            _arguments += "null";
                        }
                        else
                        {
                            _arguments += "null" + ",";
                        }
                    }
                    else
                    {
                        if (i == args.Length - 1)
                        {
                            if (CommonLogic.CheckIsString(args[i]))
                            {
                                string value = args[i].ToString();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    _arguments += "'" + Convert.ToString(args[i]).Replace("'", "''") + "'";
                                }
                                else
                                {
                                    _arguments += "'" + args[i] + "'";
                                }
                            }
                            else
                            {
                                _arguments += args[i];
                            }
                        }
                        else
                        {
                            if (CommonLogic.CheckIsString(args[i]))
                            {
                                string value = args[i].ToString();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    _arguments += "'" + Convert.ToString(args[i]).Replace("'", "''") + "',";
                                }
                                else
                                {
                                    _arguments += "'" + args[i] + "',";
                                }
                            }
                            else
                            {
                                _arguments += args[i] + ",";
                            }
                        }
                    }
                }
            }
            return _arguments;
        }
        public static string ClientHappinessSurveyEncrypt(string plainText)
        {
            string key = "jdsg4323879";
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            return System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(mStream.ToArray()));
        }


        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "uplers123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="secret">The secret.</param>
        /// <returns></returns>
        public static string CreateToken(UsrUser user, IConfiguration _iConfiguration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                   new Claim(ClaimTypes.Email, user.EmployeeId),
                   new Claim("LoginUserId", Convert.ToString(user.Id)),
                   new Claim("LoginUserTypeId", Convert.ToString(user.UserTypeId)),
              }),
                //Expires = DateTime.UtcNow.AddHours(10),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="smtpEmailNameFrom">The SMTP email name from.</param>
        /// <param name="smtpPasswordName">Name of the SMTP password.</param>
        /// <param name="smtpSSLName">Name of the SMTP SSL.</param>
        /// <param name="smtpPortName">Name of the SMTP port.</param>
        /// <param name="smtpClientName">Name of the SMTP client.</param>
        /// <param name="toEmailAddress">To email address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="emailContent">Content of the email.</param>
        /// <param name="toEmailName">Name of to email.</param>
        /// <returns></returns>
        public static bool SendEmail(string smtpEmailNameFrom, string smtpPasswordName, string smtpSSLName, string smtpPortName, string smtpClientName, string toEmailAddress, string subject, string emailContent, string toEmailName = "")
        {

            string EmailFromName = "Hire Talent";
            using (MailMessage mm = new MailMessage(new MailAddress(Convert.ToString(smtpEmailNameFrom), EmailFromName), new MailAddress(toEmailAddress, toEmailName)))
            {
                mm.Subject = subject;
                mm.Body = emailContent;
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(Convert.ToString(smtpEmailNameFrom), Convert.ToString(smtpPasswordName));
                    smtp.Host = Convert.ToString(smtpClientName);
                    smtp.EnableSsl = Convert.ToBoolean(smtpSSLName);
                    smtp.Port = Convert.ToInt32(smtpPortName);
                    smtp.Send(mm);
                }
            }
            return true;
        }

        /// <summary>
        /// Checks the token valid.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static string checkTokenValid(string token)
        {
            string stringToken = Decrypt(token);

            if (!string.IsNullOrEmpty(stringToken) && stringToken.Split('_').Length == 2)
            {
                string[] arrOfToken = stringToken.Split('_');
                CultureInfo culture = new CultureInfo("en-IN");
                DateTime tempDate = Convert.ToDateTime(arrOfToken[1], culture);
                if (DateTime.Now > tempDate)
                    return string.Empty;
                else
                    return stringToken;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public static bool UploadFile(string base64String, long Id)
        {
            string folderName = "UserProfileImage";
            string IdfolderName = string.Format(@"{0}\{1}\{2}\{3}", workingDirectory, "Media", folderName, Id.ToString());
            if (!Directory.Exists(IdfolderName))
                Directory.CreateDirectory(IdfolderName);

            string path = string.Format(@"{0}\{1}.{2}", IdfolderName, Id.ToString(), base64String.Split('/')[1].Split(';')[0]);

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64String.Split(',')[1])))
            {
                using (Bitmap bm2 = new Bitmap(ms))
                {
                    bm2.Save(path);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads the base64.
        /// </summary>
        /// <param name="base64">The base64.</param>
        /// <returns></returns>
        public static Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns></returns>
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }

        public static void DBOperator(UTSATSAPIDBConnection talentConnectContext, object model, EntityState entityState)
        {
            try
            {
                if (talentConnectContext != null)
                {
                    talentConnectContext.Entry(model).State = entityState;
                    talentConnectContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool CheckIsString(object value)
        {
            if (value is string)
            {
                return true;
            }
            if (value is DateTime)
            {
                return true;
            }
            return false;
        }

        public static string ConvertToParamString(object[] args)
        {
            string _arguments = string.Empty;
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                        args[i] = "";

                    if (i == args.Length - 1)
                    {
                        if (CommonLogic.CheckIsString(args[i]))
                        {
                            string value = args[i].ToString();
                            if(!string.IsNullOrEmpty(value))
                            {
                                _arguments += "'" + Convert.ToString(args[i]).Replace("'", "''") + "'";
                            }
                            else
                            {
                                _arguments += "'" + args[i] + "'";
                            }
                        } 
                        else
                        {
                            _arguments += args[i];
                        }
                    }
                    else
                    {
                        if (CommonLogic.CheckIsString(args[i]))
                        {
                            string value = args[i].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                _arguments += "'" + Convert.ToString(args[i]).Replace("'", "''") + "',";
                            }
                            else
                            {
                                _arguments += "'" + args[i] + "',";
                            }
                        }  
                        else
                        {
                            _arguments += args[i] + ",";
                        }
                    }
                }
            }
            return _arguments;
        }

        public static string GetFileUploadFolderFor(string subFolderfolderName)
        {

            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Media", subFolderfolderName);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
            return pathToSave;
        }

        //public static List<ErrorModel> SerializeErrors(List<FluentValidation.Results.ValidationFailure> validationFailures, string objectName)
        //{
        //    List<ErrorModel> errorModels = new List<ErrorModel>();
        //    if (validationFailures.Count > 0)
        //    {
        //        foreach (var error in validationFailures)
        //        {
        //            var errorModel = new ErrorModel();
        //            errorModel.ErrorMessage = error.ErrorMessage;
        //            errorModel.FieldName = error.PropertyName;
        //            errorModel.ModuleName = objectName;
        //            errorModels.Add(errorModel);
        //        }
        //    }
        //    return errorModels;
        //}

        public static string GetEmailTemplate(string workingdirname, string foldername, string templatename)
        {
            return File.ReadAllText(Path.Combine(workingdirname, foldername, string.Format("{0}.html", templatename)));
        }

        public static bool IsValidEmail(string email)
        {
            string[] IDs = email.Split(',');
            bool isvalid = true;
            for (int i = 0; i < IDs.Length; i++)
            {
                Regex r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
                if (string.IsNullOrEmpty(IDs[i]) && r.IsMatch(IDs[i]))
                    isvalid = false;
            }
            return isvalid;
        }

        public static DateTime ConvertString2DateTime(string DateDetail)
        {
            DateTime dtDateTime = new DateTime();

            string[] formats = { "MM/yyyy", "MM /dd/yyyy", "MM/d/yyyy", "M/d/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "d/MM/yyyy HH:mm:ss", "d/MM/yyyy hh:mm:ss tt", "d/MM/yyyy h:mm:ss tt", "d/MM/yyyy hh:mm tt", "d/MM/yyyy HH:mm", "MM/d/yyyy HH:mm:ss", "MM/d/yyyy hh:mm:ss tt", "MM/d/yyyy h:mm:ss tt", "MM/d/yyyy hh:mm tt", "MM/d/yyyy HH:mm", "M/d/yyyy HH:mm:ss", "M/d/yyyy hh:mm:ss tt", "M/d/yyyy h:mm:ss tt", "M/d/yyyy hh:mm tt", "M/d/yyyy HH:mm", "M/dd/yyyy HH:mm:ss", "M/dd/yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss", "M/dd/yyyy h:mm:ss tt", "M/dd/yyyy H:mm:ss", "M/dd/yyyy h:mm:ss", "M/dd/yyyy HH:mm", "M/dd/yyyy hh:mm tt", "M/d/yyyy HH:mm", "dd/MM/yyyy HH:mm", "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm", "d/MM/yyyy HH:mm", "d/MM/yyyy hh:mm", "d/M/yyyy HH:mm", "d/M/yyyy hh:mm", "MM/dd/yyyy HH:mm", "MM/dd/yyyy hh:mm", "MM/d/yyyy HH:mm", "MM/d/yyyy hh:mm", "M/d/yyyy HH:mm", "M/d/yyyy hh:mm", "MM/dd/yyyy hh:mm:ss tt", "MM/dd/yyyy HH:mm:ss tt", "MM/dd/yyyy H:mm:ss tt", "MM/dd/yyyy h:mm:ss tt", "dd/MM/yyyy hh:mm tt", "dd/MM/yyyy hh:mm:ss", "d/MM/yyyy h:mm tt", "d/MM/yyyy h:mm:ss", "d/MM/yyyy h:mm", "d/M/yyyy h:mm", "d/M/yyyy", "M/dd/yyyy", "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd h:mm:ss", "yyyy-mm-dd hh:mm:ss", "yyyy-mm-dd h:mm:ss", "dd/M/yyyy hh:mm:ss", "dd-MM-yyyy h:mm:ss", "dd-MM-yyyy", "yyyy-MM-dd HH:mm:ss tt", "yyyy/MM/dd HH:mm:ss tt", "yyyy-MM-dd HH:mm:ss", "yyyy-M-dd hh:mm:ss", "MM/dd/yyyy" };

            DateTime.TryParseExact(DateDetail, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDateTime);

            return dtDateTime;
        }

        //public static Meeting HttpCallForZoom(string barrerToken, string jsonrequest)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    var stringContent = new StringContent(jsonrequest, UnicodeEncoding.UTF8, "application/json");
        //    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + barrerToken);
        //    var responseData = httpClient.PostAsync(zoomSettings, stringContent).Result;
        //    if (responseData != null)
        //    {
        //        if (responseData.IsSuccessStatusCode)
        //        {
        //            var createdMeeting = JsonConvert.DeserializeObject<Meeting>(responseData.Content.ReadAsStringAsync().Result);
        //            return createdMeeting;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        public static DateTime ConvertString2DateTimeFormtatAdmin(string DateDetail)
        {
            DateTime dtDateTime = new DateTime();

            string[] formats = { "MM/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "d/MM/yyyy HH:mm:ss", "d/MM/yyyy hh:mm:ss tt", "d/MM/yyyy h:mm:ss tt", "d/MM/yyyy hh:mm tt", "d/MM/yyyy HH:mm", "MM/d/yyyy HH:mm:ss", "MM/d/yyyy hh:mm:ss tt", "MM/d/yyyy h:mm:ss tt", "MM/d/yyyy hh:mm tt", "MM/d/yyyy HH:mm", "M/d/yyyy HH:mm:ss", "M/d/yyyy hh:mm:ss tt", "M/d/yyyy h:mm:ss tt", "M/d/yyyy hh:mm tt", "M/d/yyyy HH:mm", "M/dd/yyyy HH:mm:ss", "M/dd/yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss", "M/dd/yyyy h:mm:ss tt", "M/dd/yyyy H:mm:ss", "M/dd/yyyy h:mm:ss", "M/dd/yyyy HH:mm", "M/dd/yyyy hh:mm tt", "M/d/yyyy HH:mm", "dd/MM/yyyy HH:mm", "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm", "d/MM/yyyy HH:mm", "d/MM/yyyy hh:mm", "d/M/yyyy HH:mm", "d/M/yyyy hh:mm", "MM/dd/yyyy HH:mm", "MM/dd/yyyy hh:mm", "MM/d/yyyy HH:mm", "MM/d/yyyy hh:mm", "M/d/yyyy HH:mm", "M/d/yyyy hh:mm", "MM/dd/yyyy hh:mm:ss tt", "MM/dd/yyyy HH:mm:ss tt", "MM/dd/yyyy H:mm:ss tt", "MM/dd/yyyy h:mm:ss tt", "dd/MM/yyyy hh:mm tt", "dd/MM/yyyy hh:mm:ss", "d/MM/yyyy h:mm tt", "d/MM/yyyy h:mm:ss", "d/MM/yyyy h:mm", "d/M/yyyy h:mm", "d/M/yyyy", "M/dd/yyyy", "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd h:mm:ss", "yyyy-mm-dd hh:mm:ss", "yyyy-mm-dd h:mm:ss", "dd/M/yyyy hh:mm:ss", "dd-MM-yyyy h:mm:ss", "dd-MM-yyyy", "yyyy-MM-dd", "MM/dd/yyyy", "MM-dd-yyyy" };

            DateTime.TryParseExact(DateDetail, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDateTime);

            dtDateTime = AddWorkingDays(dtDateTime, 1);
            return dtDateTime;
        }
        public static DateTime AddWorkingDays(this DateTime date, int daysToAdd)
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                daysToAdd = 0;
            }
            else
            {
                while (daysToAdd > 0)
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        daysToAdd = 0;
                    }
                    else
                    {
                        daysToAdd = daysToAdd + 1;
                        date = date.AddDays(daysToAdd);
                        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                        {
                            daysToAdd = 0;
                        }
                    }
                }
            }


            return date;
        }

        public static string StringConcatenate<T>(this IEnumerable<T> source,
           Func<T, string> func)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in source)
                sb.Append(func(item));
            return sb.ToString();
        }

        public static string Encoder(string planstring)
        {
            return UrlEncoder.Default.Encode(planstring);
        }

        public static string CoreHttpCallsPost(string url, string requestJson, string screctKey = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "POST";
                webRequest.Timeout = 500000;
                webRequest.ContentType = "application/json";
                if (!string.IsNullOrEmpty(screctKey))
                    webRequest.Headers.Add("authorization", screctKey);
                webRequest.Credentials = CredentialCache.DefaultCredentials;

                using (var requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(requestJson);
                    requestWriter.Flush();
                    requestWriter.Close();
                }
            }

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Thread.Sleep(1000);
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string ResponseJson = reader.ReadToEnd();

            return ResponseJson;
        }

        public static byte[] UploadImageFromBase64(string base64)
        {
            if (!string.IsNullOrEmpty(base64))
            {

                byte[] imageBytes = Convert.FromBase64String(base64.Split(',')[1]);
                return imageBytes;
            }
            else
            {
                return null;
            }

        }
        public static TimeSpan ConvertString2TimeFormtatAdmin(string TimeDetail)
        {
            TimeSpan time;
            if (!TimeSpan.TryParse(TimeDetail, out time))
            {
                // handle validation error

            }
            return time;
        }

        public static ResponseObject ReturnObject(int code, string message, object details = null)
        {
            return new ResponseObject() { statusCode = code, Message = message, Details = details };
        }

        public static string GetBase64FromImage(string filename,string folderName)
        {
            string base64String = string.Empty;
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Media", folderName);
            string modifiedFilename = string.Format("{0}/{1}", pathToSave, filename);
            if (File.Exists(modifiedFilename))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(modifiedFilename);
                base64String = string.Format("data:image/{0};base64,{1}",Path.GetExtension(modifiedFilename).Replace(".","") , Convert.ToBase64String(imageBytes));
            }
            return base64String;
        }

        /// <summary>
        /// Returns the month difference between the 2 dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        /// <summary>
        /// Method to handle the Get calls
        /// </summary>
        /// <param name="url"></param>
        /// <param name="screctKey"></param>
        /// <returns></returns>
        public static string CoreHttpCallsGet(string url, string screctKey = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            if (webRequest != null)
            {
                webRequest.Method = "GET";
                webRequest.Timeout = 500000;
                webRequest.ContentType = "application/json";
                if (!string.IsNullOrEmpty(screctKey))
                    webRequest.Headers.Add("authorization", screctKey);
                webRequest.Credentials = CredentialCache.DefaultCredentials;
              
            }

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            Thread.Sleep(1000);
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string ResponseJson = reader.ReadToEnd();

            return ResponseJson;
        }

        #endregion
    }
}