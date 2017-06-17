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
    public class ConsultasController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        //******************************************************
        //******************************************************
        //GET: Pedidos en un Periodo
        [Authorize(Roles = "Admin,Cliente,Vendedor")]
        public ActionResult modal()
        {
            var pedido = db.Pedidos
             .Include(p => p.Cliente)
             .Include(p => p.FormPago)
             .Where(p => p.idCliente < 0);

            return View(pedido.ToList());
        }
        //******************************************************
        //POST
        [HttpPost]
        public ActionResult modal(DateTime? FechIni, DateTime? FechFin)
        {
            var pedidos = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Cliente.Empleado)
                .Include(p => p.FormPago)
                .ToList();

            if (FechIni == null || FechFin == null)
            {
                ViewBag.ErrorPerido = "Error al determinar las Fechas del periodo";
                ModelState.AddModelError(string.Empty, "Error al determinar las Fechas del periodo");
                return View(pedidos);
            }
            // verifica si es un cliente
            var Contac = db.ContactoClies.Where(cc => cc.Email == User.Identity.Name).FirstOrDefault();

            if (Contac != null)
            {
                var cliente = db.Clientes
                .Where(c => c.idCliente == Contac.idCliente)
                .OrderBy(c => c.idCliente).FirstOrDefault();

                if (cliente != null)
                {
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.FechaPedido >= (FechIni) && p.FechaPedido <= FechFin)
                        .Where(p => p.idCliente == cliente.idCliente)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
            }
            else
            // Verifica si es un empleado
            {
                var empleado = db.Empleados.Where(e => e.Email == User.Identity.Name).FirstOrDefault();
          //      var nn = empleado.idEmpleado;

                if (empleado != null)
                {
                    //var clientes = db.Clientes
                    //    .Where(cl => cl.idEmpleado == empleado.idEmpleado).FirstOrDefault();

                    var pedid0 = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.Cliente.idEmpleado == empleado.idEmpleado)
                        .ToList();
                    //.OrderBy(cl => cl.NomClie).ToList();

                    pedidos = pedid0
                        .Where(p => p.FechaPedido >= (FechIni) && p.FechaPedido <= FechFin)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
                else
                {
                    
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.FechaPedido >= (FechIni) && p.FechaPedido <= FechFin)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                   
                }

            }

            if (pedidos.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No hay pedidos en este periodo...");

                return View(pedidos.ToList());
            }

            return View(pedidos.ToList());
        }
        //******************************************************
        //******************************************************
        // GET: Estados
        public ActionResult Index()
        {
            var estados = new List<Estado>();

            return View(estados);
        }
        //******************************************************
        //Post
        [HttpPost]
        public ActionResult Index(int? IdPedido)
        {
            int sw = 0;

            var est = db.Estados
                .Where(e => e.idEstado < 1)
                .ToList();
            var estados = est;

            var pedidos = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Cliente.Empleado)
                .Include(p => p.FormPago)
                .ToList();

            if (IdPedido == null || string.IsNullOrEmpty(IdPedido.ToString()))
            {
                ViewBag.ErrorPerido = "Error al solicitar el número de pedido";
                ModelState.AddModelError(string.Empty, "Error al solicitar el número de pedido");
                return View(est);
            }
            // verifica si es un cliente
            var Contac = db.ContactoClies.Where(cc => cc.Email == User.Identity.Name).FirstOrDefault();
            if (Contac != null)
            {
                var cliente = db.Clientes
                    .Where(c => c.idCliente == Contac.idCliente)
                    .OrderBy(c => c.idCliente).FirstOrDefault();

                if (cliente != null)
                {
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.idPedido == IdPedido)
                        .Where(p => p.idCliente == cliente.idCliente)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
                sw = 1;

            }
            else
            // Verifica si es un empleado
            {
                var empleado = db.Empleados.Where(e => e.Email == User.Identity.Name).FirstOrDefault();
            //    var nn = empleado.idEmpleado;
                if (empleado != null)
                {
                    //var clientes = db.Clientes
                    //    .Where(cl => cl.idEmpleado == empleado.idEmpleado).FirstOrDefault();

                    var pedid0 = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.Cliente.idEmpleado == empleado.idEmpleado)
                        .ToList();

                    pedidos = pedid0
                        .Where(p => p.idPedido == IdPedido)
                        .OrderBy(p => p.idPedido)
                        .ToList();

                    if (pedidos.Count > 0)
                    {
                        sw = 1;
                    }
                }
                else
                {
                    //usuario Administrador

                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.idPedido == IdPedido)
                        .OrderBy(p => p.idPedido)
                        .ToList();

                    if (pedidos.Count > 0)
                    {
                        sw = 1;
                    }
                }
            }
            if (pedidos.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No pedido no encontrado...");
                return View(est.ToList());
            }

            if (sw == 1)
            {
                estados = new List<Estado>();
                estados = db.Estados.Where(e => e.idPedido == IdPedido).ToList();

                if (estados.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "No hay pedidos y/o estados para la solicitud");

                    return View(new List<Estado>());
                }
            }
            if (sw == 1)
            {
                return View(estados);
            }
            else
            {
                return View(est);
            }
        }
        

        //******************************************************
        //******************************************************
        // GET: Un Pedido busca por ID de pedido

        [Authorize(Roles = "Admin,Cliente,Vendedor")]
        public ActionResult UnPedido()
        {
            var pedido = db.Pedidos
               .Include(p => p.Cliente)
               .Include(p => p.FormPago);

            return View(pedido.ToList());

        }
        //******************************************************
        // POST
        [HttpPost]
        public ActionResult UnPedido(int? IdPedido)
        {
            var pedidos = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Cliente.Empleado)
                .Include(p => p.FormPago)
                .ToList();

            if (IdPedido == null || string.IsNullOrEmpty(IdPedido.ToString()))
            {
                ViewBag.ErrorPerido = "Error al solicitar el número de pedido";
                ModelState.AddModelError(string.Empty, "Error al solicitar el número de pedido");
                return View(pedidos);
            }
            // verifica si es un cliente
            var Contac = db.ContactoClies.Where(cc => cc.Email == User.Identity.Name).FirstOrDefault();
            if (Contac != null)
            {
                var cliente = db.Clientes
                    .Where(c => c.idCliente == Contac.idCliente)
                    .OrderBy(c => c.idCliente).FirstOrDefault();

                if (cliente != null)
                {
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.idPedido == IdPedido)
                        .Where(p => p.idCliente == cliente.idCliente)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
            }
            else
            // Verifica si es un empleado
            {
                var empleado = db.Empleados.Where(e => e.Email == User.Identity.Name).FirstOrDefault();
              //  var nn = empleado.idEmpleado;
                if (empleado != null)
                {
                    //var clientes = db.Clientes
                    //    .Where(cl => cl.idEmpleado == empleado.idEmpleado).FirstOrDefault();

                    var pedid0 = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.Cliente.idEmpleado == empleado.idEmpleado)
                        .ToList();

                    pedidos = pedid0
                        .Where(p => p.idPedido == IdPedido)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
                else
                {
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.idPedido == IdPedido)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
            }
            if (pedidos.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No pedido no encontrado...");
                return View(pedidos.ToList());
            }             
            return View(pedidos.ToList());
        }

        
        //******************************************************
        //******************************************************
  
            //GET: Pedidos en un Periodo
        [Authorize(Roles = "Admin,Cliente,Vendedor")]
        public ActionResult UnPeriodo()
        { 
             var pedido = db.Pedidos
             .Include(p => p.Cliente)
             .Include(p => p.FormPago)
             .Where(p => p.idCliente < 0);

            return View(pedido.ToList());
        }
    //******************************************************
    //POST
    [HttpPost]
    public ActionResult UnPeriodo(DateTime? FechIni, DateTime? FechFin)
    {
        var pedidos = db.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Cliente.Empleado)
            .Include(p => p.FormPago)
            .ToList();

        if (FechIni == null || FechFin == null)
        {
            ViewBag.ErrorPerido = "Error al determinar las Fechas del periodo";
            ModelState.AddModelError(string.Empty, "Error al determinar las Fechas del periodo");
            return View(pedidos);
        }
        // verifica si es un cliente
        var Contac = db.ContactoClies.Where(cc => cc.Email == User.Identity.Name).FirstOrDefault();

        if (Contac != null)
        {
            var cliente = db.Clientes
            .Where(c => c.idCliente == Contac.idCliente)
            .OrderBy(c => c.idCliente).FirstOrDefault();

            if (cliente != null)
            {
                pedidos = db.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.FormPago)
                    .Where(p => p.FechaPedido >= (FechIni) && p.FechaPedido <= FechFin)
                    .Where(p => p.idCliente == cliente.idCliente)
                    .OrderBy(p => p.idPedido)
                    .ToList();
            }
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

                var pedid0 = db.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.FormPago)
                    .Where(p => p.Cliente.idEmpleado == empleado.idEmpleado)
                    .ToList();
                //.OrderBy(cl => cl.NomClie).ToList();

                pedidos = pedid0
                    .Where(p => p.FechaPedido >= (FechIni) && p.FechaPedido <= FechFin)
                    .OrderBy(p => p.idPedido)
                    .ToList();
            }
            else
            {
                if (User.Identity.Name == "leonardop_leon@outlook.com")
                {

                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.FechaPedido >= (FechIni) && p.FechaPedido <= FechFin)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
            }

        }

        if (pedidos.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "No hay pedidos en este periodo...");

            return View(pedidos.ToList());
        }

        return View(pedidos.ToList());
    }


    [Authorize(Roles = "Admin,Cliente,Vendedor")]
        public ActionResult PXCliente()
        {
            var pedido = db.Pedidos
               .Include(p => p.Cliente)
               .Include(p => p.FormPago);

            return View(pedido.ToList());

        }
        //******************************************************
        // POST
        [HttpPost]
        public ActionResult PXCliente(int? ClienteID)
        {
            var pedidos = db.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Cliente.Empleado)
                .Include(p => p.FormPago)
                .ToList();

            if (ClienteID == null || string.IsNullOrEmpty(ClienteID.ToString()))
            {
                ViewBag.ErrorPerido = "Error al solicitar el número de pedido";
                ModelState.AddModelError(string.Empty, "Error al solicitar el número de pedido");
                return View(pedidos);
            }
            // verifica si es un cliente
            var Contac = db.ContactoClies.Where(cc => cc.Email == User.Identity.Name).FirstOrDefault();
            if (Contac != null)
            {
                var cliente = db.Clientes
                    .Where(c => c.idCliente == Contac.idCliente)
                    .OrderBy(c => c.idCliente).FirstOrDefault();

                if (cliente != null)
                {
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.idCliente == ClienteID)
                        .Where(p => p.idCliente == cliente.idCliente)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
            }
            else
            // Verifica si es un empleado
            {
                var empleado = db.Empleados.Where(e => e.Email == User.Identity.Name).FirstOrDefault();
     //           var nn = empleado.idEmpleado;
                if (empleado != null)
                {
                    //var clientes = db.Clientes
                    //    .Where(cl => cl.idEmpleado == empleado.idEmpleado).FirstOrDefault();

                    var pedid0 = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.Cliente.idEmpleado == empleado.idEmpleado)
                        .ToList();

                    pedidos = pedid0
                        .Where(p => p.idCliente == ClienteID)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
                else
                {
                    pedidos = db.Pedidos
                        .Include(p => p.Cliente)
                        .Include(p => p.FormPago)
                        .Where(p => p.idCliente == ClienteID)
                        .OrderBy(p => p.idPedido)
                        .ToList();
                }
            }
            if (pedidos.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "No pedido no encontrado...");
                return View(pedidos.ToList());
            }
            return View(pedidos.ToList());
        }







        //******************************************************
        //******************************************************
        // GET: PedidoEnP/Details/5
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
    }
}
