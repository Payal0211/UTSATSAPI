using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgRequirementRepository : GenericRepository<PrgRequirement>, IPrgRequirementRepository
{
public PrgRequirementRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
