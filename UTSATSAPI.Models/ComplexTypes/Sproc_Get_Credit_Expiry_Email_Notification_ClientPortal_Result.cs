using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Credit_Expiry_Email_Notification_ClientPortal_Result
    {
        public long HRID { get; set; }
        public long ContactID { get; set; }
        public long CompanyID { get; set; }
        public int CreditBalance { get; set; }
        public string RequestForTalent { get; set; }
        public string HR_Number { get; set; }
        public string JobPostedDate { get; set; }
        public int DaysPending { get; set; }
        public long PocId { get; set; }
        public int JobPostCount { get; set; }
        public int ProfileSharedCount { get; set; }
        public int UsedCredit { get; set; }
        public int JobPostCountpayperHire { get; set; }
    }
}
