namespace UTSATSAPI.Controllers
{
    using DocumentFormat.OpenXml.Office2010.Excel;
    using FluentValidation.Results;
    using Google;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using UTSATSAPI.ATSCalls;
    using UTSATSAPI.Helpers;
    using UTSATSAPI.Helpers.Common;
    using UTSATSAPI.Models.ComplexTypes;
    using UTSATSAPI.Models.Models;
    using UTSATSAPI.Models.ViewModel;
    using UTSATSAPI.Models.ViewModels;
    using UTSATSAPI.Models.ViewModels.Validators;
    using UTSATSAPI.Repositories.Interfaces;
    using UTSATSAPI.ViewModel;
    using static UTSATSAPI.Helpers.Enum;
    using AuthorizeAttribute = Helpers.Common.AuthorizeAttribute;

    [Route("User/", Name = "User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Variables
        private readonly TalentConnectAdminDBContext _context;
        private readonly IUsers _iUsers;
        private readonly IConfiguration _iConfiguration;
        private readonly IUniversalProcRunner _universalProcRunner;
        private readonly IAdminUserLogin _iAdminUserLogin;
        #endregion

        #region Constructors
        public UserController(TalentConnectAdminDBContext context, IConfiguration iConfiguration, IUniversalProcRunner universalProcRunner, IAdminUserLogin iAdminUserLogin, IUsers iUsers)
        {
            _context = context;
            _iUsers = iUsers;
            _iConfiguration = iConfiguration;
            _universalProcRunner = universalProcRunner;
            _iAdminUserLogin = iAdminUserLogin;
        }
        #endregion

        #region Public APIs

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="getUsersAPIModel">The get users API model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("List")]
        public ObjectResult GetUsers([FromBody] GetUsersAPIModel getUsersAPIModel)
        {
            try
            {
                #region PreValidation
                if (getUsersAPIModel == null || (getUsersAPIModel.pagenumber == 0 || getUsersAPIModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });
                #endregion

                getUsersAPIModel.FilterFields_UserList ??= new();

                getUsersAPIModel.Sortdatafield = string.IsNullOrEmpty(getUsersAPIModel.Sortdatafield) ? "CreatedbyDatetime" : getUsersAPIModel.Sortdatafield;
                getUsersAPIModel.Sortorder = string.IsNullOrEmpty(getUsersAPIModel.Sortorder) ? "DESC" : getUsersAPIModel.Sortorder;

                //Trim the blank spaces from the end of the search text.
                string searchText = string.Empty;
                if (!string.IsNullOrEmpty(getUsersAPIModel.searchText))
                {
                    searchText = getUsersAPIModel.searchText.TrimStart().TrimEnd();
                }

                object[] param = new object[] {
                    getUsersAPIModel.pagenumber, getUsersAPIModel.totalrecord,
                    getUsersAPIModel.Sortdatafield == "string" ? "CreatedbyDatetime" : getUsersAPIModel.Sortdatafield,
                    getUsersAPIModel.Sortorder == "string" ? "desc" :getUsersAPIModel.Sortorder,
                    getUsersAPIModel.FilterFields_UserList.UserName == "string" ? "" : getUsersAPIModel.FilterFields_UserList.UserName,
                    getUsersAPIModel.FilterFields_UserList.FullName == "string" ? "" : getUsersAPIModel.FilterFields_UserList.FullName ,
                    getUsersAPIModel.FilterFields_UserList.EmployeeID == "string" ? "" : getUsersAPIModel.FilterFields_UserList.EmployeeID ,
                    getUsersAPIModel.FilterFields_UserList.UserType == "string" ? "" : getUsersAPIModel.FilterFields_UserList.UserType ,
                    getUsersAPIModel.FilterFields_UserList.EmailId == "string" ? "" : getUsersAPIModel.FilterFields_UserList.EmailId ,
                    getUsersAPIModel.FilterFields_UserList.TeamManager == "string" ? "" : getUsersAPIModel.FilterFields_UserList.TeamManager  ,
                    getUsersAPIModel.FilterFields_UserList.PriorityCount == "string" ? "" : getUsersAPIModel.FilterFields_UserList.PriorityCount ,
                    getUsersAPIModel.FilterFields_UserList.OpsManager == "string" ? "" : getUsersAPIModel.FilterFields_UserList.OpsManager  ,
                    getUsersAPIModel.FilterFields_UserList.NBD_AM == "string" ? "" : getUsersAPIModel.FilterFields_UserList.NBD_AM,
                    searchText //UTS-3780: Pass the search text to the DB.
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<sproc_UTS_GetUsers_Result> usersListData = _iUsers.GetUsers(paramasString);

                if (usersListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(usersListData, usersListData[0].TotalRecords.Value, getUsersAPIModel.totalrecord, getUsersAPIModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No User Available", Details = CustomRendering.EmptyRows() });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetUserDetail")]
        public async Task<ObjectResult> GetUser(long userId)
        {
            try
            {
                if (userId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "UserId is not valid" });

                UsrUser usrUser = _context.UsrUsers.AsNoTracking().FirstOrDefault(x => x.Id == userId);
                UsrUserHierarchy userHierarchy = _context.UsrUserHierarchies.AsNoTracking().Where(xy => xy.UserId == userId).FirstOrDefault();
                UsrUserRoleDetail UserRoleDetails = _context.UsrUserRoleDetails.AsNoTracking().Where(x => x.UserId == userId).FirstOrDefault();
                if (usrUser != null)
                {
                    UserViewModel user = new UserViewModel();
                    user.Id = usrUser.Id;
                    user.EmployeeId = usrUser.EmployeeId;
                    user.FullName = usrUser.FullName;
                    user.IsNewUser = usrUser.IsNewUser;
                    user.UserTypeId = usrUser.DeptId == 4 ? 1 : usrUser.UserTypeId;
                    user.RoleId = usrUser.RoleId;
                    user.IsOdr = usrUser.IsOdr;
                    user.PriorityCount = usrUser.PriorityCount;
                    user.SkypeId = usrUser.SkypeId;
                    user.EmailId = usrUser.EmailId;
                    user.ContactNumber = usrUser.ContactNumber;
                    user.Designation = usrUser.Designation;
                    user.ProfilePic = (!string.IsNullOrEmpty(usrUser.ProfilePic) ? usrUser.ProfilePic : string.Empty);
                    user.Description = usrUser.Description;
                    user.DeptID = usrUser.DeptId;
                    user.TeamID = usrUser.DeptId == 4 ? "0" : usrUser.TeamId;
                    user.LevelID = usrUser.DeptId == 4 ? 0 : usrUser.LevelId;
                    user.IsActive = usrUser.IsActive;
                    user.fileUpload = new FileUploadModelBase64() { Base64ProfilePic = (!string.IsNullOrEmpty(usrUser.ProfilePic) ? CommonLogic.GetBase64FromImage(usrUser.ProfilePic, Constants.MediaConstant.UserProfile) : string.Empty) };
                    //user.Another_UserTypeID = usrUser.AnotherUserTypeId.Value != 0 ? usrUser.AnotherUserTypeId.Value : 0;
                    if (user.UserTypeId == 4 && usrUser.AnotherUserTypeId == 0)
                    {
                        user.Another_UserTypeID = 4;
                    }
                    else if (user.UserTypeId == 5 && usrUser.AnotherUserTypeId == 0)
                    {
                        user.Another_UserTypeID = 5;
                    }
                    else if (user.UserTypeId == 9 && usrUser.AnotherUserTypeId == 0)
                    {
                        user.Another_UserTypeID = 4;
                    }
                    else if (user.UserTypeId == 10 && usrUser.AnotherUserTypeId == 0)
                    {
                        user.Another_UserTypeID = 5;
                    }
                    else if (user.UserTypeId == 1 && usrUser.AnotherUserTypeId == 0)
                    {
                        user.Another_UserTypeID = 1;
                    }
                    else if ((user.UserTypeId == 4 || user.UserTypeId == 5 || user.UserTypeId == 9 || user.UserTypeId == 10) && usrUser.AnotherUserTypeId > 0)
                    {
                        user.Another_UserTypeID = 6;
                    }
                    else
                    {
                        user.Another_UserTypeID = 7;
                    }

                    user.detail = new DetailList();

                    //Geo List & Ids
                    var UserGeo = _context.UsrUserGeoDetails.Where(x => x.UserId == usrUser.Id).Select(x => new MastersResponseModel { Value = x.GeoId, Id = x.Id }).ToList();

                    if (UserGeo.Any())
                        user.detail.GeoList = UserGeo;

                    SPROC_UTS_UserGeoValues_Result Geo = _iUsers.SPROC_UTS_UserGeoValues(userId);
                    if (Geo != null)
                        user.detail.geoIDs = !string.IsNullOrEmpty(Geo.GeoID) ? Array.ConvertAll(Geo.GeoID.Split(','), int.Parse) : new int[] { };

                    //User type dropdown list
                    var GetUserTypeList = _context.UsrUserTypes.Where(x => x.IsDisplay == true).Select(x => new MastersResponseModel { Value = x.UserType, Id = x.Id }).ToList();
                    user.detail.userTypeID = usrUser.DeptId == 4 ? 1 : usrUser.UserTypeId;

                    if (GetUserTypeList.Any())
                        user.detail.UserTypeList = GetUserTypeList;

                    //TalentRole Dropdown lsit
                    var TalentRole = _context.PrgTalentRoles.ToList().Where(x => x.IsActive == true).OrderBy(x => x.TalentRole).Select(x => new MastersResponseModel
                    { Value = x.TalentRole, Id = x.Id }).ToList();

                    if (TalentRole.Any())
                        user.detail.TalentRoleList = TalentRole;

                    int UserTypeTeamManager = 0;
                    if (user.UserTypeId == 5)
                    {
                        UserTypeTeamManager = 10;
                    }
                    else if (user.UserTypeId == 4)
                    {
                        UserTypeTeamManager = 9;
                    }

                    if (UserTypeTeamManager > 0)
                    {
                        user.detail.TeamManagerList = _context.UsrUsers.Where(x => x.UserTypeId == UserTypeTeamManager).ToList()
                        .Select(x => new MastersResponseModel
                        {
                            Id = x.Id,
                            Value = x.FullName
                        }).ToList();
                    }
                    else
                    {
                        List<MastersResponseModel> listItems = new List<MastersResponseModel>();
                        user.detail.TeamManagerList = listItems;
                    }

                    if (userHierarchy != null)
                    {
                        var data = usrUser.DeptId == 4 ? 0 : userHierarchy.ParentId;
                        var SalesMamanger = _context.UsrUserHierarchies.Where(x => x.UserId == (data)).FirstOrDefault();
                        if (SalesMamanger != null && SalesMamanger.ParentId > 0)
                        {
                            user.detail.teamManagerID = SalesMamanger.ParentId;
                            user.userHierarchyParentID = userHierarchy.ParentId;
                            user.detail.reporteeUserID = SalesMamanger.UserId;
                            user.ManagerID = SalesMamanger.UserId;

                            var UserTypelist = _context.UsrUserHierarchies.Where(x => x.ParentId == SalesMamanger.ParentId).ToList();
                            List<UsrUser> ReporteeUserList = new List<UsrUser>();
                            if (UserTypelist.Count > 0)
                            {
                                foreach (var item in UserTypelist)
                                {
                                    var RepUserUser = _context.UsrUsers.Where(x => x.Id == item.UserId).FirstOrDefault();
                                    if (RepUserUser != null)
                                        ReporteeUserList.Add(RepUserUser);
                                }
                                user.detail.ReporteeUserList = ReporteeUserList.Select(x => new MastersResponseModel
                                {
                                    Id = x.Id,
                                    Value = x.FullName
                                }).ToList();
                            }
                            else
                            {
                                List<MastersResponseModel> listItems = new List<MastersResponseModel>();
                                user.detail.ReporteeUserList = listItems;
                            }
                        }
                        else
                        {
                            user.detail.teamManagerID = usrUser.DeptId == 4 ? 0 : userHierarchy.ParentId;
                            user.userHierarchyParentID = usrUser.DeptId == 4 ? 0 : userHierarchy.ParentId;
                            user.ManagerID = 0;
                            List<MastersResponseModel> listItems = new List<MastersResponseModel>();
                            user.detail.ReporteeUserList = listItems;
                        }
                    }
                    else
                    {
                        List<MastersResponseModel> listItems = new List<MastersResponseModel>();
                        user.detail.ReporteeUserList = listItems;
                    }

                    if (UserRoleDetails != null)
                    {
                        user.RoleId = UserRoleDetails.UserRoleId;
                        user.detail.userRoleID = UserRoleDetails.UserRoleId;
                        int userType = user.UserTypeId == null ? 0 : Convert.ToInt32(user.UserTypeId);
                        var varUserRoleList = await _iUsers.Get_UserRoleList(userType).ConfigureAwait(false);
                        user.detail.UserRoleList = varUserRoleList;

                        //user.detail.UserRoleList = (from utrd in _context.UsrUserTypeAndRoleDetails.AsEnumerable()
                        //                            join ur in _context.UsrUserRoles on utrd.UserRoleId equals ur.Id
                        //                            where utrd.UserTypeId == userType
                        //                            select new MastersResponseModel()
                        //                            {
                        //                                Value = ur.UserRole,
                        //                                Id = ur.Id
                        //                            }).ToList();

                        if (UserRoleDetails.UserRoleId == 3 || UserRoleDetails.UserRoleId == 4 || UserRoleDetails.UserRoleId == 6 || UserRoleDetails.UserRoleId == 7)
                        {
                            int RoleID = 0;
                            if (UserRoleDetails.UserRoleId == 3)
                            {
                                RoleID = 4;
                            }
                            if (UserRoleDetails.UserRoleId == 4)
                            {
                                RoleID = 5;
                            }
                            if (UserRoleDetails.UserRoleId == 6)
                            {
                                RoleID = 7;
                            }
                            if (UserRoleDetails.UserRoleId == 7)
                            {
                                RoleID = 8;
                            }

                            object[] param = new object[] { RoleID };

                            List<MastersResponseModel> userList = _iUsers.sproc_UTS_GetBDR_Marketingusers(CommonLogic.ConvertToParamString(param)).Select(x => new MastersResponseModel
                            {
                                Id = x.ID,
                                Value = x.Fullname
                            }).ToList();

                            if (userList.Any())
                            {
                                user.detail.teamManagerID = usrUser.DeptId == 4 ? 0 : userHierarchy.ParentId;
                                user.detail.TeamManagerList = userList;
                            }
                        }
                    }
                    else
                    {
                        int userType = user.UserTypeId == null ? 0 : Convert.ToInt32(user.UserTypeId);
                        var varUserRoleList = await _iUsers.Get_UserRoleList(userType).ConfigureAwait(false);
                        user.detail.UserRoleList = varUserRoleList;


                        //user.detail.UserRoleList=(from utrd in _context.UsrUserTypeAndRoleDetails.AsNoTracking().AsEnumerable()
                        //                          join ur in _context.UsrUserRoles.AsNoTracking() on utrd.UserRoleId equals ur.Id
                        //                          where utrd.UserTypeId == userType
                        //                          select new MastersResponseModel()
                        //                          {
                        //                              Value = ur.UserRole,
                        //                              Id = ur.Id
                        //                          }).ToList();
                    }

                    if (user.UserTypeId == 11 || user.UserTypeId == 12)
                    {
                        if (userHierarchy != null)
                        {
                            user.detail.teamManagerID = usrUser.DeptId == 4 ? 0 : userHierarchy.ParentId;
                            user.userHierarchyParentID = usrUser.DeptId == 4 ? 0 : userHierarchy.ParentId;
                            user.ManagerID = 0;
                        }
                    }
                    user.ReportingHierarchy = _iUsers.sproc_GetUserHierarchy_Results(user.Id, user.EmployeeId);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = user });
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "User not found" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("IsEmployeeIDExist")]
        public ObjectResult IsEmployeeIDExist(long id, string employeeId)
        {
            bool isUserAlreadyExist = false;
            try
            {
                if (string.IsNullOrEmpty(employeeId))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide EmployeeId" });

                if (id > 0)
                    isUserAlreadyExist = _context.UsrUsers.Any(x => x.EmployeeId.ToLower().Equals(employeeId.Trim().ToLower()) && x.Id != id);
                else
                    isUserAlreadyExist = _context.UsrUsers.Any(x => x.EmployeeId.ToLower().Equals(employeeId.Trim().ToLower()));

                if (!isUserAlreadyExist)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Go ahead" });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "EmployeeID already exists. Please try again with another EmployeeID" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("IsEmployeeFullNameExist")]
        public ObjectResult IsFullNameExist(long id, string employeeFullName)
        {
            bool isUserAlreadyExist = false;
            try
            {
                if (string.IsNullOrEmpty(employeeFullName))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide Full Name" });

                if (id > 0)
                    isUserAlreadyExist = _context.UsrUsers.Any(x => x.FullName.ToLower().Equals(employeeFullName.ToLower().Trim()) && x.Id != id);
                else
                    isUserAlreadyExist = _context.UsrUsers.Any(x => x.FullName.ToLower().Equals(employeeFullName.Trim().ToLower()));

                if (!isUserAlreadyExist)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Go ahead" });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Full Name already exists. Please try again with another Name" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the edit user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <param name="LoggedInUserId">The logged in user identifier.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("AddEdit")]
        public async Task<ObjectResult> AddEditUser([FromBody] UserViewModel userModel, long LoggedInUserId = 0)
        {
            bool isUserAlreadyExist = false;
            bool isSuccess = false;
            try
            {
                LoggedInUserId = SessionValues.LoginUserId;
                #region PreValidation

                if (userModel == null || LoggedInUserId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide data for save operation" });

                UserValidator validationRules = new();
                ValidationResult validationUserResult = validationRules.Validate(userModel);
                if (!validationUserResult.IsValid)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Validation Error", Details = CommonLogic.SerializeErrors(validationUserResult.Errors, "User") });

                if (userModel.Id > 0)
                    isUserAlreadyExist = _context.UsrUsers.Any(x => (x.EmployeeId.ToLower().Equals(userModel.EmployeeId.ToLower())) && x.Id != userModel.Id);
                else
                    isUserAlreadyExist = _context.UsrUsers.Any(x => x.EmployeeId.ToLower().Equals(userModel.EmployeeId.ToLower()));

                if (isUserAlreadyExist)
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "EmployeeID already exists. Please try again with another EmployeeID" });

                #endregion

                #region Check Priority Count Validation at the time of Update
                if (userModel.Id > 0 && userModel.DeptID == 1 && userModel.LevelID == 1)
                {
                    if (userModel.PriorityCount == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide new priority count" });
                    }

                    object[] objects = new object[] { userModel.Id, userModel.PriorityCount };
                    string paramtersstring = CommonLogic.ConvertToParamString(objects);
                    CheckValidationMessage validationMessage = _iUsers.CheckvalidationMessageForHeadPriorityCount(paramtersstring);

                    if (!string.IsNullOrWhiteSpace(validationMessage.Message))
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = validationMessage.Message });
                }
                #endregion

                UsrUser user = new UsrUser();
                UsrUserHierarchy userHierarchy = new UsrUserHierarchy();
                UsrUserRoleDetail userRoleDetails = new UsrUserRoleDetail();

                if (userModel.Id > 0)
                {
                    #region Update user

                    UsrUserPriorityCountLog userprioritycount = new UsrUserPriorityCountLog();
                    int oldprioritycount = 0;
                    int newprioritycount = 0;
                    int remainingprioritycount = 0;
                    int calculatedremainingcount = 0;

                    user = _context.UsrUsers.FirstOrDefault(xy => xy.Id == userModel.Id);

                    if (user == null)
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "User not exist" });

                    user.EmployeeId = userModel.EmployeeId;
                    user.FullName = userModel.FullName;

                    if (!string.IsNullOrEmpty(userModel.FullName))
                    {
                        string[] splitName = userModel.FullName.Split(' ');
                        if (splitName.Length > 0)
                        {
                            user.FirstName = splitName[0];
                            user.LastName = splitName.Length > 1 ? splitName[1] : "";
                        }
                    }

                    user.EmailId = userModel.EmailId ?? "";
                    user.ContactNumber = userModel.ContactNumber ?? "";
                    user.IsActive = userModel.IsActive;
                    user.SkypeId = userModel.SkypeId ?? "";
                    user.Description = userModel.Description ?? "";
                    user.ProfilePic = userModel.ProfilePic ?? "";
                    user.Designation = userModel.Designation;
                    user.RoleId = userModel.RoleId;
                    user.DeptId = userModel.DeptID;
                    user.TeamId = userModel.DeptID == 4 ? "0" : userModel.TeamID;
                    user.LevelId = userModel.DeptID == 4 ? 0 : userModel.LevelID;
                    user.IsOdr = userModel.IsOdr;
                    user.GeoId = (userModel.GeoIds != null && userModel.GeoIds.Any()) ? userModel.GeoIds[0] : 0;
                    if (userModel.Another_UserTypeID == 6)
                    {
                        if (user.UserTypeId == 4 || user.UserTypeId == 9)
                            user.AnotherUserTypeId = 5;
                        else if (user.UserTypeId == 5 || user.UserTypeId == 10)
                            user.AnotherUserTypeId = 4;
                    }
                    else if (userModel.Another_UserTypeID == 1)
                    {
                        user.UserTypeId = 1;
                    }
                    else if (userModel.Another_UserTypeID == 4)
                    {
                        if (userModel.DeptID == 1 && userModel.LevelID == 1)
                            user.UserTypeId = 9;
                        else
                            user.UserTypeId = 4;
                    }
                    else if (userModel.Another_UserTypeID == 5)
                    {
                        if (userModel.DeptID == 2 && userModel.LevelID == 1)
                             user.UserTypeId = 10;
                        else
                            user.UserTypeId = 5;
                    }

                  
                    string GeoID = string.Empty;
                    if (userModel.GeoIds != null && userModel.GeoIds.Any())
                    {
                        GeoID = string.Join(",", userModel.GeoIds.ToArray());
                        object[] param = new object[] { GeoID, userModel.Id };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_UTS_InsertUserGeo, param);
                    }
                    //GenTeam teams = new GenTeam();
                    //if (userModel.DeptID != 1 && userModel.LevelID != 1)
                    //{
                    //    teams = _context.GenTeams.Where(xz => xz.Id == Convert.ToInt64(userModel.TeamID)).FirstOrDefault();
                    //    if (teams != null)
                    //    {
                    //        user.IsNewUser = teams.IsTeamForNewUser;
                    //    }
                    //    else
                    //    {
                    //        user.IsNewUser = false;
                    //    }
                    //}

                    //if (teams == null || userModel.DeptID == 4)
                    //    user.UserTypeId = 1;
                    //else
                    //{
                    //    if (userModel.DeptID == 1 && userModel.LevelID == 1)
                    //        user.UserTypeId = 9;
                    //    else if (userModel.DeptID == 2 && userModel.LevelID == 1)
                    //        user.UserTypeId = 10;
                    //}

                    oldprioritycount = user.PriorityCount == null ? 0 : user.PriorityCount.Value;
                    remainingprioritycount = user.RemainingPriorityCount == null ? 0 : user.RemainingPriorityCount.Value;
                    newprioritycount = userModel.PriorityCount == null ? 0 : userModel.PriorityCount.Value;
                    //user.IsNewUser = userModel.IsNewUser;

                    if (userModel.fileUpload != null && userModel.fileUpload.Base64ProfilePic != "string")
                    {
                        byte[] img = CommonLogic.UploadImageFromBase64(userModel.fileUpload.Base64ProfilePic);
                        if (img != null && img.Length > 0)
                        {
                            string fileName = string.Format("{0}.{1}", DateTime.Now.ToFileTime(), userModel.fileUpload.Extenstion);
                            var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.UserProfile);
                            string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                            System.IO.File.WriteAllBytes(modifiedFilename, img);
                            var insertedUser = _context.UsrUsers.FirstOrDefault(x => x.Id == userModel.Id);
                            if (insertedUser != null)
                            {
                                insertedUser.ProfilePic = fileName;
                                _context.UsrUsers.Update(insertedUser);
                                _context.SaveChanges();
                            }
                        }
                    }

                    if (userModel.DeptID == 1 && userModel.LevelID == 1)
                    {
                        int Diffcount = newprioritycount - oldprioritycount;
                        int? AssignedPriorityCount = user.PriorityCount - user.RemainingPriorityCount;

                        if (newprioritycount < AssignedPriorityCount)
                        {
                            user.RemainingPriorityCount = user.RemainingPriorityCount;
                        }
                        else if (newprioritycount == AssignedPriorityCount)
                        {
                            user.RemainingPriorityCount = 0;
                        }
                        else
                        {
                            user.RemainingPriorityCount = user.RemainingPriorityCount + Diffcount;
                        }
                        user.PriorityCount = newprioritycount;
                    }
                    else
                    {
                        user.PriorityCount = 0;
                        user.RemainingPriorityCount = 0;
                        userModel.PriorityCount = 0;
                    }
                    user.ModifyByDatetime = DateTime.Now;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();

                    if (oldprioritycount != userModel.PriorityCount)
                    {
                        userprioritycount.UserId = user.Id;
                        userprioritycount.OldPriorityCount = oldprioritycount;
                        userprioritycount.NewPriorityCount = userModel.PriorityCount == null ? 0 : userModel.PriorityCount;
                        userprioritycount.ModifiedById = Convert.ToInt32(LoggedInUserId);
                        userprioritycount.ModifiedByDatetime = System.DateTime.Now;
                        _context.UsrUserPriorityCountLogs.Add(userprioritycount);
                        _context.SaveChanges();

                        if (userModel.DeptID == 1 && userModel.LevelID == 1)
                        {
                            EmailBinder binder = new EmailBinder(_iConfiguration, _context);
                            var UserPriorityList = _iUsers.GetHRDetailsForUserPriorityEmail(user.Id);
                            binder.SendEmailForPriorityCountChange(user.Id, oldprioritycount, newprioritycount, UserPriorityList);
                        }
                    }

                    userHierarchy = _context.UsrUserHierarchies.Where(x => x.UserId == userModel.Id).FirstOrDefault();
                    if (userHierarchy != null)
                    {
                        userHierarchy.ParentId = (userModel.ManagerID == null && userModel.DeptID == 4) ? 0 : (long)userModel.userHierarchyParentID;
                        userHierarchy.ModifiedById = Convert.ToInt32(LoggedInUserId);
                        userHierarchy.ModifiedByDatetime = System.DateTime.Now;
                        _context.Entry(userHierarchy).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        userHierarchy = new UsrUserHierarchy
                        {
                            UserId = userModel.Id,
                            ParentId = (userModel.ManagerID == null && userModel.DeptID == 4) ? 0 : (long)userModel.userHierarchyParentID,
                            CreatedById = Convert.ToInt32(LoggedInUserId),
                            CreatedByDatetime = System.DateTime.Now
                        };
                        _context.UsrUserHierarchies.Add(userHierarchy);
                        _context.SaveChanges();
                    }

                    #region Insert/Update User Role Details
                    object[] Sqlparam = new object[] { userModel.DeptID, userModel.TeamID, userModel.LevelID, userModel.Id };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_UpdateOrInsert_UserRoleDetails, Sqlparam);
                    #endregion

                    #region Manage UserType Based on Team And Level
                    if (userModel.DeptID != 1 || userModel.LevelID != 1)
                    {
                        Sqlparam = new object[] { userModel.Id, userModel.TeamID.Split(',')[0], userModel.LevelID };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_MaintainUserType_TeamAndLevelWise, Sqlparam);
                    }
                    #endregion

                    userRoleDetails = _context.UsrUserRoleDetails.Where(x => x.UserId == userModel.Id).FirstOrDefault();

                    #endregion

                    #region insert User History 

                    object[] historyParam = new object[]
                    {
                            Action_Of_History.Update_User,
                            user.Id,
                            (userModel.detail != null && userModel.detail.userTypeID > 0 && userModel.DeptID != 4) ? userModel.detail.userTypeID : 0,
                            (userModel.detail != null && userModel.detail.userRoleID > 0) ? userModel.detail.userRoleID : 0,
                            (userModel.detail != null && userModel.detail.teamManagerID > 0) ? userModel.detail.teamManagerID : 0,
                            (userModel.detail != null && userModel.detail.reporteeUserID > 0) ?userModel.detail.reporteeUserID : 0,
                            LoggedInUserId
                    };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_User_History_Insert, historyParam);

                    #endregion

                    #region ATS call for User sync
                    if (_iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                    {
                        long loggedinuserid = SessionValues.LoginUserId;
                        GiveUserDetailsToATS giveUserDetailsToATS = await _iUsers.GiveUserDetailsToATS(userModel.EmployeeId);

                        try
                        {
                            string json = JsonConvert.SerializeObject(giveUserDetailsToATS);
                            ATSCall aTSCall = new ATSCall(_iConfiguration, _context);
                            aTSCall.SendUserDetails(json, loggedinuserid, 0);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "User has been Add/Updated" });
                        }
                    }
                    #endregion

                    isSuccess = true;
                }
                else
                {
                    #region Insert user

                    GenTeam teams = new GenTeam();
                    if (userModel.DeptID != 1 && userModel.LevelID != 1)
                    {
                        teams = _context.GenTeams.Where(xz => xz.Id == Convert.ToInt64(userModel.TeamID)).FirstOrDefault();
                    }
                    user.EmployeeId = userModel.EmployeeId;
                    user.FullName = userModel.FullName ?? "";

                    if (!string.IsNullOrEmpty(userModel.FullName))
                    {
                        string[] splitName = userModel.FullName.Split(' ');
                        if (splitName.Length > 0)
                        {
                            user.FirstName = splitName[0];
                            user.LastName = splitName.Length > 1 ? splitName[1] : "";
                        }
                    }

                    user.EmailId = userModel.EmailId ?? "";
                    user.ContactNumber = userModel.ContactNumber ?? "";
                    user.SkypeId = userModel.SkypeId ?? "";
                    user.IsActive = userModel.IsActive;
                    user.ProfilePic = userModel.ProfilePic;
                    user.Designation = userModel.Designation;
                    user.RoleId = userModel.RoleId;
                    user.Description = userModel.Description;
                    user.PriorityCount = userModel.PriorityCount == null ? 0 : userModel.PriorityCount;
                    user.RemainingPriorityCount = userModel.PriorityCount == null ? 0 : userModel.PriorityCount;
                    user.IsNewUser = userModel.IsNewUser;
                    user.IsOdr = userModel.IsOdr;
                    user.GeoId = (userModel.GeoIds != null && userModel.GeoIds.Any()) ? userModel.GeoIds[0] : 0;
                    user.Username = user.EmployeeId;
                    user.Password = "uplers@123";
                    user.DeptId = userModel.DeptID;
                    user.TeamId = userModel.DeptID == 4 ? "0" : userModel.TeamID;
                    user.LevelId = userModel.DeptID == 4 ? 0 : userModel.LevelID;
                    user.CreatedbyDatetime = DateTime.Now;
                    if (userModel.Another_UserTypeID == 6)
                    {
                        if (user.UserTypeId == 4 || user.UserTypeId == 9)
                            user.AnotherUserTypeId = 5;
                        else if (user.UserTypeId == 5 || user.UserTypeId == 10)
                            user.AnotherUserTypeId = 4;
                    }
                    else if (userModel.Another_UserTypeID == 1)
                    {
                        user.UserTypeId = 1;
                    }
                    else if (userModel.Another_UserTypeID == 4)
                    {
                        if (userModel.DeptID == 1 && userModel.LevelID == 1)
                            user.UserTypeId = 9;
                        else
                            user.UserTypeId = 4;
                    }
                    else if (userModel.Another_UserTypeID == 5)
                    {
                        if (userModel.DeptID == 2 && userModel.LevelID == 1)
                            user.UserTypeId = 10;
                        else
                            user.UserTypeId = 5;
                    }


                    //if (teams == null || userModel.DeptID == 4)
                    //    user.UserTypeId = 1;
                    //else
                    //{
                    //    if (userModel.DeptID == 1 && userModel.LevelID == 1)
                    //        user.UserTypeId = 9;
                    //    else if (userModel.DeptID == 2 && userModel.LevelID == 1)
                    //        user.UserTypeId = 10;
                    //}

                    _context.UsrUsers.Add(user);
                    _context.SaveChanges();

                    string GeoID = string.Empty;
                    if (userModel.GeoIds != null && userModel.GeoIds.Any())
                    {
                        GeoID = string.Join(",", userModel.GeoIds.ToArray());
                        object[] param = new object[] { GeoID, user.Id };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_UTS_InsertUserGeo, param);
                    }

                    long InsertedUserId = user.Id;

                    if (InsertedUserId > 0)
                    {
                        #region Insert/Update User Role Details
                        object[] Sqlparam = new object[] { userModel.DeptID, userModel.TeamID, userModel.LevelID, InsertedUserId };
                        _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_UpdateOrInsert_UserRoleDetails, Sqlparam);
                        #endregion

                        #region Manage UserType Based on Team And Level
                        if (userModel.DeptID != 1 || userModel.LevelID != 1)
                        {
                            Sqlparam = new object[] { InsertedUserId, userModel.TeamID.Split(',')[0], userModel.LevelID };
                            _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_MaintainUserType_TeamAndLevelWise, Sqlparam);
                        }
                        #endregion
                    }

                    userModel.Id = user.Id;
                    if (InsertedUserId > 0)
                    {
                        if (userModel.userHierarchyParentID > 0)
                        {
                            userHierarchy.UserId = InsertedUserId;
                            userHierarchy.ParentId = userModel.DeptID == 4 ? 0 : (long)userModel.userHierarchyParentID;
                            userHierarchy.CreatedById = Convert.ToInt32(LoggedInUserId);
                            userHierarchy.CreatedByDatetime = System.DateTime.Now;

                            _context.UsrUserHierarchies.Add(userHierarchy);
                            _context.SaveChanges();
                        }
                        else
                        {
                            userHierarchy.UserId = InsertedUserId;
                            userHierarchy.ParentId = 0;
                            userHierarchy.CreatedById = Convert.ToInt32(LoggedInUserId);
                            userHierarchy.CreatedByDatetime = DateTime.Now;
                            _context.UsrUserHierarchies.Add(userHierarchy);
                            _context.SaveChanges();
                        }
                    }

                    if (userModel.fileUpload != null && userModel.fileUpload.Base64ProfilePic != "string")
                    {
                        byte[] img = CommonLogic.UploadImageFromBase64(userModel.fileUpload.Base64ProfilePic);
                        if (img != null && img.Length > 0)
                        {
                            string fileName = string.Format("{0}.{1}", DateTime.Now.ToFileTime(), userModel.fileUpload.Extenstion);
                            var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.UserProfile);
                            string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                            System.IO.File.WriteAllBytes(modifiedFilename, img);
                            _context.Entry(user).Reload();
                            var insertedUser = _context.UsrUsers.FirstOrDefault(x => x.Id == InsertedUserId);
                            if (insertedUser != null)
                            {
                                insertedUser.ProfilePic = fileName;
                                _context.UsrUsers.Update(insertedUser);
                                _context.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region insert User History 

                    object[] historyParam = new object[]
                    {
                            Action_Of_History.Create_User,
                            user.Id,
                            (userModel.detail != null && userModel.detail.userTypeID > 0 && userModel.DeptID != 4) ? userModel.detail.userTypeID : 0,
                            (userModel.detail != null && userModel.detail.userRoleID > 0) ? userModel.detail.userRoleID : 0,
                            (userModel.detail != null && userModel.detail.teamManagerID > 0) ? userModel.detail.teamManagerID : 0,
                            (userModel.detail != null && userModel.detail.reporteeUserID > 0) ?userModel.detail.reporteeUserID : 0,
                            LoggedInUserId
                    };
                    _universalProcRunner.Manipulation(Constants.ProcConstant.sproc_User_History_Insert, historyParam);

                    #endregion

                    isSuccess = true;

                    #region ATS call for User sync
                    if (_iConfiguration["HRDataSendSwitchForPHP"].ToLower() != "local")
                    {
                        long loggedinuserid = SessionValues.LoginUserId;
                        GiveUserDetailsToATS giveUserDetailsToATS = await _iUsers.GiveUserDetailsToATS(userModel.EmployeeId);

                        try
                        {
                            string json = JsonConvert.SerializeObject(giveUserDetailsToATS);
                            ATSCall aTSCall = new ATSCall(_iConfiguration, _context);
                            aTSCall.SendUserDetails(json, loggedinuserid, 0);
                        }
                        catch (Exception)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "User has been Add/Updated" });
                        }
                    }
                    #endregion
                }

                if (isSuccess)
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "User has been Add/Updated" });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Invalid Data" });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ATSUSerDetailSync")]
        public async Task<ObjectResult> testATSUSerDetailSync(string EmployeeID)
        {
            try
            {
                GiveUserDetailsToATS giveUserDetailsToATS = await _iUsers.GiveUserDetailsToATS(EmployeeID);

                return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "User details", Details = giveUserDetailsToATS });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpGet("RequiredListForNewUser")]
        public ResponseObject GetDropDowList(int type)
        {
            if (type > 0)
            {
                switch (type)
                {
                    case (int)ManagerType.PracticeHead:
                        return new ResponseObject() { statusCode = (short)ResponseCode.Success, Message = "Roles are fetched", Details = _iUsers.GetListOfRequiredNewUser(type) };
                        break;
                    case (int)ManagerType.Sales:
                        return new ResponseObject() { statusCode = (short)ResponseCode.Success, Message = "Sales Manager are fetched", Details = _iUsers.GetListOfRequiredNewUser(type) };
                        break;
                    case (int)ManagerType.TalentOps:
                        return new ResponseObject() { statusCode = (short)ResponseCode.Success, Message = "Ops team Manager are fetched", Details = _iUsers.GetListOfRequiredNewUser(type) };
                        break;
                    default:
                        return new ResponseObject() { statusCode = (short)ResponseCode.Failed, Message = "Type is not valid" };
                        break;
                }
            }
            else
                return new ResponseObject() { statusCode = (short)ResponseCode.Failed, Message = "Type is not valid" };
        }

        [Authorize]
        [HttpGet("GetUserType")]
        public ResponseObject GetUserType()
        {
            var typeList = _iUsers.GetUserTypes();
            if (typeList.Count() > 0)
                return new ResponseObject() { statusCode = (short)ResponseCode.Success, Message = "User type fetched", Details = typeList };
            else
                return new ResponseObject() { statusCode = (short)ResponseCode.Failed, Message = "User types empty" };
        }

        /// <summary>
        /// Gets the user role list.
        /// </summary>
        /// <param name="userRoleViewModel">The user role view model.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Roles/List")]
        public ObjectResult GetUserRoleList([FromBody] UserRoleViewModel userRoleViewModel)
        {
            try
            {
                #region PreValidation

                if (userRoleViewModel == null || (userRoleViewModel.pagenumber == 0 || userRoleViewModel.totalrecord == 0))
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide pagenumber and totalrecord" });

                #endregion

                string Sortdatafield = "CreatedbyDatetime";
                string Sortorder = "desc";

                object[] param = new object[]
                {
                    userRoleViewModel.pagenumber, userRoleViewModel.totalrecord, Sortdatafield, Sortorder,0,
                    userRoleViewModel.FilterFieldsUserRole.EmployeeID,
                    userRoleViewModel.FilterFieldsUserRole.FullName, userRoleViewModel.FilterFieldsUserRole.UserRole
                };

                string paramasString = CommonLogic.ConvertToParamString(param);
                List<sproc_Inc_GetUsers_RoleDetails_Result> userroleListData = _iUsers.GetUserRoleList(paramasString);

                if (userroleListData.Any())
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = CustomRendering.ListingResponses(userroleListData, userroleListData[0].TotalRecords, userRoleViewModel.totalrecord, userRoleViewModel.pagenumber) });
                else
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "No UserRole Available" });

            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost]
        [Route("UTSFeedBack")]
        public async Task<ObjectResult> UTSFeedBack([FromBody] UserFeedBack userFeedBack)
        {
            try
            {

                //_context.Database.CloseConnection();

                long LoggedInUserId = SessionValues.LoginUserId;

                string fileName = "";
                if (userFeedBack.fileUpload != null)
                {
                    byte[] img = CommonLogic.UploadImageFromBase64(userFeedBack.fileUpload.Base64ProfilePic);
                    if (img != null && img.Length > 0)
                    {
                        Guid obj = Guid.NewGuid();
                        fileName = string.Format("{0}.{1}", obj, userFeedBack.fileUpload.Extenstion);
                        var pathToSave = CommonLogic.GetFileUploadFolderFor(Constants.MediaConstant.UTSFeedback);
                        string modifiedFilename = string.Format("{0}/{1}", pathToSave, fileName);
                        System.IO.File.WriteAllBytes(modifiedFilename, img);


                        var Companylogopathsave = _iConfiguration["UTSFeedback"].ToString();
                        string companylogomodifiedFilename = string.Format("{0}/{1}", Companylogopathsave, fileName);
                        System.IO.File.WriteAllBytes(companylogomodifiedFilename, img);

                    }
                }

                object[] param = new object[]
            {
                        userFeedBack.Feedback,
                          fileName,
                           userFeedBack.RatingStar,
                          LoggedInUserId

            };

                string paramString = CommonLogic.ConvertToParamString(param);
                Sproc_InsertTicket_Result result = _iUsers.SaveUTSFeedBack(paramString);
                if (result.ID > 0)
                {
                    var userDetails = _context.UsrUsers.Where(x => x.Id == LoggedInUserId).FirstOrDefault();
                    var uri = _iConfiguration["chatgoogleapis"].ToString();
                    var varProjectURL_API = _iConfiguration["ProjectURL_API"].ToString();

                    StringBuilder sb = new();
                    sb.Append("New UTS feedback recived,\\n");
                    sb.Append("*User Name:* " + userDetails.FullName.ToString() + "\\n");
                    sb.Append("*Email:* " + userDetails.EmailId.ToString() + "\\n");
                    sb.Append("*Rating:* " + userFeedBack.RatingStar + "\\n");
                    sb.Append("*Message:* " + userFeedBack.Feedback + "\\n");
                    sb.Append("*Page Url:* " + userFeedBack.PageUrl + "\\n");
                    sb.Append("*Browser:* " + userFeedBack.Browser + "\\n");
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        sb.Append("*Screenshot:* " + varProjectURL_API + "Media/UTSFeedback/" + fileName);
                    }

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                    if (webRequest != null)
                    {
                        webRequest.Method = "POST";
                        webRequest.Timeout = 500000;
                        webRequest.ContentType = "application/json";
                        webRequest.Credentials = CredentialCache.DefaultCredentials;

                        using (var requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                        {


                            requestWriter.Write("{text:\"" + sb.ToString() + "\"}");
                            requestWriter.Flush();
                            requestWriter.Close();
                        }
                    }

                    HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                    Stream resStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(resStream);
                    string ResponseJson = reader.ReadToEnd();
                    if ((int)response.StatusCode == 200)
                    {

                    }




                    //EmailBinder emailBinder = new EmailBinder(_iConfiguration, _context);

                    //emailBinder.SetPasswordSendEmail(genContact, loggedInUserDetail.FullName, loggedInUserDetail.EmailId, loggedInUserDetail.Designation);

                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "feedback submitted successfully", Details = result });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status500InternalServerError, Message = "Registration failed" });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        [Authorize]
        [HttpGet]
        [Route("CheckvalidationMessageForHeadPriorityCount")]
        public async Task<ObjectResult> CheckvalidationMessageForHeadPriorityCount(long userId, int? newPriorityCount)
        {
            try
            {
                if (userId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide user id" });
                }
                if (newPriorityCount == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseObject() { statusCode = StatusCodes.Status400BadRequest, Message = "Please provide new priority count" });
                }
                if (userId > 0)
                {
                    object[] objects = new object[] { userId, newPriorityCount };
                    string paramtersstring = CommonLogic.ConvertToParamString(objects);
                    CheckValidationMessage validationMessage = _commonInterface.Users.CheckvalidationMessageForHeadPriorityCount(paramtersstring);
                    return StatusCode(StatusCodes.Status200OK, new ResponseObject() { statusCode = StatusCodes.Status200OK, Message = "Success", Details = validationMessage.Message });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseObject() { statusCode = StatusCodes.Status404NotFound, Message = "no records found" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        */

        #endregion
    }
}