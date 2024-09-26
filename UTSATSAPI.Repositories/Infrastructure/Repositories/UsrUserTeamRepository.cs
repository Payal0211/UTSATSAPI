using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserTeamRepository : GenericRepository<UsrUserTeam>, IUsrUserTeamRepository
{
public UsrUserTeamRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
