using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.Models
{
    public class JwtTokenDataModel
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string JwtToken { get; set; }
    }
}
