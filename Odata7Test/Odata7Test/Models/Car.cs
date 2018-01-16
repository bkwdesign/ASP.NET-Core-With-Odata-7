using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Odata7Test.Models
{
    public class Car
    {
     
        public int Id { get; set; }
        [Required]
      
        public string Name { get; set; }
    }
}
