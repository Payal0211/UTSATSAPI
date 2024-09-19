using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentEducationDetailRepository : GenericRepository<GenTalentEducationDetail>, IGenTalentEducationDetailRepository
{
public GenTalentEducationDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
