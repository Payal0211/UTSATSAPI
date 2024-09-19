using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncTargetAcheivedDetailRepository : GenericRepository<GenIncTargetAcheivedDetail>, IGenIncTargetAcheivedDetailRepository
{
public GenIncTargetAcheivedDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
