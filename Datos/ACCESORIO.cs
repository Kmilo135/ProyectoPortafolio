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
    
    public partial class ACCESORIO
    {
        public ACCESORIO()
        {
            this.DETALLE_ACCESORIOS = new HashSet<DETALLE_ACCESORIOS>();
        }
    
        public short ID_ACCESORIO { get; set; }
        public string NOMBRE_ACCESORIO { get; set; }
        public Nullable<int> PRECIO { get; set; }
        public Nullable<short> CANTIDAD { get; set; }
    
        public virtual ICollection<DETALLE_ACCESORIOS> DETALLE_ACCESORIOS { get; set; }
    }
}
