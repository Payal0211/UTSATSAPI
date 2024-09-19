using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentTimeZoneRepository : GenericRepository<PrgTalentTimeZone>, IPrgTalentTimeZoneRepository
{
public PrgTalentTimeZoneRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
