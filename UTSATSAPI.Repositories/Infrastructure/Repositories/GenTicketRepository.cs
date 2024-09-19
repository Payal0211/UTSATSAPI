using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTicketRepository : GenericRepository<GenTicket>, IGenTicketRepository
{
public GenTicketRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
