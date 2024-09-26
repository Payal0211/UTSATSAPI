using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentRoleTemplateRepository : GenericRepository<PrgTalentRoleTemplate>, IPrgTalentRoleTemplateRepository
{
public PrgTalentRoleTemplateRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
