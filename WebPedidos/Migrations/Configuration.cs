namespace WebPedidos.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebPedidos.Models.WebPedidosContext>
    {
        public Configuration()
        {
            //Migraciones automaticas
            AutomaticMigrationDataLossAllowed = true;

            AutomaticMigrationsEnabled = true;
            ContextKey = "WebPedidos.Models.WebPedidosContext";
        }

        protected override void Seed(WebPedidos.Models.WebPedidosContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
