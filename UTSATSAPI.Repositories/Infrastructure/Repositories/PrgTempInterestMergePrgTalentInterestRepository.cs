using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempInterestMergePrgTalentInterestRepository : GenericRepository<PrgTempInterestMergePrgTalentInterest>, IPrgTempInterestMergePrgTalentInterestRepository
{
public PrgTempInterestMergePrgTalentInterestRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
