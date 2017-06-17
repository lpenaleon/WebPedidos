using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPedidos.Models;
using WebPedidos.ViewModels;
using PagedList;
using System.Data.Entity;
using System.Net;
using WebPedidos.Help;
using System.Data;


namespace WebPedidos.Controllers
{
    public class PedidosController : Controller
    {
        WebPedidosContext db = new WebPedidosContext();

        //GET
        [Authorize(Roles = "Admin,Cliente,Vendedor")]
        public ActionResult NewPedido()
        {
            var formapagos = new List<FormPago>();

            foreach (var item in db.FormPagos.ToList())
            {
                var formapago = new FormPago
                {
                    NomFPago = item.NomFPago,
                    Pedidos = item.Pedidos,
                };
                formapagos.Add(formapago);
            }

            var clientes = new List<Cliente>();

            foreach (var itemC in db.Clientes.ToList())
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


            vistaCliente();
            vistaFormPago();
            //ViewBag.idCliente = new SelectList(db.Clientes
            //    .OrderBy(c => c.NomClie).ToList(), "idCliente", "NomClieConca");

            //ViewBag.idFormPago = new SelectList(db.FormPagos
            //    .OrderBy(f => f.NomFPago).ToList(), "idFormPago", "NomFPago");

            return View(orderView);
        }
        // POST
        [HttpPost]
        public ActionResult NewPedido(OrderView orderView)
        {
            var orderView1 = Session["orderView"] as OrderView;

            // Validaciones
            if (orderView1.Productos.Count == 0)
            {

                vistaCliente();
                ViewBag.Error = "Debe ingresar el Detalle";
                vistaFormPago();
                return View(orderView1);
            }

            var ClienteID = int.Parse(Request["idCliente"]);
            if (ClienteID == 0)
            {
                vistaCliente();
                ViewBag.Error = "Debe seleccionar un Cliente";

                return View(orderView1);
            }

            var client = db.Clientes.Find(ClienteID);
            if (client == null)
            {
                vistaCliente();
                ViewBag.Error = "El Cliente no Existe";
                return View(orderView1);
            }

            var FPagoID = byte.Parse(Request["idFormPago"]);
            if (FPagoID == 0)
            {
                vistaFormPago();
                ViewBag.Error = "Debe seleccionar una Forma de Pago";
                return View(orderView1);
            }
            //
            var nDiasCre = byte.Parse(Request["DiasCred"]);

            if (FPagoID == 1)
            {
                if (nDiasCre == 0)
                {
                    vistaFormPago();
                    vistaCliente();
                    ViewBag.Error = "Los días de Crédito debe ser mayor que cero (0)";
                    return View(orderView1);
                }
            }
            else
            {
                nDiasCre = 0;
            }

            var FPago = db.FormPagos.Find(FPagoID);
            if (FPago == null)
            {
                vistaFormPago();
                ViewBag.Error = "La Forma de Pago no Existe";

                return View(orderView1);
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
                        OrdenEstado = OrdenEstado.Creado,
                        DiasCred = nDiasCre
                    };
                    db.Pedidos.Add(pedido);
                    db.SaveChanges();

                    pedidoID = db.Pedidos.ToList().Select(pd => pd.idPedido).Max();
                    foreach (var item in orderView1.Productos)
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

                    var pedidoFl = new PedFlete
                    {
                        idPedido = pedidoID,
                        idFlete = 2,
                        idEmpTran = 1,
                        Valor = 0,
                        Obervaciones = ""

                    };
                    db.PedFletes.Add(pedidoFl);
                    db.SaveChanges();

                    var estado = new Estado
                    {
                        idPedido = pedidoID,
                        FechaEstado = DateTime.Now,
                        OrdenEstado = OrdenEstado.Creado,
                        Nota = ""
                    };
                    db.Estados.Add(estado);
                    db.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error: " + ex.Message;

                    vistaCliente();
                    vistaFormPago();
                    return View(orderView1);
                }
            }
            // Fin de la transacción

            ViewBag.Message = string.Format("El Pedido: {0}, se ha grabado", pedidoID);

            // Limpia la pantalla


            var formapagos = new List<FormPago>();

            foreach (var item in db.FormPagos.ToList())
            {
                var formapago = new FormPago
                {
                    NomFPago = item.NomFPago,
                    Pedidos = item.Pedidos,
                };
                formapagos.Add(formapago);
            }
            var clientes = new List<Cliente>();

            foreach (var itemC in db.Clientes.ToList())
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

            var orderView2 = new OrderView()
            {
                Cliente = clientes,
                FormPagos = formapagos,
                Productos = productos
            };

            Session["orderView"] = orderView2;

            vistaCliente();
            vistaFormPago();

            return View(orderView2);
        }
        public ActionResult Add_Producto()
        {
            vistaProducto();
            return View();
        }

        [HttpPost]
        public ActionResult Add_Producto(ProductOrder productOrder)
        {
            var orderView = Session["orderView"] as OrderView;

            var productoID = int.Parse(Request["idProducto"]);

            if (productoID == 0)
            {
                vistaProducto();
                ViewBag.Error = "Debe seleccionar un Producto";

                return View(productOrder);
            }

            var product = db.Productos.Find(productoID);
            if (product == null)
            {
                vistaProducto();
                ViewBag.Error = "El Producto no Existe";

                return View(productOrder);
            }

            var vcan = Request["Cantidad"];

            float ncantidad = 0;
            var ncan = Request["Cantidad"];

            if (vcan == "")
            {
                ncantidad = 0;
            }
            else
            {
                ncantidad = float.Parse(Request["Cantidad"]);
                if (ncantidad < 0)
                {
                    vistaProducto();
                    ViewBag.Error = "La Cantidad debe ser mayor que cero (0)";
                    return View(productOrder);
                }
            }

            // verifica si ya existe en mi lista temporal 
            // si lo encuentra le suma la cantidad 
            // SINO agrega un registro nuevo

            productOrder = orderView.Productos.Find(p => p.idProducto == productoID);
            if (productOrder == null)
            {
                if (ncantidad == 0)
                {
                    vistaProducto();
                    ViewBag.Error = "La Cantidad debe ser mayor que cero (0)";
                    return View(productOrder);
                }
                else
                {
                    productOrder = new ProductOrder
                    {
                        idProducto = product.idProducto,
                        Descripcion = product.Descripcion,
                        iva = product.iva,
                        Precio = product.Precio,
                        Cantidad = float.Parse(Request["Cantidad"])
                    };
                    orderView.Productos.Add(productOrder);
                }
            }
            else
            {
                productOrder.Cantidad += float.Parse(Request["Cantidad"]);
            }
            
            decimal TotPre = 0;
            decimal TotAcu = 0;
            decimal TotIVA = 0;

            foreach (var items in orderView.Productos)
            {
                TotIVA = items.ValorIVA + TotIVA;
                TotPre = items.ValorTot + TotPre;
            }

            TotAcu = TotIVA + TotPre;

            var ncant = orderView.Productos.Count;

            ViewBag.Tot_IVA = string.Format("{0:C2}", TotIVA);
            ViewBag.Tot_Pre = string.Format("{0:C2}", TotPre);
            ViewBag.Tot_Acu = string.Format("{0:C2}", TotAcu);

            vistaCliente1();
            vistaFormPago();

            return View("NewPedido", orderView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void vistaCliente()
        {
            
            var listacl = db.Clientes
                .Where(cl => cl.Estado != false) 
                .ToList();
            listacl.Add(new Cliente { idCliente = 0, NomClie = "[Seleccione un Cliente... ]" });
            listacl = listacl.OrderBy(cl => cl.NomClie).ToList();
            ViewBag.idCliente = new SelectList(listacl, "idCliente", "NomClieConca");
        }
        public void vistaCliente1()
        {
            var listac = db.Clientes
                .Where(cl => cl.Estado != false)
                .ToList();
            listac.Add(new Cliente { idCliente = 0, NomClie = "[Seleccione un Cliente... ]" });
            listac = listac.OrderBy(cl => cl.NomClie).ToList();
            ViewBag.idCliente = new SelectList(listac, "idCliente", "NomClieConca");
        }

        public void vistaFormPago()
        {
            var listafp = db.FormPagos.OrderBy(fp => fp.NomFPago).ToList();
            listafp.Add(new FormPago { idFormPago = 0, NomFPago = "[Seleccione una Forma de Pago... ]" });
            listafp = listafp.OrderBy(fp => fp.NomFPago).ToList();
            ViewBag.idFormPago = new SelectList(listafp, "idFormPago", "NomFPago");
        }

        public void vistaProducto()
        {
            var listap = db.Productos.ToList();
            listap.Add(new ProductOrder { idProducto = 0, Descripcion = "[Selecione un Producto... ]" });
            listap = listap.OrderBy(p => p.Descripcion).ToList();
            ViewBag.idProducto = new SelectList(listap, "idProducto", "Descripcion");
        }
    }

}
