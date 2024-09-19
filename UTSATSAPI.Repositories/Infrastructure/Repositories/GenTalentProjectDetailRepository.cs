using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentProjectDetailRepository : GenericRepository<GenTalentProjectDetail>, IGenTalentProjectDetailRepository
{
public GenTalentProjectDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
