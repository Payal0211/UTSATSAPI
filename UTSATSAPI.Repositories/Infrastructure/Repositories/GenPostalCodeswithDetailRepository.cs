using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenPostalCodeswithDetailRepository : GenericRepository<GenPostalCodeswithDetail>, IGenPostalCodeswithDetailRepository
{
public GenPostalCodeswithDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
