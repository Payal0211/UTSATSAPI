using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgClientEngagementModelRepository : GenericRepository<PrgClientEngagementModel>, IPrgClientEngagementModelRepository
{
public PrgClientEngagementModelRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
