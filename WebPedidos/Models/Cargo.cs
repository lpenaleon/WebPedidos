using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("Cargos")]
    public class Cargo
    {
        [Key]
        public int idCargo { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Cargo")]
        [StringLength(25, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NomCargo { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}