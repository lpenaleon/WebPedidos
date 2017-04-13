using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPedidos;
using WebPedidos.Models;

namespace WebPedidos.ViewModels
{
    public class PedidoView
    {
        public virtual Cliente Clientes { get; set; }
        public List<ProductoPedido> Productos { get; set; }
    }
}