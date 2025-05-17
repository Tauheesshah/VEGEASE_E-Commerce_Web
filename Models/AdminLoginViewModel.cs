using System.ComponentModel.DataAnnotations;

namespace VegEaseBackend.Models
{
    public class AdminLoginViewModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
