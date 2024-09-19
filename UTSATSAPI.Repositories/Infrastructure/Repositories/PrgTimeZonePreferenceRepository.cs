using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTimeZonePreferenceRepository : GenericRepository<PrgTimeZonePreference>, IPrgTimeZonePreferenceRepository
{
public PrgTimeZonePreferenceRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
