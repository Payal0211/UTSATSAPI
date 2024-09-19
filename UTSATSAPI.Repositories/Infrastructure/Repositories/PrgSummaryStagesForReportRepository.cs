using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSummaryStagesForReportRepository : GenericRepository<PrgSummaryStagesForReport>, IPrgSummaryStagesForReportRepository
{
public PrgSummaryStagesForReportRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
