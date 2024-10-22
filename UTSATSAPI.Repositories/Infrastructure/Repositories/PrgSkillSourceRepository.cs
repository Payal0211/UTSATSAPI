using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSkillSourceRepository : GenericRepository<PrgSkillSource>, IPrgSkillSourceRepository
{
public PrgSkillSourceRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
