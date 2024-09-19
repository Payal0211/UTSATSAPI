using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels
{
    public class OnBoardTalents_ClientFeedback
    {
        public long HiringRequest_ID { get; set; }
        public long ContactID { get; set; }
        public long OnBoardID { get; set; }
        public string FeedbackType { get; set; }
        public string FeedbackComment { get; set; }
        public string FeedbackActionToTake { get; set; }
        public Nullable<System.DateTime> FeedbackCreatedDateTime { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<int> ModifiedByID { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public List<SelectListItem> drpFeedbackType { get; set; }
        public string HRNumber { get; set; }
        public string TalentName { get; set; }
        public Nullable<long> TalentID { get; set; }
        public string EngagemenID { get; set; }
        public string SupportingFilename { get; set; }
    }
}
