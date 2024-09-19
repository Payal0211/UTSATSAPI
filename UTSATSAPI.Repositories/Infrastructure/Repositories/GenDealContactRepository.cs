using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenDealContactRepository : GenericRepository<GenDealContact>, IGenDealContactRepository
{
public GenDealContactRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
