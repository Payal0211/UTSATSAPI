using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentRepository : GenericRepository<GenTalent>, IGenTalentRepository
{
public GenTalentRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
