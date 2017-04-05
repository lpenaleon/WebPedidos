using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebPedidos.Help
{
    public class Utilities
    {

        public static string CargarFoto(HttpPostedFileBase archivo, string carpeta)
        {
            string ruta = string.Empty;
            string imagen = string.Empty;

            if (archivo !=null)
            {
                imagen = Path.GetFileName(archivo.FileName);

                ruta = Path.Combine(HttpContext.Current.Server.MapPath(carpeta), imagen);

                archivo.SaveAs(ruta);
            }

            return imagen;
        }
    }
}