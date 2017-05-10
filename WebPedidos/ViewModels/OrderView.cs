using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebPedidos.Models;

namespace WebPedidos.ViewModels
{
    public class OrderView
    {
        public List<Cliente> Cliente { get; set; }
        public List<FormPago> FormPagos { get; set; }
        public ProductOrder Products { get; set; }
        public List<ProductOrder> Productos { get; set; }   
    }
}