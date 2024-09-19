using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentCertificationsDetailRepository : GenericRepository<GenTalentCertificationsDetail>, IGenTalentCertificationsDetailRepository
{
public GenTalentCertificationsDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
