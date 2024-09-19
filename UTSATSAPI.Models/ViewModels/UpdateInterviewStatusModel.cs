using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateInterviewStatusModel
    {
        public long? HrID { get; set; }
        public int? InterviewStatusID { get; set; }
        public long? InterviewMasterID { get; set; }
        public long? TalentSelectedInterviewDetailsID { get; set; }
    }
}
