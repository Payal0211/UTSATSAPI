using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncUserTargetsHistoryRepository : GenericRepository<GenIncUserTargetsHistory>, IGenIncUserTargetsHistoryRepository
{
public GenIncUserTargetsHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
