using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentInterestRepository : GenericRepository<PrgTalentInterest>, IPrgTalentInterestRepository
{
public PrgTalentInterestRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
