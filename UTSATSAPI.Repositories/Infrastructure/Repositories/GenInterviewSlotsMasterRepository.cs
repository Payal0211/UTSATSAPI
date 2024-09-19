using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenInterviewSlotsMasterRepository : GenericRepository<GenInterviewSlotsMaster>, IGenInterviewSlotsMasterRepository
{
public GenInterviewSlotsMasterRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
