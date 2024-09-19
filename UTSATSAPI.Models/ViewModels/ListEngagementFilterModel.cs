using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ListEngagementFilterModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public FilterFieldsEngagement filterFieldsEngagement { get; set; }
        public bool IsExport { get; set; }
        public string? searchText { get; set; }
    }

    public class FilterFieldsEngagement
    {
        public string ClientFeedback { get; set; }
        public string TypeOfHiring { get; set; }

        public string CurrentStatus { get; set; }
        public string TSCName { get; set; }

        public string Company { get; set; }
        public string GEO { get; set; }
        public string Position { get; set; }
        public int EngagementTenure { get; set; }
        public string NBDName { get; set; }
        public string AMName { get; set; }
        public string Pending { get; set; }
        public int SearchMonth { get; set; }
        public int SearchYear { get; set; }
        public string SearchType { get; set; }

        public string Islost { get; set; }

        public string EngagementId  { get; set; }
        public string HRId  { get; set; }
        public string ClientIds { get; set; }

        public string TalentIds { get; set; }

        public string DeployedSource { get; set; }   

        public int LoggedInUser { get; set; }
        public string OnBoardLostReasons { get; set; }
    }
}
