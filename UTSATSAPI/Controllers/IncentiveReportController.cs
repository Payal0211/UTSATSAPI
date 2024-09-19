using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("IncentiveReport/", Name = "IncentiveReport")]
    [ApiController]
    public class IncentiveReportController : ControllerBase
    {
        #region Variables
        private readonly IIncentiveReport _incentiveReport;

        #endregion

        #region Constructors
        public IncentiveReportController(IIncentiveReport incentiveReport)
        {
            _incentiveReport = incentiveReport;
        }
        #endregion

        #region Public Method

        [Authorize]
        [HttpGet("MonthYearFilter")]
        public ObjectResult MonthYearFilter()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                var start = DateTime.Now.AddMonths(-12);
                var end = DateTime.Now.AddMonths(11);

                List<SelectListItem> MonthYearList = new List<SelectListItem>();
                var list = new SelectListItem();

                while (end >= start)
                {
                    list = new SelectListItem();
                    var temp = start;
                    list.Text = String.Concat(temp.ToString("MMMM") + " " + temp.Year);
                    list.Value = String.Concat(temp.Month + "#" + temp.Year);

                    if (temp.Month == DateTime.Now.Month)
                    {
                        list.Selected = true;
                    }

                    start = start.AddMonths(1);
                    MonthYearList.Add(list);
                }

                dObject.MonthYear = MonthYearList;

                if (dObject != null)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = dObject });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("GetIncentiveReport")]
        public async Task<ObjectResult> GetIncentiveReport()
        {
            try
            {
                long LoggedInUserId = SessionValues.LoginUserId;
                IncentiveReportViewModel incentiveReportViewModel = new IncentiveReportViewModel();
                incentiveReportViewModel.SalesUserDDL = await _incentiveReport.GetUserTypeDDL().ConfigureAwait(false);
                if (LoggedInUserId != 0)
                {
                    var UserDetails = await _incentiveReport.GetUsrUserById(LoggedInUserId).ConfigureAwait(false);
                    if (UserDetails != null)
                    {

                        incentiveReportViewModel.UserTypeId = UserDetails.UserTypeId.Value;
                        var varSalesUserRoleDDL = await _incentiveReport.GetAllUsrUserRoleList();
                        incentiveReportViewModel.SalesUserRoleDDL = varSalesUserRoleDDL.Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.UserRole
                        }).OrderBy(x => x.Text).ToList();
                    }
                    var varUserDDL = await _incentiveReport.GetAllUsrUserList();
                    incentiveReportViewModel.UserDDL = varUserDDL.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString() }).OrderBy(x => x.Text).ToList();
                    incentiveReportViewModel.UserId = LoggedInUserId;
                    int today = DateTime.Now.Day;
                    int lastdayofMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    if (today == lastdayofMonth)
                        incentiveReportViewModel.IsLastDayOfMonth = true;

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = incentiveReportViewModel });
                }

                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("List")]
        public async Task<ObjectResult> GetIncentiveData(int month, int year, int salesManagerId)
        {
            try
            {
                 if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month > 12 || month < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                }
                else if (year > 9999 || year < 1900)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                }
                if (salesManagerId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide sales manger id" });
                }
                object[] param = { salesManagerId, month, year };
                string paramterstring = CommonLogic.ConvertToParamString(param);
                var incentiverreportList = await _incentiveReport.GetIncentiveReportData(paramterstring);
                if (incentiverreportList.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = incentiverreportList });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("CheckUserIsAM")]
        public async Task<ObjectResult> CheckUserIsAM(int salesManagerId)
        {
            try
            {
                var inc_ListAMNR = 0;
                var UserRoleDetail = await _incentiveReport.GetUsrUserRoleDetailById(salesManagerId);
                if (UserRoleDetail != null)
                {
                    if (UserRoleDetail.UserRoleId == 9)
                    {
                        inc_ListAMNR = 1;
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = inc_ListAMNR });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("CheckValidationForNBDandAM")]
        public async Task<ObjectResult> CheckValidationForNBDandAM(int userId, int month, int year)
        {
            try
            {
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month > 12 || month < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                }
                else if (year > 9999 || year < 1900)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                }
                if (userId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide user id" });
                }
                Object[] paramaters = { userId, month, year };
                string paramterstring = CommonLogic.ConvertToParamString(paramaters);
                ValidationMessageForIncentiveReport message = await _incentiveReport.GetValidationMessageForIncentiveReport(paramterstring);
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = message });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetIncentiveReportDetails")]
        public async Task<ObjectResult> GetIncentiveReportDetails(int month, int year, long userId, bool isSelfTargets)
        {
            try
            {
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month > 12 || month < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                }
                else if (year > 9999 || year < 1900)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                }
                if (userId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide user id" });
                }
                object[] parameter = { userId, month, year, isSelfTargets };
                string parameterstring = CommonLogic.ConvertToParamString(parameter);

                var result = await _incentiveReport.GetIncentiveReportDetails(parameterstring);
                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("CheckIsAMOrNR")]
        public async Task<ObjectResult> CheckIsAMOrNR(int salesManagerId)
        {
            try
            {
                var UserRoleDetail = await _incentiveReport.GetUsrUserRoleDetailById(salesManagerId);
                if (UserRoleDetail != null)
                {
                    if (UserRoleDetail.UserRoleId == 9 || UserRoleDetail.UserRoleId == 10)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = 1 });
                    }
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = 0 });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("GetIncentiveReportDataAMNR")]
        public async Task<ObjectResult> GetIncentiveReportDataAMNR(int month, int year, int salesManagerId)
        {
            try
            {
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month > 12 || month < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                }
                else if (year > 9999 || year < 1900)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                }
                if (salesManagerId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide sales manger id" });
                }
                object[] param = { salesManagerId, month, year };
                string paramterstring = CommonLogic.ConvertToParamString(param);
                var incentiveListAMOrNR = await _incentiveReport.GetIncentiveReportDataAMNR(paramterstring);
                if (incentiveListAMOrNR.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = incentiveListAMOrNR });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetIncentiveReportDetailsAMNR")]
        public async Task<ObjectResult> GetIncentiveReportDetailsAMNR(int month, int year, long userId, bool isSelfTargets)
        {
            try
            {
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month > 12 || month < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                }
                else if (year > 9999 || year < 1900)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                }
                if (userId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide user id." });
                }
                object[] param = { userId, month, year, isSelfTargets };
                string paramterstring = CommonLogic.ConvertToParamString(param);
                var result = await _incentiveReport.GetIncentiveReportDetailsAMOrNR(paramterstring);
                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetIncentiveReportDetailsContractBooster")]
        public async Task<ObjectResult> GetIncentiveReportDetailsContractBooster(int month, int year, long userId, bool isSelfTargets)
        {
            try
            {
                if (month == 0)
                {
                    month = DateTime.Now.Month;
                }
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                if (month > 12 || month < 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                }
                else if (year > 9999 || year < 1900)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                }
                if (userId <= 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide user id" });
                }

                object[] param = { userId, month, year, isSelfTargets };
                string paramterstring = CommonLogic.ConvertToParamString(param);
                var result = await _incentiveReport.GetIncentiveReportDetailsContractBooster(paramterstring);
                if (result != null && result.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("GetSalesUsersBasedOnUserRole")]
        public async Task<ObjectResult> GetSalesUsersBasedOnUserRole(int UserRoleId)
        {
            if (UserRoleId > 0)
            {
                IncentiveReportViewModel inc_ReportViewModel = new IncentiveReportViewModel();

                object[] param = { UserRoleId };
                string paramterstring = CommonLogic.ConvertToParamString(param);

                var varSalesUserDDL = await _incentiveReport.GetUsersBasedOnUserRole(paramterstring);

                inc_ReportViewModel.SalesUserDDL = varSalesUserDDL.Select(x => new SelectListItem { Text = x.UserName, Value = x.UserId.ToString() }).OrderBy(x => x.Text).ToList();
                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = inc_ReportViewModel.SalesUserDDL });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
        }


        [HttpPost("CalculateIncentiveData")]
        public async Task<ObjectResult> CalculateIncentiveData(int month, int year)
        {
            bool result = false;
            try
            {
                int lastdayofMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (DateTime.Now.Day != lastdayofMonth)
                {
                    if (month == 0)
                    {
                        month = DateTime.Now.Month;
                    }
                    if (year == 0)
                    {
                        year = DateTime.Now.Year;
                    }
                    if (month > 12 || month < 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid month." });
                    }
                    else if (year > 9999 || year < 1900)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid year." });
                    }
                    long LoggedInUserID = SessionValues.LoginUserId;


                    object[] param = { month, year, LoggedInUserID };
                    string paramterstring = CommonLogic.ConvertToParamString(param);

                    result = await _incentiveReport.InsertTargetAcheivedDetails(paramterstring);
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetUserHierarchy")]
        public async Task<ObjectResult> GetUserHierarchy(int Parentid)
        {
            try
            {
                List<SROC_GET_HIERARCHY_Result> ListUsers = new List<SROC_GET_HIERARCHY_Result>();
                if (Parentid == 0)
                {
                    ListUsers = await _incentiveReport.GetHierarchy(0, 0);
                }
                else
                {

                    var UserDetail = await _incentiveReport.GetUsrUserById(Parentid).ConfigureAwait(false);
                    if (UserDetail != null)
                    {
                        if (UserDetail.UserTypeId == 4 || UserDetail.UserTypeId == 9)
                        {
                            ListUsers = await _incentiveReport.GetHierarchy(Parentid, 0).ConfigureAwait(false);
                        }
                        else if (UserDetail.UserTypeId == 5 || UserDetail.UserTypeId == 10)
                        {
                            ListUsers = await _incentiveReport.GetHierarchy(Parentid, 1).ConfigureAwait(false);
                        }
                        else if (UserDetail.UserTypeId == 11)
                        {
                            ListUsers = await _incentiveReport.GetHierarchy(Parentid, 2).ConfigureAwait(false);
                        }
                        else if (UserDetail.UserTypeId == 12)
                        {
                            ListUsers = await _incentiveReport.GetHierarchy(Parentid, 3).ConfigureAwait(false);
                        }

                    }
                }
                if (ListUsers.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = ListUsers });

                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No records found" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}