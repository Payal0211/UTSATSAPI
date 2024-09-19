using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgContactStatusRepository : GenericRepository<PrgContactStatus>, IPrgContactStatusRepository
{
public PrgContactStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
