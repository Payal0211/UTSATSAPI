using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgBenchMarkRepository : GenericRepository<PrgBenchMark>, IPrgBenchMarkRepository
{
public PrgBenchMarkRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
