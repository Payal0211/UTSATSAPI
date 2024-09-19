using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempSkillRepository : GenericRepository<PrgTempSkill>, IPrgTempSkillRepository
{
public PrgTempSkillRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
