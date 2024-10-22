using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenAibasedHrtalentMatchMakingDetailRepository : GenericRepository<GenAibasedHrtalentMatchMakingDetail>, IGenAibasedHrtalentMatchMakingDetailRepository
{
public GenAibasedHrtalentMatchMakingDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
