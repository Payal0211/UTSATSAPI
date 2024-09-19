using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentPendingActionRepository : GenericRepository<GenTalentPendingAction>, IGenTalentPendingActionRepository
{
public GenTalentPendingActionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
