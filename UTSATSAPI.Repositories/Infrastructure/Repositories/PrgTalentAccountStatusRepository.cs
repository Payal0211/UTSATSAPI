using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentAccountStatusRepository : GenericRepository<PrgTalentAccountStatus>, IPrgTalentAccountStatusRepository
{
public PrgTalentAccountStatusRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
