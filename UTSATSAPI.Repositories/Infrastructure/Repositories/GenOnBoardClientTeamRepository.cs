using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnBoardClientTeamRepository : GenericRepository<GenOnBoardClientTeam>, IGenOnBoardClientTeamRepository
{
public GenOnBoardClientTeamRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
