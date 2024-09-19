using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_JobPost_Details_For_HomePage_ClientPortal_Result
    {
        public long ClientID { get; set; }
        public long HRID { get; set; }
        public string? JobTitle { get; set; }
        public string? CompletedStage { get; set; }
        public int? CompletedStageID { get; set; }
        public int NoOfApplicants { get; set; }
        public int NoOfTalentsinScreening { get; set; }
        public int NoOfTalentsinVetting { get; set; }
        public int NoOfTalentsinMatcherInterview { get; set; }
        public int NoOfTalentShortlisted { get; set; }
        public int NoOfTalentinInterview { get; set; }
        public int NoOfTalentHired { get; set; }
        public int NoOfTalentOnBoraded { get; set; }
        public string? ActiveStatusBeforeMessage { get; set; }
        public string? ActiveStatusAFterMessage { get; set; }
        public int? NoOfTalents { get; set; }
        public string? SLADate { get; set; }
        public string? SLADateStatus { get; set; }
    }
}
