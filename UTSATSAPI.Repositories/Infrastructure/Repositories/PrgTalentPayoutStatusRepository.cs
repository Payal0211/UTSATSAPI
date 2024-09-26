using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentPayoutStatusRepository : GenericRepository<PrgTalentPayoutStatus>, IPrgTalentPayoutStatusRepository
{
public PrgTalentPayoutStatusRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
