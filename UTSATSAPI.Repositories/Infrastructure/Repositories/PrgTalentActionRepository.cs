using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentActionRepository : GenericRepository<PrgTalentAction>, IPrgTalentActionRepository
{
public PrgTalentActionRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
