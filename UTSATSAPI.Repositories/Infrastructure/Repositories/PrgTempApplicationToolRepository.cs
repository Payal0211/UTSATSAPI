using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempApplicationToolRepository : GenericRepository<PrgTempApplicationTool>, IPrgTempApplicationToolRepository
{
public PrgTempApplicationToolRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
