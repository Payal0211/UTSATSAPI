namespace UTSATSAPI.Helpers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Org.BouncyCastle.Crypto;
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Channels;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModels;
    public class ExternalCalls
    {
        #region Variables
        private static string _easternZoneId = "Central Standard Time";
        private static string _apiSecret = "UloZVWsDym07IqAm2KQIY2xdgPbYmhuOPFFM";
        private static string _Issuer = "NA0TBfL6QdWnMymoceTgKA";     
        #endregion       

        //#region Public Methods
        //public static Meeting ToScheduleZoomMeeting(string timeZone, DateTime dateTime, string googleMeetUrl = "")
        //{
        //    Meeting meeting = new Meeting();
        //    var now = DateTime.Now;
        //    DateTime easternTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        //    TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(_easternZoneId);
        //    var dateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(easternTime, easternZone);

        //    byte[] symmetricKey = Encoding.ASCII.GetBytes(_apiSecret);
        //    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        //    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Issuer = _Issuer,
        //        Expires = now.AddMinutes(60),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    var tokenString = tokenHandler.WriteToken(token);
        //    //var tokenString = BearerToken;

        //    string password = Guid.NewGuid().ToString("N").ToLower()
        //              .Replace("1", "").Replace("o", "").Replace("0", "")
        //              .Substring(0, 10);
        //    Meeting iMeeting = new Meeting();

        //    iMeeting.topic = $"TC Host";
        //    iMeeting.type = 2;
        //    iMeeting.timezone = timeZone;
        //    iMeeting.start_time = dateTime.ToString("s");
        //    iMeeting.duration = 60;
        //    iMeeting.password = password;
        //    Settings iMeetingSettings = new Settings();
        //    iMeetingSettings.join_before_host = true;
        //    iMeetingSettings.jbh_time = 5;
        //    iMeeting.settings = iMeetingSettings;
        //    iMeetingSettings.approval_type = 2;
        //    iMeetingSettings.show_share_button = true;

        //    var json = JsonConvert.SerializeObject(iMeeting);

        //    meeting = CommonLogic.HttpCallForZoom(tokenString, json);

        //    // Create google meet link when zoom link not generated.
        //    if (meeting == null) 
        //    {
        //        googleMeetUrl = string.Format(googleMeetUrl, dateTime.ToString("s"), dateTime.AddMinutes(60).ToString("s"), timeZone);
        //        string content;

        //        using (WebResponse wr = WebRequest.Create(googleMeetUrl).GetResponse())
        //        {
        //            using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
        //            {
        //                content = sr.ReadToEnd();
        //                content = content.Replace("\r\n", "");
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(content))
        //        {
        //            meeting = new Meeting();
        //            meeting.join_url = content;
        //            meeting.host_email = "hr@uplers.in";
        //            int index = content.IndexOf(".com/") + 5;
        //            meeting.id = content.Substring(index);
        //            meeting.start_time = dateTime.ToString("s");
        //            meeting.duration = 60;
        //        }
        //    }           

        //    return meeting;
        //}
        //#endregion

    }
}
