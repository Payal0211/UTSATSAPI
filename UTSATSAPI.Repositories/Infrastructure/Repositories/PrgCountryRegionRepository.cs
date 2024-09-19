using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCountryRegionRepository : GenericRepository<PrgCountryRegion>, IPrgCountryRegionRepository
{
public PrgCountryRegionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
