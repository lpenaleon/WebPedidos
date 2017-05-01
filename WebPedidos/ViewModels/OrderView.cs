using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebPedidos.Models;

namespace WebPedidos.ViewModels
{
    public class OrderView
    {
        public List<Cliente> Cliente { get; set; }
        public List<FormPago> FormPagos { get; set; }
        public ProductOrder Products { get; set; }
        public PedidoDetAcu PedidoDetAcu { get; set; }
        public List<ProductOrder> Productos { get; set; }   
    }
}