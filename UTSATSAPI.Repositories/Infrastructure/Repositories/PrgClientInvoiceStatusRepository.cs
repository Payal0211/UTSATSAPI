using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgClientInvoiceStatusRepository : GenericRepository<PrgClientInvoiceStatus>, IPrgClientInvoiceStatusRepository
{
public PrgClientInvoiceStatusRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
