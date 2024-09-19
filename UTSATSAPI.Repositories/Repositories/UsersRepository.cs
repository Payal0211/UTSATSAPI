using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class UsersRepository : IUsers
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors
        public UsersRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public Methods

        #region User list

        public List<sproc_UTS_GetUsers_Result> GetUsers(string strParams)
        {
            return db.Set<sproc_UTS_GetUsers_Result>().FromSqlRaw(string.Format("{0} {1}",
                          Constants.ProcConstant.sproc_UTS_GetUsers, strParams)).ToList();
        }

        #endregion

        #region User Operations

        public IEnumerable<SelectListItem> GetListOfRequiredNewUser(int type)
        {
            try
            {
                if (type == (int)Helpers.Enum.ManagerType.PracticeHead)
                {
                    return db.PrgTalentRoles.ToList().Where(x => x.IsActive == true).OrderBy(x => x.TalentRole).Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.TalentRole
                    });
                }
                else
                {
                    return db.UsrUsers.Where(x => x.UserTypeId == type).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SelectListItem> GetUserTypes()
        {
            try
            {
                return db.UsrUserTypes.Where(x => x.IsDisplay == true).ToList().Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.UserType
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<sproc_Inc_GetUsers_RoleDetails_Result> GetUserRoleList(string paramasString)
        {
            return db.Set<sproc_Inc_GetUsers_RoleDetails_Result>().FromSqlRaw(string.Format("{0} {1}",
                          Constants.ProcConstant.sproc_UTS_Inc_GetUsers_RoleDetails, paramasString)).ToList();
        }

        public List<sproc_UTS_GetBDR_Marketingusers> sproc_UTS_GetBDR_Marketingusers(string paramasString)
        {
            return db.Set<sproc_UTS_GetBDR_Marketingusers>().FromSqlRaw(string.Format("{0} {1}",
                          Constants.ProcConstant.sproc_UTS_GetBDR_Marketingusers, paramasString)).ToList();
        }

        public SPROC_UTS_UserGeoValues_Result SPROC_UTS_UserGeoValues(long userId)
        {
            return db.Set<SPROC_UTS_UserGeoValues_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.SPROC_UTS_UserGeoValues, userId)).AsEnumerable().FirstOrDefault();
        }

        public List<Sproc_Get_HRDetails_For_UserPriority_Email_Result> GetHRDetailsForUserPriorityEmail(long UserId)
        {
            return db.Set<Sproc_Get_HRDetails_For_UserPriority_Email_Result>().FromSqlRaw(string.Format("{0} {1}",
                         Constants.ProcConstant.sproc_Get_HRDetails_For_UserPriority_Email, UserId)).ToList();
        }

        public List<sproc_GetUserHierarchy_Result> sproc_GetUserHierarchy_Results(long userID, string employeeID)
        {
            return db.Set<sproc_GetUserHierarchy_Result>().FromSqlRaw(string.Format("{0} '{1}'",
                             Constants.ProcConstant.sproc_GetUserHierarchy, userID, employeeID)).ToList();
        }

        public CheckValidationMessage CheckvalidationMessageForHeadPriorityCount(string paramasString)
        {

            return db.Set<CheckValidationMessage>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_Check_Validation_Message_For_Updating_Priority_Counts, paramasString)).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Fetch user by lead types. +  sales user 
        /// </summary>
        /// <param name="leadType"></param>
        /// <returns></returns>
        public List<Sproc_GetUserBy_LeadType_Result> Sproc_GetUserBy_LeadType(string param)
        {
            return db.Set<Sproc_GetUserBy_LeadType_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_GetUserBy_LeadType, param)).AsEnumerable().ToList();
        }

        public async Task<List<MastersResponseModel>> Get_UserRoleList(int userType)
        {
            try
            {
                var usrUser = (from utrd in await _unitOfWork.usrUserTypeAndRoleDetails.GetAll()
                               join ur in await _unitOfWork.usrUserRoles.GetAll() on utrd.UserRoleId equals ur.Id
                               where utrd.UserTypeId == userType
                               select new MastersResponseModel()
                               {
                                   Value = ur.UserRole,
                                   Id = ur.Id
                               }).ToList();
                return usrUser;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GiveUserDetailsToATS> GiveUserDetailsToATS(string EmployeeID)
        {
            try
            {
                GiveUserDetailsToATS UserDetails = null;

                var user = await _unitOfWork.usrUsers.GetSingleByCondition(x => x.EmployeeId == EmployeeID);
                if (user != null)
                {
                    UserDetails = new();
                    UserDetails.name = user.FullName ?? string.Empty;
                    UserDetails.email = user.EmailId ?? string.Empty;
                    UserDetails.contact_number = user.ContactNumber ?? string.Empty;
                    UserDetails.skype = user.SkypeId ?? string.Empty;
                    UserDetails.designation = user.Designation ?? string.Empty;
                    UserDetails.department = string.Empty;
                    UserDetails.employee_id = user.EmployeeId;
                    UserDetails.IsActive = user.IsActive;

                    if (user.DeptId > 0)
                    {
                        var Department = await _unitOfWork.prgDepartments.GetSingleByCondition(x => x.Id == user.DeptId);
                        if(Department != null)
                        {
                            UserDetails.department = Department.DeptName ?? string.Empty;
                        }
                    }
                    
                    UserDetails.reporting_to = null;

                    var Lead_UserMapping = await _unitOfWork.usrUserHierarchys.GetSingleByCondition(x => x.UserId == user.Id);

                    if (Lead_UserMapping != null)
                    {
                        var ReportingUser = await _unitOfWork.usrUsers.GetSingleByCondition(x => x.Id == Lead_UserMapping.ParentId);
                        if (ReportingUser != null)
                        {
                            UserDetails.reporting_to = new();
                            UserDetails.reporting_to.name = ReportingUser.FullName ?? string.Empty;
                            UserDetails.reporting_to.email = ReportingUser.EmailId ?? string.Empty;
                            UserDetails.reporting_to.contact_number = ReportingUser.ContactNumber ?? string.Empty;
                            UserDetails.reporting_to.skype = ReportingUser.SkypeId ?? string.Empty;
                            UserDetails.reporting_to.designation = ReportingUser.Designation ?? string.Empty;
                            UserDetails.reporting_to.department = string.Empty;
                            UserDetails.reporting_to.employee_id = ReportingUser.EmployeeId;

                            if (user.DeptId > 0)
                            {
                                var Department = await _unitOfWork.prgDepartments.GetSingleByCondition(x => x.Id == ReportingUser.DeptId);
                                if (Department != null)
                                {
                                    UserDetails.reporting_to.department = Department.DeptName ?? string.Empty;
                                }
                            }
                        }
                    }
                }

                return UserDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Sproc_InsertTicket_Result SaveUTSFeedBack(string paramString)
        {
            return db.Set<Sproc_InsertTicket_Result>().FromSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.Sproc_InsertTicket, paramString)).AsEnumerable().FirstOrDefault();
        }

        #endregion

        #endregion
    }
}
