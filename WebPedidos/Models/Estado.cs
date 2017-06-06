using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("Estados")]
    public class Estado
    {
        [Key]
        public long idEstado { get; set; }

        public long idPedido { get; set; }

        [Required(ErrorMessage = "Debe ingresar una {0}")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaEstado { get; set; }

        [Display(Name = "Estado")]
        public OrdenEstado OrdenEstado { get; set; }

        [Display(Name = "Nota")]
        [StringLength(100, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 0)]
        public string Nota { get; set; }

        //Muchos a uno
        public virtual Pedido Pedido { get; set; }
    }
}