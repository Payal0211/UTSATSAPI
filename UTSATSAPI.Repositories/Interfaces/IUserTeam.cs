using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IUserTeam
    {
        Task<GenTeam> GetGenTeamsByTeam(string team);
        Task<GenTeam> GetGenTeamsById(long Id);
        Task<List<Sproc_GetTeams_Result>> GetGenTeamsList(string paramasString);

        Task<IEnumerable<UsrUserRole>> GetUsrUserRoleList();
        Task<IEnumerable<UsrUser>> GetUsrUserList();
        Task<IEnumerable<PrgDepartment>> GetPrgDepartmentList();

    }
}
