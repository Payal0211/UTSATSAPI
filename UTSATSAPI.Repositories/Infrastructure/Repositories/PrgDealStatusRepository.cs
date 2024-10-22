using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgDealStatusRepository : GenericRepository<PrgDealStatus>, IPrgDealStatusRepository
{
public PrgDealStatusRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
