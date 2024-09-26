using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenDealTicketRepository : GenericRepository<GenDealTicket>, IGenDealTicketRepository
{
public GenDealTicketRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
