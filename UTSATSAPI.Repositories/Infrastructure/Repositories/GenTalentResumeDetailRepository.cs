using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentResumeDetailRepository : GenericRepository<GenTalentResumeDetail>, IGenTalentResumeDetailRepository
{
public GenTalentResumeDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
