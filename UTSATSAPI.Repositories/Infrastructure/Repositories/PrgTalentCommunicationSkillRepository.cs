using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentCommunicationSkillRepository : GenericRepository<PrgTalentCommunicationSkill>, IPrgTalentCommunicationSkillRepository
{
public PrgTalentCommunicationSkillRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
