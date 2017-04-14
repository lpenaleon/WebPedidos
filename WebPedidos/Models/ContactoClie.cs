using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("ContactoClies") ]
    public class ContactoClie
    {   
        [Key]
        public int idContactoClie { get; set; }        

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Contacto")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 3)]
        public string NomContacto { get; set; }

        [Required(ErrorMessage = "Ingrese su Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electrónico")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Por favor ingrese el Correo Electrónico")]
        public string Email { get; set; }

        public int idCliente { get; set; }

        public virtual Cliente Cliente { get; set; }//muchos a uno
        public virtual ICollection<Telefono> telefonos { get; set; }//uno a muchos

    }
}