using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class Flete
    {
        [Key]
        public int IdFlete { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Flete")]
        [StringLength(14, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NomFlete { get; set; }
        //uno a muchos
        public virtual PedFlete PedFletes { get; set; }

    }
}