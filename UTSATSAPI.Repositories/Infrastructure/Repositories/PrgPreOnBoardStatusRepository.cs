using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgPreOnBoardStatusRepository : GenericRepository<PrgPreOnBoardStatus>, IPrgPreOnBoardStatusRepository
{
public PrgPreOnBoardStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
