using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTicketTechnicalJustificationRepository : GenericRepository<PrgTicketTechnicalJustification>, IPrgTicketTechnicalJustificationRepository
{
public PrgTicketTechnicalJustificationRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
