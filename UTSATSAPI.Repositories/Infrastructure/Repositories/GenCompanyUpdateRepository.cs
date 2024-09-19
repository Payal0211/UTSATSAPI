using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenCompanyUpdateRepository : GenericRepository<GenCompanyUpdate>, IGenCompanyUpdateRepository
{
public GenCompanyUpdateRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
