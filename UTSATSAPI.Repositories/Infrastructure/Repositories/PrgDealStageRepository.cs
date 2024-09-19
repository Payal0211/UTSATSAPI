using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgDealStageRepository : GenericRepository<PrgDealStage>, IPrgDealStageRepository
{
public PrgDealStageRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
