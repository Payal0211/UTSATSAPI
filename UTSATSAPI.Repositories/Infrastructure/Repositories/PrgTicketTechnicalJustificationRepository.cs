using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTicketTechnicalJustificationRepository : GenericRepository<PrgTicketTechnicalJustification>, IPrgTicketTechnicalJustificationRepository
{
public PrgTicketTechnicalJustificationRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
