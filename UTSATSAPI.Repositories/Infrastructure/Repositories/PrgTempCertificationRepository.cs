using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempCertificationRepository : GenericRepository<PrgTempCertification>, IPrgTempCertificationRepository
{
public PrgTempCertificationRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
