using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCompanyTargetGroupRepository : GenericRepository<PrgCompanyTargetGroup>, IPrgCompanyTargetGroupRepository
{
public PrgCompanyTargetGroupRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
