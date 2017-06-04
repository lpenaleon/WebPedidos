using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebPedidos.ViewModels;

namespace WebPedidos.ViewModels
{
    public class UserView
    {
        public string UserID { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Nombre")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ingrese su Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(60)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Por favor ingrese el Correo Electrónico")]
        public string Email { get; set; }

        public RoleView Role { get; set; }

        public List<RoleView> Roles { get; set; }
    }
}