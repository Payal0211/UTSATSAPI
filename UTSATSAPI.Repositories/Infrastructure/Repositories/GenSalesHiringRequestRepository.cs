using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHiringRequestRepository : GenericRepository<GenSalesHiringRequest>, IGenSalesHiringRequestRepository
{
public GenSalesHiringRequestRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
