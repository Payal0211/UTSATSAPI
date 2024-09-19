using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Add_CompanyData_Send_Details_To_ATS_Result
    {
        public long? CompanyID { get; set; }
    }
}
