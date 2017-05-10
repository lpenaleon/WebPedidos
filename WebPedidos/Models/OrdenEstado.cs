using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public enum OrdenEstado
    {
        Creada,
        EnProceso,
        Autorizada,
        Bloqueada,
        EnDespacho,
        Entregada
    }
}