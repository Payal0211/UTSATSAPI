using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenCompanyRepository : GenericRepository<GenCompany>, IGenCompanyRepository
{
public GenCompanyRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
