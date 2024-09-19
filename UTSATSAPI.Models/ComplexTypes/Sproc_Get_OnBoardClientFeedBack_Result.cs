using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_OnBoardClientFeedBack_Result
    {
        public string FeedbackComment { get; set; }
        public string FeedbackType { get; set; }
        public string FeedbackActionToTake { get; set; }
        public string FeedbackCreatedDateTime { get; set; }
        public int TotalRecords { get; set; }
        public long OnBoardID { get; set; }
        public string EngagemenID { get; set; }
    }
}
