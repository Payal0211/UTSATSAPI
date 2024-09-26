using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgHowSoonClientWantTalentRepository : GenericRepository<PrgHowSoonClientWantTalent>, IPrgHowSoonClientWantTalentRepository
{
public PrgHowSoonClientWantTalentRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
