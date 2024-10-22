using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentPointofContactRepository : GenericRepository<GenTalentPointofContact>, IGenTalentPointofContactRepository
{
public GenTalentPointofContactRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
