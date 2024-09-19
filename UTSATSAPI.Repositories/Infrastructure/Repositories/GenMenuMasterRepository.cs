using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenMenuMasterRepository : GenericRepository<GenMenuMaster>, IGenMenuMasterRepository
{
public GenMenuMasterRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
