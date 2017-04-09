using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("Telefonos")]
    public class Telefono
    {
        [Key]
        public int IdTelefono { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Teléfono")]
        [RegularExpression(@"[0-9]{1,9}(\.[0-9]{0,2})?$", ErrorMessage = "Por favor revise el número registrado")]
        [StringLength(15, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 3)]
        public string NumTel { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Tipo")]
        public int idTipoTel { get; set; }
      
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Contacto")]
        public Int32 idContactoCli { get; set; }

        public virtual TipoTel TipoTels { get; set; }//Muchos a uno
        public virtual ContactoClie ContactoClies { get; set; }//Muchos a uno
    }
}