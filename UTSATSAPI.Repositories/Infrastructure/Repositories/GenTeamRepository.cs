using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTeamRepository : GenericRepository<GenTeam>, IGenTeamRepository
{
public GenTeamRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
