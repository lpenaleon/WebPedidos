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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public double acumulado { get; set; }
        public double IVA { get; set; }
        public int dCred { get; set; }
        public int idCliente { get; set; }
        public int idFormPago { get; set; }
        //Muchos a uno
        public virtual Cliente Clientes { get; set; }
        public virtual FormPago FormPagos { get; set; }
        public virtual ICollection<PedFlete> PedFletes { get; set; }
        //uno a muchos
        public virtual ICollection<PedidoDet> PedidoDets { get; set; }


    }

}