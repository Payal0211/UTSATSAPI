using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHiringRequestDetailRepository : GenericRepository<GenSalesHiringRequestDetail>, IGenSalesHiringRequestDetailRepository
{
public GenSalesHiringRequestDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
