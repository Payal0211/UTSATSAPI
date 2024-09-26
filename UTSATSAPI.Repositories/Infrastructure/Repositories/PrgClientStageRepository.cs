using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgClientStageRepository : GenericRepository<PrgClientStage>, IPrgClientStageRepository
{
public PrgClientStageRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
