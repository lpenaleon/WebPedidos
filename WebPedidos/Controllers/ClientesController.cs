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
    public class ClientesController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: Clientes
        public ActionResult Index(int? page = null) //paginación
        { 
            page = (page ?? 1);

            var clientes = db.Clientes.OrderBy(cl => cl.NomClie).Include(c => c.Empleados).Include(c => c.Municipios);
            return View(clientes.ToPagedList((int)page, 8)); //arreglo de paginación cambio de .ToList()) por ToPagedList((int)page, 8)); 
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre");
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio");
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCliente,ideNIT,NomClie,idMunicipio,Direccion,idEmpleado,Estado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre", cliente.idEmpleado);
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio", cliente.idMunicipio);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre", cliente.idEmpleado);
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio", cliente.idMunicipio);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCliente,ideNIT,NomClie,idMunicipio,Direccion,idEmpleado,Estado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre", cliente.idEmpleado);
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio", cliente.idMunicipio);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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
