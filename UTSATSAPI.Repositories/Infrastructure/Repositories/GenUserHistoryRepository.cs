using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenUserHistoryRepository : GenericRepository<GenUserHistory>, IGenUserHistoryRepository
{
public GenUserHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
