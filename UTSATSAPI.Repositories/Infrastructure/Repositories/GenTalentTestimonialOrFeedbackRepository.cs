using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentTestimonialOrFeedbackRepository : GenericRepository<GenTalentTestimonialOrFeedback>, IGenTalentTestimonialOrFeedbackRepository
{
public GenTalentTestimonialOrFeedbackRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
