using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class EmpTran
    {
        [Key]
        public int idEmpTran { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Empresa Transportadora")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NomEmpTran { get; set; }
        public virtual ICollection<PedFlete> PedFletes { get; set; }
    }
}