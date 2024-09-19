using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentPreferableWorkingRepository : GenericRepository<PrgTalentPreferableWorking>, IPrgTalentPreferableWorkingRepository
{
public PrgTalentPreferableWorkingRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
