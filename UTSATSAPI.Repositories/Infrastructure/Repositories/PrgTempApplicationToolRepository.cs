using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempApplicationToolRepository : GenericRepository<PrgTempApplicationTool>, IPrgTempApplicationToolRepository
{
public PrgTempApplicationToolRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
