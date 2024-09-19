using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class sproc_UTS_GetDealActivity_Result
    {
        public string SubscriptionType { get; set; }
        public string CreatedDateTime { get; set; }
    }
}
