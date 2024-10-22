using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentNotificationHistoryRepository : GenericRepository<GenTalentNotificationHistory>, IGenTalentNotificationHistoryRepository
{
public GenTalentNotificationHistoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
