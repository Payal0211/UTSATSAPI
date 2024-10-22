using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenCompanyLeadTypeUserDetailRepository : GenericRepository<GenCompanyLeadTypeUserDetail>, IGenCompanyLeadTypeUserDetailRepository
{
public GenCompanyLeadTypeUserDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
