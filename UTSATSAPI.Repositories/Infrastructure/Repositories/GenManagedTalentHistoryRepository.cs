using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenManagedTalentHistoryRepository : GenericRepository<GenManagedTalentHistory>, IGenManagedTalentHistoryRepository
{
public GenManagedTalentHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
