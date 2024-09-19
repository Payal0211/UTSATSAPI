using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserTeamRepository : GenericRepository<UsrUserTeam>, IUsrUserTeamRepository
{
public UsrUserTeamRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
