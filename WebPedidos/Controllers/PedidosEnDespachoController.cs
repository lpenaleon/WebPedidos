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
    public class PedidosEnDespachoController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        // GET: PedidosEnDespacho
        [Authorize(Roles = "Despachos")]
        public ActionResult Index()
        {
            var pedido1 = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.FormPago);

            var pedidos = pedido1
                .Where(p => p.OrdenEstado == OrdenEstado.En_Despacho)
                .OrderBy(p => p.idPedido);

            return View(pedidos.ToList());
        }

        // GET: PedidoEnP/Details/5
        [Authorize(Roles = "Despachos")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }

            var pedDet0 = db.PedidoDets
                .Include(pd => pd.Producto)
                .OrderBy(pd => pd.idPedidoDet);

            var pedDet = pedDet0
                .Where(p => p.idPedido == id);

            decimal TotPre = 0;

            foreach (var items in pedDet)
            {
                TotPre = items.PrecioT + TotPre;
            }

            ViewBag.Tot_Pre = string.Format("{0:C2}", TotPre);

            return View(pedDet.ToList());
        }

        // GET: PedidosEnDespacho/Edit/5
        [Authorize(Roles = "Despachos")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        // POST: PedidosEnDespacho/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedido,FechaPedido,DiasCred,idCliente,idFormPago,OrdenEstado")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();

                var estado = new Estado
                {
                    idPedido = pedido.idPedido,
                    FechaEstado = DateTime.Now,
                    OrdenEstado = pedido.OrdenEstado,
                    Nota = ""
                };
                db.Estados.Add(estado);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "NumIde", pedido.idCliente);
            ViewBag.idFormPago = new SelectList(db.FormPagos, "idFormPago", "NomFPago", pedido.idFormPago);
            return View(pedido);
        }

        //**********************************************************************
        //**********************************************************************
        //**********************************************************************

        // GET: PedidoEnP/Details/5
        [Authorize(Roles = "Despachos")]
        public ActionResult IndexEntPed(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }

            var pedDet0 = db.PedidoDets
                .Include(pd => pd.Producto)
                .OrderBy(pd => pd.idPedidoDet);

            var pedFletes = db.PedFletes
                .Include(p => p.Flete)
                .Include(p => p.EmpTran)
                .Include(p => p.Pedido)
                .Where(p => p.idPedido == id);

            return View(pedFletes.ToList());   
        }

        // GET: PedFletes/Details/5
        [Authorize(Roles = "Despachos")]
        public ActionResult DetailsEntPed(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //PedFlete pedFlete = db.PedFletes.Find(id);

            var pedFlete = db.PedFletes
                .Include(p => p.Flete)
                .Include(p => p.EmpTran)
                .Include(p => p.Pedido)
                .Where(p => p.idPedido == id);

          //  PedFlete pedFlete = pedFletes.ToList();

            if (pedFlete == null)
            {
                return HttpNotFound();
            }
            return View(pedFlete);
        }

        // GET: PedFletes/Create
        [Authorize(Roles = "Despachos")]
        public ActionResult CreateEntPed()
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
        public ActionResult CreateEnt([Bind(Include = "idPedFlete,idPedido,idFlete,idEmpTran,Valor,Obervaciones")] PedFlete pedFlete)
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
        [Authorize(Roles = "Despachos")]
        public ActionResult EditEntPed(long? id)
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
            ViewBag.idFlete = new SelectList(db.Fletes, "idFlete", "NomFlete", pedFlete.idFlete);
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", pedFlete.idPedido);
            return View(pedFlete);
        }

        // POST: PedFletes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEntPed([Bind(Include = "idPedFlete,idPedido,idFlete,idEmpTran,Valor,Obervaciones")] PedFlete pedFlete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedFlete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexEndPed", pedFlete);
            }
            ViewBag.idEmpTran = new SelectList(db.EmpTrans, "idEmpTran", "NomEmpTran", pedFlete.idEmpTran);
            ViewBag.idFlete = new SelectList(db.Fletes, "idFlete", "NomFlete", pedFlete.idFlete);
            ViewBag.idPedido = new SelectList(db.Pedidos, "idPedido", "idPedido", pedFlete.idPedido);
            return View(pedFlete);

  

            
        }

        // GET: PedFletes/Delete/5
        [Authorize(Roles = "Despachos")]
        public ActionResult DeleteEntPed(long? id)
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
        public ActionResult DeleteEntPed(long id)
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
