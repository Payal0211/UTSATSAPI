using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserRoleDetailRepository : GenericRepository<UsrUserRoleDetail>, IUsrUserRoleDetailRepository
{
public UsrUserRoleDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
