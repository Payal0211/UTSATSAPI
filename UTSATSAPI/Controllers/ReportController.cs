using Aspose.Words;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Models.ViewModels.Reports;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Route("Report/", Name = "Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        #region Variables
        private readonly IReport _iReport;
        #endregion

        #region Constructors
        public ReportController(IReport reportInterface)
        {
            _iReport = reportInterface;
        }
        #endregion

        #region Public API

        #region Demand Funnel Report

        [Authorize]
        [HttpGet("DemandFunnel/Filters")]
        public async Task<ObjectResult> GetDemandFunnel_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                dObject.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(dObject.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = Convert.ToDateTime(dObject.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");

                dObject.HiringNeed = new List<SelectListItem>();
                dObject.HiringNeed.Add(new SelectListItem() { Value = "--Select--", Text = "0" });
                dObject.HiringNeed.Add(new SelectListItem() { Value = "Yes, it's for a limited Project", Text = "1" });
                dObject.HiringNeed.Add(new SelectListItem() { Value = "No, They want to hire for long term", Text = "2" });

                var modeOfWorkingList = await _iReport.GetALLModeOFWorkingByCondition(x => x.IsActive == true).ConfigureAwait(false);

                dObject.ModeOfWorking = modeOfWorkingList.Select(x => new SelectListItem
                {
                    Value = x.ModeOfWorking,
                    Text = x.Id.ToString()
                }).ToList();
                dObject.ModeOfWorking.Insert(0, new SelectListItem() { Text = "0", Value = "--Select--" });

                dObject.TypeOfHR = new List<SelectListItem>();
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "--Select--", Text = "-1" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Contractual", Text = "0" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Direct Placement", Text = "1" });

                var Company_Categories = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "A" },
                    { "2", "B" },
                    { "3", "C" }
                };

                dObject.CompanyCategory = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Key.ToString()
                });

                var Replacement = new Dictionary<string, string>
                {
                    { "0", "No" },
                    { "1", "Yes" },
                };
                dObject.Replacement = Replacement.ToList().Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Key.ToString()
                });

                var TeamManagerUserList = await _iReport.GetALLUsrUserByCondition(x => x.DeptId == 1 && x.LevelId == 1 && x.UserTypeId == 9 && x.IsActive == true).ConfigureAwait(false);

                dObject.TeamManager = TeamManagerUserList.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                }).ToList();

                //UTS-3987: Add the Lead types as a filter.
                List<SelectListItem> bindLeadTypes = new List<SelectListItem>();

                SelectListItem bindLeadType1 = new SelectListItem();
                bindLeadType1.Text = "1";
                bindLeadType1.Value = "InBound";
                bindLeadTypes.Add(bindLeadType1);

                SelectListItem bindLeadType2 = new SelectListItem();
                bindLeadType2.Text = "2";
                bindLeadType2.Value = "OutBound";
                bindLeadTypes.Add(bindLeadType2);

                dObject.LeadSource = bindLeadTypes;

                var LeadTypeList = await _iReport.GetALLUsrUserByCondition(x => x.DeptId == 1 && x.LevelId == 1 && (x.UserTypeId == 11 || x.UserTypeId == 12) && x.IsActive == true).ConfigureAwait(false);

                dObject.LeadTypeList = LeadTypeList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = (x.UserTypeId == 12 ? "InBound" : "OutBound") + " - " + Convert.ToString(x.FullName)
                }).ToList();

                dObject.LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("DemandFunnel/Listing")]
        public async Task<ObjectResult> GetDemandFunnel_Listing(DemandFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate, filter.EndDate, "", "",
                    string.IsNullOrEmpty(filter.IsHiringNeedTemp) ? "0" : filter.IsHiringNeedTemp,
                    string.IsNullOrEmpty(filter.ModeOfWork) ? "0" : filter.ModeOfWork,
                    string.IsNullOrEmpty(filter.TypeOfHR) ? "-1" : filter.TypeOfHR,
                    string.IsNullOrEmpty(filter.CompanyCategory) ? "" : filter.CompanyCategory,
                    string.IsNullOrEmpty(filter.Replacement) ? "0" : filter.Replacement,
                    string.IsNullOrEmpty(filter.Head) ? "" : filter.Head,
                    filter.LeadUserID ?? 0,
                    filter.IsHRFocused ?? null,
                    string.IsNullOrEmpty(filter.Geos)?"":filter.Geos
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                string DFRDataJson = "";

                if (filter.IsActionWise)
                {
                    DFRDataJson = await (_iReport.DFR_ActionWise_Listing(paramasString).ConfigureAwait(false));
                }
                else
                    DFRDataJson = await (_iReport.DFR_HRWise_Listing(paramasString).ConfigureAwait(false));

                if (DFRDataJson.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DFRDataJson });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("DemandFunnel/Summary")]
        public async Task<ObjectResult> GetDemandFunnel_Summary(DemandFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate, filter.EndDate, "","",
                    filter.IsHiringNeedTemp ?? "0",
                    filter.ModeOfWork ?? "0",
                    string.IsNullOrEmpty(filter.TypeOfHR) ? "-1" : filter.TypeOfHR,
                    filter.CompanyCategory ?? "",
                    filter.Replacement ?? "0",
                    filter.Head ?? "",
                    filter.LeadUserID ?? 0,
                    filter.IsHRFocused ?? null,
                    string.IsNullOrEmpty(filter.Geos)?"":filter.Geos
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                var summaryData = string.Empty;

                if (filter.IsActionWise)
                {
                    summaryData = await (_iReport.DFR_ActionWise_Summary(paramasString).ConfigureAwait(false));
                }
                else
                    summaryData = await (_iReport.DFR_HRWise_Summary(paramasString).ConfigureAwait(false));

                if (!string.IsNullOrEmpty(summaryData))
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = summaryData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("DemandFunnel/HRDetails")]
        public async Task<IActionResult> GetDemandFunnel_HrDetails(Demand_HrDetailPopUpFilter popUpFilter)
        {
            try
            {
                if (popUpFilter == null || string.IsNullOrEmpty(popUpFilter.CurrentStage))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Current stage" });
                }

                int SelectedRow_Manager_ID = 0;

                if (!string.IsNullOrEmpty(popUpFilter.TeamManagerName))
                {
                    UsrUser UserDetails = await _iReport.GetUsrUserByCondition(x => x.FullName.ToLower() == popUpFilter.TeamManagerName.ToLower() && x.UserTypeId == 9).ConfigureAwait(false);

                    if (UserDetails != null)
                        SelectedRow_Manager_ID = Convert.ToInt32(UserDetails.Id);
                }

                if (string.IsNullOrEmpty(popUpFilter.FunnelFilter.StartDate) && string.IsNullOrEmpty(popUpFilter.FunnelFilter.EndDate))
                {
                    popUpFilter.FunnelFilter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.EndDate = Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    popUpFilter.AdhocType,
                    SelectedRow_Manager_ID,
                    popUpFilter.CurrentStage,
                    popUpFilter.FunnelFilter.StartDate,
                    popUpFilter.FunnelFilter.EndDate,
                    "", "",
                    popUpFilter.FunnelFilter.IsActionWise,
                    popUpFilter.FunnelFilter.ModeOfWork ?? "0",
                    popUpFilter.HrFilter.HR_No,
                    popUpFilter.HrFilter.SalesPerson,
                    popUpFilter.HrFilter.CompnayName,
                    popUpFilter.HrFilter.Role,
                    popUpFilter.HrFilter.Managed_Self,
                    popUpFilter.HrFilter.Availability,
                    popUpFilter.HrFilter.TalentName,
                    popUpFilter.FunnelFilter.IsHiringNeedTemp ?? "0",
                    string.IsNullOrEmpty(popUpFilter.FunnelFilter.TypeOfHR) ? "-1" : popUpFilter.FunnelFilter.TypeOfHR,
                    popUpFilter.FunnelFilter.CompanyCategory ?? "",
                    popUpFilter.FunnelFilter.Replacement ?? "0",
                    popUpFilter.FunnelFilter.Head ?? "",
                    popUpFilter.FunnelFilter.LeadUserID ?? 0,
                    popUpFilter.FunnelFilter.IsHRFocused ?? null,
                    string.IsNullOrEmpty(popUpFilter.FunnelFilter.Geos) ? "" : popUpFilter.FunnelFilter.Geos,
            };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_GetChannelWiseFunnelReportData_For_PopUP_Result> DetailList =
                await _iReport.DFR_HRDetails(paramasString).ConfigureAwait(false);

                if (popUpFilter.IsExport && DetailList.Any())
                {
                    try
                    {
                        return ExportDF_HrDetails(DetailList);
                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Some data issue in Export to excel" });
                    }
                }

                if (DetailList != null && DetailList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DetailList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Supply Funnel Report

        [Authorize]
        [HttpGet("SupplyFunnel/Filters")]
        public async Task<ObjectResult> GetSupplyFunnel_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                dObject.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(dObject.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = Convert.ToDateTime(dObject.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");

                dObject.Managed = new List<SelectListItem>();
                dObject.Managed.Add(new SelectListItem() { Value = "Select ", Text = "0", Selected = true });
                dObject.Managed.Add(new SelectListItem() { Value = "Managed", Text = "1", Selected = false });
                dObject.Managed.Add(new SelectListItem() { Value = "Self Managed ", Text = "2", Selected = false });

                dObject.HiringNeed = new List<SelectListItem>();
                dObject.HiringNeed.Add(new SelectListItem() { Value = "--Select--", Text = "0" });
                dObject.HiringNeed.Add(new SelectListItem() { Value = "Yes, it's for a limited Project", Text = "1" });
                dObject.HiringNeed.Add(new SelectListItem() { Value = "No, They want to hire for long term", Text = "2" });

                var modeOfWorkingList = await _iReport.GetALLModeOFWorkingByCondition(x => x.IsActive == true).ConfigureAwait(false);

                dObject.ModeOfWorking = modeOfWorkingList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = x.ModeOfWorking
                }).ToList();
                dObject.ModeOfWorking.Insert(0, new SelectListItem() { Text = "0", Value = "--Select--" });

                dObject.TypeOfHR = new List<SelectListItem>();
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "--Select--", Text = "-1" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Contractual", Text = "0" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Direct Placement", Text = "1" });

                var Company_Categories = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "A" },
                    { "2", "B" },
                    { "3", "C" }
                };

                dObject.CompanyCategory = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Text = x.Key.ToString(),
                    Value = x.Value
                });

                var Replacement = new Dictionary<string, string>
                {
                    { "0", "No" },
                    { "1", "Yes" },
                };
                dObject.Replacement = Replacement.ToList().Select(x => new SelectListItem
                {
                    Text = x.Key.ToString(),
                    Value = x.Value
                });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("SupplyFunnel/Listing")]
        public async Task<ObjectResult> GetSupplyFunnel_Listing(SupplyFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate, filter.EndDate, "", "",
                    string.IsNullOrEmpty(filter.Managed) ? "0" : filter.Managed,
                    string.IsNullOrEmpty(filter.IsHiringNeedTemp) ? "0" : filter.IsHiringNeedTemp,
                    string.IsNullOrEmpty(filter.ModeOfWork) ? "0" : filter.ModeOfWork,
                    string.IsNullOrEmpty(filter.TypeOfHR) ? "-1" : filter.TypeOfHR,
                    string.IsNullOrEmpty(filter.CompanyCategory) ? "" : filter.CompanyCategory,
                    string.IsNullOrEmpty(filter.Replacement) ? "0" : filter.Replacement
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                string SFRDataJson = "";

                if (filter.IsActionWise)
                    SFRDataJson = await (_iReport.SFR_ActionWise_Listing(paramasString).ConfigureAwait(false));
                else
                    SFRDataJson = await (_iReport.SFR_HRWise_Listing(paramasString).ConfigureAwait(false));

                if (SFRDataJson.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = SFRDataJson });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("SupplyFunnel/Summary")]
        public async Task<ObjectResult> GetSupplyFunnel_Summary(SupplyFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate, filter.EndDate, "","",
                    filter.Managed ?? "",
                    filter.IsHiringNeedTemp ?? "0",
                    filter.ModeOfWork ?? "0",
                    string.IsNullOrEmpty(filter.TypeOfHR) ? "-1" : filter.TypeOfHR,
                    filter.CompanyCategory ?? "",
                    filter.Replacement ?? "0"
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                var summaryData = string.Empty;

                if (filter.IsActionWise)
                    summaryData = await (_iReport.SFR_ActionWise_Summary(paramasString).ConfigureAwait(false));
                else
                    summaryData = await (_iReport.SFR_HRWise_Summary(paramasString).ConfigureAwait(false));

                if (!string.IsNullOrEmpty(summaryData))
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = summaryData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("SupplyFunnel/HRDetails")]
        public async Task<IActionResult> GetSupplyFunnel_HrDetails(Supply_HrDetailPopUpFilter popUpFilter)
        {
            try
            {
                if (popUpFilter == null || string.IsNullOrEmpty(popUpFilter.CurrentStage))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Current stage" });
                }

                int SelectedRow_Manager_ID = 0;

                if (!string.IsNullOrEmpty(popUpFilter.TeamManagerName))
                {
                    UsrUser UserDetails = await _iReport.GetUsrUserByCondition(x => x.FullName.ToLower() == popUpFilter.TeamManagerName.ToLower() && x.UserTypeId == 10).ConfigureAwait(false);

                    if (UserDetails != null)
                        SelectedRow_Manager_ID = Convert.ToInt32(UserDetails.Id);
                }

                if (string.IsNullOrEmpty(popUpFilter.FunnelFilter.StartDate) && string.IsNullOrEmpty(popUpFilter.FunnelFilter.EndDate))
                {
                    popUpFilter.FunnelFilter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.EndDate = Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    popUpFilter.NewExistingType,
                    SelectedRow_Manager_ID,
                    popUpFilter.CurrentStage,
                    popUpFilter.FunnelFilter.StartDate,
                    popUpFilter.FunnelFilter.EndDate,
                    "", "",
                    popUpFilter.FunnelFilter.IsActionWise,
                    popUpFilter.FunnelFilter.Managed ?? "",
                    popUpFilter.FunnelFilter.IsHiringNeedTemp ?? "0",
                    popUpFilter.FunnelFilter.ModeOfWork ?? "0",
                    string.IsNullOrEmpty(popUpFilter.FunnelFilter.TypeOfHR) ? "-1" : popUpFilter.FunnelFilter.TypeOfHR,
                    popUpFilter.FunnelFilter.CompanyCategory ?? "",
                    popUpFilter.FunnelFilter.Replacement ?? "0",
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP_Result> DetailList =
                await _iReport.SFR_HRDetails(paramasString).ConfigureAwait(false);

                if (popUpFilter.IsExport && DetailList.Any())
                {
                    try
                    {
                        return ExportSF_HrDetails(DetailList);
                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Some data issue in Export to excel" });
                    }
                }

                if (DetailList != null && DetailList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DetailList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Team Demand Funnel Report

        [Authorize]
        [HttpGet("TeamDemandFunnel/Filters")]
        public async Task<ObjectResult> GetTeamDemandFunnel_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                dObject.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(dObject.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = Convert.ToDateTime(dObject.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");

                var TeamManagerUserList = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsActive == true).ConfigureAwait(false);

                dObject.SalesManager = TeamManagerUserList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = x.FullName
                }).ToList();

                dObject.HiringNeed = new List<SelectListItem>();
                dObject.HiringNeed.Add(new SelectListItem() { Value = "--Select--", Text = "0" });
                dObject.HiringNeed.Add(new SelectListItem() { Value = "Yes, it's for a limited Project", Text = "1" });
                dObject.HiringNeed.Add(new SelectListItem() { Value = "No, They want to hire for long term", Text = "2" });

                var modeOfWorkingList = await _iReport.GetALLModeOFWorkingByCondition(x => x.IsActive == true).ConfigureAwait(false);

                dObject.ModeOfWorking = modeOfWorkingList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = x.ModeOfWorking
                }).ToList();
                dObject.ModeOfWorking.Insert(0, new SelectListItem() { Text = "0", Value = "--Select--" });

                dObject.TypeOfHR = new List<SelectListItem>();
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "--Select--", Text = "-1" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Contractual", Text = "0" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Direct Placement", Text = "1" });

                var Company_Categories = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "A" },
                    { "2", "B" },
                    { "3", "C" }
                };

                dObject.CompanyCategory = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Text = x.Key.ToString(),
                    Value = x.Value
                });

                //UTS-3987: Add the Lead types as a filter.
                List<SelectListItem> bindLeadTypes = new List<SelectListItem>();

                SelectListItem bindLeadType1 = new SelectListItem();
                bindLeadType1.Text = "1";
                bindLeadType1.Value = "InBound";
                bindLeadTypes.Add(bindLeadType1);

                SelectListItem bindLeadType2 = new SelectListItem();
                bindLeadType2.Text = "2";
                bindLeadType2.Value = "OutBound";
                bindLeadTypes.Add(bindLeadType2);

                dObject.LeadSource = bindLeadTypes;

                var LeadTypeList = await _iReport.GetALLUsrUserByCondition(x => x.DeptId == 1 && x.LevelId == 1 && x.UserTypeId == 12 && x.IsActive == true).ConfigureAwait(false);

                dObject.LeadTypeList = LeadTypeList.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName.ToString()
                }).ToList();

                dObject.LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("TeamDemandFunnel/Listing")]
        public async Task<ObjectResult> GetTeamDemandFunnel_Listing(TeamDemandFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate,
                    filter.EndDate,
                    "",
                    "",
                    string.IsNullOrEmpty(filter.IsHiringNeedTemp) ? "0" : filter.IsHiringNeedTemp,
                    string.IsNullOrEmpty(filter.SalesManagerID) ? "0" : filter.SalesManagerID,
                    string.IsNullOrEmpty(filter.ModeOfWork) ? "0" : filter.ModeOfWork,
                    string.IsNullOrEmpty(filter.TypeOfHR) ? "-1" : filter.TypeOfHR,
                    string.IsNullOrEmpty(filter.CompanyCategory) ? "" : filter.CompanyCategory,
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                string TeamDemandReportJson = "";

                if (filter.IsActionWise)
                {
                    // Below line is added because we want to apply below two filters only for the action wise report.
                    // Added below line by Ashwin on 01-Aug-2023
                    paramasString = $" {paramasString}, '{filter.LeadUserID ?? 0}', {(filter.IsHRFocused == true ? "'true'" : "null")} ";

                    TeamDemandReportJson = await (_iReport.TDF_ActionWise_Listing(paramasString).ConfigureAwait(false));
                }
                else
                    TeamDemandReportJson = await (_iReport.TDF_HRWise_Listing(paramasString).ConfigureAwait(false));

                if (TeamDemandReportJson.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = TeamDemandReportJson });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("TeamDemandFunnel/Summary")]
        public async Task<ObjectResult> GetTeamDemandFunnel_Summary(TeamDemandFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate,
                    filter.EndDate,
                    "",
                    "",
                    filter.IsHiringNeedTemp == null ? "0" : filter.IsHiringNeedTemp,
                    filter.SalesManagerID == null ? "0" : filter.SalesManagerID,
                    filter.ModeOfWork == null ? "0" : filter.ModeOfWork,
                    string.IsNullOrEmpty(filter.TypeOfHR) ? "-1" : filter.TypeOfHR,
                    filter.CompanyCategory == null ? "" : filter.CompanyCategory,
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                var summaryData = string.Empty;

                if (filter.IsActionWise)
                {
                    // Below line is added because we want to apply below two filters only for the action wise report.
                    // Added below line by Ashwin on 01-Aug-2023
                    paramasString = $" {paramasString}, '{filter.LeadUserID ?? 0}', {(filter.IsHRFocused == true ? "'true'" : "null")} ";

                    summaryData = await (_iReport.TDF_ActionWise_Summary_ForSalesUsers(paramasString));
                }
                else
                    summaryData = await (_iReport.TDF_HRWise_Summary_ForSalesUsers(paramasString));

                if (!string.IsNullOrEmpty(summaryData))
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = summaryData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("TeamDemandFunnel/HRDetails")]
        public async Task<ObjectResult> GetTeamDemandFunnel_HrDetails(TeamDemand_HrDetailPopUpFilter popUpFilter)
        {
            try
            {
                if (popUpFilter == null || string.IsNullOrEmpty(popUpFilter.CurrentStage))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Current stage" });
                }

                if (string.IsNullOrEmpty(popUpFilter.FunnelFilter.StartDate) && string.IsNullOrEmpty(popUpFilter.FunnelFilter.EndDate))
                {
                    popUpFilter.FunnelFilter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.EndDate = Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                int SelectedRow_SalesUser_ID = 0;

                if (!string.IsNullOrEmpty(popUpFilter.SelectedRow_SalesUserName))
                {
                    UsrUser SalesUserDetails = await _iReport.GetUsrUserByCondition(x => x.FullName.ToLower() == popUpFilter.SelectedRow_SalesUserName.ToLower() && x.UserTypeId == 4).ConfigureAwait(false);

                    if (SalesUserDetails != null)
                        SelectedRow_SalesUser_ID = Convert.ToInt32(SalesUserDetails.Id);
                }

                object[] param = new object[]
                {
                    popUpFilter.AdhocType,
                    SelectedRow_SalesUser_ID,
                    popUpFilter.CurrentStage,
                    popUpFilter.FunnelFilter.StartDate,
                    popUpFilter.FunnelFilter.EndDate,
                    "",
                    "",
                    popUpFilter.FunnelFilter.IsActionWise,
                    popUpFilter.HrFilter.HR_No,
                    popUpFilter.HrFilter.SalesPerson,
                    popUpFilter.HrFilter.CompnayName,
                    popUpFilter.HrFilter.Role,
                    popUpFilter.HrFilter.Managed_Self,
                    popUpFilter.HrFilter.Availability,
                    popUpFilter.HrFilter.TalentName,
                    popUpFilter.FunnelFilter.IsHiringNeedTemp ?? "0",
                    popUpFilter.FunnelFilter.SalesManagerID ?? "0",
                    popUpFilter.FunnelFilter.ModeOfWork ?? "0",
                    string.IsNullOrEmpty(popUpFilter.FunnelFilter.TypeOfHR) ? "-1" : popUpFilter.FunnelFilter.TypeOfHR,
                    popUpFilter.FunnelFilter.CompanyCategory ?? "",
                    popUpFilter.FunnelFilter.LeadUserID ?? 0,
                    popUpFilter.FunnelFilter.IsHRFocused ?? null
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_GetChannelWiseFunnelReportData_For_SalesUser_PopUP_Revised_Result> DetailList =
                await _iReport.TDF_HRDetails(paramasString).ConfigureAwait(false);

                if (DetailList != null && DetailList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DetailList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Client Report

        [Authorize]
        [HttpGet("ClientReport/Filters")]
        public async Task<ObjectResult> GetClient_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = DateTime.Now.ToString("yyyy-MM-dd");

                var Company_Categories = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "A" },
                    { "2", "B" },
                    { "3", "C" }
                };

                dObject.CompanyCategory = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Text = x.Key.ToString(),
                    Value = x.Value
                });

                var TeamManagerUserList = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsNewUser == true && x.IsActive == true).ConfigureAwait(false);

                dObject.SalesManager = TeamManagerUserList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = x.FullName
                }).ToList();

                var LeadTypeList = await _iReport.GetALLUsrUserByCondition(x => x.DeptId == 1 && x.LevelId == 1 && (x.UserTypeId == 11 || x.UserTypeId == 12) && x.IsActive == true).ConfigureAwait(false);

                dObject.LeadTypeList = LeadTypeList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = (x.UserTypeId == 12 ? "InBound" : "OutBound") + " - " + Convert.ToString(x.FullName)
                }).ToList();

                dObject.LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ClientReport/GetClientReport")]
        public async Task<ObjectResult> GetClientReport(ClientReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    filter.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate, filter.EndDate, 0, filter.CompanyCategory, 0, filter.SalesManagerID
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_ClientBasedReport_Result> ClientBasedReport = await _iReport.GetClientReport(paramasString).ConfigureAwait(false);

                if (ClientBasedReport != null && ClientBasedReport.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientBasedReport });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ClientReport/GeClientPopUpReport")]
        public async Task<ObjectResult> ClientPopUpReport(ClientBasedReport_PopUp filter)
        {
            try
            {
                if (filter == null || string.IsNullOrEmpty(filter.ClientStage))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Client stage" });
                }

                if (filter != null && filter.ReportFilter == null)
                {
                    filter.ReportFilter = new ClientReportFilter();
                }

                if (string.IsNullOrEmpty(filter.ReportFilter.StartDate) && string.IsNullOrEmpty(filter.ReportFilter.EndDate))
                {
                    filter.ReportFilter.StartDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    filter.ReportFilter.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.ReportFilter.StartDate ?? "",
                    filter.ReportFilter.EndDate ?? "",
                    filter.ClientStage,
                    filter.Client ?? "",
                    filter.Company ?? "",
                    "",
                    filter.SalesUser ?? "",
                    filter.Hr_Number ?? "",
                    filter.Talent ?? "",
                    filter.ReportFilter.CompanyCategory ?? "",
                    0,
                    filter.Status ?? "",
                    filter.ReportFilter.SalesManagerID ?? "",
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_ClientBasedReport_PopUp_Result> ClientBasedReport = await _iReport.GetClientReport_Popup(paramasString).ConfigureAwait(false);

                if (ClientBasedReport != null && ClientBasedReport.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientBasedReport });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Client Log Report

        [Authorize]
        [HttpGet("ClientLogReport/Filters")]
        public async Task<ObjectResult> GetClientLog_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddMonths(-3).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = DateTime.Now.ToString("yyyy-MM-dd");

                var Login_Status = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "Yes", "Yes" },
                    { "No", "No" }
                };

                dObject.LoginStatus = Login_Status.ToList().Select(x => new SelectListItem
                {
                    Text = x.Key.ToString(),
                    Value = x.Value
                });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ClientLogReport/GetClientLogReport")]
        public async Task<ObjectResult> GetClientLogReport(ClientLogReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                    filter.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "CompanyId" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "asc" : filter.SortDirection,
                    filter.StartDate??"", filter.EndDate??"", "", filter.LoginStatus??""
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_GetClientLogReport_Result> ClientBasedReport = await _iReport.GetClientLogReport(paramasString).ConfigureAwait(false);

                if (ClientBasedReport != null && ClientBasedReport.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientBasedReport });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ClientLogReport/ShowAllClientHistory")]
        public async Task<ObjectResult> ShowAllClientHistory(long? ContactID)
        {
            try
            {
                if (ContactID == null || ContactID == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Contact ID" });
                }

                List<Sproc_GetClientActivityLog_Result> ClientBasedReport = await _iReport.GetClientActivityLog(ContactID).ConfigureAwait(false);

                if (ClientBasedReport != null && ClientBasedReport.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientBasedReport });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Skill wise report

        [Authorize]
        [HttpPost("SkillWise/Listing")]
        public async Task<ObjectResult> GetSkillWiseReport(SkillWiseReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddMonths(-3).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[] { filter.StartDate, filter.EndDate };
                string paramasString = CommonLogic.ConvertToParamString(param);

                if (filter.IsActionWise)
                {
                    List<sproc_SkillReport_ActionWise_Result> ActionWise_Result =
                        await (_iReport.sproc_SkillReport_ActionWise(paramasString).ConfigureAwait(false));

                    if (ActionWise_Result.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ActionWise_Result });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
                }
                else
                {
                    List<sproc_SkillReport_HRWise_Result> HRWise_Result =
                        await (_iReport.sproc_SkillReport_HRWise(paramasString).ConfigureAwait(false));

                    if (HRWise_Result.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HRWise_Result });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("SkillWise/HRListing")]
        public async Task<ObjectResult> SkillWise_HRListing(SkillWise_PopupFilter popup)
        {
            try
            {
                if (string.IsNullOrEmpty(popup.reportFilter.StartDate) && string.IsNullOrEmpty(popup.reportFilter.EndDate))
                {
                    popup.reportFilter.StartDate = DateTime.Now.AddMonths(-3).Date.ToString("yyyy-MM-dd");
                    popup.reportFilter.EndDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    popup.reportFilter.StartDate, popup.reportFilter.EndDate,
                    popup.SkillName, popup.TypeOfCount, popup.AdhocType
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                if (popup.reportFilter.IsActionWise)
                {
                    List<sproc_SkillReport_ActionWise_PopUp_Result> ActionWise_Result =
                        await (_iReport.sproc_SkillReport_ActionWise_PopUp(paramasString).ConfigureAwait(false));

                    if (ActionWise_Result.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ActionWise_Result });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
                }
                else
                {
                    List<sproc_SkillReport_HRWise_PopUp_Result> HRWise_Result =
                        await (_iReport.sproc_SkillReport_HRWise_PopUp(paramasString).ConfigureAwait(false));

                    if (HRWise_Result.Any())
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HRWise_Result });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region SLA Report

        [Authorize]
        [HttpGet("SLA/Filters")]
        public async Task<ObjectResult> GetSLAFilters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.SLAType = new List<SelectListItem>();
                dObject.SLAType.Add(new SelectListItem() { Value = "Overall SLA", Text = "0" });
                dObject.SLAType.Add(new SelectListItem() { Value = "SLA Missed", Text = "1" });

                dObject.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                dObject.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

                var Stages_List = await _iReport.GetALLPrgSummaryStagesForReports().ConfigureAwait(false);

                dObject.Stages = Stages_List.Select(x => new SelectListItem
                {
                    Value = x.SummaryStage,
                    Text = x.Id.ToString()
                });

                var ActionFilter_List = await _iReport.GetALLPrgActionFilters().ConfigureAwait(false);

                dObject.ActionFilter = ActionFilter_List.Select(x => new SelectListItem
                {
                    Value = x.ActionFilter,
                    Text = x.Id.ToString()
                });

                dObject.Pool_ODR = new List<SelectListItem>();
                dObject.Pool_ODR.Add(new SelectListItem() { Value = "Select", Text = "0" });
                dObject.Pool_ODR.Add(new SelectListItem() { Value = "Pool", Text = "1" });
                dObject.Pool_ODR.Add(new SelectListItem() { Value = "ODR", Text = "2" });

                dObject.AM_NBD = new List<SelectListItem>();
                dObject.AM_NBD.Add(new SelectListItem() { Value = "Select", Text = "0" });
                dObject.AM_NBD.Add(new SelectListItem() { Value = "AM", Text = "1" });
                dObject.AM_NBD.Add(new SelectListItem() { Value = "NBD", Text = "2" });

                var SalesManagerList = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9).ConfigureAwait(false);

                dObject.SalesManager = SalesManagerList.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var SalesManager_AM_List = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsNewUser == false).ConfigureAwait(false);

                dObject.SalesManager_AM = SalesManager_AM_List.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var SalesManager_NBD_List = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsNewUser == true).ConfigureAwait(false);

                dObject.SalesManager_NBD = SalesManager_NBD_List.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var SV_List = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsNewUser == true).ConfigureAwait(false);

                dObject.SV = SV_List.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var SalesPerson_List = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 4).ConfigureAwait(false);

                dObject.SalesPerson = SalesPerson_List.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var SalesPerson_AM_List = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 4 && x.IsNewUser == false).ConfigureAwait(false);

                dObject.SalesPerson_AM = SalesPerson_AM_List.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var SalesPerson_NBD_List = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 4 && x.IsNewUser == true).ConfigureAwait(false);

                dObject.SalesPerson_NBD = SalesPerson_NBD_List.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.Id.ToString()
                });

                var TalentRoles = _iReport.GetALLPrgTalentRolesByCondition(x => x.IsActive == true);

                dObject.Roles = TalentRoles.Select(x => new SelectListItem
                {
                    Value = x.TalentRole,
                    Text = x.Id.ToString()
                });

                dObject.ActionFilterWithColor = new List<SelectListItem>();
                dObject.ActionFilterWithColor.Add(new SelectListItem() { Text = "ON Time", Value = "#17A2B8" });
                dObject.ActionFilterWithColor.Add(new SelectListItem() { Text = "Before Time", Value = "#28A745" });
                dObject.ActionFilterWithColor.Add(new SelectListItem() { Text = "Exceeded SLA", Value = "#FFC107" });
                dObject.ActionFilterWithColor.Add(new SelectListItem() { Text = "Running Late", Value = "#DC3545" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("SLA/Summary")]
        public async Task<ObjectResult> GetSLASummary(SLAFilter sLAFilter)
        {
            try
            {
                if (string.IsNullOrEmpty(sLAFilter.StartDate) && string.IsNullOrEmpty(sLAFilter.EndDate))
                {
                    sLAFilter.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    sLAFilter.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    sLAFilter.StartDate, sLAFilter.EndDate, null,
                    sLAFilter.SalesManagerID, sLAFilter.SV, sLAFilter.SalesPerson,
                    sLAFilter.Stage, sLAFilter.IsAdHoc, sLAFilter.Role,
                    sLAFilter.HR_Number, sLAFilter.Company,
                    sLAFilter.ActionFilter, sLAFilter.AMNBD
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result> summaryData = null;

                summaryData = await (_iReport.SLA_Summary(paramasString).ConfigureAwait(false));

                if (summaryData != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = summaryData });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("SLA/List")]
        public async Task<ObjectResult> GetSLAReport(SLAReport_Filter filter)
        {
            try
            {
                SLAFilter sLAFilter = null;
                if (filter != null && filter.sLAFilter != null)
                {
                    sLAFilter = filter.sLAFilter;
                }

                if (string.IsNullOrEmpty(sLAFilter.StartDate) && string.IsNullOrEmpty(sLAFilter.EndDate))
                {
                    sLAFilter.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    sLAFilter.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.PageIndex, filter.PageSize,
                    sLAFilter.StartDate, sLAFilter.EndDate, null,
                    sLAFilter.SalesManagerID, sLAFilter.SV, sLAFilter.SalesPerson,
                    sLAFilter.Stage, sLAFilter.IsAdHoc, sLAFilter.Role,
                    filter.SLAType, 0,
                    sLAFilter.HR_Number, sLAFilter.Company,
                    sLAFilter.ActionFilter, sLAFilter.AMNBD
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_SLA_Report_For_Static_Stages_Result> SLA_Report_Data = null;

                SLA_Report_Data = await (_iReport.SLA_Report(paramasString).ConfigureAwait(false));

                if (SLA_Report_Data != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = SLA_Report_Data });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region JDParsingDump

        [Authorize]
        [HttpPost("JDParsingDump/JDParsingDumpReport")]
        public async Task<ObjectResult> GetJDParsingDumpReport(GetJDParsingDumpReportViewModel filter)
        {
            try
            {
                object[] param = new object[]
                {
                    filter.PageIndex > 0 ? filter.PageIndex : 1,
                    filter.PageSize > 0 ? filter.PageSize : 50,
                    string.IsNullOrEmpty(filter.SortExpression) ? "HRCreatedDate" : filter.SortExpression,
                    string.IsNullOrEmpty(filter.SortDirection) ? "desc" : filter.SortDirection,

                    filter.HRNumber, filter.JDSkillPercentage,filter.JDRolesResponsibilities,filter.JDRequirement,
                    filter.OverAllRowWise, filter.StartDate,filter.EndDate
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                filter.PageIndex = filter.PageIndex > 0 ? filter.PageIndex : 1;
                filter.PageSize = filter.PageSize > 0 ? filter.PageSize : 50;
                List<sproc_JDParsingDumpReport_Result> jDParsingDumpReport = new();

                string val = paramasString.Replace("''", "null");
                jDParsingDumpReport = await (_iReport.GetJDParsingDumpReport(val).ConfigureAwait(false));

                if (jDParsingDumpReport.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(jDParsingDumpReport, Convert.ToInt64(jDParsingDumpReport[0].TotalRecords), Convert.ToInt64(filter.PageSize), Convert.ToInt64(filter.PageIndex)) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region HubSpot Funnel Report
        [Authorize]
        [HttpGet("HubSpotFunnel/Filters")]
        public async Task<ObjectResult> GetHubSpotFunnel_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                dObject.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(dObject.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = Convert.ToDateTime(dObject.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("HubSpotFunnel/Listing")]
        public async Task<ObjectResult> GetHubSpotFunnel_Listing(HubSpotFunnelReportFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.StartDate) && string.IsNullOrEmpty(filter.EndDate))
                {
                    filter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    filter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(filter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    filter.EndDate = Convert.ToDateTime(filter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.StartDate, filter.EndDate,
                    string.IsNullOrEmpty(filter.Head) ? "" : filter.Head
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                string DFRDataJson = "";

                DFRDataJson = await (_iReport.HFR_ActionWise_Listing(paramasString).ConfigureAwait(false));

                if (DFRDataJson.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DFRDataJson });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("HubSpotFunnel/DealDetails")]
        public async Task<IActionResult> GetHubSpotFunnel_DealDetails(HubSpot_DealDetailPopUpFilter popUpFilter)
        {
            try
            {
                if (popUpFilter == null || string.IsNullOrEmpty(popUpFilter.Stage))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide Current stage" });
                }

                int SelectedRow_Manager_ID = 0;

                if (!string.IsNullOrEmpty(popUpFilter.TeamManagerName))
                {
                    UsrUser UserDetails = await _iReport.GetUsrUserByCondition(x => x.FullName.ToLower() == popUpFilter.TeamManagerName.ToLower() && (x.UserTypeId == 9 || x.UserTypeId == 11 || x.UserTypeId == 12)).ConfigureAwait(false);

                    if (UserDetails != null)
                        SelectedRow_Manager_ID = Convert.ToInt32(UserDetails.Id);
                }

                if (string.IsNullOrEmpty(popUpFilter.FunnelFilter.StartDate) && string.IsNullOrEmpty(popUpFilter.FunnelFilter.EndDate))
                {
                    popUpFilter.FunnelFilter.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.StartDate = CommonFunction.GetPreviousWeekday(Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate), DayOfWeek.Monday).Date.ToString("yyyy-MM-dd");
                    popUpFilter.FunnelFilter.EndDate = Convert.ToDateTime(popUpFilter.FunnelFilter.StartDate).AddDays(5).Date.ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {

                    popUpFilter.Stage,
                    popUpFilter.FunnelFilter.StartDate,
                    popUpFilter.FunnelFilter.EndDate,
                    SelectedRow_Manager_ID,
                    popUpFilter.FunnelFilter.Head ?? ""
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_HubSpot_Client_Funnel_Report_PopUP_Result> DetailList =
                await _iReport.HFR_DealDetails(paramasString).ConfigureAwait(false);

                if (popUpFilter.IsExport && DetailList.Any())
                {
                    try
                    {
                        return ExportHF_DealDetails(DetailList);
                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Some data issue in Export to excel" });
                    }
                }

                if (DetailList != null && DetailList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = DetailList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Hiring Request report

        [Authorize]
        [HttpGet("HRReport/Filters")]
        public async Task<ObjectResult> GetHRReportFilters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                dObject.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

                //var SalesManagerList = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsActive == true).ConfigureAwait(false);
                //dObject.SalesManager = SalesManagerList.Select(x => new SelectListItem
                //{
                //    Value = x.FullName,
                //    Text = x.Id.ToString()
                //});

                var Heads = _iReport.Sproc_Get_SalesHead_Users_Result().Result.ToList();
                dObject.SalesManager = Heads.Select(x => new SelectListItem
                {
                    Value = x.FullName,
                    Text = x.ID.ToString()
                });


                var modeOfWorkingList = await _iReport.GetALLModeOFWorkingByCondition(x => x.IsActive == true).ConfigureAwait(false);

                dObject.ModeOfWorking = modeOfWorkingList.Select(x => new SelectListItem
                {
                    Value = x.ModeOfWorking,
                    Text = x.Id.ToString()
                }).ToList();
                dObject.ModeOfWorking.Insert(0, new SelectListItem() { Text = "0", Value = "--Select--" });

                dObject.TypeOfHR = new List<SelectListItem>();
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "--Select--", Text = "-1" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Contractual", Text = "0" });
                dObject.TypeOfHR.Add(new SelectListItem() { Value = "Direct Placement", Text = "1" });

                var hiringStatus = await _iReport.GetPrgHiringRequestStatus().ConfigureAwait(false);

                dObject.HiringStatus = hiringStatus.Select(x => new SelectListItem
                {
                    Value = x.HiringRequestStatus,
                    Text = x.Id.ToString()
                }).ToList();

                dObject.ClientType = new List<SelectListItem>();
                dObject.ClientType.Add(new SelectListItem() { Value = "All", Text = "0" });
                dObject.ClientType.Add(new SelectListItem() { Value = "AM", Text = "1" });
                dObject.ClientType.Add(new SelectListItem() { Value = "NBD", Text = "2" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the hr report list.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("HRReport/List")]
        public async Task<ObjectResult> GetHRReportList(HiringRequestReport_Filter filter)
        {
            try
            {
                HiringRequestReportFilter hiringRequestReportFilter = null;
                if (filter != null && filter.hiringRequestReportFilter != null)
                    hiringRequestReportFilter = filter.hiringRequestReportFilter;

                if (string.IsNullOrEmpty(hiringRequestReportFilter.FromDate) && string.IsNullOrEmpty(hiringRequestReportFilter.ToDate))
                {
                    hiringRequestReportFilter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    hiringRequestReportFilter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                   hiringRequestReportFilter.FromDate,
                    hiringRequestReportFilter.ToDate,
                    hiringRequestReportFilter.TypeOfHR,
                    hiringRequestReportFilter.ModeOfWorkId,
                    hiringRequestReportFilter.Heads,
                    hiringRequestReportFilter.HRStatusID,
                    hiringRequestReportFilter.IsHRFocused ?? null,
                    hiringRequestReportFilter.ClientType,
                    hiringRequestReportFilter.Geos,
                    hiringRequestReportFilter.Sales_ManagerIDs
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_HiringRequest_Report_Result> HR_Report_Data = null;

                HR_Report_Data = await (_iReport.HR_Report(paramasString).ConfigureAwait(false));

                if (HR_Report_Data != null && HR_Report_Data.Count > 0)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HR_Report_Data });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the hr report popup.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("HRReport/Popup")]
        public async Task<IActionResult> GetHRReportPopup(HiringRequestReportPopup_Filter filter)
        {
            try
            {
                HiringRequestReportPopupFilter hiringRequestReportPopup = null;
                if (filter != null && filter.hiringRequestReportPopupFilter != null)
                    hiringRequestReportPopup = filter.hiringRequestReportPopupFilter;

                if (string.IsNullOrEmpty(hiringRequestReportPopup.FromDate) && string.IsNullOrEmpty(hiringRequestReportPopup.ToDate))
                {
                    hiringRequestReportPopup.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    hiringRequestReportPopup.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                   hiringRequestReportPopup.FromDate,
                    hiringRequestReportPopup.ToDate,
                    hiringRequestReportPopup.TypeOfHR,
                    hiringRequestReportPopup.ModeOfWorkId,
                    hiringRequestReportPopup.Heads,
                    hiringRequestReportPopup.HRStatusID,
                    hiringRequestReportPopup.Stages,
                    hiringRequestReportPopup.IsHRFocused ?? null,
                    hiringRequestReportPopup.ClientType,
                    hiringRequestReportPopup.Geos,
                    hiringRequestReportPopup.Sales_ManagerIDs
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_HiringRequest_PopupReport_Result> HR_PopupReport_Data = null;

                HR_PopupReport_Data = await (_iReport.HR_PopupReport(paramasString).ConfigureAwait(false));

                if (filter.hiringRequestReportPopupFilter.IsExport && HR_PopupReport_Data.Any())
                {
                    try
                    {
                        return Export_HrReportDetails(HR_PopupReport_Data);
                    }
                    catch
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Some data issue in Export to excel" });
                    }
                }

                if (HR_PopupReport_Data != null && HR_PopupReport_Data.Count > 0)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HR_PopupReport_Data });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ClientBasedReportWithHubSpot

        [Authorize]
        [HttpGet("ClientBasedReportWithHubSpot/Filters")]
        public async Task<ObjectResult> GetClientBasedFiltersWithHubSpot()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.StartDate = DateTime.Now.AddDays(-7).Date.ToString("yyyy-MM-dd");
                dObject.EndDate = DateTime.Now.ToString("yyyy-MM-dd");

                var Company_Categories = new Dictionary<string, string>
                {
                    { "0", "Select" },
                    { "1", "A" },
                    { "2", "B" },
                    { "3", "C" }
                };

                dObject.CompanyCategory = Company_Categories.ToList().Select(x => new SelectListItem
                {
                    Text = x.Key.ToString(),
                    Value = x.Value
                });

                var TeamManagerUserList = await _iReport.GetALLUsrUserByCondition(x => x.UserTypeId == 9 && x.IsNewUser == true && x.IsActive == true).ConfigureAwait(false);

                dObject.SalesManager = TeamManagerUserList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = x.FullName
                }).ToList();

                //UTS-3987: Add the Lead types as a filter.
                List<SelectListItem> bindLeadTypes = new List<SelectListItem>();

                SelectListItem bindLeadType1 = new SelectListItem();
                bindLeadType1.Text = "1";
                bindLeadType1.Value = "InBound";
                bindLeadTypes.Add(bindLeadType1);

                SelectListItem bindLeadType2 = new SelectListItem();
                bindLeadType2.Text = "2";
                bindLeadType2.Value = "OutBound";
                bindLeadTypes.Add(bindLeadType2);

                dObject.LeadSource = bindLeadTypes;

                var LeadTypeList = await _iReport.GetALLUsrUserByCondition(x => x.DeptId == 1 && x.LevelId == 1 && (x.UserTypeId == 11 || x.UserTypeId == 12) && x.IsActive == true).ConfigureAwait(false);

                dObject.LeadTypeList = LeadTypeList.Select(x => new SelectListItem
                {
                    Text = x.Id.ToString(),
                    Value = (x.UserTypeId == 12 ? "InBound" : "OutBound") + " - " + Convert.ToString(x.FullName)
                }).ToList();

                dObject.LeadTypeList.Insert(0, new SelectListItem() { Value = "0", Text = "--Select--" });

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(dObject) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [HttpPost("ClientBasedReportWithHubSpot/List")]
        public async Task<ObjectResult> GetClientBasedReportWithHubSpot(ClientBasedReportWithHubSpotFilter filter)
        {
            try
            {
                if (string.IsNullOrEmpty(filter.FromDate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    DateTime now = DateTime.Now;
                    var StartDate = new DateTime(now.Year, now.Month, 1);
                    filter.FromDate = StartDate.ToString("yyyy-MM-dd");
                    filter.ToDate = StartDate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }

                object[] param = new object[]
                {
                    filter.FromDate, filter.ToDate, filter.GeoId, filter.CompanyCategory, filter.SalesMangerId,filter.SalesManagerIds,filter.LeadUserId, filter.IsHRFocused
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<sproc_ClientBasedReport_WithHubSpot_Result> ClientBasedReportWithHubSpot = await _iReport.GetClientBasedReportWithHubSpot(paramasString).ConfigureAwait(false);
                if (ClientBasedReportWithHubSpot != null && ClientBasedReportWithHubSpot.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientBasedReportWithHubSpot });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]
        [HttpPost("ClientBasedReportWithHubSpot/PopUpReport")]
        public async Task<ObjectResult> GetClientBasedReportWithHubSpotPopUp(ClientBasedReportWithHubSpotPopUpFilter filter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filter.FromDate))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide from Date" });
                if (string.IsNullOrWhiteSpace(filter.ToDate))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide to Date" });
                if (string.IsNullOrWhiteSpace(filter.StageName))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide stage name" });

                object[] param = new object[]
                {
                    filter.FromDate, filter.ToDate, filter.StageName, filter.FullName, filter.Company,filter.GEO,filter.SalesUser,filter.Hr_Number,filter.Name,filter.CompanyCategory,filter.SalesManagerID,filter.Status,filter.SalesManagerIDs, filter.LeadUserID, filter.IsHRFocused
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<sproc_ClientBasedReport_WithHubSpot_PopUp_Result> ClientBasedReportWithHubSpotPopUp = await _iReport.GetClientBasedReportWithHubSpotPopUp(paramasString).ConfigureAwait(false);
                if (ClientBasedReportWithHubSpotPopUp != null && ClientBasedReportWithHubSpotPopUp.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientBasedReportWithHubSpotPopUp });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region HR Lost report



        /// <summary>
        /// Gets the HR Lost list.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("HRLostReport/List")]
        public async Task<ObjectResult> GetHRLostReportList(HRLostReportFilter filter)
        {
            try
            {

                if (string.IsNullOrEmpty(filter.filterFeild_HRLost.HRLostFromDate) && string.IsNullOrEmpty(filter.filterFeild_HRLost.HRLostToDate))
                {
                    filter.filterFeild_HRLost.HRLostFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.filterFeild_HRLost.HRLostToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                string Sortdatafield = "LostDate";
                string Sortorder = "desc";
                object[] param = new object[]
                {
                    filter.PageIndex,
                    filter.PageSize,
                    Sortdatafield, Sortorder,
                    filter.filterFeild_HRLost.HRLostFromDate??"",
                    filter.filterFeild_HRLost.HRLostToDate??"",
                    filter.filterFeild_HRLost.LostReason??"",
                    filter.filterFeild_HRLost.SalesUser??"",
                    filter.filterFeild_HRLost.Client??"",
                    filter.filterFeild_HRLost.searchText??"",
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_GetHRLost_Report_Result> HRLost_Report_Data = null;

                HRLost_Report_Data = await (_iReport.HRLost_Report(paramasString).ConfigureAwait(false));

                if (HRLost_Report_Data != null && HRLost_Report_Data.Count > 0)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(HRLost_Report_Data, HRLost_Report_Data[0].TotalRecords, filter.PageIndex, filter.PageSize) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the hr Lost report popup.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("HRLostReport/TalentDetailPopup")]
        public async Task<IActionResult> GetTalentDetailPopup(long? HRID)
        {
            try
            {
                if (HRID <= 0 && HRID == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Please Provide proper TalentID" });
                }

                List<sproc_UTS_GetTalentDetailHRLostPopUp_Result> getTalentDetailHRLostPopUp_Result = await (_iReport.GetTalentDetail(HRID).ConfigureAwait(false));
                if (getTalentDetailHRLostPopUp_Result != null && getTalentDetailHRLostPopUp_Result.Count > 0)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = getTalentDetailHRLostPopUp_Result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        // HRLost Report Filter
        [HttpGet]
        [Route("GetAllFilterDataForHRLostReport")]
        public ObjectResult GetAllFilterDataForHRLostReport()
        {
            try
            {
                HRLostFilterViewModel model = new();
                model = _iReport.GetHRLostFiltersLists();

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(model) });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Tracking Lead Detail
        [Authorize]
        [HttpGet("GetAllFilterDataForTrackingLeadDetail")]
        public async Task<ObjectResult> GetAllFilterDataForTrackingLeadDetail()
        {
            try
            {
                TrackingLeadDetailFilterViewModel model = new();
                model = await _iReport.GetTrackingLeadFiltersData().ConfigureAwait(false);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.FilterListsResponse(model) });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("TrackingLeadDetail/List")]
        public async Task<ObjectResult> TrackingLeadDetail(TrackingLeadDetailFilter filter)
        {
            try
            {

                if (string.IsNullOrEmpty(filter.Fromdate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    filter.Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                object[] param = new object[]
                {
                    filter.NoOfJobs??null,
                    filter.Fromdate??"",
                    filter.ToDate??"",
                    filter.UTM_Source??"",
                    filter.UTM_Medium??"",
                    filter.UTM_Campaign??"",
                    filter.UTM_Content??"",
                    filter.UTM_Term??"",
                    filter.UTM_Placement??"",
                    filter.ref_url??"",
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_UTS_TrackingLeadDetails_Result> result = null;

                result = await (_iReport.TrackingLeadDetail(paramasString).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("TrackingLeadDetailPopUP/PopUPList")]
        public async Task<ObjectResult> LeadDetailPopUPList(TrackingLeadReport_Details_PopUPViewModel filter)
        {
            try
            {

                if (string.IsNullOrEmpty(filter.Fromdate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    filter.Fromdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                object[] param = new object[]
                {
                    filter.NoOfJobs??null,
                    filter.Fromdate??"",
                    filter.ToDate??"",
                    filter.UTM_Source??"",
                    filter.UTM_Medium??"",
                    filter.UTM_Campaign??"",
                    filter.UTM_Content??"",
                    filter.UTM_Term??"",
                    filter.UTM_Placement??"",
                    filter.ref_url??"",
                    filter.Stage??"",
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_GET_UTM_Tracking_Report_Details_PopUP_Result> result = null;

                result = await (_iReport.LeadDetailPopUP_List(paramasString).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Client portal tracking details
        [Authorize]
        [HttpGet("ClientPortalTrackingDetails/Filters")]
        public async Task<ObjectResult> ClientPortalTrackingDetails_Filters()
        {
            try
            {
                List<sproc_UTS_ClientList_Result> ClientList = new();
                ClientList = await _iReport.ClientList().ConfigureAwait(false);

                if (ClientList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ClientPortalTrackingDetails/List")]
        public async Task<ObjectResult> ClientPortalTrackingDetails_List(ClientPortalTrackingDetailFilter filter)
        {
            try
            {
                if (filter == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                if (string.IsNullOrEmpty(filter.FromDate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    filter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                object[] param = new object[]
                {
                    filter.ClientID ?? 0,
                    filter.FromDate ?? "",
                    filter.ToDate ?? ""
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_UTS_ClientPortalTracking_Details_Result> result = null;

                result = await (_iReport.ClientPortalTracking_Details(paramasString).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("ClientPortalTrackingDetails/PopUPList")]
        public async Task<ObjectResult> ClientPortalTrackingDetails_PopUPList(ClientPortalTrackingDetail_Popup_Filter filter)
        {
            try
            {
                if (filter == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                if (string.IsNullOrEmpty(filter.FromDate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    filter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                object[] param = new object[]
                {
                    filter.ActionID ?? 1, // if null then default login action will be display
                    filter.FromDate ?? "",
                    filter.ToDate ?? "",
                    filter.ClientID ?? 0
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_UTS_ClientPortalTracking_Details_Popup_Result> result = null;

                result = await (_iReport.ClientPortalTracking_Details_Popup(paramasString).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Replacement Report
        [HttpPost("ReplacementReport")]
        public async Task<ObjectResult> ReplacementReport([FromBody] ReplacementReportFilter replacementReportFilter)
        {
            try
            {
                var varLoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (replacementReportFilter == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "ReplacementDate";
                string Sortorder = "DESC";

                object[] param = new object[]
                {
                    replacementReportFilter.pageIndex > 0? replacementReportFilter.pageIndex : 1,
                    replacementReportFilter.pageSize > 0 ? replacementReportFilter.pageSize : 1,
                    Sortdatafield, Sortorder,
                    replacementReportFilter.filterFields.fromDate ?? "",
                    replacementReportFilter.filterFields.toDate ?? "",
                    replacementReportFilter.filterFields.searchText ?? ""
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<sproc_Get_Replacement_Report_Result> ReplacementData = new();
                ReplacementData = await _iReport.sproc_Get_Replacement_Report(paramasString).ConfigureAwait(false);
                dynamic responseObject = new ExpandoObject();
                if (ReplacementData != null && ReplacementData.Count > 0)
                {
                    var TotalPages = (float)replacementReportFilter.pageSize > 0 ? (float)replacementReportFilter.pageSize : 1;
                    responseObject.totalPages = ReplacementData[0].TotalRecords > 0 ? (int)Math.Ceiling((float)ReplacementData[0].TotalRecords / TotalPages) : replacementReportFilter.pageIndex;
                    responseObject.pagenumber = replacementReportFilter.pageIndex;
                    responseObject.totalrows = ReplacementData[0].TotalRecords;
                    responseObject.rows = ReplacementData;
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = responseObject });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available." });                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Talent Backout Report
        [HttpPost("TalentBackoutReport")]
        public async Task<ObjectResult> TalentBackoutReport([FromBody] TalentBackoutReportFilter backoutReportFilter)
        {
            try
            {
                var varLoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation
                if (backoutReportFilter == null || (backoutReportFilter.pagenumber == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                string Sortdatafield = "CreatedDateTime";
                string Sortorder = "DESC";

                object[] param = new object[]
                {
                    backoutReportFilter.pagenumber,
                    backoutReportFilter.totalrecord,
                    Sortdatafield, Sortorder,
                    backoutReportFilter.filterFields.fromDate ?? "",
                    backoutReportFilter.filterFields.toDate ?? "",
                    backoutReportFilter.filterFields.searchText ?? ""
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_TalentBackout_Report_Result> backoutData = await _iReport.Sproc_Get_TalentBackout_Report(paramasString).ConfigureAwait(false);

                if (backoutData != null && backoutData.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(backoutData, Convert.ToInt64(backoutData[0].TotalRecords), backoutReportFilter.pagenumber, backoutReportFilter.totalrecord) });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available." });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region AWS SES tracking details
        [Authorize]
        [HttpGet("AWSSESTrackingDetails/Filters")]
        public async Task<ObjectResult> AWSSESTrackingDetails_Filters()
        {
            try
            {
                List<sproc_UTS_ClientList_Result> ClientList = new();
                ClientList = await _iReport.ClientList().ConfigureAwait(false);

                if (ClientList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ClientList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("AWSSESTrackingDetails/List")]
        public async Task<ObjectResult> AWSSESTrackingDetails_List(AWSSESTrackingDetailFilter filter)
        {
            try
            {
                if (filter == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                if (string.IsNullOrEmpty(filter.FromDate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    filter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                object[] param = new object[]
                {
                    filter.CompanyID ?? 0,
                    filter.FromDate ?? "",
                    filter.ToDate ?? "",
                    filter.SubjectID ?? 0
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_AWS_SES_Tracking_Details_Result> result = null;

                result = await (_iReport.AWSSESTracking_Details(paramasString).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("AWSSESTrackingDetails/PopUPList")]
        public async Task<ObjectResult> AWSSESTrackingDetails_PopUPList(AWSSESTrackingDetail_Popup_Filter filter)
        {
            try
            {
                if (filter == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Request object is empty" });
                }

                if (string.IsNullOrEmpty(filter.FromDate) && string.IsNullOrEmpty(filter.ToDate))
                {
                    filter.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    filter.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                object[] param = new object[]
                {
                    filter.EventType ?? "Send", // if null then default login action will be display
                    filter.FromDate ?? "",
                    filter.ToDate ?? "",
                    filter.CompanyID ?? 0,
                    filter.SubjectID ?? 0
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_AWS_SES_Tracking_Details_Popup_Result> result = null;

                result = await (_iReport.AWSSESTracking_Details_Popup(paramasString).ConfigureAwait(false));

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("AWSSESTrackingDetails/EmailSubjectFilters")]
        public async Task<ObjectResult> AWSSESTrackingDetails_EmailSubjectFilters()
        {
            try
            {
                List<Sproc_Get_Email_SubjectList_Result> EmailSubjectList = new();
                EmailSubjectList = await _iReport.Sproc_Get_Email_SubjectList().ConfigureAwait(false);

                if (EmailSubjectList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = EmailSubjectList });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #endregion

        #region Private
        private IActionResult ExportDF_HrDetails(List<sproc_GetChannelWiseFunnelReportData_For_PopUP_Result> DetailList)
        {
            var ExportFileName = "HRDetail_" + DateTime.Now.ToString("yyyyMMdd") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("HRDetails");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "HR #";
            worksheet.Cell(currentRow, 2).Value = "Sales Person";
            worksheet.Cell(currentRow, 3).Value = "Company Name";
            worksheet.Cell(currentRow, 4).Value = "Role";
            worksheet.Cell(currentRow, 5).Value = "Managed/Self";
            worksheet.Cell(currentRow, 6).Value = "Availability";
            worksheet.Cell(currentRow, 7).Value = "# of TR";

            foreach (var details in DetailList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = details.HR_No;
                worksheet.Cell(currentRow, currentColumn++).Value = details.SalesPerson;
                worksheet.Cell(currentRow, currentColumn++).Value = details.CompnayName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Role;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Managed_Self;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Availability;
                worksheet.Cell(currentRow, currentColumn++).Value = details.TalentName;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }
        private IActionResult ExportSF_HrDetails(List<sproc_GetSupplyWiseChannelWiseFunnelReportData_For_PopUP_Result> DetailList)
        {
            var ExportFileName = "HRDetail_" + DateTime.Now.ToString("yyyyMMdd") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("HRDetails");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "HR #";
            worksheet.Cell(currentRow, 2).Value = "Sales Person";
            worksheet.Cell(currentRow, 3).Value = "Company Name";
            worksheet.Cell(currentRow, 4).Value = "Role";
            worksheet.Cell(currentRow, 5).Value = "Managed/Self";
            worksheet.Cell(currentRow, 6).Value = "Availability";
            worksheet.Cell(currentRow, 7).Value = "# of TR";

            foreach (var details in DetailList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = details.HR_No;
                worksheet.Cell(currentRow, currentColumn++).Value = details.SalesPerson;
                worksheet.Cell(currentRow, currentColumn++).Value = details.CompnayName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Role;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Managed_Self;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Availability;
                worksheet.Cell(currentRow, currentColumn++).Value = details.TalentName;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }

        private IActionResult ExportHF_DealDetails(List<Sproc_HubSpot_Client_Funnel_Report_PopUP_Result> DetailList)
        {
            var ExportFileName = "DealDetail_" + DateTime.Now.ToString("yyyyMMdd") + @".xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DealDetails");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Deal";
            worksheet.Cell(currentRow, 2).Value = "Sales Person";
            worksheet.Cell(currentRow, 3).Value = "Stage Name";
            worksheet.Cell(currentRow, 4).Value = "Deal #";

            foreach (var details in DetailList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = details.DealID;
                worksheet.Cell(currentRow, currentColumn++).Value = details.SalesPerson;
                worksheet.Cell(currentRow, currentColumn++).Value = details.StageName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.DealNumber;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }

        /// <summary>
        /// Exports the hr report details.
        /// </summary>
        /// <param name="DetailList">The detail list.</param>
        /// <returns></returns>
        private IActionResult Export_HrReportDetails(List<sproc_HiringRequest_PopupReport_Result> DetailList)
        {
            var ExportFileName = $"HRReportDetail_{DateTime.Now.ToString("yyyyMMdd")}.xlsx";
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("HRReportDetails");

            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Client";
            worksheet.Cell(currentRow, 2).Value = "Company Name";
            worksheet.Cell(currentRow, 3).Value = "HR #";

            foreach (var details in DetailList)
            {
                currentRow++;
                var currentColumn = 1;

                worksheet.Cell(currentRow, currentColumn++).Value = details.FullName;
                worksheet.Cell(currentRow, currentColumn++).Value = details.Company;
                worksheet.Cell(currentRow, currentColumn++).Value = details.HR_NUMBER;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportFileName);
        }

        #endregion
    }
}
