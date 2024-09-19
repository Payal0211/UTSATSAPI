using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenPmsTalentRedirectUrlDetailRepository : GenericRepository<GenPmsTalentRedirectUrlDetail>, IGenPmsTalentRedirectUrlDetailRepository
{
public GenPmsTalentRedirectUrlDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
