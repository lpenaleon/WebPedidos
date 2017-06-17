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
    public class EstadosController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: Estados

        public ActionResult BuscarNumPedido()
        {
            var pedidosT = db.Pedidos.OrderByDescending(p => p.idPedido);
            
            ViewBag.idPedido = new SelectList(pedidosT, "idPedido", "idPedido");

            return View();

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var estados = db.Estados.Include(e => e.Pedido);
            return View(estados.ToList());
        }

        // GET: Estados/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // GET: Estados/Create
        public ActionResult Create()
        {
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido");
            return View();
        }

        // POST: Estados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEstado,idPedido,FechaEstado,OrdenEstado,Nota")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Estados.Add(estado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", estado.idPedido);
            return View(estado);
        }

        // GET: Estados/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", estado.idPedido);
            return View(estado);
        }

        // POST: Estados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEstado,idPedido,FechaEstado,OrdenEstado,Nota")] Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", estado.idPedido);
            return View(estado);
        }

        // GET: Estados/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        // POST: Estados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Estado estado = db.Estados.Find(id);
            db.Estados.Remove(estado);
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
