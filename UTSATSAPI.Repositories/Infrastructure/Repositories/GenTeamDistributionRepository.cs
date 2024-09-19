using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTeamDistributionRepository : GenericRepository<GenTeamDistribution>, IGenTeamDistributionRepository
{
public GenTeamDistributionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
