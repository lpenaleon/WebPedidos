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
    public class EmpTransController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        [Authorize(Roles = "ViewETrans")]
        // GET: EmpTrans
        public ActionResult Index()
        {
            return View(db.EmpTrans.OrderBy(et => et.NomEmpTran).ToList());
        }
        [Authorize(Roles = "View")]
        // GET: EmpTrans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpTran empTran = db.EmpTrans.Find(id);
            if (empTran == null)
            {
                return HttpNotFound();
            }
            return View(empTran);
        }

        [Authorize(Roles = "Create")]
        // GET: EmpTrans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpTrans/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmpTran,NomEmpTran")] EmpTran empTran)
        {
            if (ModelState.IsValid)
            {
                db.EmpTrans.Add(empTran);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empTran);
        }

        [Authorize(Roles = "Edit")]
        // GET: EmpTrans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpTran empTran = db.EmpTrans.Find(id);
            if (empTran == null)
            {
                return HttpNotFound();
            }
            return View(empTran);
        }

        // POST: EmpTrans/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmpTran,NomEmpTran")] EmpTran empTran)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empTran).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empTran);
        }

        [Authorize(Roles = "Delete")]
        // GET: EmpTrans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpTran empTran = db.EmpTrans.Find(id);
            if (empTran == null)
            {
                return HttpNotFound();
            }
            return View(empTran);
        }

        // POST: EmpTrans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmpTran empTran = db.EmpTrans.Find(id);
            db.EmpTrans.Remove(empTran);
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
