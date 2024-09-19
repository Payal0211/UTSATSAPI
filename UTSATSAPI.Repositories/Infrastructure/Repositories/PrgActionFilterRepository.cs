using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgActionFilterRepository : GenericRepository<PrgActionFilter>, IPrgActionFilterRepository
{
public PrgActionFilterRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
