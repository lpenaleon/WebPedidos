using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class Maquina
    {
        [Key]
        public int IdMaquina { get; set; }
        [Display(Name = "Máquina")]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [StringLength(35, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 2)]
        public string NomMaquina { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }

    }
}