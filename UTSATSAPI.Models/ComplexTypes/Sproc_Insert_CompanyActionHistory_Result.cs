using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Insert_CompanyActionHistory_Result
    {
        public long InsertedID { get; set; }
    }
}
