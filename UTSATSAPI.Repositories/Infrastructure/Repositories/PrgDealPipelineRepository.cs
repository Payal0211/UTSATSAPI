using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgDealPipelineRepository : GenericRepository<PrgDealPipeline>, IPrgDealPipelineRepository
{
public PrgDealPipelineRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
