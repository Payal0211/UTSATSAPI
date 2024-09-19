using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    [Authorize]
    [Route("IncentiveTarget/", Name = "IncentiveTarget")]
    [ApiController]
    public class IncentiveTargetController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _context;
        private readonly ICommonInterface _commonInterface;
        private readonly IConfiguration _iConfiguration;
        private readonly IIncentiveTarget _incentiveTarget;
        private readonly IIncentiveReport _incentiveReport;
        #endregion

        #region Constructors
        public IncentiveTargetController(TalentConnectAdminDBContext context, IIncentiveTarget incentiveTarget, IIncentiveReport incentiveReport, ICommonInterface commonInterface, IConfiguration iConfiguration)
        {
            _context = context;
            _commonInterface = commonInterface;
            _iConfiguration = iConfiguration;
            _incentiveTarget = incentiveTarget;
            _incentiveReport = incentiveReport;
        }
        #endregion

        #region Public APIs

        #region Incentive Target List
        [Authorize]
        [HttpGet("MonthYearFilter")]
        public ObjectResult MonthYearFilter()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                var start = DateTime.Now.AddMonths(-10);
                var end = DateTime.Now.AddMonths(9);

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

        [Authorize]
        [HttpGet("List")]
        public async Task<ObjectResult> IncentiveTargetList(IncentiveTargetListFilter filter)
        {
            try
            {
                if (filter == null || string.IsNullOrEmpty(filter.targetMonthYear))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid Month year for check the data" });
                }

                int month = 0, year = 0;
                if (!string.IsNullOrEmpty(filter.targetMonthYear))
                {
                    month = Convert.ToInt32(filter.targetMonthYear.Split('#')[0]);
                    year = Convert.ToInt32(filter.targetMonthYear.Split('#')[1]);
                }

                object[] param = new object[] 
                { 
                    filter.PageIndex ?? 1, filter.PageSize ?? 50,
                    filter.SortExpression ?? "CreatedByDatetime", filter.SortDirection ?? "ASC",
                    filter.UserName ?? string.Empty, filter.UserRole ?? string.Empty,
                    filter.ParentId ?? 0, month, year
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<sproc_Get_IncentiveTarget_List_New_flow> result =
                    await _incentiveTarget.GetIncentiveTargetList(paramasString).ConfigureAwait(false);

                if (result.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = result });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add/Update Incentive Target
        [Authorize]
        [HttpGet("Filters")]
        public async Task<ObjectResult> IncentiveTarget_Filters()
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                dObject.SalesManager = await _incentiveReport.GetUserTypeDDL().ConfigureAwait(false);
                dObject.IsLastDayOfMonth = false;

                int today = DateTime.Now.Day;
                int lastdayofMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                if (today == lastdayofMonth)
                    dObject.IsLastDayOfMonth = true;

                var start = DateTime.Now.AddMonths(-1);
                var end = start.AddYears(1);

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
        [HttpGet("GetUserRole")]
        public ObjectResult GetUserRole(int userId)
        {
            try
            {
                dynamic dObject = new ExpandoObject();

                var Role = (from urd in _context.UsrUserRoleDetails.AsEnumerable()
                            join ur in _context.UsrUserRoles on urd.UserRoleId equals ur.Id
                            where urd.UserId == userId
                            select new
                            {
                                UserRole = ur.UserRole
                            }).Select(x => x.UserRole).FirstOrDefault();

                dObject.UserRole = "User Role : " + Role;

                string extraText = string.Empty;
                if (dObject.UserRole == "AM" || dObject.UserRole == "AM Head")
                {
                    extraText = "talent deployed";
                }
                else
                {
                    extraText = "client closure";
                }

                dObject.Notes = "Please Add Number of " + extraText + " targets for individual here";
                dObject.Notes1 = "The number you need to add is a count of targets you are going to chase for every month.";

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

        [Authorize]
        [HttpGet("GetSalesUserHierarchy")]
        public async Task<ObjectResult> GetSalesUserHierarchy(int ManagerID, string targetMonthYear)
        {
            try
            {
                if (ManagerID == 0 || string.IsNullOrEmpty(targetMonthYear))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid Manager ID or Month year for check the data" });
                }

                int month = 0, year = 0;
                if (!string.IsNullOrEmpty(targetMonthYear))
                {
                    month = Convert.ToInt32(targetMonthYear.Split('#')[0]);
                    year = Convert.ToInt32(targetMonthYear.Split('#')[1]);
                }

                object[] param = new object[] { ManagerID, month, year };

                string paramasString = CommonLogic.ConvertToParamString(param);

                List<Sproc_Get_ALL_User_HIERARCHY_SaleTarget_For_Parent_Result> HierachyStructure =
                    await _incentiveTarget.GetSalesUserHierarchy(paramasString).ConfigureAwait(false);

                if (HierachyStructure.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = HierachyStructure });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Data Available" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost("SaveIncentiveTarget")]
        public async Task<ObjectResult> SaveIncentiveTarget(SalesTarget salesTarget)
        {
            try
            {
                var LoggedInUserID = SessionValues.LoginUserId;

                if (salesTarget == null || string.IsNullOrEmpty(salesTarget.targetMonthYear))
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Please provide valid data & Month year for save operation" });
                }

                int month = 0, year = 0;
                if (!string.IsNullOrEmpty(salesTarget.targetMonthYear))
                {
                    month = Convert.ToInt32(salesTarget.targetMonthYear.Split('#')[0]);
                    year = Convert.ToInt32(salesTarget.targetMonthYear.Split('#')[1]);
                }

                if (salesTarget.UserHierarchySalesTarget.Any())
                {
                    object[] param = null;
                    string paramasString = string.Empty;

                    foreach (var item in salesTarget.UserHierarchySalesTarget)
                    {
                        decimal self_Target = 0, child_Target = 0;
                        child_Target = salesTarget.UserHierarchySalesTarget.Where(x => x.UNDER_PARENT == item.UserID).Sum(x => x.UserTarget);
                        if (item.UserTarget > child_Target)
                            self_Target = (item.UserTarget - child_Target);

                        param = new object[] { item.UserID, item.UserTarget, LoggedInUserID, month, year, self_Target };
                        paramasString = CommonLogic.ConvertToParamString(param);

                        _incentiveTarget.InsertIncentiveUserTarget(paramasString);
                    }

                    param = new object[] { month, year, LoggedInUserID };
                    paramasString = CommonLogic.ConvertToParamString(param);

                    await _incentiveReport.InsertTargetAcheivedDetails(paramasString);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No any Records for save operation" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion
    }
}
