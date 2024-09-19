using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSkillRepository : GenericRepository<PrgSkill>, IPrgSkillRepository
{
public PrgSkillRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
