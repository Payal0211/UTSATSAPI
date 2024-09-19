using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public  class Sproc_Get_AMNBD_For_Replacement_Result
    {
        public long ID { get; set; }
        public string Fullname { get; set; }
    }
}
