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
    public class PedidoDetsController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: PedidoDets
        public ActionResult Index()
        {
            var pedidoDets = db.PedidoDets.Include(p => p.Producto);
            return View(pedidoDets.ToList());
        }

        // GET: PedidoDets/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoDet pedidoDet = db.PedidoDets.Find(id);
            if (pedidoDet == null)
            {
                return HttpNotFound();
            }
            return View(pedidoDet);
        }

        // GET: PedidoDets/Create
        public ActionResult Create()
        {
            ViewBag.idProducto = new SelectList(db.Productos, "idProducto", "Codigo");
            return View();
        }

        // POST: PedidoDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedido,idProducto,Cantidad,ValorUnit,SubTotal")] PedidoDet pedidoDet)
        {
            if (ModelState.IsValid)
            {
                db.PedidoDets.Add(pedidoDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProducto = new SelectList(db.Productos, "idProducto", "Codigo", pedidoDet.idProducto);
            return View(pedidoDet);
        }

        // GET: PedidoDets/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoDet pedidoDet = db.PedidoDets.Find(id);
            if (pedidoDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProducto = new SelectList(db.Productos, "idProducto", "Codigo", pedidoDet.idProducto);
            return View(pedidoDet);
        }

        // POST: PedidoDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedido,idProducto,Cantidad,ValorUnit,SubTotal")] PedidoDet pedidoDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedidoDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProducto = new SelectList(db.Productos, "idProducto", "Codigo", pedidoDet.idProducto);
            return View(pedidoDet);
        }

        // GET: PedidoDets/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoDet pedidoDet = db.PedidoDets.Find(id);
            if (pedidoDet == null)
            {
                return HttpNotFound();
            }
            return View(pedidoDet);
        }

        // POST: PedidoDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PedidoDet pedidoDet = db.PedidoDets.Find(id);
            db.PedidoDets.Remove(pedidoDet);
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
