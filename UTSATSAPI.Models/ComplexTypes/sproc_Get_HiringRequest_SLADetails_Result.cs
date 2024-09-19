using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_HiringRequest_SLADetails_Result
    {
        public int RowNo { get; set; }
        public long HrID { get; set; }
        public string? JobTitle { get; set; }
        public int? RequiredTalents { get; set; }
        public int? StageID { get; set; }
        public string? StageName { get; set; }
        public int? NoOfTalents { get; set; }
        public string? SLADate { get; set; }
        public string? SLAStatus { get; set; }
        public string? CompletedStage { get; set; }
        public int? CompletedStageID { get; set; }
        //public bool? ShowSLATab { get; set; }
    }
}
