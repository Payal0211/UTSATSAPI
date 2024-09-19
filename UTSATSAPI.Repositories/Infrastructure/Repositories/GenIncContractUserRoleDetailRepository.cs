using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncContractUserRoleDetailRepository : GenericRepository<GenIncContractUserRoleDetail>, IGenIncContractUserRoleDetailRepository
{
public GenIncContractUserRoleDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
