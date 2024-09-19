using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentRoleRepository : GenericRepository<PrgTalentRole>, IPrgTalentRoleRepository
{
public PrgTalentRoleRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
