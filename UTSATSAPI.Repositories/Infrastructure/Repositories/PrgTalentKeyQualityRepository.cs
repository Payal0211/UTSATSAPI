using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentKeyQualityRepository : GenericRepository<PrgTalentKeyQuality>, IPrgTalentKeyQualityRepository
{
public PrgTalentKeyQualityRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
