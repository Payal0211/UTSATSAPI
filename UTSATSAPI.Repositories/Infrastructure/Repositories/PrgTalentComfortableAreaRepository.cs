using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentComfortableAreaRepository : GenericRepository<PrgTalentComfortableArea>, IPrgTalentComfortableAreaRepository
{
public PrgTalentComfortableAreaRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
