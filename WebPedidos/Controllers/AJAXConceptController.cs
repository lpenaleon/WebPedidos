using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPedidos.Controllers
{
    public class AJAXConceptController : Controller
    {
        // GET: AJAXConcept
        public ActionResult Index()
        {
            return View();
        }

        // Método publico
        public JsonResult JsonFactorial(int n)
        {
            //validar si hay un llamado Json
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            //creamos un objeto Json con la propiedad Factorial 
            var result = new JsonResult
            {
                Data = new { Factorial = Factorial(n) }
            };
            return result;       
        }
        // Implementar el Factorial
        private double Factorial(int n)
        {
            double factorial = 1;
            for(int i = 2; i<= n ; i++)
            {
                factorial *= i;
            }
            return factorial;
               
        } 
    }
}