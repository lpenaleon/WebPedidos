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
        public long idPedido { get; set; }

        [Required(ErrorMessage ="Debe ingresar una {0}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaPedido { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public double acumulado { get; set; }
        public double vrIVA { get; set; }
        public int dCred { get; set; }
        public int idCliente { get; set; }
        public int idFormPago { get; set; }
        //Muchos a uno
        public virtual Cliente Clientes { get; set; }
        public virtual FormPago FormPagos { get; set; }
        public virtual OrdenEstado OrdenEstado { get; set; }
        //uno a muchos
        public virtual ICollection<PedFlete> PedFletes { get; set; }
        public virtual ICollection<PedidoDet> PedidoDets { get; set; }


    }

}