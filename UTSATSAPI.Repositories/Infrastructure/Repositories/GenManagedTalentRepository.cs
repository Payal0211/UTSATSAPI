using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenManagedTalentRepository : GenericRepository<GenManagedTalent>, IGenManagedTalentRepository
{
public GenManagedTalentRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
