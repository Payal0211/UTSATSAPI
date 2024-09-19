using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgHiringRequestStatusRepository : GenericRepository<PrgHiringRequestStatus>, IPrgHiringRequestStatusRepository
{
public PrgHiringRequestStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
