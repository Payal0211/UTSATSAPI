using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Check_Validation_Message_For_User_Edit_TR_Result
    {
        public Nullable<int> Status { get; set; }
        public string? Message { get; set; }
    }
}
