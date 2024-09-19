using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentCategoryRepository : GenericRepository<PrgTalentCategory>, IPrgTalentCategoryRepository
{
public PrgTalentCategoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
