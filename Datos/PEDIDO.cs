//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class PEDIDO
    {
        public PEDIDO()
        {
            this.DETALLE_PEDIDO = new HashSet<DETALLE_PEDIDO>();
        }
    
        public short NUMERO_PEDIDO { get; set; }
        public System.DateTime FECHA_PEDIDO { get; set; }
        public string ESTADO_PEDIDO { get; set; }
        public int RUT_EMPLEADO { get; set; }
        public Nullable<short> NUMERO_RECEPCION { get; set; }
        public int RUT_PROVEEDOR { get; set; }
    
        public virtual ICollection<DETALLE_PEDIDO> DETALLE_PEDIDO { get; set; }
        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual PROVEEDOR PROVEEDOR { get; set; }
        public virtual RECEPCION RECEPCION { get; set; }
    }
}