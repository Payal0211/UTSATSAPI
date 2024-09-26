using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentComfortableWorkingRepository : GenericRepository<PrgTalentComfortableWorking>, IPrgTalentComfortableWorkingRepository
{
public PrgTalentComfortableWorkingRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
