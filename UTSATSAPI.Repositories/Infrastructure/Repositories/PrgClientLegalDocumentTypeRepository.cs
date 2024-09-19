using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgClientLegalDocumentTypeRepository : GenericRepository<PrgClientLegalDocumentType>, IPrgClientLegalDocumentTypeRepository
{
public PrgClientLegalDocumentTypeRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
