using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTempInterestRepository : GenericRepository<PrgTempInterest>, IPrgTempInterestRepository
{
public PrgTempInterestRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
