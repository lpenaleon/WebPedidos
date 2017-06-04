using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class WebPedidosContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
        //abre db
        public WebPedidosContext() : base("WebPedidosContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();            
        }
        //cierra db
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public System.Data.Entity.DbSet<WebPedidos.Models.Maquina> Maquinas { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Marca> Marcas { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.EmpTran> EmpTrans { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Flete> Fletes { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Cargo> Cargos { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Telefono> Telefonos { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Departamento> Departamentos { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Municipio> Municipios { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Producto> Productos { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Empleado> Empleados { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.TipoTel> TipoTels { get; set; }
        public System.Data.Entity.DbSet<WebPedidos.Models.FormPago> FormPagos { get; set; }
        public System.Data.Entity.DbSet<WebPedidos.Models.Cliente> Clientes { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Pedido> Pedidos { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.PedidoDet> PedidoDets { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.PedFlete> PedFletes { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.ContactoClie> ContactoClies { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.TipoIde> TipoIdes { get; set; }

        public System.Data.Entity.DbSet<WebPedidos.Models.Estado> Estados { get; set; }

    }
}
