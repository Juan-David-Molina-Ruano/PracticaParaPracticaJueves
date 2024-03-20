using System.ComponentModel.DataAnnotations;

namespace PracticaParaPracticaJueves.Models
{
    public class Customer
    {
        public Customer()
        {
            Phones = new List<Phone>();
        }
        // Propiedades de la clase Customer
        [Key]
        public int Id { get; set; }

        public virtual List<Phone> Phones { get; set; }

        [Display(Name = "Imagen")]
        public byte[]? Imagen { get; set; } // Recibe bytes

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El campo {0} solo puede contener letras")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El campo {0} solo puede contener letras")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo {0} solo puede contener números")]
        [EmailAddress(ErrorMessage = "El campo {0} no es un correo válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo {0} solo puede contener números")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Address { get; set; }
    }
}
