using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebPedidos.Models
{
    [Table("PedFletes")]
    public class PedFlete
    {
        [Key]
        public long idPedFlete { get; set; }
        public long idPedido { get; set; }
        public int idFlete { get; set; }
        public int idEmpTran { get; set; }
        public decimal Valor { get; set; }
        public string Obervaciones { get; set; }

        //muchos a uno
        public virtual Flete Flete { get; set; }
        public virtual EmpTran EmpTran { get; set; }
        public virtual Pedido Pedido { get; set; }

    }
}