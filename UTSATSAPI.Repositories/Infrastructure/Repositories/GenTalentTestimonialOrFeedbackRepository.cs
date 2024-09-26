using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentTestimonialOrFeedbackRepository : GenericRepository<GenTalentTestimonialOrFeedback>, IGenTalentTestimonialOrFeedbackRepository
{
public GenTalentTestimonialOrFeedbackRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
