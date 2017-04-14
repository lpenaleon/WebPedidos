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
    public class ContactoCliesController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: ContactoClies
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var contClies = db.ContactoClies.OrderBy(CC => CC.NomContacto).Include(cl => cl.Cliente);
            return View(contClies.ToPagedList((int)page, 8));
        }

        // GET: ContactoClies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactoClie contactoClie = db.ContactoClies.Find(id);
            if (contactoClie == null)
            {
                return HttpNotFound();
            }
            return View(contactoClie);
        }

        // GET: ContactoClies/Create
        public ActionResult Create()
        {
            ViewBag.idCliente = new SelectList(db.Clientes.OrderBy(cl =>cl.NomClie),"idCliente", "NomClie");
            return View();
        }

        // POST: ContactoClies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContactoClie,NomContacto,Email,idCliente")] ContactoClie contactoClie)
        {
            if (ModelState.IsValid)
            {
                db.ContactoClies.Add(contactoClie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NomClie", contactoClie.idCliente);
            return View(contactoClie);
        }

        // GET: ContactoClies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactoClie contactoClie = db.ContactoClies.Find(id);
            if (contactoClie == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "ideNIT", contactoClie.idCliente);
            return View(contactoClie);
        }

        // POST: ContactoClies/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idContactoClie,idCliente,NomContacto,Email")] ContactoClie contactoClie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactoClie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "ideNIT", contactoClie.idCliente);
            return View(contactoClie);
        }

        // GET: ContactoClies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactoClie contactoClie = db.ContactoClies.Find(id);
            if (contactoClie == null)
            {
                return HttpNotFound();
            }
            return View(contactoClie);
        }

        // POST: ContactoClies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactoClie contactoClie = db.ContactoClies.Find(id);
            db.ContactoClies.Remove(contactoClie);
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
