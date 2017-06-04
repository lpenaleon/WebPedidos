using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    public enum OrdenEstado
    {
        Creado,
        En_Proceso,
        Autorizado,
        Bloqueado,
        En_Despacho,
        Entregada
    }
}