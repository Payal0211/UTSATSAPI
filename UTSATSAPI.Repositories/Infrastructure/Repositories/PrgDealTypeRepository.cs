using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgDealTypeRepository : GenericRepository<PrgDealType>, IPrgDealTypeRepository
{
public PrgDealTypeRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
