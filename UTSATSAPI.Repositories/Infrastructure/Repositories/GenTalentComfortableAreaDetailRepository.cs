using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentComfortableAreaDetailRepository : GenericRepository<GenTalentComfortableAreaDetail>, IGenTalentComfortableAreaDetailRepository
{
public GenTalentComfortableAreaDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
