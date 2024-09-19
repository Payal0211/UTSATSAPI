using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetClientHappynessSurveyFeedbackDetailForEmail_Result
    {
        public string? Company { get; set; }
        public string? Client { get; set; }
        public string? Email { get; set; }
        public int? Rating { get; set; }
        public string? Question { get; set; }
        public string? Options { get; set; }
        public string? Comments { get; set; }
    }
}
