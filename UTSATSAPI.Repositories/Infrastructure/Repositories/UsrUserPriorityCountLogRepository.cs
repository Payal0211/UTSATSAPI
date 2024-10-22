using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserPriorityCountLogRepository : GenericRepository<UsrUserPriorityCountLog>, IUsrUserPriorityCountLogRepository
{
public UsrUserPriorityCountLogRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
