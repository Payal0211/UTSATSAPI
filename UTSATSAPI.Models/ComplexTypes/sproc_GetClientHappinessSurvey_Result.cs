using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetClientHappinessSurvey_Result
    {
        public long ID { get; set; }
        public string AddedDate { get; set; }
        public bool Is_EmailSend { get; set; }
        public string FeedbackDate { get; set; }
        public string Company { get; set; }
        public string Client { get; set; }
        public string Email { get; set; }
        public string Other_Company_Name { get; set; }
        public string Other_Client_Name { get; set; }
        public string Other_ClientEmailID { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Question { get; set; }
        public string Options { get; set; }
        public string Comments { get; set; }
        public string Link { get; set; }
        public int TotalRecords { get; set; }
        public string Category { get; set; }
        public string Sales { get; set; }
        public string FeedbackStatus { get; set; }  
        public string TestimonialOptions { get; set; }
        public long? SaleUserId { get; set; }
    }
}
