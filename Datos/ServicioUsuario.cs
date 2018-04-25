using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ServicioUsuario
    {
        HostalEntities ent = new HostalEntities();

        public USUARIO EncontrarUsuario(USUARIO u)
        {
            USUARIO usuario = ent.USUARIO.FirstOrDefault(objeto =>
            objeto.NOMBRE_USUARIO.Equals(u.NOMBRE_USUARIO) && 
            u.PASSWORD.Equals(u.PASSWORD));
            if (usuario == null)
            {
                return null;
            }
            else
            {
                return usuario;
            }
        }

        public bool RegistrarUsuario(USUARIO u)
        {
            ent.USUARIO.Add(u);
            ent.SaveChanges();
            return true;
        }

        public bool ExisteUsuario(USUARIO u)
        {
            USUARIO usuario = ent.USUARIO.FirstOrDefault(objeto =>
            objeto.NOMBRE_USUARIO.Equals(u.NOMBRE_USUARIO));
            if (usuario == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EliminarUsuario(USUARIO u)
        {
            USUARIO usuario = ent.USUARIO.FirstOrDefault(objeto =>
            objeto.ID_USUARIO.Equals(u.ID_USUARIO));

            if (usuario == null)
            {
                return false;
            }
            else
            {
                ent.USUARIO.Remove(u);
                ent.SaveChanges();
                return true;
            }
        }

        public List<USUARIO> ListarUsuarios()
        {
            var lista = (from consulta in ent.USUARIO
                         orderby consulta.ID_USUARIO
                         select consulta).ToList();
            return lista;
        }

        public bool ModificarUsuario(USUARIO u)
        {
            USUARIO usuario = ent.USUARIO.FirstOrDefault(objeto => objeto.ID_USUARIO == u.ID_USUARIO);

            if (usuario == null)
            {
                return false;
            }
            else
            {
                //Se modifica cada atributo y se guardan los cambios en la base de datos
                usuario.PASSWORD = u.PASSWORD;
                usuario.ESTADO = u.ESTADO;
                ent.SaveChanges();
                return true;
            }
        }
    }
}
