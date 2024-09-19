using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTicketCategoryRepository : GenericRepository<PrgTicketCategory>, IPrgTicketCategoryRepository
{
public PrgTicketCategoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
