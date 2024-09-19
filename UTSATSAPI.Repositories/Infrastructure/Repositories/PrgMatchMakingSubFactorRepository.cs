using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgMatchMakingSubFactorRepository : GenericRepository<PrgMatchMakingSubFactor>, IPrgMatchMakingSubFactorRepository
{
public PrgMatchMakingSubFactorRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
