using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenHistoryRepository : GenericRepository<GenHistory>, IGenHistoryRepository
{
public GenHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
