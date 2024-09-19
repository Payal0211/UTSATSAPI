using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenCompanyLegalInfoRepository : GenericRepository<GenCompanyLegalInfo>, IGenCompanyLegalInfoRepository
{
public GenCompanyLegalInfoRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
