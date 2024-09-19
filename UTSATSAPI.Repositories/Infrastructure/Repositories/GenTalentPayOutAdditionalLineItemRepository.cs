using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentPayOutAdditionalLineItemRepository : GenericRepository<GenTalentPayOutAdditionalLineItem>, IGenTalentPayOutAdditionalLineItemRepository
{
public GenTalentPayOutAdditionalLineItemRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
