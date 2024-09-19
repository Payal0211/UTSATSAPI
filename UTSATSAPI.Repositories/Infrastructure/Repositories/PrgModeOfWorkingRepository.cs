using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgModeOfWorkingRepository : GenericRepository<PrgModeOfWorking>, IPrgModeOfWorkingRepository
{
public PrgModeOfWorkingRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
