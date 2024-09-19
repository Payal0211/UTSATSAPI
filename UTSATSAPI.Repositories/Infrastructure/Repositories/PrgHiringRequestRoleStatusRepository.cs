using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgHiringRequestRoleStatusRepository : GenericRepository<PrgHiringRequestRoleStatus>, IPrgHiringRequestRoleStatusRepository
{
public PrgHiringRequestRoleStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
