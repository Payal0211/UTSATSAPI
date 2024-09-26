using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncAmNrUserRoleDetailRepository : GenericRepository<GenIncAmNrUserRoleDetail>, IGenIncAmNrUserRoleDetailRepository
{
public GenIncAmNrUserRoleDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
