using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class Departamento
    {
        [Key]
        public int idDepto { get; set; }
        [Required(ErrorMessage =("El campo {0} es requerido"))]
        [Display(Name ="Departamento")]
        [StringLength(60,ErrorMessage ="El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength =5)]
        public string NomDepto { get; set; }
        public int indicativo { get; set; }
        //relaciones
        // uno a muchos
        public virtual ICollection<Municipio> Municipios { get; set; }
    }

}