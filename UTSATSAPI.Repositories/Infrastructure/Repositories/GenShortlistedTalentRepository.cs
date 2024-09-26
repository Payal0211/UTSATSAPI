using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenShortlistedTalentRepository : GenericRepository<GenShortlistedTalent>, IGenShortlistedTalentRepository
{
public GenShortlistedTalentRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
