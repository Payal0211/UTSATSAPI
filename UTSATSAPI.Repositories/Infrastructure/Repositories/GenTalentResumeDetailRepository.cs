using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentResumeDetailRepository : GenericRepository<GenTalentResumeDetail>, IGenTalentResumeDetailRepository
{
public GenTalentResumeDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
