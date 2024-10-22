using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserTypeRepository : GenericRepository<UsrUserType>, IUsrUserTypeRepository
{
public UsrUserTypeRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
