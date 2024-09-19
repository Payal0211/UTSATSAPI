using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class SLAViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IEnumerable<SelectListItem> SalesManager { get; set; }
        public IEnumerable<SelectListItem> SV { get; set; }
        public IEnumerable<SelectListItem> SalesPerson { get; set; }
        public IEnumerable<SelectListItem> Stages { get; set; }
        public IEnumerable<SelectListItem> PoolODR { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public Dictionary<int, string> BindODR_Pool { get; set; }
        public Dictionary<int, string> BindAMNDB { get; set; }
        public IEnumerable<SelectListItem> AmBDRDrp { get; set; }
        public IEnumerable<SelectListItem> ODRPoolDrop { get; set; }
        public int SLAType { get; set; }
        public long? HRID { get; set; }
        public long SalesManagerID { get; set; }
        public long SVID { get; set; }
        public long OpsLead { get; set; }
        public long SalesPersonId { get; set; }
        public string Stage { get; set; }
        public int IsAdHoc { get; set; }
        public string Role { get; set; }
        public string HRNumber { get; set; }
        public string Company { get; set; }
        public string CurrentActionDate { get; set; }
        public string IsAdHocHR { get; set; }
        public IEnumerable<SelectListItem> ActionFilterDrop { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public int ActionFilter { get; set; }
        public List<Sproc_Get_SalesHead_Users_Result> salesHead_List { get; set; }
    }
}
