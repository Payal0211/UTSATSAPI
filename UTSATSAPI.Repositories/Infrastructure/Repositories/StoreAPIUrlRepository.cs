using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class StoreAPIUrlRepository : GenericRepository<StoreApiurl>, IStoreAPIUrlRepository
{
public StoreAPIUrlRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
