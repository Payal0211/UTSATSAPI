using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgAmdealContactRepository : GenericRepository<PrgAmdealContact>, IPrgAmdealContactRepository
{
public PrgAmdealContactRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
