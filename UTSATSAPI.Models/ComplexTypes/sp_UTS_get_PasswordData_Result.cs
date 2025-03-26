using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_get_PasswordData_Result
    {
        public string? Value { get; set; }
    }
}
