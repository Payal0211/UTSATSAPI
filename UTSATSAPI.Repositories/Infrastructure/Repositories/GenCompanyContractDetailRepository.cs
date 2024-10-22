using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenCompanyContractDetailRepository : GenericRepository<GenCompanyContractDetail>, IGenCompanyContractDetailRepository
{
public GenCompanyContractDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
