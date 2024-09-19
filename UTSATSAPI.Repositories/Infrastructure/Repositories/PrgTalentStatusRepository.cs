using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentStatusRepository : GenericRepository<PrgTalentStatus>, IPrgTalentStatusRepository
{
public PrgTalentStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
