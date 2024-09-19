using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class OnBoardListViewModel
    {
        public int pagenumber { get; set; }
        public int totalrecord { get; set; }
        public FilterFields_OnBoard filterFields_OnBoard { get; set; }
    }
    public class FilterFields_OnBoard
    {
        public string Engagement_Id { get; set; }
        public string Position { get; set; }
        public string Client { get; set; }
        public string Talent { get; set; }
        public string HR_Number { get; set; }
        public string Company { get; set; }
        public decimal Final_HR_Cost { get; set; }
        public decimal Talent_Cost { get; set; }
        public decimal NRPercentage { get; set; }
        public string SalesPerson { get; set; }
        public string AMAssignmentuser { get; set; }
        public string ContractStatus { get; set; }
        public string IsLost { get; set; }
        public decimal DPAmount { get; set; }
        public decimal DP_Percentage { get; set; }
        public string TypeOfHR { get; set; }
        public string search { get; set; }
    }

}
