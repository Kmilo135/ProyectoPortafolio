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
    
    public partial class ORDEN_COMPRA
    {
        public ORDEN_COMPRA()
        {
            this.DETALLE_ORDEN = new HashSet<DETALLE_ORDEN>();
        }
    
        public short NUMERO_ORDEN { get; set; }
        public int CANTIDAD_HUESPEDES { get; set; }
        public System.DateTime FECHA_LLEGADA { get; set; }
        public Nullable<System.DateTime> FECHA_SALIDA { get; set; }
        public Nullable<int> RUT_EMPLEADO { get; set; }
        public Nullable<int> RUT_CLIENTE { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual EMPLEADO EMPLEADO { get; set; }
        public virtual ICollection<DETALLE_ORDEN> DETALLE_ORDEN { get; set; }
    }
}
