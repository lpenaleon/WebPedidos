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
    public class PedFletesController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        [Authorize(Roles = "Admin")]
        // GET: PedFletes
        public ActionResult Index()
        {
            var pedFletes = db.PedFletes.Include(p => p.EmpTran).Include(p => p.Pedido);
            return View(pedFletes.ToList());
        }

        // GET: PedFletes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedFlete pedFlete = db.PedFletes.Find(id);
            if (pedFlete == null)
            {
                return HttpNotFound();
            }
            return View(pedFlete);
        }

        // GET: PedFletes/Create
        public ActionResult Create()
        {
            ViewBag.idEmpTran = new SelectList(db.EmpTrans, "idEmpTran", "NomEmpTran");
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido");
            return View();
        }

        // POST: PedFletes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedFlete,idPedido,idFlete,idEmpTran,Valor,Obervaciones")] PedFlete pedFlete)
        {
            if (ModelState.IsValid)
            {
                db.PedFletes.Add(pedFlete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpTran = new SelectList(db.EmpTrans, "idEmpTran", "NomEmpTran", pedFlete.idEmpTran);
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", pedFlete.idPedido);
            return View(pedFlete);
        }

        // GET: PedFletes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedFlete pedFlete = db.PedFletes.Find(id);
            if (pedFlete == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpTran = new SelectList(db.EmpTrans, "idEmpTran", "NomEmpTran", pedFlete.idEmpTran);
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", pedFlete.idPedido);
            return View(pedFlete);
        }

        // POST: PedFletes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedFlete,idPedido,idFlete,idEmpTran,Valor,Obervaciones")] PedFlete pedFlete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedFlete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpTran = new SelectList(db.EmpTrans, "idEmpTran", "NomEmpTran", pedFlete.idEmpTran);
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", pedFlete.idPedido);
            return View(pedFlete);
        }

        // GET: PedFletes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedFlete pedFlete = db.PedFletes.Find(id);
            if (pedFlete == null)
            {
                return HttpNotFound();
            }
            return View(pedFlete);
        }

        // POST: PedFletes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PedFlete pedFlete = db.PedFletes.Find(id);
            db.PedFletes.Remove(pedFlete);
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
