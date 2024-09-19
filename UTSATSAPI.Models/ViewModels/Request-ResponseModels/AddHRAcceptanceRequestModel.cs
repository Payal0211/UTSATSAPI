using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels.Request_ResponseModels
{
    public class AddHRAcceptanceRequestModel
    {
        public long TalentId { get; set; }
        public long LoggedInUserId { get; set; }
        public int TotalCount { get; set; }
        public int TotalCountAvailability { get; set; }
        public int TotalCountHowSoon { get; set; }
        public long HRAcceptanceDetailHowSoon { get; set; }
        public long HRAcceptanceDetailAvailability { get; set; }
        public long HRAcceptanceDetail { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_Result> HRAcceptanceDetailList { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_Availability_Result> HRAcceptanceDetailAvailabilityList { get; set; }
        public List<Sproc_GET_PostAcceptance_detail_HowSoon_Result> HRAcceptanceDetailHowSoonList { get; set; }
    }
}
