using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class ViewClient
    {
        public sp_UTS_GetCompanyClientDetails_Result ClientDetails { get; set; }
        public List<sp_UTS_GetClientWiseHRDetails_Result> HRList { get; set; }
    }


}
