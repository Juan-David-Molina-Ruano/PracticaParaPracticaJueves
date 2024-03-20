using System.ComponentModel.DataAnnotations;

namespace PracticaParaPracticaJueves.Models
{
    public class Phone
    {
        // Propiedades de la clase Phones
        [Key]
        public int Id { get; set; }

        //--------------customer
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        //--------------

        //validaciones que solo se puedan numeros y que no se puedan poner mas de 8 numeros
        [StringLength(8, ErrorMessage = "El campo {0} debe tener {1} caracteres", MinimumLength = 8)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo {0} solo puede contener números")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "El campo {0} solo puede contener letras")]
        public string Description { get; set; }
    }
}
