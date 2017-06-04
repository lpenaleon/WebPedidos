using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebPedidos.ViewModels
{
    public class RoleView
    {
        public string RoleID { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Nombre")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Name { get; set; }

    }
}