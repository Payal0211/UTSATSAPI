using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenCompanyWeightedAverageCriterionRepository : GenericRepository<GenCompanyWeightedAverageCriterion>, IGenCompanyWeightedAverageCriterionRepository
{
public GenCompanyWeightedAverageCriterionRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
