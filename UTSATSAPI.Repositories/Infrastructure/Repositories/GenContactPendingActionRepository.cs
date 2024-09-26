using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactPendingActionRepository : GenericRepository<GenContactPendingAction>, IGenContactPendingActionRepository
{
public GenContactPendingActionRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
