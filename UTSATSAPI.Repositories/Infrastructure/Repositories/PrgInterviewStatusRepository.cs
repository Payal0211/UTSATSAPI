using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgInterviewStatusRepository : GenericRepository<PrgInterviewStatus>, IPrgInterviewStatusRepository
{
public PrgInterviewStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
