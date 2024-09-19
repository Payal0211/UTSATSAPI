using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    [Keyless]
    public class Sproc_Get_AWS_SES_Tracking_Details_Popup_Result
    {
        public string? MessageID { get; set; }
        public long? ClientID { get; set; }
        public string? Client { get; set; }
        public string? EventType { get; set; }
        public string? Email_Subject { get; set; }
        public string? User_Agent { get; set; }
        public string? IpAddress { get; set; }
        public string? Email_Link { get; set; }
        public string? Email_LinkTags { get; set; }
        public string? TrackingDate { get; set; }
        public string? bounceType { get; set; }
        public string? bounceSubType { get; set; }
        public string? bouncedRecipients { get; set; }
    }
}
