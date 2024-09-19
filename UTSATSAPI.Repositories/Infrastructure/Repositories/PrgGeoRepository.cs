using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgGeoRepository : GenericRepository<PrgGeo>, IPrgGeoRepository
{
public PrgGeoRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
