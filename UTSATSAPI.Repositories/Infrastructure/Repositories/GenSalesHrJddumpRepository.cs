using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHrJddumpRepository : GenericRepository<GenSalesHrJddump>, IGenSalesHrJddumpRepository
{
public GenSalesHrJddumpRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
