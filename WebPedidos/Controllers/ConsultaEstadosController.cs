using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebPedidos.Models;

namespace WebPedidos.Controllers
{
    public class ConsultaEstadosController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: Estados
        public ActionResult Index()
        {
            var estados = new List<Estado>();

            return View(estados);
        }

        [HttpPost]
        public ActionResult Index(int? IdPedido)
        {
            var estados = new List<Estado>();

            if (IdPedido == null || string.IsNullOrEmpty(IdPedido.ToString()))
            {
                return View(estados);
            }

            estados = db.Estados.Where(e => e.idPedido == IdPedido).ToList();

            if (estados.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No hay pedidos y/o estados para la solicitud");

                return View(new List<Estado>());
            }

            return View(estados);
        }

    }
}