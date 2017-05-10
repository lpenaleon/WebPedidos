using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [NotMapped]
    public class PedidoDetAcu
    {    
        
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Tot_IVA { get; set; }

        
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Tot_Pre { get; set; }

        
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Tot_Acu { get; set; }
    }
}