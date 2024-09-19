using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentAssociatedWithUplerRepository : GenericRepository<PrgTalentAssociatedWithUpler>, IPrgTalentAssociatedWithUplerRepository
{
public PrgTalentAssociatedWithUplerRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
