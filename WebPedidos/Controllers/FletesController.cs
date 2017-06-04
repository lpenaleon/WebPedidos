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
    public class FletesController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: Fletes
        public ActionResult Index()
        {
            return View(db.Fletes.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Fletes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flete flete = db.Fletes.Find(id);
            if (flete == null)
            {
                return HttpNotFound();
            }
            return View(flete);
        }

        // GET: Fletes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fletes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFlete,NomFlete")] Flete flete)
        {
            if (ModelState.IsValid)
            {
                db.Fletes.Add(flete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flete);
        }

        // GET: Fletes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flete flete = db.Fletes.Find(id);
            if (flete == null)
            {
                return HttpNotFound();
            }
            return View(flete);
        }

        // POST: Fletes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFlete,NomFlete")] Flete flete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flete);
        }

        // GET: Fletes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flete flete = db.Fletes.Find(id);
            if (flete == null)
            {
                return HttpNotFound();
            }
            return View(flete);
        }

        // POST: Fletes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flete flete = db.Fletes.Find(id);
            db.Fletes.Remove(flete);
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
