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
    public class PedidosController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: Pedidos
        public ActionResult Index()
        {
            var pedidoes = db.Pedidos.Include(p => p.Clientes).Include(p => p.FormPagos);
            return View(pedidoes.ToList());
        }

        // GET: Pedidos/Details/5
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
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "ideNIT");
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago");
            return View();
        }

        // POST: Pedidos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedido,Fecha,acumulado,IVA,dCred,idCliente,idFormPago")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "ideNIT", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
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
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "ideNIT", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedido,Fecha,acumulado,IVA,dCred,idCliente,idFormPago")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "ideNIT", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
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

        // POST: Pedidos/Delete/5
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
