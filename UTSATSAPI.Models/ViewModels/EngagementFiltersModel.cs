using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace UTSATSAPI.Models.ViewModels
{
    public class EngagementFiltersModel
    {
        public List<SelectListItem> ClientFeedback { get; set; }
        public List<SelectListItem> TypeOfHiring { get; set; }
        public List<SelectListItem> CurrentStatus { get; set; }
        public List<SelectListItem> TSCName { get; set; }

        // Commented by Ashwin on 26 jul,2023
        // because of large data it is taking time to load on REACT side.
        //public IEnumerable<SelectListItem> Company { get; set; }
        public IEnumerable<SelectListItem> GEO { get; set; }
        public IEnumerable<SelectListItem> Postion { get; set; }
        public List<SelectListItem> EngagementTenure { get; set; }
        public IEnumerable<SelectListItem> NBDName { get; set; }
        public IEnumerable<SelectListItem> AMName { get; set; }
        public List<SelectListItem> Pending { get; set; }
        public List<SelectListItem> Lost { get; set; }
        public IEnumerable<SelectListItem> OnBoardingLostReasons { get; set; }
        public IEnumerable<SelectListItem> Months
        {
            get
            {
                return DateTimeFormatInfo
                       .InvariantInfo
                       .MonthNames
                       .Where((monthname, index) => monthname != string.Empty)
                       .Select((monthName, index) => new SelectListItem
                       {
                           Value = (index + 1).ToString(),
                           Text = monthName
                       });
            }
        }
        public int Month { get; set; }
        public int year { get; set; }
        public List<SelectListItem> SearchType { get; set; }
        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> DeployedSource { get; set; }
        

        // Commented by Ashwin on 26 jul,2023
        // because of large data it is taking time to load on REACT side.
        //public IEnumerable<SelectListItem> EngagementIds { get; set; }
        //public IEnumerable<SelectListItem> HRIds { get; set; }
        //public IEnumerable<SelectListItem> Clients { get; set; }
        //public IEnumerable<SelectListItem> Talents { get; set; }
    }
}
