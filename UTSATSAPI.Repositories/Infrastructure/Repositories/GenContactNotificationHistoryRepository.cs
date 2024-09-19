using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactNotificationHistoryRepository : GenericRepository<GenContactNotificationHistory>, IGenContactNotificationHistoryRepository
{
public GenContactNotificationHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
