using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentJoinningRepository : GenericRepository<PrgTalentJoinning>, IPrgTalentJoinningRepository
{
public PrgTalentJoinningRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
