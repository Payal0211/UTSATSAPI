using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentHistoryRepository : GenericRepository<GenTalentHistory>, IGenTalentHistoryRepository
{
public GenTalentHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
