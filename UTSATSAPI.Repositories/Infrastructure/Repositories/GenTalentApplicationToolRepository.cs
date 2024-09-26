using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentApplicationToolRepository : GenericRepository<GenTalentApplicationTool>, IGenTalentApplicationToolRepository
{
public GenTalentApplicationToolRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
