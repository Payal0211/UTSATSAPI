using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTicketStatusRepository : GenericRepository<PrgTicketStatus>, IPrgTicketStatusRepository
{
public PrgTicketStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
