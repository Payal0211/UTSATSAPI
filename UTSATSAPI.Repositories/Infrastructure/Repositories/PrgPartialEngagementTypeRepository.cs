using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgPartialEngagementTypeRepository : GenericRepository<PrgPartialEngagementType>, IPrgPartialEngagementTypeRepository
{
public PrgPartialEngagementTypeRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
