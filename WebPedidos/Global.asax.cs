using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebPedidos.Migrations;
using WebPedidos.Models;

namespace WebPedidos
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Migraciones automaticas
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<WebPedidosContext, Configuration>());

            ApplicationDbContext bd = new ApplicationDbContext(); // Abre la Base de datos
            CreateRoles(bd);
            CreateSuperUser(bd);
            AddPermisionToSuperUser(bd);
            bd.Dispose();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermisionToSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var user = userManager.FindByName("leonardop_leon@outlook.com");

            if (!userManager.IsInRole(user.Id, "Admin"))
            {
                userManager.AddToRole(user.Id, "Admin");
            }
            if (!userManager.IsInRole(user.Id, "Cliente"))
            {
                userManager.AddToRole(user.Id, "Cliente");
            }
            if (!userManager.IsInRole(user.Id, "Tesoreria"))
            {
                userManager.AddToRole(user.Id, "Tesoreria");
            }
            if (!userManager.IsInRole(user.Id, "Despachos"))
            {
                userManager.AddToRole(user.Id, "Despachos");
            }
            if (!userManager.IsInRole(user.Id, "Vendedor"))
            {
                userManager.AddToRole(user.Id, "Vendedor");   
            }

        }

        private void CreateSuperUser(ApplicationDbContext bd)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(bd));
            var user = userManager.FindByName("leonardop_leon@outlook.com");
            if (user == null)
            {
                user = new ApplicationUser 
                {
                    UserName = "leonardop_leon@outlook.com",
                    Email = "leonardop_leon@outlook.com"
                };
                userManager.Create(user, "Papscasslpl121211.");
            }
        }
        
        private void CreateRoles(ApplicationDbContext db)
        {
            //permitir manipular los roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            //Crear Roles
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
            if (!roleManager.RoleExists("Cliente"))
            {
                roleManager.Create(new IdentityRole("Cliente"));
            }
            if (!roleManager.RoleExists("Despachos"))
            {
                roleManager.Create(new IdentityRole("Despachos"));
            }
            if (!roleManager.RoleExists("Tesoreria"))
            {
                roleManager.Create(new IdentityRole("Tesoreria"));
            }
            if (!roleManager.RoleExists("Vendedor"))
            {
                roleManager.Create(new IdentityRole("Vendedor"));
            }
        }
    }
}
