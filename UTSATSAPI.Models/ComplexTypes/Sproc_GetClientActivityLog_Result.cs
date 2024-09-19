using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class Sproc_GetClientActivityLog_Result
    {
        public string? DisplayName { get; set; }
        public Nullable<System.DateTime> CreatedByDatetime { get; set; }
        public string? Name { get; set; }
        public string? TName { get; set; }
    }
}
