using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class Cliente
    {
        [Key]
        public int idCliente { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Identificación")]
        [RegularExpression(@"[0-9]{1,9}(\.[0-9]{0,2})?$", ErrorMessage = "Por favor revise la Identificación o NIT")]
        [StringLength(14, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string ideNIT { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Cliente")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NomClie { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public int idMunicipio { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Dirección")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public bool Estado { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Vendedor")]
        public int idEmpleado { get; set; }
        //muchos a uno
        public virtual Municipio Municipios { get; set; } //Muchos a uno      
        public virtual Empleado Empleados { get; set; }//Muchos a uno
        //uno a muchos
        public virtual ICollection<ContactoClie> ContactoClies { get; set; }//Uno a muchos
        public virtual ICollection<Pedido> Pedidos { get; set; } //Uno a muchos
    }
}