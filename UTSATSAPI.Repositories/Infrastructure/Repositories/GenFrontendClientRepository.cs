using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenFrontendClientRepository : GenericRepository<GenFrontendClient>, IGenFrontendClientRepository
{
public GenFrontendClientRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
