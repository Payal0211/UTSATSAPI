using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTimeZoneRepository : GenericRepository<PrgTimeZone>, IPrgTimeZoneRepository
{
public PrgTimeZoneRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
