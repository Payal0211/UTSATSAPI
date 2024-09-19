using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_GetClientHappinessSurveyOptions_Result
    {
        public int? ID { get; set; }
        public string? HappynessSurvay_Option { get; set; }
    }
}
