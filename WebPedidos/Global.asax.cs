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

            if (!userManager.IsInRole(user.Id, "View"))
            {
                userManager.AddToRole(user.Id, "View");
            }
            if (!userManager.IsInRole(user.Id, "Create"))
            {
                userManager.AddToRole(user.Id, "Create");
            }
            if (!userManager.IsInRole(user.Id, "Edit"))
            {
                userManager.AddToRole(user.Id, "Edit");
            }
            if (!userManager.IsInRole(user.Id, "Delete"))
            {
                userManager.AddToRole(user.Id, "Delete");
            }
            if (!userManager.IsInRole(user.Id, "Users"))
            {
                userManager.AddToRole(user.Id, "Users");
            }
            if (!userManager.IsInRole(user.Id, "Admin"))
            {
                userManager.AddToRole(user.Id, "Admin");
            }
            if (!userManager.IsInRole(user.Id, "ViewClie"))
            {
                userManager.AddToRole(user.Id, "ViewClie");
            }
            if (!userManager.IsInRole(user.Id, "ViewETrans"))
            {
                userManager.AddToRole(user.Id, "ViewETrans");
            }
            if (!userManager.IsInRole(user.Id, "ViewMuni"))
            {
                userManager.AddToRole(user.Id, "ViewMuni");
            }
            if (!userManager.IsInRole(user.Id, "CreatePed"))
            {
                userManager.AddToRole(user.Id, "CreatePed");
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
            if (!roleManager.RoleExists("View"))
            {
                roleManager.Create(new IdentityRole("View"));
            }

            if (!roleManager.RoleExists("Create"))
            {
                roleManager.Create(new IdentityRole("Create"));
            }

            if (!roleManager.RoleExists("Edit"))
            {
                roleManager.Create(new IdentityRole("Edit"));
            }

            if (!roleManager.RoleExists("Delete"))
            {
                roleManager.Create(new IdentityRole("Delete"));
            }
            if (!roleManager.RoleExists("Users"))
            {
                roleManager.Create(new IdentityRole("Users"));
            }
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
            if (!roleManager.RoleExists("ViewClie"))
            {
                roleManager.Create(new IdentityRole("ViewClie"));
            }
            if (!roleManager.RoleExists("ViewETrans"))
            {
                roleManager.Create(new IdentityRole("ViewETrans"));
            }
            if (!roleManager.RoleExists("ViewMuni"))
            {
                roleManager.Create(new IdentityRole("ViewMuni"));
            }
            if (!roleManager.RoleExists("CreatePed"))
            {
                roleManager.Create(new IdentityRole("CreatePed"));
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
