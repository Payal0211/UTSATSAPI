using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public  class sproc_Get_TSC_List_Result
    {
        public long ID { get; set; }
        public string FullName { get; set; }
    }
}
