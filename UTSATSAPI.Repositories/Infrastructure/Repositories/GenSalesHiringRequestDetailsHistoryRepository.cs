using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHiringRequestDetailsHistoryRepository : GenericRepository<GenSalesHiringRequestDetailsHistory>, IGenSalesHiringRequestDetailsHistoryRepository
{
public GenSalesHiringRequestDetailsHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
