using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncSalesIncentiveTargetSummaryRepository : GenericRepository<GenIncSalesIncentiveTargetSummary>, IGenIncSalesIncentiveTargetSummaryRepository
{
public GenIncSalesIncentiveTargetSummaryRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
