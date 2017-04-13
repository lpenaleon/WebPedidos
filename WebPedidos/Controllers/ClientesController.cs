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
        public ActionResult Index()
        {
          //  View(db.Departamentos.OrderBy(d => d.NomDepto).ToPagedList((int)page, 8));
            var clientes = db.Clientes.OrderBy(cl =>cl.NomClie).
                Include(em => em.Empleados).
                Include(mu => mu.Municipios).
                Include(ti => ti.TipoIdes);

            //   Include("TipoIdes").GroupBy(ti => ti.TipoIdes);

            return View(clientes.ToList());
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
            ViewBag.idTipoIde = new SelectList(db.TipoIdes, "idTipoIde", "NomTipoIde");
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCliente,idTipoIde,Numide,NomClie,idMunicipio,Direccion,Estado,idEmpleado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre", cliente.idEmpleado);
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio", cliente.idMunicipio);
            ViewBag.idTipoIde = new SelectList(db.TipoIdes, "idTipoIde", "NomTipoIde", cliente.idTipoIde);
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
            ViewBag.idTipoIde = new SelectList(db.TipoIdes, "idTipoIde", "NomTipoIde", cliente.idTipoIde);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCliente,idTipoIde,Numide,NomClie,idMunicipio,Direccion,Estado,idEmpleado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre", cliente.idEmpleado);
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio", cliente.idMunicipio);
            ViewBag.idTipoIde = new SelectList(db.TipoIdes, "idTipoIde", "NomTipoIde", cliente.idTipoIde);
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
