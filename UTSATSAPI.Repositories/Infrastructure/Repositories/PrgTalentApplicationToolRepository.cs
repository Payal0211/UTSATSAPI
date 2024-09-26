using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentApplicationToolRepository : GenericRepository<PrgTalentApplicationTool>, IPrgTalentApplicationToolRepository
{
public PrgTalentApplicationToolRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
