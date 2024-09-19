using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHiringRequestPriorityRepository : GenericRepository<GenSalesHiringRequestPriority>, IGenSalesHiringRequestPriorityRepository
{
public GenSalesHiringRequestPriorityRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
