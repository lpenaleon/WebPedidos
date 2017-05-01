using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPedidos.Models;
using WebPedidos.ViewModels;

namespace WebPedidos.Controllers
{
    public class PedidosController : Controller
    {
        WebPedidosContext db = new WebPedidosContext();
        //GET
        public ActionResult NewPedido()
        {

            //var orderView        = new OrderView();
            //orderView.Cliente    = new Cliente();
            //orderView.FormPagos = new FormPago();
            //orderView.Productos  = new List<ProductOrder>();
            //Session["orderView"] = orderView;

            //var listacl = db.Clientes.ToList();
            //listacl.Add(new Cliente { idCliente = 0, NomClie = "[Selecciones un Cliente... ]" });
            //listacl = listacl.OrderBy(cl => cl.NomClieConca).ToList();
            //ViewBag.idCliente = new SelectList(listacl, "idCliente", "NomClieConca");

            //var listafp = db.FormPagos;
            //listafp.Add(new FormPago { idFormPago = 0, NomFPago = "[Selecciones una Forma de Pago... ]" });
            //ViewBag.idFormPago = new SelectList(listafp.OrderBy(fp => fp.NomFPago).ToList(), "idFormPago", "NomFPago");

            var formapagos = new List<FormPago>();

            foreach (var item in db.FormPagos)
            {
                var formapago = new FormPago
                {
                    NomFPago = item.NomFPago,
                    Pedidos = item.Pedidos,
                };
                formapagos.Add(formapago);
            }
            var clientes = new List<Cliente>();
            foreach (var itemC in db.Clientes)
            {
                var cliente = new Cliente
                {
                    idTipoIde = itemC.idTipoIde,
                    NumIde = itemC.NumIde,
                    NomClie = itemC.NomClie,
                    idMunicipio = itemC.idMunicipio,
                    Direccion = itemC.Direccion,
                    Estado = itemC.Estado,
                    idEmpleado = itemC.idEmpleado
                };
                clientes.Add(cliente);
            }

        var productos = new List<ProductOrder>();


            var orderView = new OrderView()
            {
                Cliente = clientes,
                FormPagos = formapagos,
                Productos = productos
            };
            Session["orderView"] = orderView;

            ViewBag.idCliente = new SelectList(db.Clientes
                .OrderBy(c => c.NomClie).ToList(), "idCliente", "NomClieConca");

            ViewBag.idFormPago = new SelectList(db.FormPagos
                .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

            

            return View(orderView);
        }
        // POST
        [HttpPost]
        public ActionResult NewPedido( OrderView orderView)
        {
            orderView = Session["orderView"] as OrderView;

            // Validaciones

            if (orderView.Productos.Count == 0)
            {
                var listacl = db.Clientes.ToList();
                listacl = listacl.OrderBy(cl => cl.NomClieConca).ToList();
                listacl.Add(new Cliente { idCliente = 0, NomClie = "[Seleccione un Cliente... ]" });
                ViewBag.idCliente = new SelectList(listacl, "idCliente", "NomClieConca");
                ViewBag.Error = "Debe ingresar el Detalle";

                ViewBag.idFormPago = new SelectList(db.FormPagos
                .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

                return View(orderView);
            }

            var ClienteID = int.Parse(Request["idCliente"]);
            if (ClienteID == 0)
            {
                var listacl = db.Clientes.ToList();
                listacl = listacl.OrderBy(cl => cl.NomClieConca).ToList();
                listacl.Add(new Cliente { idCliente = 0, NomClie = "[Selecciones un Cliente... ]" });
                ViewBag.idCliente = new SelectList(listacl, "idCliente", "NomClieConca");
                ViewBag.Error = "Debe seleccionar un Cliente";

                return View(orderView);
            }

            var client = db.Clientes.Find(ClienteID);
            if (client == null)
            {
                var listacl = db.Clientes.ToList();                
                listacl = listacl.OrderBy(cl => cl.NomClieConca).ToList();
                listacl.Add(new Cliente { idCliente = 0, NomClie = "[Selecciones un Cliente... ]" });
                ViewBag.idCliente = new SelectList(listacl, "idCliente", "NomClieConca");
                ViewBag.Error = "El Cliente no Existe";

                return View(orderView);
            }

            var FPagoID = byte.Parse(Request["idFormPago"]);
            if (FPagoID == 0)
            {
                ViewBag.idFormPago = new SelectList(db.FormPagos
                .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

                ViewBag.Error = "Debe seleccionar una Forma de Pago";

                return View(orderView);
            }


            //
            var nDiasCre = byte.Parse(Request["DiasCred"]);

            if (FPagoID == 1)
            {
                if (nDiasCre == 0)
                {
                    ViewBag.idFormPago = new SelectList(db.FormPagos
                    .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

                    ViewBag.Error = "Los días de Crédito debe ser mayor que cero (0)";

                    return View(orderView);
                }
            }
            else
            {
                nDiasCre = 0;
            }


            var FPago = db.FormPagos.Find(FPagoID);
            if (FPago == null)
            {
                ViewBag.idFormPago = new SelectList(db.FormPagos
                .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

                ViewBag.Error = "La Forma de Pago no Existe";

                return View(orderView);
            }
            // hacer una transacción
            long pedidoID = 0;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var pedido = new Pedido
                    {
                        idCliente = ClienteID,
                        idFormPago = FPagoID,
                        FechaPedido = DateTime.Now,
                        OrdenEstado = OrdenEstado.Creada,
                        DiasCred = nDiasCre
                    };
                    db.Pedidos.Add(pedido);
                    db.SaveChanges();

                    pedidoID = db.Pedidos.ToList().Select(pd => pd.idPedido).Max();
                    foreach (var item in orderView.Productos)
                    {
                        var pedidoDet = new PedidoDet
                        {
                            idPedido = pedidoID,
                            idProducto = item.idProducto,
                            Descripcion = item.Descripcion,
                            Cantidad = item.Cantidad,
                            iva = item.iva,
                            Precio = item.Precio
                        };
                        db.PedidoDets.Add(pedidoDet);
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error: " + ex.Message;

                    ViewBag.idCliente = new SelectList(db.Clientes
                        .OrderBy(c => c.NomClie).ToList(), "idCliente", "NomClieConca");

                    ViewBag.idFormPago = new SelectList(db.FormPagos
                        .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

                    return View(orderView); 
                }
            }
            // Fin de la transacción

            ViewBag.Message = string.Format("El Pedido: {0}, se ha grabado", pedidoID);

            // Limpia la pantalla
            var formapagos = new List<FormPago>();

            foreach (var item in db.FormPagos)
            {
                var formapago = new FormPago
                {
                    NomFPago = item.NomFPago,
                    Pedidos = item.Pedidos,
                };
                formapagos.Add(formapago);
            }
            var clientes = new List<Cliente>();
            foreach (var itemC in db.Clientes)
            {
                var cliente = new Cliente
                {
                    idTipoIde = itemC.idTipoIde,
                    NumIde = itemC.NumIde,
                    NomClie = itemC.NomClie,
                    idMunicipio = itemC.idMunicipio,
                    Direccion = itemC.Direccion,
                    Estado = itemC.Estado,
                    idEmpleado = itemC.idEmpleado
                };
                clientes.Add(cliente);
            }

            var productos = new List<ProductOrder>();

            orderView = new OrderView()
            {
                Cliente = clientes,
                FormPagos = formapagos,
                Productos = productos
            };
            Session["orderView"] = orderView;

            ViewBag.idCliente = new SelectList(db.Clientes
                .OrderBy(c => c.NomClie).ToList(), "idCliente", "NomClieConca");

            ViewBag.idFormPago = new SelectList(db.FormPagos
                .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

            return View(orderView);
        }
        public ActionResult Add_Producto()
        {
            var listap = db.Productos.ToList();
            listap.Add(new ProductOrder { idProducto = 0, Descripcion = "[Selecione un Producto... ]" });
            listap = listap.OrderBy(p => p.Descripcion).ToList();
            ViewBag.idProducto = new SelectList(listap, "idProducto", "Descripcion");

            return View();
        }

        [HttpPost]
        public ActionResult Add_Producto(ProductOrder productOrder)
        {
            var orderView = Session["orderView"] as OrderView;

            var productoID = int.Parse(Request["idProducto"]);

            if  (productoID == 0)
            {
                var listap = db.Productos.ToList();
                listap.Add(new ProductOrder { idProducto = 0, Descripcion = "[Selecione un Producto... ]" });
                listap = listap.OrderBy(p => p.Descripcion).ToList();
                ViewBag.idProducto = new SelectList(listap, "idProducto", "Descripcion");
                ViewBag.Error = "Debe seleccionar un Producto";

                return View(productOrder);
            }

            var product = db.Productos.Find(productoID);
            if (product == null)
            {
                var listap = db.Productos.ToList();
                listap.Add(new ProductOrder { idProducto = 0, Descripcion = "[Selecione un Producto... ]" });
                listap = listap.OrderBy(p => p.Descripcion).ToList();
                ViewBag.idProducto = new SelectList(listap, "idProducto", "Descripcion");
                ViewBag.Error = "El Producto no Existe";

                return View(productOrder);
            }

            productOrder = orderView.Productos.Find(p => p.idProducto == productoID);
            if (productOrder == null)
            {
                productOrder = new ProductOrder
                {
                    idProducto  = product.idProducto,
                    Descripcion = product.Descripcion,
                    iva         = product.iva,
                    Precio      = product.Precio,
                    Cantidad    = float.Parse(Request["Cantidad"])
                    //RutaFoto = product.RutaFoto                
                };
                orderView.Productos.Add(productOrder);
            }
            else
            {
                productOrder.Cantidad += float.Parse(Request["Cantidad"]);
            }

            var ncant = orderView.Productos.Count;
            if (ncant == 1)
            { }
            else
                if (ncant > 1)
            { }


            //decimal TotIVA = 0;
            //decimal TotPrecio = 0;

            //foreach (var items in orderView.Productos)
            //{
            //    TotIVA = items.ValorIVA + TotIVA;
            //    TotPrecio = items.Precio + TotPrecio;
            //}

            var listacl = db.Clientes.ToList();
            listacl = listacl.OrderBy(cl => cl.NomClieConca).ToList();
            listacl.Add(new Cliente { idCliente = 0, NomClie = "[Seleccione un Cliente... ]" });
            ViewBag.idCliente = new SelectList(listacl, "idCliente", "NomClieConca");

            ViewBag.idFormPago = new SelectList(db.FormPagos
            .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

            return View("NewPedido",orderView);
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