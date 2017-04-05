using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    //No va a la base de datos
    [NotMapped]
    public class PedidoView
    {
        public Cliente Cliente { get; set; }

        public Producto Producto { get; set; }


    }
}