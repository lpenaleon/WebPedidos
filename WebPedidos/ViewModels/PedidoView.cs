using System.Collections.Generic;
using WebPedidos.Models;

namespace WebPedidos.ViewModels
{
    public class PedidoView
    {
        public Cliente Cliente { get; set; }
        public List<ProductoPedido> Productos { get; set; }
        
    }
}