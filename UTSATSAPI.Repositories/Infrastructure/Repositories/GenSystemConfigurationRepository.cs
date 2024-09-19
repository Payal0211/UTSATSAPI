using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSystemConfigurationRepository : GenericRepository<GenSystemConfiguration>, IGenSystemConfigurationRepository
{
public GenSystemConfigurationRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
