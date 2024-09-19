using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSkillsRoleRepository : GenericRepository<PrgSkillsRole>, IPrgSkillsRoleRepository
{
public PrgSkillsRoleRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
