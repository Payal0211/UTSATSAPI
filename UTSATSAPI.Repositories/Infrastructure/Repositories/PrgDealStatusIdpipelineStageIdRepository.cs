using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgDealStatusIdpipelineStageIdRepository : GenericRepository<PrgDealStatusIdpipelineStageId>, IPrgDealStatusIdpipelineStageIdRepository
{
public PrgDealStatusIdpipelineStageIdRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
