using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class PedidoDet
    {
        [Key]
        public long idPedido { get; set; }
        public int idProducto { get; set; }
        public double Cantidad { get; set; }
        public double ValorUnit { get; set; }
        public double SubTotal { get; set; }
        //muchos a uno
        public virtual Pedido Pedidos { get; set; }
        public virtual Producto Productos { get; set; }

    }
}