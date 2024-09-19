using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_TalentDetails_For_ContractualtoDP_Conversion_Result
    {
        public string Talentname { get; set; }
        public long TalentID { get; set; }
        public decimal? ExpectedCTC { get; set; }
        public decimal? DPPercentage { get; set; }
        public decimal? DPAmount { get; set; }
        public long? ContactTalentPriorityID { get; set; }
        public long? OnBoardID { get; set; }
        public long HRID { get; set; }
        public decimal? CurrentCTC { get; set; }
    }
}
