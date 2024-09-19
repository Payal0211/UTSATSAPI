using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgExtractedSkillsforJdparsingRepository : GenericRepository<PrgExtractedSkillsforJdparsing>, IPrgExtractedSkillsforJdparsingRepository
{
public PrgExtractedSkillsforJdparsingRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
