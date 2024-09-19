using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModel;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;
using AuthorizeAttribute = UTSATSAPI.Helpers.Common.AuthorizeAttribute;

namespace UTSATSAPI.Controllers
{
    [Route("SLAReport/", Name = "SLAReport")]
    [ApiController]
    public class SLAReportController : ControllerBase
    {
        #region Variable
        public ISLAReport sLAReport;
        #endregion

        #region Constructor
        public SLAReportController(ISLAReport _sLAReport)
        {
            sLAReport = _sLAReport;
        }
        #endregion

        #region Public Method
        [Authorize]
        [HttpGet("GETSLAFilters")]
        public async Task<ObjectResult> GETSLAFilters()
        {
            try
            {
                SLAViewModel sLAViewModel = sLAReport.GetSLAReportDetail();

                var Heads = sLAReport.Sproc_Get_SalesHead_Users_Result().Result.ToList();

                sLAViewModel.salesHead_List = Heads;

                if (sLAViewModel != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = sLAViewModel });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }

        }

        [Authorize]
        [HttpPost("GetSLAReport")]
        public async Task<IActionResult> GetSLAReport(ListSLAFilterModel listSLAFilterModel)
        {
            try
            {
                #region Pre-Validation
                if (listSLAFilterModel == null || (listSLAFilterModel.pagenumber == 0 || listSLAFilterModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                listSLAFilterModel.totalrecord = (listSLAFilterModel.IsExport) ? 0 : listSLAFilterModel.totalrecord;
                object[] param = new object[] {
                    listSLAFilterModel.pagenumber, listSLAFilterModel.totalrecord,
                    listSLAFilterModel.filterFieldsSLA.StartDate,
                    listSLAFilterModel.filterFieldsSLA.EndDate,
                    listSLAFilterModel.filterFieldsSLA.HRID,
                    listSLAFilterModel.filterFieldsSLA.Sales_ManagerID,
                    listSLAFilterModel.filterFieldsSLA.Ops_Lead,
                    listSLAFilterModel.filterFieldsSLA.SalesPerson,
                    listSLAFilterModel.filterFieldsSLA.Stage,
                    listSLAFilterModel.filterFieldsSLA.IsAdHoc,
                    listSLAFilterModel.filterFieldsSLA.Role,
                    listSLAFilterModel.filterFieldsSLA.SLAType,
                    listSLAFilterModel.filterFieldsSLA.Type,
                    listSLAFilterModel.filterFieldsSLA.HR_Number,
                    listSLAFilterModel.filterFieldsSLA.Company,
                    listSLAFilterModel.filterFieldsSLA.ActionFilter,
                    listSLAFilterModel.filterFieldsSLA.AMBDR,
                    listSLAFilterModel.filterFieldsSLA.StageIDs,
                    listSLAFilterModel.filterFieldsSLA.ActionFilterIDs,
                    listSLAFilterModel.filterFieldsSLA.CompanyIds,
                    listSLAFilterModel.filterFieldsSLA.IsHRFocused ?? null,
                    listSLAFilterModel.filterFieldsSLA.Sales_ManagerIDs??null,
                    listSLAFilterModel.filterFieldsSLA.Heads??null
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<Sproc_SLA_Report_For_Static_Stages_Result> list = await sLAReport.GetSLAReportData(paramasString).ConfigureAwait(false);
                if (list.Any())
                {

                    if (listSLAFilterModel.IsExport)
                    {
                        return ExportToExcelSLAReport(list);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(list, list.Count(), listSLAFilterModel.totalrecord, listSLAFilterModel.pagenumber) });
                    }
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("OverAllSLASummary")]
        public async Task<ObjectResult> OverAllSLASummary(FilterFieldsSLA filterFieldsSLA)
        {
            try
            {
                #region Pre-Validation
                if (filterFieldsSLA == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data" });
                #endregion

                object[] param = new object[] {
                    filterFieldsSLA.StartDate,
                    filterFieldsSLA.EndDate,
                    filterFieldsSLA.HRID,
                    filterFieldsSLA.Sales_ManagerID,
                    filterFieldsSLA.Ops_Lead,
                    filterFieldsSLA.SalesPerson,
                    filterFieldsSLA.Stage,
                    filterFieldsSLA.IsAdHoc,
                    filterFieldsSLA.Role,
                    filterFieldsSLA.HR_Number,
                    filterFieldsSLA.Company,
                    filterFieldsSLA.ActionFilter,
                    filterFieldsSLA.AMBDR,
                    filterFieldsSLA.StageIDs,
                    filterFieldsSLA.ActionFilterIDs,
                    filterFieldsSLA.CompanyIds,
                    filterFieldsSLA.IsHRFocused ?? null,
                    filterFieldsSLA.Sales_ManagerIDs??null,
                    filterFieldsSLA.Heads??null
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result> list = await sLAReport.OverAllSLASummary(paramasString).ConfigureAwait(false);
                if (list.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = list });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost("SLAMissedSummary")]
        public async Task<ObjectResult> SLAMissedSummary(FilterFieldsSLA filterFieldsSLA)
        {
            try
            {
                #region Pre-Validation
                if (filterFieldsSLA == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data" });
                #endregion

                object[] param = new object[] {
                    filterFieldsSLA.StartDate,
                    filterFieldsSLA.EndDate,
                    filterFieldsSLA.HRID,
                    filterFieldsSLA.Sales_ManagerID,
                    filterFieldsSLA.Ops_Lead,
                    filterFieldsSLA.SalesPerson,
                    filterFieldsSLA.Stage,
                    filterFieldsSLA.IsAdHoc,
                    filterFieldsSLA.Role,
                    filterFieldsSLA.HR_Number,
                    filterFieldsSLA.Company,
                    filterFieldsSLA.ActionFilter
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<Sproc_SLA_Missed_Summary_Report_Result> list = await sLAReport.SLAMissedSummary(paramasString);
                if (list.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = list });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("FetchUserBasedonAmNBD")]
        public async Task<ObjectResult> FetchUserBasedonAmNBD(int? Id)
        {
            try
            {
                if (Id == null || Id < 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data" });
                IEnumerable<SelectListItem> data = sLAReport.FetchUserBasedonAmNBD(Id);
                if (data != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = data });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost("FetchManagerBasedonAmNBD")]
        public async Task<ObjectResult> FetchManagerBasedonAmNBD(int? Id)
        {
            try
            {
                if (Id == null || Id < 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data" });
                IEnumerable<SelectListItem> data = sLAReport.FetchManagerBasedonAmNBD(Id);
                if (data != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = data });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost("ExportToExcelSLAReport")]
        public IActionResult ExportToExcelSLAReport(List<Sproc_SLA_Report_For_Static_Stages_Result> DetailList)
        {
            var ExportFileName = "SLAReportExport_" + DateTime.Now.ToString("yyyyMMdd") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("SLAReportExport");

            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "ID";
            worksheet.FirstRow().Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Value = "HR_NUmber";
            worksheet.Cell(currentRow, 3).Value = "Company";
            worksheet.Cell(currentRow, 4).Value = "Client";
            worksheet.Cell(currentRow, 5).Value = "CurrentStage";
            worksheet.Cell(currentRow, 6).Value = "NextStage";
            worksheet.Cell(currentRow, 7).Value = "Current_Action_date";
            worksheet.Cell(currentRow, 8).Value = "TalentName";
            worksheet.Cell(currentRow, 9).Value = "Expected_Next_action_date";
            worksheet.Cell(currentRow, 10).Value = "Actual_Next_Action_date";
            worksheet.Cell(currentRow, 11).Value = "Expected_SLA_day";
            worksheet.Cell(currentRow, 12).Value = "Actual_SLA_day";
            worksheet.Cell(currentRow, 13).Value = "Sales_Person";
            worksheet.Cell(currentRow, 14).Value = "Sales_Manager";
            worksheet.Cell(currentRow, 15).Value = "Ops_Lead";
            worksheet.Cell(currentRow, 16).Value = "Role";
            worksheet.Cell(currentRow, 17).Value = "IsAdHocHR";
            worksheet.Cell(currentRow, 18).Value = "ActionFilter";


            foreach (var details in DetailList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = details.ID;
                worksheet.Cell(currentRow, currentColumn++).Value = details.HR_NUmber;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Company;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Client;
                worksheet.Cell(currentRow, currentColumn++).Value = details.CurrentStage;
                worksheet.Cell(currentRow, currentColumn++).Value = details.NextStage;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Current_Action_date;
                worksheet.Cell(currentRow, currentColumn++).Value = details.TalentName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Expected_Next_action_date;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Actual_Next_Action_date;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Expected_SLA_day;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Actual_SLA_day;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Sales_Person;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Sales_Manager;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Ops_Lead;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Role;
                worksheet.Cell(currentRow, currentColumn++).Value = details.IsAdHocHR;
                worksheet.Cell(currentRow, currentColumn++).Value = details.ActionFilter;

            }
            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }
        #endregion
    }
}