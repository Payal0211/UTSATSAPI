using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgIncAmNrSlabRepository : GenericRepository<PrgIncAmNrSlab>, IPrgIncAmNrSlabRepository
{
public PrgIncAmNrSlabRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
