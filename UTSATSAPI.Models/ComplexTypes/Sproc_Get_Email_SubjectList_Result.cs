using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Email_SubjectList_Result
    {
        public long? ID { get; set; }
        public string? DisplayName { get; set; }
    }
}
