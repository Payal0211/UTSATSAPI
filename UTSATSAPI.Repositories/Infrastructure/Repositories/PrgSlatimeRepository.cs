using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSlatimeRepository : GenericRepository<PrgSlatime>, IPrgSlatimeRepository
{
public PrgSlatimeRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
