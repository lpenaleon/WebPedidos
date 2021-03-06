﻿using PagedList;
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
using System.Text.RegularExpressions;

namespace WebPedidos.Controllers
{
   
    public class ProductosController : Controller
    {
        private WebPedidosContext db = new WebPedidosContext();

        //*********************************************************
        //*********************************************************
        [Authorize(Roles = "Admin,Cliente,Vendedor,Despacho")]
        // GET: Productos
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);

            var productos = db.Productos
                .Include(p => p.Maquinas)
                .Include(p => p.Marcas)
                .OrderBy(p => p.Descripcion);

            return View(productos.ToPagedList((int)page, 8));
        }
        //*********************************************************
        // POST: Productos
        [HttpPost]
        public ActionResult Index(string BuscadorP, int? page, int? page1 = 0)
        {

           page = (page ?? 1);

            var productos = db.Productos
                .Include(p => p.Maquinas)
                .Include(p => p.Marcas)                
                .OrderBy(p => p.Descripcion)
                .Where(p => p.Descripcion.Contains(BuscadorP) || p.Maquinas.NomMaquina.Contains(BuscadorP) || p.Marcas.NomMarca.Contains(BuscadorP))
                .ToList();
            var nreg = productos.Count();
            if (nreg==0)
            {
                nreg = 1;
            }
            return View(productos.ToPagedList((int)page, nreg));
        }
        //*********************************************************
        //*********************************************************
        // GET: Productos/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }


        // GET: Productos/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.idMaquina = new SelectList(db.Maquinas, "IdMaquina", "NomMaquina");
            ViewBag.idMarca = new SelectList(db.Marcas, "idMarca", "NomMarca");

            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductoView view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imagen = string.Empty;
                    var carpeta = "~/Imagenes";

                    if (view.ImagenFile != null)
                    {
                        imagen = Utilities.CargarFoto(view.ImagenFile, carpeta);

                        imagen = string.Format("{0}/{1}", carpeta, imagen);
                    }

                    view.RutaFoto = imagen;

                    var producto = toProducto(view);

                    producto.RutaFoto = imagen;

                    db.Productos.Add(producto);

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                ViewBag.idMaquina = new SelectList(db.Maquinas, "IdMaquina", "NomMaquina", view.idMaquina);
                ViewBag.idMarca = new SelectList(db.Marcas, "idMarca", "NomMarca", view.idMarca);
                return View(view);

            }

            ViewBag.idMaquina = new SelectList(db.Maquinas, "IdMaquina", "NomMaquina", view.idMaquina);
            ViewBag.idMarca = new SelectList(db.Marcas, "idMarca", "NomMarca", view.idMarca);
            return View(view);
        }

        private Producto toProducto(ProductoView view)
        {
            return new Producto
            {
                idProducto = view.idProducto,
                Codigo = view.Codigo,
                Empaque = view.Empaque,
                Descripcion = view.Descripcion,
                Precio = view.Precio,
                iva = view.iva,
                Estado = view.Estado,
                RutaFoto = view.RutaFoto,
                idMarca = view.idMarca,
                idMaquina = view.idMaquina,
                
                
                
                Maquinas = view.Maquinas,
                Marcas = view.Marcas,
                PedidoDets = view.PedidoDets,
                
                
            };
        }
        // GET: Productos/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = db.Productos.Find(id);

            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.idMaquina = new SelectList(db.Maquinas, "IdMaquina", "NomMaquina", producto.idMaquina);
            ViewBag.idMarca = new SelectList(db.Marcas, "idMarca", "NomMarca", producto.idMarca);

            var view = toView(producto);

            return View(view);
        }
        private ProductoView toView(Producto producto)
        {
            return new ProductoView
            {
                Codigo = producto.Codigo,
                Descripcion = producto.Descripcion,
                Empaque = producto.Empaque,
                Estado = producto.Estado,
                idMaquina = producto.idMaquina,
                idMarca = producto.idMarca,
                idProducto = producto.idProducto,
                iva = producto.iva,
                Maquinas = producto.Maquinas,
                Marcas = producto.Marcas,
                PedidoDets = producto.PedidoDets,
                Precio = producto.Precio,
                RutaFoto = producto.RutaFoto,
            };
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductoView view)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var image1 = "banco.png";
                    var imagen = view.RutaFoto;
                    var carpeta = "~/Imagenes";

                    if (view.ImagenFile != null)
                    {
                        imagen = Utilities.CargarFoto(view.ImagenFile, carpeta);

                        imagen = string.Format("{0}/{1}", carpeta, imagen);
                    }
                    else
                    {
                        imagen = Utilities.CargarFoto(view.ImagenFile, carpeta);

                        imagen = string.Format("{0}/{1}", carpeta, image1);
                    }

                    view.RutaFoto = imagen;

                    var producto = toProducto(view);

                    producto.RutaFoto = imagen;

                    db.Entry(producto).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                ViewBag.idMaquina = new SelectList(db.Maquinas, "IdMaquina", "NomMaquina", view.idMaquina);
                ViewBag.idMarca = new SelectList(db.Marcas, "idMarca", "NomMarca", view.idMarca);
                return View(view);

            }

            ViewBag.idMaquina = new SelectList(db.Maquinas, "IdMaquina", "NomMaquina", view.idMaquina);
            ViewBag.idMarca = new SelectList(db.Marcas, "idMarca", "NomMarca", view.idMarca);
            return View(view);
        }

        // GET: Productos/Delete/5
        [Authorize (Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
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
