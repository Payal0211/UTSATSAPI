using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentSelectedInterviewerDetailRepository : GenericRepository<GenTalentSelectedInterviewerDetail>, IGenTalentSelectedInterviewerDetailRepository
{
public GenTalentSelectedInterviewerDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
