using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgIncAmSlabRepository : GenericRepository<PrgIncAmSlab>, IPrgIncAmSlabRepository
{
public PrgIncAmSlabRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
