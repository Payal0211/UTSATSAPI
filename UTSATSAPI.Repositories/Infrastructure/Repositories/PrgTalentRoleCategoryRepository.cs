using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentRoleCategoryRepository : GenericRepository<PrgTalentRoleCategory>, IPrgTalentRoleCategoryRepository
{
public PrgTalentRoleCategoryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
