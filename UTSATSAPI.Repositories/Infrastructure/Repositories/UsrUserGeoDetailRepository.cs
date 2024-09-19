using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserGeoDetailRepository : GenericRepository<UsrUserGeoDetail>, IUsrUserGeoDetailRepository
{
public UsrUserGeoDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
