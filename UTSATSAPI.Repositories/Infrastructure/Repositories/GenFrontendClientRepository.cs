using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenFrontendClientRepository : GenericRepository<GenFrontendClient>, IGenFrontendClientRepository
{
public GenFrontendClientRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
