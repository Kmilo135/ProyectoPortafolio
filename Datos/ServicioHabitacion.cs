using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ServicioHabitacion
    {
        HostalEntities ent = new HostalEntities();

        public List<TIPO_HABITACION> ListarTipoHabitacion()
        {
            return ent.TIPO_HABITACION.ToList();
        }

        public bool AgregarHabitacion(HABITACION habitacion)
        {
            ent.HABITACION.Add(habitacion);
            ent.SaveChanges();
            return true;
        }

        public List<HABITACION> listarHabitacion()
        {
            return ent.HABITACION.ToList();
        }
    }
}
