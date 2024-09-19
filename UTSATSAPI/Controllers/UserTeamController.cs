using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using UTSATSAPI.Helpers.Common;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("UserTeamOperationsAPI/", Name = "User Team Related Services")]
    public class UserTeamController : Controller
    {
        #region Variables

        private readonly ILogger<UsrUserController> _logger;
        private readonly IConfiguration _iConfiguration;
        private readonly IUserTeam _iUserTeam;

        #endregion

        #region Constructors

        public UserTeamController(IUserTeam iUserTeam, ILogger<UsrUserController> logger, IConfiguration iConfiguration)
        {
            _logger = logger;
            _iConfiguration = iConfiguration;
            _iUserTeam = iUserTeam;
        }

        #endregion

        #region Public APIs


        [HttpPost("GetUserTeamsList")]
        public async Task<ObjectResult> GetUserTeamsList(GetTeamsViewModel getTeamViewModel)
        {
            try
            {

                object[] param = new object[]
                {
                    getTeamViewModel.PageIndex > 0 ? getTeamViewModel.PageIndex : 1,
                    getTeamViewModel.PageSize > 0 ? getTeamViewModel.PageSize : 50,
                    string.IsNullOrEmpty(getTeamViewModel.SortExpression) ? "ID" : getTeamViewModel.SortExpression,
                    string.IsNullOrEmpty(getTeamViewModel.SortDirection) ? "asc" : getTeamViewModel.SortDirection,
                    getTeamViewModel.Team,
                    getTeamViewModel.DeptName,
                    getTeamViewModel.UserType
                };

                string paramasString = CommonLogic.ConvertToParamString(param);

                getTeamViewModel.PageIndex = getTeamViewModel.PageIndex > 0 ? getTeamViewModel.PageIndex : 1;
                getTeamViewModel.PageSize = getTeamViewModel.PageSize > 0 ? getTeamViewModel.PageSize : 50;

                List<Sproc_GetTeams_Result> objGetTeams_Results = await _iUserTeam.GetGenTeamsList(paramasString);

                if (objGetTeams_Results.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(objGetTeams_Results, objGetTeams_Results[0].TotalRecords, Convert.ToInt64(getTeamViewModel.PageSize), Convert.ToInt64(getTeamViewModel.PageIndex)) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Team available" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("CheckTeamExistOrNot")]
        public async Task<ObjectResult> CheckTeamExistOrNot(string team)
        {
            try
            {
                if (!string.IsNullOrEmpty(team))
                {
                    var teamValue = await _iUserTeam.GetGenTeamsByTeam(team).ConfigureAwait(false);
                    if (teamValue == null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Team Not Exists" });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "Team Exists" });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Team Required" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAddEditUserTeam")]
        public async Task<ObjectResult> GetAddEditUserTeam(int? id)
        {
            try
            {
                UserTeamViewModel objUserTeamViewModel = new UserTeamViewModel();
                if (id != null && id != 0)
                {
                    GenTeam teams = await _iUserTeam.GetGenTeamsById(Convert.ToInt64(id)).ConfigureAwait(false);
                    if (teams != null && teams.Id > 0)
                    {
                        objUserTeamViewModel.ID = teams.Id;
                        objUserTeamViewModel.Team = teams.Team;
                        objUserTeamViewModel.DeptID = teams.DeptId;
                        objUserTeamViewModel.TeamHeadID = teams.TeamHeadId;
                        objUserTeamViewModel.IsActive = Convert.ToInt32(teams.IsActive);
                        objUserTeamViewModel.IsTeamForNewUser = Convert.ToInt32(teams.IsTeamForNewUser);

                        var varPrgDepartment = await _iUserTeam.GetPrgDepartmentList().ConfigureAwait(false);
                        objUserTeamViewModel.DepartmentDrp = varPrgDepartment.ToList().Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.DeptName
                        });

                        var varUsrUser = await _iUserTeam.GetUsrUserList().ConfigureAwait(false);
                        objUserTeamViewModel.TeamHeadDrp = varUsrUser.ToList().Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.FullName
                        });
                        objUserTeamViewModel.BindIsActive = new Dictionary<string, string>();
                        objUserTeamViewModel.BindIsActive.Add("0", "No");
                        objUserTeamViewModel.BindIsActive.Add("1", "Yes");

                        objUserTeamViewModel.BindIsNewUser = new Dictionary<string, string>();
                        objUserTeamViewModel.BindIsNewUser.Add("0", "No");
                        objUserTeamViewModel.BindIsNewUser.Add("1", "Yes");

                        objUserTeamViewModel.DrpIsActive = objUserTeamViewModel.BindIsActive.ToList().Select(x => new SelectListItem
                        {
                            Value = x.Key.ToString(),
                            Text = x.Value
                        });
                        objUserTeamViewModel.DrpIsNewUser = objUserTeamViewModel.BindIsNewUser.ToList().Select(x => new SelectListItem
                        {
                            Value = x.Key.ToString(),
                            Text = x.Value
                        });
                        var varUsrUserRole = await _iUserTeam.GetUsrUserRoleList().ConfigureAwait(false);
                        objUserTeamViewModel.RoleHeadDrp = varUsrUserRole.ToList().Select(x => new SelectListItem
                        {
                            Value = x.GroupRoleId.ToString(),
                            Text = x.UserRole
                        });
                        objUserTeamViewModel.RoleHeadID = teams.TeamRoleId;
                    }


                    if (objUserTeamViewModel != null)
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = objUserTeamViewModel });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Team available" });
                }
                else
                {
                    var varPrgDepartment = await _iUserTeam.GetPrgDepartmentList().ConfigureAwait(false);
                    objUserTeamViewModel.DepartmentDrp = varPrgDepartment.ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.DeptName
                    });
                    var varUsrUser = await _iUserTeam.GetUsrUserList().ConfigureAwait(false);
                    objUserTeamViewModel.TeamHeadDrp = varUsrUser.ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    });
                    objUserTeamViewModel.BindIsActive = new Dictionary<string, string>();
                    objUserTeamViewModel.BindIsActive.Add("0", "No");
                    objUserTeamViewModel.BindIsActive.Add("1", "Yes");

                    objUserTeamViewModel.BindIsNewUser = new Dictionary<string, string>();
                    objUserTeamViewModel.BindIsNewUser.Add("0", "No");
                    objUserTeamViewModel.BindIsNewUser.Add("1", "Yes");

                    objUserTeamViewModel.DrpIsActive = objUserTeamViewModel.BindIsActive.ToList().Select(x => new SelectListItem
                    {
                        Value = x.Key.ToString(),
                        Text = x.Value
                    });
                    objUserTeamViewModel.DrpIsNewUser = objUserTeamViewModel.BindIsNewUser.ToList().Select(x => new SelectListItem
                    {
                        Value = x.Key.ToString(),
                        Text = x.Value
                    });
                    var varUsrUserRole = await _iUserTeam.GetUsrUserRoleList().ConfigureAwait(false);
                    objUserTeamViewModel.RoleHeadDrp = varUsrUserRole.ToList().Select(x => new SelectListItem
                    {
                        Value = x.GroupRoleId.ToString(),
                        Text = x.UserRole
                    });
                    if (objUserTeamViewModel != null)
                        return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = objUserTeamViewModel });
                    else
                        return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No Team data available" });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
