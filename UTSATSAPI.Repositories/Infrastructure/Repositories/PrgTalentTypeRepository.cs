using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentTypeRepository : GenericRepository<PrgTalentType>, IPrgTalentTypeRepository
{
public PrgTalentTypeRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
