using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ISLAReport
    {
        SLAViewModel GetSLAReportDetail();
        Task<List<Sproc_SLA_Report_For_Static_Stages_Result>> GetSLAReportData(string param);
        Task<List<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result>> OverAllSLASummary(string param);
        Task<List<Sproc_SLA_Missed_Summary_Report_Result>> SLAMissedSummary(string param);
        IEnumerable<SelectListItem> FetchUserBasedonAmNBD(int? Id);
        IEnumerable<SelectListItem> FetchManagerBasedonAmNBD(int? Id);
        Task<List<Sproc_Get_SalesHead_Users_Result>> Sproc_Get_SalesHead_Users_Result();
    }
}
