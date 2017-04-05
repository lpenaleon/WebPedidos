using PagedList;
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
    public class MaquinasController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: Maquinas
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            return View(db.Maquinas.OrderBy(mq => mq.NomMaquina).ToPagedList((int)page, 7));
        }

        // GET: Maquinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquina maquina = db.Maquinas.Find(id);
            if (maquina == null)
            {
                return HttpNotFound();
            }
            return View(maquina);
        }

        // GET: Maquinas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maquinas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMaquina,NomMaquina")] Maquina maquina)
        {
            if (ModelState.IsValid)
            {
                db.Maquinas.Add(maquina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maquina);
        }

        // GET: Maquinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquina maquina = db.Maquinas.Find(id);
            if (maquina == null)
            {
                return HttpNotFound();
            }
            return View(maquina);
        }

        // POST: Maquinas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMaquina,NomMaquina")] Maquina maquina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maquina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maquina);
        }

        // GET: Maquinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquina maquina = db.Maquinas.Find(id);
            if (maquina == null)
            {
                return HttpNotFound();
            }
            return View(maquina);
        }

        // POST: Maquinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maquina maquina = db.Maquinas.Find(id);
            db.Maquinas.Remove(maquina);
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
