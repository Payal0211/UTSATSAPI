using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTestimonialRepository : GenericRepository<GenTestimonial>, IGenTestimonialRepository
{
public GenTestimonialRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
