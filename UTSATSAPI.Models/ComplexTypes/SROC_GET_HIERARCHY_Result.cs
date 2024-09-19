using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class SROC_GET_HIERARCHY_Result
    {
        public Nullable<long> UserID { get; set; }
        public Nullable<int> UNDER_PARENT { get; set; }
        public string child { get; set; }
        public string? parent { get; set; }
        public int childExists { get; set; }
    }
}
