using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentCertificationRepository : GenericRepository<PrgTalentCertification>, IPrgTalentCertificationRepository
{
public PrgTalentCertificationRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
