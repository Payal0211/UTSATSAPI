using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgIncAmNrSlabRepository : GenericRepository<PrgIncAmNrSlab>, IPrgIncAmNrSlabRepository
{
public PrgIncAmNrSlabRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
