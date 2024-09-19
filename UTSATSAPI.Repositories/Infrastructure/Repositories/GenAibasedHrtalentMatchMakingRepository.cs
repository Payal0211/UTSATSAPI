using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenAibasedHrtalentMatchMakingRepository : GenericRepository<GenAibasedHrtalentMatchMaking>, IGenAibasedHrtalentMatchMakingRepository
{
public GenAibasedHrtalentMatchMakingRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
