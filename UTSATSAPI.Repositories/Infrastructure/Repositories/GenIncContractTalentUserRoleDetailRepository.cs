using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenIncContractTalentUserRoleDetailRepository : GenericRepository<GenIncContractTalentUserRoleDetail>, IGenIncContractTalentUserRoleDetailRepository
{
public GenIncContractTalentUserRoleDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
