using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserRoleRepository : GenericRepository<UsrUserRole>, IUsrUserRoleRepository
{
public UsrUserRoleRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
