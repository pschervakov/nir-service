
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mephi.Cyber.AuthService.Core.Models
{
  

 
    public class Relation
    {
        
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }
    }
}