using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgIncPlacementFeesSlabRepository : GenericRepository<PrgIncPlacementFeesSlab>, IPrgIncPlacementFeesSlabRepository
{
public PrgIncPlacementFeesSlabRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
