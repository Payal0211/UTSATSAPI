using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ContactTalentPriorityResponseModel
    {
        public ContactTalentPriorityResponseModel()
        {
            TalentDetails = new();
        }
        public long HRID { get; set; }
        public List<outTalentDetail> TalentDetails { get; set; }
        public int HRStatusID { get; set; }
        public string? HRStatus { get; set; }

    }
   
}
