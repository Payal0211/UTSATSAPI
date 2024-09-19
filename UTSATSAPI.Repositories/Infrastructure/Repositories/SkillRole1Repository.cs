using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class SkillRole1Repository : GenericRepository<SkillRole1>, ISkillRole1Repository
{
public SkillRole1Repository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
