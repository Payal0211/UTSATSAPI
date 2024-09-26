using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class UsrUserHierarchyRepository : GenericRepository<UsrUserHierarchy>, IUsrUserHierarchyRepository
{
public UsrUserHierarchyRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
