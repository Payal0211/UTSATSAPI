using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSocialProviderRepository : GenericRepository<PrgSocialProvider>, IPrgSocialProviderRepository
{
public PrgSocialProviderRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
