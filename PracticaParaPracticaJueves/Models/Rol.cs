using System.ComponentModel.DataAnnotations;

namespace PracticaParaPracticaJueves.Models
{
    public class Rol
    {
        public Rol()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public int Id { get; set; }

        public virtual ICollection<User> Users { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string Description { get; set; }

    }
}
