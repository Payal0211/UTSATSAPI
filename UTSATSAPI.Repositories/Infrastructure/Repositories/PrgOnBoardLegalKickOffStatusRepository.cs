using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgOnBoardLegalKickOffStatusRepository : GenericRepository<PrgOnBoardLegalKickOffStatus>, IPrgOnBoardLegalKickOffStatusRepository
{
public PrgOnBoardLegalKickOffStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
