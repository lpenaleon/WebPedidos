using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [Display(Name = "Pedido")]
        public long idPedido { get; set; }

        //[Required(ErrorMessage ="Debe ingresar una {0}")]
        // [DataType(DataType.Date)]
        [Display(Name = "Fecha Pedido")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaPedido { get; set; }
        
        [NotMapped]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Tot_IVA { get; set; }

        [NotMapped]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Tot_Pre { get; set; }

        [NotMapped]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Tot_Acu { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Días de Crédito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public byte DiasCred { get; set; }

        [Display(Name = "Cliente")]
        public int idCliente { get; set; }

        [Display(Name = "Forma Pago")]
        public int idFormPago { get; set; }

        [Display(Name = "Estado")]
        public OrdenEstado OrdenEstado { get; set; }

        //Muchos a uno
        public virtual Cliente Cliente { get; set; }
        public virtual FormPago FormPago { get; set; }
        
        //uno a muchos
        public virtual ICollection<PedFlete> PedFletes { get; set; }
        public virtual ICollection<PedidoDet> PedidoDets { get; set; }
        public virtual ICollection<Estado> Estados { get; set; }
    }
}