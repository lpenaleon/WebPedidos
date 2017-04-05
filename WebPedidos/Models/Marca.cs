using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class Marca
    {
        [Key]
        public int idMarca { get; set; }
        
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Marca")]
        [StringLength(10, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 2)]
        public string NomMarca { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}