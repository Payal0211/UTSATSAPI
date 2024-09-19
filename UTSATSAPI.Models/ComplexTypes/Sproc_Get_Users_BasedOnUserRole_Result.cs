using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Users_BasedOnUserRole_Result
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
    }
}
