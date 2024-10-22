using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgSkillsInAssesmentRepository : GenericRepository<PrgSkillsInAssesment>, IPrgSkillsInAssesmentRepository
{
public PrgSkillsInAssesmentRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
