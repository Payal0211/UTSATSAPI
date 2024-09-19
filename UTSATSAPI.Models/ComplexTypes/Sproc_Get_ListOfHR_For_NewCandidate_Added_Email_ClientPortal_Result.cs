using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_ListOfHR_For_NewCandidate_Added_Email_ClientPortal_Result
    {
        public long? HRID { get; set; }
        public long? ContactID { get; set; }
        public string? EmailID { get; set; }
        public string? FullName { get; set; }
        public string? RequestForTalent { get; set; }
        public int? NoOfTalents { get; set; }
        public int? IsAITalent { get; set; }
    }
}
