using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentSelectedInterviewDetailRepository : GenericRepository<GenTalentSelectedInterviewDetail>, IGenTalentSelectedInterviewDetailRepository
{
public GenTalentSelectedInterviewDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
