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
    public class FormPagosController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        [Authorize(Roles = "ViewETrans")]
        // GET: FormPagos
        public ActionResult Index()
        {
            return View(db.FormPagos.ToList());
        }

        // GET: FormPagos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPago formPago = db.FormPagos.Find(id);
            if (formPago == null)
            {
                return HttpNotFound();
            }
            return View(formPago);
        }

        // GET: FormPagos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormPagos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idFormPago,NomFPago")] FormPago formPago)
        {
            if (ModelState.IsValid)
            {
                db.FormPagos.Add(formPago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formPago);
        }

        // GET: FormPagos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPago formPago = db.FormPagos.Find(id);
            if (formPago == null)
            {
                return HttpNotFound();
            }
            return View(formPago);
        }

        // POST: FormPagos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idFormPago,NomFPago")] FormPago formPago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formPago);
        }

        // GET: FormPagos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormPago formPago = db.FormPagos.Find(id);
            if (formPago == null)
            {
                return HttpNotFound();
            }
            return View(formPago);
        }

        // POST: FormPagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormPago formPago = db.FormPagos.Find(id);
            db.FormPagos.Remove(formPago);
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
