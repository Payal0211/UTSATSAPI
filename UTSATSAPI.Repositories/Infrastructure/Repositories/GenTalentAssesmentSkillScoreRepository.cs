using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentAssesmentSkillScoreRepository : GenericRepository<GenTalentAssesmentSkillScore>, IGenTalentAssesmentSkillScoreRepository
{
public GenTalentAssesmentSkillScoreRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
