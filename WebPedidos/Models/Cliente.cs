using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int idCliente { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Tipo de Identificación")]
        public int idTipoIde { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Identificación")]
        [RegularExpression(@"[0-9]{1,9}(\.[0-9]{0,2})?$", ErrorMessage = "Por favor revise la Identificación o NIT")]
        [StringLength(14, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NumIde { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Cliente")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string NomClie { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public int idMunicipio { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Dirección")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public bool Estado { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Vendedor")]
        public int idEmpleado { get; set; }

        [NotMapped]
        public string NomClieConca { get { return string.Format("{0} - {1} - {2}", NomClie, idMunicipio, Direccion);}set { NomClieConca = value;  } }

        [NotMapped]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Días de Credito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public byte DiasCred { get; set; }


        //muchos a uno
        public virtual Municipio Municipio { get; set; } //Muchos a uno      
        public virtual Empleado Empleado { get; set; }//Muchos a uno
        public virtual TipoIde TipoIde { get; set; }

        //uno a muchos
        public virtual ICollection<ContactoClie> ContactoClies { get; set; }//Uno a muchos
        public virtual ICollection<Pedido> Pedidos { get; set; } //Uno a muchos
    }
}