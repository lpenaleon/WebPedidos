using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace WebPedidos.Models
{
    [Table("Empleados")]
    public class Empleado
    {
                
        [Key]
        public int idEmpleado { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Nombre")]
        [StringLength(45, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Apellidos")]
        [StringLength(45, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 2)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Ingrese su Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Por favor ingrese el Correo Electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public int idCargo { get; set; }

        public virtual Cargo Cargo { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}