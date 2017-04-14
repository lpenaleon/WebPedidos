using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPedidos.Help;
using WebPedidos.Models;
using System.Collections.Generic;
using System.Data;
using System.Web;
using WebPedidos.ViewModels;

namespace WebPedidos.Controllers
{
    public class PedidosController : Controller
    {

        public ActionResult NewPedido()
        {
            var orderView       = new PedidoView();
            orderView.Cliente   = new Cliente();
            orderView.Productos = new List<ProductoPedido>();
            return View(orderView);
        }
    }
}