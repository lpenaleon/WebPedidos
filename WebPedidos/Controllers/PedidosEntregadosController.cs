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
    public class PedidosEntregadosController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: PedidosEntregados
        [Authorize(Roles = "Admin,Despachos")]
        public ActionResult Index()
        {
            

            var pedido1 = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.FormPago);

            var pedidos = pedido1
                .Where(p => p.OrdenEstado == OrdenEstado.Entregada)
                .OrderBy(p => p.idPedido);

            return View(pedidos.ToList());
        }

        // GET: PedidoEnP/Details/5
        [Authorize(Roles = "Admin,Despachos")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }

            var pedDet0 = db.PedidoDets
                .Include(pd => pd.Producto)
                .OrderBy(pd => pd.idPedidoDet);

            var pedDet = pedDet0
                .Where(p => p.idPedido == id);

            decimal TotPre = 0;

            foreach (var items in pedDet)
            {
                TotPre = items.PrecioT + TotPre;
            }

            ViewBag.Tot_Pre = string.Format("{0:C2}", TotPre);

            return View(pedDet.ToList());
        }


        // GET: PedidosEntregados/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde");
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago");
            return View();
        }

        // POST: PedidosEntregados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedido,FechaPedido,DiasCred,idCliente,idFormPago,OrdenEstado")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedidos.Add(pedido);
                db.SaveChanges();

                var estado = new Estado
                {
                    idPedido = pedido.idPedido,
                    FechaEstado = DateTime.Now,
                    OrdenEstado = pedido.OrdenEstado,
                    Nota = ""
                };
                db.Estados.Add(estado);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // GET: PedidosEntregados/Edit/5
        [Authorize(Roles = "Despachos")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // POST: PedidosEntregados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedido,FechaPedido,DiasCred,idCliente,idFormPago,OrdenEstado")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // GET: PedidosEntregados/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: PedidosEntregados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
