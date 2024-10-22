using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncBaseSalUserRoleDetailRepository : GenericRepository<GenIncBaseSalUserRoleDetail>, IGenIncBaseSalUserRoleDetailRepository
{
public GenIncBaseSalUserRoleDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
