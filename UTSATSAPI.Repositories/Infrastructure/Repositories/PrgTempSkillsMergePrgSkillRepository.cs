using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempSkillsMergePrgSkillRepository : GenericRepository<PrgTempSkillsMergePrgSkill>, IPrgTempSkillsMergePrgSkillRepository
{
public PrgTempSkillsMergePrgSkillRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
