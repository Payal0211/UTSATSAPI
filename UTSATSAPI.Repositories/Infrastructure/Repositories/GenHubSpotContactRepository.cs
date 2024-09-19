using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenHubSpotContactRepository : GenericRepository<GenHubSpotContact>, IGenHubSpotContactRepository
{
public GenHubSpotContactRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
