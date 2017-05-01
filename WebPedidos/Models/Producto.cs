using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public int idProducto { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [StringLength(14)]
        public string Codigo { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [StringLength(12, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Empaque { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Descripción")]
        [StringLength(60, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        public string Descripcion { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode =false)]
        public decimal Precio { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [DisplayFormat(DataFormatString ="{0:P2}",ApplyFormatInEditMode =false)]
        public float iva { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public bool Estado { get; set; }

        [StringLength(155, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 5)]
        [Display(Name ="Imagen") ]
        public string RutaFoto { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public int idMarca { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        public int idMaquina { get; set; }

        //muchos a uno
        public virtual Marca Marcas { get; set; }
        public virtual Maquina Maquinas { get; set; }
        //uno a muchos 
        public virtual ICollection<PedidoDet> PedidoDets { get; set; }
    }
}