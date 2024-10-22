using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSkillCategoryRepository : GenericRepository<PrgSkillCategory>, IPrgSkillCategoryRepository
{
public PrgSkillCategoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
