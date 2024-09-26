using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentKeyQualityDetailRepository : GenericRepository<GenTalentKeyQualityDetail>, IGenTalentKeyQualityDetailRepository
{
public GenTalentKeyQualityDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
