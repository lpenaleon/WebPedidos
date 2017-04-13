using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class TipoIde
    {
        [Key]
        public int idTipoIde { get; set; }
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Tipo de Identificación")]
        [StringLength(33, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 2)]
        public string NomTipoIde { get; set; }
        public virtual ICollection<Cliente> Clientes { get; set; }



    }
}