using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgMatchMakingFactorRepository : GenericRepository<PrgMatchMakingFactor>, IPrgMatchMakingFactorRepository
{
public PrgMatchMakingFactorRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
