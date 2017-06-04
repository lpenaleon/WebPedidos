using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("PedidoDet")]
    public class PedidoDet
    {
        [Key]
        public long idPedidoDet { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Descripción")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Descripcion { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float iva { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Precio { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Cantidad { get; set; }

        [NotMapped]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal PrecioT { get { return Precio * (decimal)Cantidad; } }


        public long idPedido { get; set; }
        public int idProducto { get; set; }

        //muchos a uno
        public virtual Pedido Pedido { get; set; }
        public virtual Producto Producto { get; set; }
    }
}