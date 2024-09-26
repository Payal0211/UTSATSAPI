using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgEmploymentStatusRepository : GenericRepository<PrgEmploymentStatus>, IPrgEmploymentStatusRepository
{
public PrgEmploymentStatusRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
