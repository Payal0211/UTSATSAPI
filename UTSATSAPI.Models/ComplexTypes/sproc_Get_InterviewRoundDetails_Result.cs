using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_InterviewRoundDetails_Result
    {
        public long? Talent_ID { get; set; }
        public string? StrInterviewRound { get; set; }
    }
}
