using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenHubSpotCompanyRepository : GenericRepository<GenHubSpotCompany>, IGenHubSpotCompanyRepository
{
public GenHubSpotCompanyRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
