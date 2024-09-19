using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncSalesIncentiveTargetDetailRepository : GenericRepository<GenIncSalesIncentiveTargetDetail>, IGenIncSalesIncentiveTargetDetailRepository
{
public GenIncSalesIncentiveTargetDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
