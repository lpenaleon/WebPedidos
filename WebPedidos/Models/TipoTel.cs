using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class TipoTel
    {
        [Key]
        public int idTipoTel { get; set; }
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Tipo de Teléfono")]
        [StringLength(15, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 4)]
        public string NomTipoTel { get; set; }
        public virtual ICollection<Telefono> Telefonos { get; set; }
    }
}