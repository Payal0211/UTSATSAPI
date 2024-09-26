using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHiringRequestPriorityHistoryRepository : GenericRepository<GenSalesHiringRequestPriorityHistory>, IGenSalesHiringRequestPriorityHistoryRepository
{
public GenSalesHiringRequestPriorityHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
