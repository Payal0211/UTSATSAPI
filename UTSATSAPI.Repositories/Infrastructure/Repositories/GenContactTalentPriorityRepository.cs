using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactTalentPriorityRepository : GenericRepository<GenContactTalentPriority>, IGenContactTalentPriorityRepository
{
public GenContactTalentPriorityRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
