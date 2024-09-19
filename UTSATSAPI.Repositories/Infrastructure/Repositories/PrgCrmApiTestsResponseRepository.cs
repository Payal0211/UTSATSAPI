using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCrmApiTestsResponseRepository : GenericRepository<PrgCrmApiTestsResponse>, IPrgCrmApiTestsResponseRepository
{
public PrgCrmApiTestsResponseRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
