using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentPreferableWorkingDetailRepository : GenericRepository<GenTalentPreferableWorkingDetail>, IGenTalentPreferableWorkingDetailRepository
{
public GenTalentPreferableWorkingDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
