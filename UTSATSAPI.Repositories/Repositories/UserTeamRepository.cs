using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class UserTeamRepository : IUserTeam
    {
        #region Variables
        private IUnitOfWork _unitOfWork;
        private TalentConnectAdminDBContext _db;
        #endregion

        #region Constructor
        public UserTeamRepository(IUnitOfWork unitOfWork, TalentConnectAdminDBContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        #endregion
        #region Public Methods
        public async Task<GenTeam> GetGenTeamsByTeam(string team)
        {
            return await _unitOfWork.genTeams.GetSingleByCondition(x => x.Team == team);
        }
        public async Task<GenTeam> GetGenTeamsById(long Id)
        {
            return await _unitOfWork.genTeams.GetSingleByCondition(x => x.Id == Id);
        }
        public async Task<List<Sproc_GetTeams_Result>> GetGenTeamsList(string paramasString)
        {
            return await _db.Set<Sproc_GetTeams_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_GetTeams, paramasString)).ToListAsync();
        }
        public async Task<IEnumerable<UsrUserRole>> GetUsrUserRoleList()
        {
            return await _unitOfWork.usrUserRoles.GetAllByCondition(x => x.RoleLevelId == 1);
        }
        public async Task<IEnumerable<UsrUser>> GetUsrUserList()
        {
            return await _unitOfWork.usrUsers.GetAllByCondition(x => x.DeptId == 1 && x.LevelId == 1);
        }
        public async Task<IEnumerable<PrgDepartment>> GetPrgDepartmentList()
        {
            return await _unitOfWork.prgDepartments.GetAllByCondition(x => x.IsActive == true && x.Id != 4);
        }
        #endregion

    }
}
