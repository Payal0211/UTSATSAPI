using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentLegalInfoRepository : GenericRepository<GenTalentLegalInfo>, IGenTalentLegalInfoRepository
{
public GenTalentLegalInfoRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
