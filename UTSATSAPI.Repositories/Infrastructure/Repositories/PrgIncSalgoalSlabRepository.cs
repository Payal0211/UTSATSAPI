using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgIncSalgoalSlabRepository : GenericRepository<PrgIncSalgoalSlab>, IPrgIncSalgoalSlabRepository
{
public PrgIncSalgoalSlabRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
