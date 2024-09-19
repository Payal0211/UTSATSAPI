using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgApitimeZoneResponseRepository : GenericRepository<PrgApitimeZoneResponse>, IPrgApitimeZoneResponseRepository
{
public PrgApitimeZoneResponseRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
