using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenClientAmAssignmentHistoryRepository : GenericRepository<GenClientAmAssignmentHistory>, IGenClientAmAssignmentHistoryRepository
{
public GenClientAmAssignmentHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
