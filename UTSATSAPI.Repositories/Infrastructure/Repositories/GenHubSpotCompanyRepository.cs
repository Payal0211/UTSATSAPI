using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenHubSpotCompanyRepository : GenericRepository<GenHubSpotCompany>, IGenHubSpotCompanyRepository
{
public GenHubSpotCompanyRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
