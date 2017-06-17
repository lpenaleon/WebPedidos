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
        //********************************************************************
        //********************************************************************
        int IDClie = 0;
        int IDCont = 0;


        [Authorize(Roles = "Admin,Vendedor,Cliente")]
        // GET: Clientes
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);

            var empleado = db.Empleados.Where(e => e.Email == User.Identity.Name).FirstOrDefault();

            var clientes = db.Clientes.OrderBy(cl => cl.NomClie)
                .Include(em => em.Empleado)
                .Include(mu => mu.Municipio)
                .Include(ti => ti.TipoIde)
                .OrderBy(cl => cl.NomClie);

            if (empleado != null)
            {
                clientes = clientes
                .Where(c => c.idEmpleado == empleado.idEmpleado)
                .OrderBy(cl => cl.NomClie);
            }

            return View(clientes.ToPagedList((int)page, 8));

        }
        //********************************************************************
        //POST: Clientes
        [HttpPost]
        public ActionResult Index(string BuscadorC, int? page = null)
        {
            page = (page ?? 1);

            var cliente = db.Clientes
                .Where(c => c.idCliente < 1);

            if (BuscadorC == null || string.IsNullOrEmpty(BuscadorC.ToString()))
            {
                ViewBag.ErrorPerido = "Error al solicitar el número de pedido";
                ModelState.AddModelError(string.Empty, "Error al solicitar el número de pedido");
                return View(cliente);
            }
            // verifica si es un cliente
            var Contac = db.ContactoClies.Where(cc => cc.Email == User.Identity.Name).FirstOrDefault();
            if (Contac != null)
            {
                cliente = db.Clientes
                    .Where(cl => cl.idCliente == Contac.idCliente)
                    .Where(cl => cl.NomClie.Contains(BuscadorC) || cl.Direccion.Contains(BuscadorC))
                    .OrderBy(cl => cl.NomClie);

            }
            else
            // Verifica si es un empleado
            {
                var empleado = db.Empleados.Where(e => e.Email == User.Identity.Name).FirstOrDefault();

           //     var nn = empleado.idEmpleado;
                if (empleado != null)
                {
                    //var clientes = db.Clientes
                    //    .Where(cl => cl.idEmpleado == empleado.idEmpleado).FirstOrDefault();

                    cliente = db.Clientes
                        .Where(cl => cl.idEmpleado == empleado.idEmpleado)
                        .Where(cl => cl.NomClie.Contains(BuscadorC) || cl.Direccion.Contains(BuscadorC))
                        .OrderBy(cl => cl.NomClie);
                }
                else
                {
                    //Admin
                    cliente = db.Clientes
                        .Where(cl => cl.NomClie.Contains(BuscadorC) || cl.Direccion.Contains(BuscadorC))
                        .OrderBy(cl => cl.NomClie);



                    //c.(Convert.ToString(c.idCliente)).Contains(BuscadorC) ||
                }
            }

            var nreg = cliente.Count();

            if (cliente.Count() == 0)   
            {
                nreg = 1;
                ModelState.AddModelError(string.Empty, "No pedido no encontrado...");
                return View(cliente.ToList());
            }

            return View(cliente.ToPagedList((int)page, nreg));
        }



        //********************************************************************
        //********************************************************************

        [Authorize(Roles = "View")]
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
        [Authorize(Roles = "Admin,Vendedor")]
        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleado = new SelectList(db.Empleados, "idEmpleado", "Nombre");
            ViewBag.idMunicipio = new SelectList(db.Municipios, "idMunicipio", "NomMunicipio");
            //Include("TipoIdes").GroupBy(ti => ti.TipoIdes);

            var list = db.TipoIdes.ToList();
            list.Add(new TipoIde { idTipoIde = 1, NomTipoIde ="[Selecione un tipo de Documento]" });
            list = list.OrderBy(t => t.NomTipoIde).ToList(); 
            ViewBag.idTipoIde = new SelectList(list, "idTipoIde", "NomTipoIde");

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

        [Authorize(Roles = "Admin,Vendedor")]
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
            var tipIdes = db.TipoIdes
                .Where(ti => ti.NomTipoIde != "")
                .OrderBy(ti => ti.idTipoIde); 
            ViewBag.idTipoIde = new SelectList(tipIdes, "idTipoIde", "NomTipoIde", cliente.idTipoIde);
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

        [Authorize(Roles = "Admin")]
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
        // ********************************************************
        // ** Contacto ********************************************
        // ********************************************************


        public ActionResult IndexCont(int id, int? page = null)
        {
            IDClie = id;
            page = (page ?? 1);

            var contClies = db.ContactoClies
                .Include(cl => cl.Cliente)
                .Where(cl => cl.idCliente == id)
                .OrderBy(CC => CC.NomContacto);

            
            return View(contClies.ToPagedList((int)page, 8));

        }

        [Authorize(Roles = "Admin,Vendedor")]
        // GET: ContactoClies/Create
        public ActionResult CreateCont(int? id,int IDClie)
        {
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde");
            return View();
        }

        // POST: ContactoClies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCont([Bind(Include = "idContactoClie,NomContacto,Email,idCliente")] ContactoClie contactoClie)
        {
            if (ModelState.IsValid)
            {
                db.ContactoClies.Add(contactoClie);
                db.SaveChanges();
                return RedirectToAction("IndexCont");
            }

            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", contactoClie.idCliente);
            return View(contactoClie);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        // GET: ContactoClies/Edit/5
        public ActionResult EditCont(int? id)
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
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", contactoClie.idCliente);
            return View(contactoClie);
        }

        // POST: ContactoClies/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCont([Bind(Include = "idContactoClie,NomContacto,Email,idCliente")] ContactoClie contactoClie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactoClie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", contactoClie.idCliente);
            return View(contactoClie);
        }

        [Authorize(Roles = "Admin")]
        // GET: ContactoClies/Delete/5
        public ActionResult DeleteCont(int? id)
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
        public ActionResult DeleteCont(int id)
        {
            ContactoClie contactoClie = db.ContactoClies.Find(id);
            db.ContactoClies.Remove(contactoClie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //*********************************************************
        //***** TELEFONOS *****************************************
        //*********************************************************

        
        public ActionResult IndexTel(int id, int? page = null)
        {
            IDCont = id;

            page = (page ?? 1);
            var telefonos = db.Telefonos
                .Include(tt => tt.TipoTels)
                .Where(tl => tl.idContactoClie == IDCont)
                .OrderBy(te => te.IdTelefono);
            return View(telefonos.ToPagedList((int)page, 9)); 
        }

        [Authorize(Roles = "Admin,Vendedor")]
        // GET: Telefonos/Details/5
        public ActionResult DetailsTel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefono telefono = db.Telefonos.Find(id);
            if (telefono == null)
            {
                return HttpNotFound();
            }
            return View(telefono);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        // GET: Telefonos/Create
        public ActionResult CreateTel()
        {

            var tel = new Telefono
            {
                idContactoClie =IDCont,
                idTipoTel = 1,
                NumTel = ""

            };
            db.Telefonos.Add(tel);
            db.SaveChanges();

            var teleID = db.Telefonos.ToList().Select(pd => pd.IdTelefono).Max();

            var telefonos = db.Telefonos
                .Include(tt => tt.TipoTels)
                .Where(tl => tl.IdTelefono == teleID)                
                .OrderBy(te => te.IdTelefono);

//            return View(telefonos.ToPagedList((int)page, 9));

            ViewBag.idTipoTel = new SelectList(db.TipoTels, "idTipoTel", "NomTipoTel");
            return View();
        }

        // POST: Telefonos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTel([Bind(Include = "IdTelefono,idContactoCli,idTipoTel,NumTel")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                db.Telefonos.Add(telefono);
                db.SaveChanges();
                return RedirectToAction("IndexTel");
            }

            ViewBag.idTipoTel = new SelectList(db.TipoTels, "idTipoTel", "NomTipoTel", telefono.idTipoTel);
            return View(telefono);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        // GET: Telefonos/Edit/5
        public ActionResult EditTel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefono telefono = db.Telefonos.Find(id);
            if (telefono == null)
            {
                return HttpNotFound();
            }
            ViewBag.idTipoTel = new SelectList(db.TipoTels, "idTipoTel", "NomTipoTel", telefono.idTipoTel);
            return View(telefono);
        }

        // POST: Telefonos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTel([Bind(Include = "IdTelefono,idContactoCli,idTipoTel,NumTel")] Telefono telefono)
        {
            if (ModelState.IsValid)
            {
                db.Entry(telefono).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexTel");
            }
            ViewBag.idTipoTel = new SelectList(db.TipoTels, "idTipoTel", "NomTipoTel", telefono.idTipoTel);
            return View(telefono);
        }

        [Authorize(Roles = "Admin")]
        // GET: Telefonos/Delete/5
        public ActionResult DeleteTel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefono telefono = db.Telefonos.Find(id);
            if (telefono == null)
            {
                return HttpNotFound();
            }
            return View(telefono);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTel(int id)
        {
            Telefono telefono = db.Telefonos.Find(id);
            db.Telefonos.Remove(telefono);
            db.SaveChanges();
            return RedirectToAction("IndexTel");
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
