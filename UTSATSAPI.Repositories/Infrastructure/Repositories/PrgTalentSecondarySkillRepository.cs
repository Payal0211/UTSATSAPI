using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
    public class PrgTalentSecondarySkillRepository : GenericRepository<PrgTalentSecondarySkill>, IPrgTalentSecondarySkillRepository
    {
        public PrgTalentSecondarySkillRepository(UTSATSAPIDBConnection dbContext) : base(dbContext) { }
    }
}
