using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class FormPago
    {
        [Key]
        public int idFormPago { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Forma de Pago")]
        [StringLength(14, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NomFPago { get; set; }

        //uno a muchos 
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}