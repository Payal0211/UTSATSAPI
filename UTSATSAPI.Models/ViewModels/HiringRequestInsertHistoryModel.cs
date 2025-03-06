using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class HiringRequestInsertHistoryModel
    {       
        public long HiringRequestId { get; set; } = 0;
        public long TalentId { get; set; } = 0;
        public bool CreatedFrom { get; set; } = false;
        public long LoggedInUserId { get; set; }
        public long? ContactTalentPriorityId { get; set; } = 0;
        public long? InterviewMasterId { get; set; } = 0;
        public string HRAcceptedDateTime { get; set; } = string.Empty;
        public long? OnBoardId { get; set; } = 0;
        public bool? IsManagedByClient { get; set; } = false;
        public bool? IsManagedByTalent { get; set; } = false;
        public int? SalesUserId { get; set; } = 0;
        public int? OldSalesUserId { get; set; } = 0;        
    }
}
