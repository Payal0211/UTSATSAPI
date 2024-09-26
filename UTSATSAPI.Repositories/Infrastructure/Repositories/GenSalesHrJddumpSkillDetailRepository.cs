using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHrJddumpSkillDetailRepository : GenericRepository<GenSalesHrJddumpSkillDetail>, IGenSalesHrJddumpSkillDetailRepository
{
public GenSalesHrJddumpSkillDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
