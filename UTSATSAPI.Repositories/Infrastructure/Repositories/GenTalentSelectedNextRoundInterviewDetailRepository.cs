using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentSelectedNextRoundInterviewDetailRepository : GenericRepository<GenTalentSelectedNextRoundInterviewDetail>, IGenTalentSelectedNextRoundInterviewDetailRepository
{
public GenTalentSelectedNextRoundInterviewDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
