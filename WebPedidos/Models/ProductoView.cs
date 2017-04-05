using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace WebPedidos.Models
{
    [NotMapped]
    public class ProductoView : Producto
    {
        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImagenFile { get; set; }
    }
}