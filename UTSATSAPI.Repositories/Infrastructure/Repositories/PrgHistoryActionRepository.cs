using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgHistoryActionRepository : GenericRepository<PrgHistoryAction>, IPrgHistoryActionRepository
{
public PrgHistoryActionRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
