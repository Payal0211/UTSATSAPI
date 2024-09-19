using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentPrimarySkillRepository : GenericRepository<PrgTalentPrimarySkill>, IPrgTalentPrimarySkillRepository
{
public PrgTalentPrimarySkillRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
