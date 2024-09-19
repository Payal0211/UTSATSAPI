using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHrTracceptedDetailRepository : GenericRepository<GenSalesHrTracceptedDetail>, IGenSalesHrTracceptedDetailRepository
{
public GenSalesHrTracceptedDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
