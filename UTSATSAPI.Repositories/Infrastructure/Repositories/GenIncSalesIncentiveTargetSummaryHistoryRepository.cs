using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncSalesIncentiveTargetSummaryHistoryRepository : GenericRepository<GenIncSalesIncentiveTargetSummaryHistory>, IGenIncSalesIncentiveTargetSummaryHistoryRepository
{
public GenIncSalesIncentiveTargetSummaryHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
