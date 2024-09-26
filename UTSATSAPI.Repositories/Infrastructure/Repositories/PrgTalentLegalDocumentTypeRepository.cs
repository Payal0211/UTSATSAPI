using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgTalentLegalDocumentTypeRepository : GenericRepository<PrgTalentLegalDocumentType>, IPrgTalentLegalDocumentTypeRepository
{
public PrgTalentLegalDocumentTypeRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
