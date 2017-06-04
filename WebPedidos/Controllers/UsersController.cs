using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPedidos.Models;
using WebPedidos.ViewModels;
using PagedList;
using System.Net;

namespace WebPedidos.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: Users
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();
            foreach (var user in users)
            {
                var userView = new UserView
                {
                    Email = user.Email,
                    Name = user.UserName,
                    UserID = user.Id
                };
                usersView.Add(userView);
            }
            return View(usersView.ToPagedList((int)page, 8));


        }

        public ActionResult Roles(string idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == idUser);
            if (user == null)
            {
                return HttpNotFound();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();
             
            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }
            
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };
            return View(userView);
        }

        //GET

        public ActionResult AddRole(string idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == idUser);

            if (user == null)
            {
                return HttpNotFound();
            }
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id  
            };

            //  vistaRole();


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var listaR = roleManager.Roles.ToList();
            listaR.Add(new IdentityRole { Id = "", Name = "[Seleccione un Role... ]" });
            listaR = listaR.OrderBy(r => r.Name).ToList();
            ViewBag.RoleID = new SelectList(listaR, "Id", "Name");

            return View(userView);
        }

        //POST
        [HttpPost]
        public ActionResult AddRole(string idUser, FormCollection form)
        {
            var idRole = Request["RoleID"];

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == idUser);
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };

            if (string.IsNullOrEmpty(idRole))
            {
                ViewBag.Error = "Debe Seleccionar Un Role";              
                var listaR = roleManager.Roles.ToList();
                listaR.Add(new IdentityRole { Id = "", Name = "[Seleccione un Role... ]" });
                listaR = listaR.OrderBy(r => r.Name).ToList();
                ViewBag.RoleID = new SelectList(listaR, "Id", "Name");
                return View(userView);
            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == idRole);

            if (!userManager.IsInRole(idUser,role.Name))
            {
                userManager.AddToRole(idUser, role.Name);
            }

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }

            userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };
            return View("Roles",userView);

        }

        public ActionResult Delete(string idUser, string idRole)
        {
            if (string.IsNullOrEmpty(idUser) || string.IsNullOrEmpty(idRole))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            //var users = userManager.Users.ToList();
            //var user = users.Find(u => u.Id == idUser);
            var user = userManager.Users.ToList().Find(u => u.Id == idUser);
            var role = roleManager.Roles.ToList().Find(r => r.Id == idRole);

            if(userManager.IsInRole(user.Id, role.Name))
            {
                userManager.RemoveFromRole(user.Id, role.Name);
            }

            var Users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };
            return View("Roles", userView);


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public void vistaRole()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var listaR = roleManager.Roles.ToList();
            listaR.Add(new IdentityRole { Id = "", Name = "[Seleccione un Role... ]" });
            listaR = listaR.OrderBy(r => r.Name).ToList();
            ViewBag.RoleID = new SelectList(listaR, "Id", "Name");
        }

        public void armar1(string idUser)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == idUser);

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };
        }

    }
}