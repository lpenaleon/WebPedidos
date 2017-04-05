using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public class Municipio
    {
        [Key]
        public int idMunicipio { get; set; }

        [Display(Name = "Cod. Municipio")]
        public int codMunicipio { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Municipio")]
        [StringLength(35, ErrorMessage = "El campo {0} debe contener maximo {1} y minimo {2}", MinimumLength = 3)]
        public string NomMunicipio { get; set; }

        [Required(ErrorMessage = ("El campo {0} es requerido"))]
        [Display(Name = "Departamento")]
        public int idDepto { get; set; }

        // Relaciones
        
        //muchos a uno
        public virtual Departamento Departamentos { get; set; }//mucho a uno
        //uno a muchos
        public virtual ICollection<Cliente> Clientes { get; set; }//Uno a muchos

    }
}