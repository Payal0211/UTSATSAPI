using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentInterestDetailRepository : GenericRepository<GenTalentInterestDetail>, IGenTalentInterestDetailRepository
{
public GenTalentInterestDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
