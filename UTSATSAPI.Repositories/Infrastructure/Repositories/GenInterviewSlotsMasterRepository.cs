using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenInterviewSlotsMasterRepository : GenericRepository<GenInterviewSlotsMaster>, IGenInterviewSlotsMasterRepository
{
public GenInterviewSlotsMasterRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
