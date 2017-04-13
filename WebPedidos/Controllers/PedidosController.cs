using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPedidos.Models;

namespace WebPedidos.Controllers
{
    public class PedidosController : Controller
    {
        // GET: Pedidos
        public ActionResult NewPedido()
        {
            var orderView      = new PedidoView();
            orderView.Cliente  = new Cliente();
            orderView.Producto = new Producto();
            return View(orderView);



        }
    }
}