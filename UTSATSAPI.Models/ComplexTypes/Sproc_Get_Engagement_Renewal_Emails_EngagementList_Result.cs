using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Engagement_Renewal_Emails_EngagementList_Result
    {
        public long? OnBoardID { get; set; }
        public string? EngagemenID { get; set; }
    }
}
