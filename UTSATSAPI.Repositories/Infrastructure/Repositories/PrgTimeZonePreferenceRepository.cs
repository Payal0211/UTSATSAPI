using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTimeZonePreferenceRepository : GenericRepository<PrgTimeZonePreference>, IPrgTimeZonePreferenceRepository
{
public PrgTimeZonePreferenceRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
