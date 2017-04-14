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
    public class TipoTelsController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: TipoTels
        public ActionResult Index()
        {
            return View(db.TipoTels.OrderBy(tt => tt.NomTipoTel).ToList());
        }

        // GET: TipoTels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTel tipoTel = db.TipoTels.Find(id);
            if (tipoTel == null)
            {
                return HttpNotFound();
            }
            return View(tipoTel);
        }

        // GET: TipoTels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoTels/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoTel,NomTipoTel")] TipoTel tipoTel)
        {
            if (ModelState.IsValid)
            {
                db.TipoTels.Add(tipoTel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoTel);
        }

        // GET: TipoTels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTel tipoTel = db.TipoTels.Find(id);
            if (tipoTel == null)
            {
                return HttpNotFound();
            }
            return View(tipoTel);
        }

        // POST: TipoTels/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoTel,NomTipoTel")] TipoTel tipoTel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoTel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoTel);
        }

        // GET: TipoTels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTel tipoTel = db.TipoTels.Find(id);
            if (tipoTel == null)
            {
                return HttpNotFound();
            }
            return View(tipoTel);
        }

        // POST: TipoTels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoTel tipoTel = db.TipoTels.Find(id);
            db.TipoTels.Remove(tipoTel);
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
