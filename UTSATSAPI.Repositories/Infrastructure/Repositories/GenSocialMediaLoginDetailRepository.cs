using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSocialMediaLoginDetailRepository : GenericRepository<GenSocialMediaLoginDetail>, IGenSocialMediaLoginDetailRepository
{
public GenSocialMediaLoginDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
